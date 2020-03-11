using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace galaga.Squadron {
    public interface ISquadron {
        EntityContainer<Enemy> Enemies { get; }
        int MaxEnemies { get; }
        void CreateEnemies(List<Image> enemyStrides);
    }
}