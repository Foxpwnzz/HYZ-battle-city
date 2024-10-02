using System;
using XYZ_Tanks.Engine;

namespace XYZ_Tanks.Rendering
{
    // Перечисление типов перерисовки объектов на карте
    public enum RedrawType
    {
        StaticObject,
        Projectile,
        PlayerTank,
        EnemyTank
    }

    // Аргументы для событий, требующих перерисовки карты
    public class RedrawRequiredAtArgs : EventArgs
    {
        public Vector2Int Position { get; }
        public RedrawType RedrawType { get; }

        public RedrawRequiredAtArgs(Vector2Int position, RedrawType redrawType)
        {
            Position = position;
            RedrawType = redrawType;
        }
    }
}