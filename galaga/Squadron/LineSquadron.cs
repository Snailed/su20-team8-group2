using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace galaga.Squadron {
    public class LineSquadron : ISquadron {
        public EntityContainer<Enemy> Enemies { get; }
        public int MaxEnemies { get; }

        public LineSquadron(int maxEnemies) {
            this.MaxEnemies = maxEnemies;
            this.Enemies = new EntityContainer<Enemy>();
        }
        public void CreateEnemies(List<Image> enemyStrides) {
            for (int i = 0; i < MaxEnemies; i++)
            {
                Enemies.AddDynamicEntity(new Enemy(
                    new DynamicShape(
                        // The +0.2f is for centering purposes
                        new Vec2F(i*0.1f + 0.2f, 0.9f), 
                        new Vec2F(0.1f, 0.1f)),
                    new ImageStride(80, enemyStrides)));
            }
        }
    }
}