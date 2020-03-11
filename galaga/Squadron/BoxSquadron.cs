using System;
using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace galaga.Squadron {
    public class BoxSquadron : ISquadron {
        public EntityContainer<Enemy> Enemies { get; }
        public int MaxEnemies { get; }

        public BoxSquadron(int maxEnemies) {
            this.MaxEnemies = maxEnemies;
            this.Enemies = new EntityContainer<Enemy>();
        }
        public void CreateEnemies(List<Image> enemyStrides) {
            int width = (int) Math.Floor(Math.Sqrt(MaxEnemies));
            for (int i = 0; i < MaxEnemies; i++) {
                int layer = i / width;
                Enemies.AddDynamicEntity(new Enemy(
                    new DynamicShape(
                        // The +0.2f is for centering purposes
                        new Vec2F(i%width*0.1f + 0.2f, 0.9f - layer*0.1f), 
                        new Vec2F(0.1f, 0.1f)),
                    new ImageStride(80, enemyStrides)));
            }
        }
    }
}
