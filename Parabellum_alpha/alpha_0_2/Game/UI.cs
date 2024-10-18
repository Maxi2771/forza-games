using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alpha_0_2.Game
{
    public class UI
    {
        private SpriteFont _font;
        private Vector2 _position;

        public UI(SpriteFont font, Vector2 position)
        {
            _font = font;
            _position = position;
        }

        public void Draw(SpriteBatch spriteBatch, int ammo)
        {
            string ammoText = $"Ammo: {ammo}";
            spriteBatch.DrawString(_font, ammoText, _position, Color.White);
        }
    }
}
