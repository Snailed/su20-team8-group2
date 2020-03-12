using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;



namespace galaga.MovementStrategy
{
    public class Down : IMovementStrategy

    {
        public Down() {}
        public void MoveEnemy(Enemy enemy)
        {
            enemy.Shape.MoveY(-0.001f);
        }
        
        public void MoveEnemies(EntityContainer<Enemy> enemies)
        {
            enemies.Iterate(MoveEnemy);
        }
    }
}