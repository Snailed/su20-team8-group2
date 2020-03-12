using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;


namespace galaga.MovementStrategy 
{
    public class NoMove : IMovementStrategy
    {
        public void MoveEnemy(Enemy enemy)
        {
            enemy.Shape.MoveY(0.00f);
        }

        public void MoveEnemies(EntityContainer<Enemy> enemies)
        {
            enemies.Iterate(MoveEnemy);
        }
    }
}