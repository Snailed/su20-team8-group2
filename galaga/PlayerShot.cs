using System;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using Microsoft.VisualBasic;
using DIKUArcade.Math;

namespace galaga
{
    public class PlayerShot : Entity
    {
        public PlayerShot(DynamicShape shape, IBaseImage image) : base(shape, image)
        {
        }
    }
}