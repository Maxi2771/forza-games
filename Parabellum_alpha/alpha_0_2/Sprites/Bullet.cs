using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace alpha_0_2.Sprites
{
    public class Bullet : Sprite
    {
        private float _timer;
        private Texture2D _bulletTexture;
        private Vector2 weaponPosition;
        private Vector2 playerPosition;

        public Bullet(Texture2D texture, Vector2 weaponPosition, Vector2 playerPosition)
            : base(texture)
        {
            _bulletTexture = texture;
            this.weaponPosition = weaponPosition;
            this.playerPosition = playerPosition;
        }

        public override void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer >= LifeSpan)
                IsRemoved = true;

            Position += Direction * LinearVelocity;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_bulletTexture, playerPosition + weaponPosition + Position, Color.White);
        }
    }
}
