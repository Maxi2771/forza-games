using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alpha_0_2.Game
{
    public class Camera
    {
        public Matrix Transform {  get; private set; }

        public void Follow(Sprite target)
        {
            Transform = Matrix.CreateTranslation(-target.Position.X -(target.Rectangle.Width / 2), -target.Position.Y -(target.Rectangle.Height/2), 0);
        }
    }
}
