using System;
using DIKUArcade.Entities;
using DIKUArcade.Math;

namespace galaga.MovementStrategy
{
    public class ZigZagDown : IMovementStrategy
    {
        public void MoveEnemy(Enemy enemy)
        {
            float s = 0.0003f;
            float p = 0.045f;
            float a = 0.05f;
            Vec2F start = enemy.startPos;
            float yi = enemy.Shape.Position.Y - s;
            var xi = start.X + a * Math.Sin((2*Math.PI * (start.Y - yi)) / p);

            enemy.Shape.SetPosition(new Vec2F((float)xi, yi));
        }

        public void MoveEnemies(EntityContainer<Enemy> enemies)
        {
            enemies.Iterate(MoveEnemy);
        }
    }
}