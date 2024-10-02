using XYZ_Tanks.Input;
using XYZ_Tanks.Map;
using XYZ_Tanks.Rendering;
using XYZ_Tanks.Units;
using System;
using System.Collections.Generic;
using XYZ_Tanks.Engine;

namespace XYZ_Tanks
{
    // Интерфейс для объектов, которые должны обновляться (например, танки и снаряды)
    public interface IUpdateable
    {
        void Update(double deltaSeconds);
    }

    // Интерфейс для чтения пользовательского ввода
    public interface IInputReader
    {
        void Update();
        // Событие, которое вызывается при регистрации действия пользователя
        event EventHandler<InputEventArgs> InputActionCalled;
    }

    // Интерфейс для рендеринга объектов игры на консоли
    public interface IRenderer
    {
        void RenderWalls();
        void RenderGameInfo(int level, int health);
        void DrawTank(Vector2Int position, Orientation orientation, bool playerTank = false);
        void DrawProjectileAt(Vector2Int position);
        void EraseAtMapCoordinate(Vector2Int coordinate);
    }

    // Интерфейс для управления состоянием и поведением карты уровня
    public interface ILevelMapManager
    {
        // Свойства для управления состоянием карты
        StaticObject[][] Map { get; }
        List<Projectile> Projectiles { get; }
        List<EnemyTank> EnemyTanks { get; }

        // Событие для перерисовки карты
        event EventHandler<RedrawRequiredAtArgs> RedrawRequired;

        // Методы для работы с картой уровня
        void LoadLevel(string levelFilePath);
        void Clear();
        bool IsWalkableAtCoordinate(Vector2Int coordinate);
        bool IsProjectilePassable(Vector2Int coordinate);
        bool IsDamageable(Vector2Int coordinate);
        void Damage(Vector2Int coordinate);
        Vector2Int GetRandomTankPosition();
        void SpawnProjectile(Vector2Int position, Orientation orientation);
        void Update(double deltaSeconds);
    }
}