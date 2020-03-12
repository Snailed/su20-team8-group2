using System;
using DIKUArcade.Entities;
using DIKUArcade.Math;

namespace galaga.MovementStrategy
{
    public class ZigZagDown : IMovementStrategy
    {
        private float speed;

        public ZigZagDown() {speed = 0.0003f;}
        public void MoveEnemy(Enemy enemy)
        {
            float p = 0.045f;
            float a = 0.05f;
            Vec2F start = enemy.startPos;
            float yi = enemy.Shape.Position.Y - speed;
            var xi = start.X + a * Math.Sin((2*Math.PI * (start.Y - yi)) / p);

            enemy.Shape.SetPosition(new Vec2F((float)xi, yi));
        }

        public void MoveEnemies(EntityContainer<Enemy> enemies)
        {
            enemies.Iterate(MoveEnemy);
        }

        public void IncreaseSpeedBy(float s) {this.speed += s;}
    }
}