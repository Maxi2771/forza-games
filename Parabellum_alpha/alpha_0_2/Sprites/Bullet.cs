using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace alpha_0_2.Sprites
{
    public class Bullet : Sprite
    {
        private float _timer;
        private Texture2D _bulletTexture;
        private int ammo = 15;
        private GraphicsDevice _graphicsDevice;

        public Bullet(Texture2D texture)
            : base(texture)
        {
            _bulletTexture = texture;
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer >= LifeSpan)
                IsRemoved = true;

            Position += Direction * LinearVelocity;

            if (_currentKey.IsKeyDown(Keys.Space) &&
            _previousKey.IsKeyUp(Keys.Space) &&
            ammo > 0) // Verificar que haya munición
            {
                AddBullet(_bulletTexture);
            }
        }
        private void AddBullet(Texture2D texture)
        {
            //var bullet = Bullet.Clone() as Bullet;
            var bullet = new Bullet(texture);

            Vector2 weaponTipPosition = this.Position;

            // Ajustar la posición de la punta del arma según la dirección en la que está mirando
            if (Direction == new Vector2(1, 0)) // Derecha
            {
                weaponTipPosition.X += _texture.Width; // Suma el ancho del arma
            }
            else if (Direction == new Vector2(-1, 0)) // Izquierda
            {
                weaponTipPosition.X -= 1; // Resta el ancho del arma
            }
            /*else if (Direction == new Vector2(0, -1)) // Arriba
            {
                weaponTipPosition.Y -= _texture.Height; // Resta el alto del arma
            }
            else if (Direction == new Vector2(0, 1)) // Abajo
            {
                weaponTipPosition.Y += _texture.Height; // Suma el alto del arma
            }*/

            // Configuramos la dirección y posición inicial de la bala
            bullet.Direction = this.Direction; // Usamos la dirección del arma
            bullet.Position = weaponTipPosition; // Posición inicial desde la punta del arma
            bullet.LinearVelocity = this.LinearVelocity * 2; // Velocidad de la bala (ajusta según sea necesario)
            bullet.LifeSpan = 2f; // Tiempo de vida de la bala
            bullet.Parent = this;

            ammo--; // Resta la munición
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            spriteBatch.Draw(_bulletTexture, Position, null, Color.White, _rotation, Origin, 1, SpriteEffects.None, 0);
        }
    }
}
