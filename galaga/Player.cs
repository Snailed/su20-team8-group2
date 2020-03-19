using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System;
namespace galaga
{
    public class Player : IGameEventProcessor<object>
    {
        private IBaseImage image;
        private DynamicShape shape;
        public Entity Entity { get; private set; }

        public Player(DynamicShape shape, IBaseImage image) {
            this.image = image;
            this.shape = shape;
            Entity = new Entity(shape, image);
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.MovementEvent) {
                switch (gameEvent.Message) {
                    case "MOVE_RIGHT":
                        Console.WriteLine("move1");
                        this.Direction(new Vec2F(-0.01f, 0.0f));
                        break;
                    case "MOVE_LEFT":
                        this.Direction(new Vec2F(0.01f, 0.0f));
                        break;
                    case "MOVE_STOP":
                        Console.WriteLine("Stop");
                        this.Direction(new Vec2F(0.0f, 0.0f));
                        break;
                }
            }
        }

        private void Direction(Vec2F vec) {
            Entity.Shape.AsDynamicShape().Direction = vec;
        }


        public void Move() {
            DynamicShape shape = Entity.Shape.AsDynamicShape();
            float x = shape.Position.X + shape.Direction.X;
            float y = shape.Position.Y + shape.Direction.Y;
            if (x > 0 && x < 1) {
                if (x + shape.Extent.X < 1 && y + shape.Extent.Y < 1) Entity.Shape.Move();
            }
            Console.WriteLine(shape.Position);
        }
    }
}