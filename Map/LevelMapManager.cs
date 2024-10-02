﻿using XYZ_Tanks.Engine;
using XYZ_Tanks.Extensions;
using XYZ_Tanks.Units;
using Microsoft.Extensions.DependencyInjection;
using XYZ_Tanks.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace XYZ_Tanks.Map
{
    public class LevelMapManager : ILevelMapManager
    {
        private readonly Random _random = new(DateTime.Now.Second);
        public const int LevelHeight = 13;
        public const int LevelWidth = 13;
        private readonly List<EnemyTank> _enemyTanks = new();
        private readonly List<Projectile> _projectiles = new();
        private readonly IServiceProvider _serviceProvider;

        private readonly List<List<StaticObject>> _mapData;

        // Преобразование _mapData в StaticObject[][]
        public StaticObject[][] Map => _mapData.Select(row => row.ToArray()).ToArray();

        public List<EnemyTank> EnemyTanks => _enemyTanks;
        public List<Projectile> Projectiles => _projectiles;

        public event EventHandler<RedrawRequiredAtArgs> RedrawRequired = null!;

        public LevelMapManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _mapData = new List<List<StaticObject>>(LevelHeight);

            for (int i = 0; i < LevelHeight; i++)
            {
                var row = new List<StaticObject>(LevelWidth);

                for (int j = 0; j < LevelWidth; j++)
                {
                    row.Add(StaticObject.Empty);
                }

                _mapData.Add(row);
            }
        }

        public void LoadLevel(string name)
        {
            var rows = File.ReadAllLines(name);

            for (int y = 0; y < LevelHeight; y++)
            {
                for (int x = 0; x < LevelWidth; x++)
                {
                    Set(x, y, rows[y][x].ToStaticObject());
                }
            }
        }

        public void Set(Vector2Int position, StaticObject staticObject)
            => Set(position.X, position.Y, staticObject);

        public void Set(int x, int y, StaticObject staticObject)
        {
            _mapData[x][y] = staticObject;
        }

        public bool IsWalkableAtCoordinate(Vector2Int position) =>
            IsWalkableAtCoordinate(position.X, position.Y);

        public bool IsWalkableAtCoordinate(int x, int y) =>
            IsOnMap(x, y)
            && _mapData[x][y] == StaticObject.Empty
            && !_enemyTanks.Any(et => et.Transform.Position.X == x && et.Transform.Position.Y == y);

        private static bool IsOnMap(Vector2Int vector) =>
            IsOnMap(vector.X, vector.Y);

        private static bool IsOnMap(int x, int y) =>
            x >= 0
            && y >= 0
            && x < LevelWidth
            && y < LevelHeight;

        public bool IsProjectilePassable(Vector2Int position) =>
            IsProjectilePassable(position.X, position.Y);

        public bool IsProjectilePassable(int x, int y) =>
            IsOnMap(x, y)
            && (_mapData[x][y] == StaticObject.Empty
                || _mapData[x][y] == StaticObject.River);

        public bool IsDamageable(Vector2Int position) =>
            IsDamageable(position.X, position.Y);

        public bool IsDamageable(int x, int y) =>
            IsOnMap(x, y)
            && (_mapData[x][y] == StaticObject.Wall
                || _mapData[x][y] == StaticObject.DamagedWall);

        public void Damage(Vector2Int position)
            => Damage(position.X, position.Y);

        public void Damage(int x, int y)
        {
            if (_mapData[x][y] == StaticObject.Wall)
            {
                Set(x, y, StaticObject.DamagedWall);
            }
            else if (_mapData[x][y] == StaticObject.DamagedWall)
            {
                Set(x, y, StaticObject.Empty);
            }
        }

        public Vector2Int GetRandomTankPosition()
        {
            int x;
            int y;
            do
            {
                x = _random.Next(LevelWidth);
                y = _random.Next(LevelHeight);
            }
            while (!IsWalkableAtCoordinate(x, y));

            return new Vector2Int(x, y);
        }

        public void Update(double deltaSeconds)
        {
            UpdateProjectiles(deltaSeconds);
            UpdateEnemyTanks(deltaSeconds);
        }

        public void SpawnProjectile(Vector2Int position, Orientation orientation)
        {
            var projectile = _serviceProvider.GetRequiredService<Projectile>();

            projectile.Transform.Position = position;
            projectile.Transform.Orientation = orientation;

            _projectiles.Add(projectile);
        }

        private void UpdateEnemyTanks(double deltaSeconds)
        {
            foreach (var tank in _enemyTanks)
            {
                tank.Update(deltaSeconds);
            }
        }

        private void UpdateProjectiles(double deltaSeconds)
        {
            var projectilesToRemove = new List<Projectile>();

            foreach (var projectile in Projectiles)
            {
                projectile.Update(deltaSeconds);

                if (projectile.Transform.PreviousPosition != null)
                {
                    RedrawRequired?.Invoke(this, new(projectile.Transform.PreviousPosition.Value, RedrawType.StaticObject));
                }

                if (!projectile.IsAlive)
                {
                    projectilesToRemove.Add(projectile);

                    if (IsOnMap(projectile.Transform.Position))
                    {
                        RedrawRequired?.Invoke(this, new(projectile.Transform.Position, RedrawType.StaticObject));
                    }

                    var tankToDamage = EnemyTanks.FirstOrDefault(et => et.Transform.Position == projectile.Transform.Position);

                    if (tankToDamage != null)
                    {
                        tankToDamage.Health--;
                    }

                    projectile.Dispose();
                }
                else
                {
                    RedrawRequired?.Invoke(this, new(projectile.Transform.Position, RedrawType.Projectile));
                }
            }

            Projectiles.RemoveAll(projectilesToRemove.Contains);
        }

        public void Clear()
        {
            _projectiles.Clear();
            _enemyTanks.Clear();
        }
    }
}