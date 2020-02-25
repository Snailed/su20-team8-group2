using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace galaga {
    public class Player : IGameEventProcessor<object> {
        private IBaseImage image;
        private DynamicShape shape;
        public Entity Entity { get; private set;}

        public Player(DynamicShape shape, IBaseImage image) {
            this.image = image;
            this.shape = shape;
            Entity = new Entity(shape, image);
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            throw new System.NotImplementedException();
        }

        public void Direction(Vec2F vec) {
            Entity.Shape.AsDynamicShape().Direction = vec;
        }

        public void Move() {
            DynamicShape shape = Entity.Shape.AsDynamicShape();
            float x = shape.Position.X + shape.Direction.X;
            float y = shape.Position.Y + shape.Direction.Y;
            if (x > 0 && x < 1 && y > 0 && y < 1) Entity.Shape.Move();
        }
    }
}