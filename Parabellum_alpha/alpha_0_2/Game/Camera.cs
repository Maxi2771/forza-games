using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using alpha_0_2.Sprites;

namespace alpha_0_2.Game
{
    public class Camera
    {
        public Matrix Transform {  get; private set; }

        public void Follow(Rectangle target)
        {
            Transform = Matrix.CreateTranslation(-target.X -(target.Width / 2), -target.Y - (target.Height / 2), 0);
        }
    }
}
