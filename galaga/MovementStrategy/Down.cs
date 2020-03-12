using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;



namespace galaga.MovementStrategy
{
    public class Down : IMovementStrategy

    {
        private float speed;

        public Down()
        {
            speed = 1.0f;}
        public void MoveEnemy(Enemy enemy)
        {
            enemy.Shape.MoveY(-0.001f*speed);
        }
        
        public void MoveEnemies(EntityContainer<Enemy> enemies)
        {
            enemies.Iterate(MoveEnemy);
        }

        public void IncreaseSpeedBy(float s) {this.speed += s;}
    }
}