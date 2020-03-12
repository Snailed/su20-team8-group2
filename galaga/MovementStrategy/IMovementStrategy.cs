using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;


namespace galaga.MovementStrategy
{
    public interface IMovementStrategy
    {
        void MoveEnemy(Enemy enemy);
        void MoveEnemies(EntityContainer<Enemy> enemies);
        void IncreaseSpeedBy(float speed);
    }
}