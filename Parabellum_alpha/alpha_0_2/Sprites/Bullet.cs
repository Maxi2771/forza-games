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
        private Vector2 entityPosition;
        private bool isTurret = false;
        private Vector2 positionAir;

        public Rectangle CollisionRectangle
        {
            get
            {
                return new Rectangle(
                    (int)(entityPosition.X + weaponPosition.X + Position.X),
                    (int)(entityPosition.Y + weaponPosition.Y + Position.Y),
                    _bulletTexture.Width,
                    _bulletTexture.Height
                );
            }
        }

        public Vector2 PositionAir
        {
            get { return positionAir; }
            set { positionAir = value; }
        }

        public Bullet(Texture2D texture, Vector2 weaponPosition, Vector2 entityPosition)
            : base(texture)
        {
            _bulletTexture = texture;
            this.weaponPosition = weaponPosition;
            this.entityPosition = entityPosition;
        }

        public Bullet(Texture2D texture, Vector2 entityPosition, bool isTurret)
        : base(texture)
        {
            _bulletTexture = texture;
            this.entityPosition = entityPosition;
            this.isTurret = isTurret;
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
            if (!IsRemoved)
            {
                if (!isTurret)
                {
                    positionAir = entityPosition + weaponPosition + Position;
                    spriteBatch.Draw(_bulletTexture, positionAir, Color.Red * 0.6f);
                }
                else
                {
                    positionAir = entityPosition + Position;
                    spriteBatch.Draw(_bulletTexture, positionAir, Color.White);
                }
            }
        }
    }
}
