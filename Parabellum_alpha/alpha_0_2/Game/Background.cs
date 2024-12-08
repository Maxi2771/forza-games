using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace alpha_0_2.Game
{
    public class Background
    {
        Texture2D _backgroundTexture;
        Vector2 position;
        int scrollSpeed = 2;
        GraphicsDevice _graphicsDevice;

        public Background(Texture2D _backgroundTexture, GraphicsDevice graphicsDevice)
        {
            this._backgroundTexture = _backgroundTexture;
            position = new Vector2(0, 0);
            _graphicsDevice = graphicsDevice;
        }

        public void Update(GameTime gameTime)
        {
            position.X -= scrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (position.X <= -_backgroundTexture.Width)
            {
                position.X = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw
            (
                _backgroundTexture,
                new Rectangle(0, 0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height),
                Color.White
            );
        }
    }
}
