using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace galaga
{
    public class Enemy : Entity
    {
        public Vec2F startPos;
        public Enemy(DynamicShape shape, IBaseImage image)
            : base(shape, image)
        {
            startPos = shape.Position;
        }
    }
}