using alpha_0_2.Game;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace alpha_0_2.Sprites
{
    public class Weapon : Sprite
    {
        private List<Bullet> Cargador = new List<Bullet>();
        private List<Bullet> disparadas = new List<Bullet>();
        private Texture2D texture;
        private Texture2D textureRight;
        private Texture2D textureLeft;
        private Texture2D bulletTexture;
        private Vector2 playerPosition;
        private int ammo = 15;


        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public Texture2D TextureRight
        {
            get { return textureRight; }
            set { textureRight = value; }
        }
        public Texture2D TextureLeft
        {
            get { return textureLeft; }
            set { textureLeft = value; }
        }

        public List<Bullet> Disparadas
        {
            get { return disparadas; }
            set { disparadas = value; }
        }

        public Vector2 PlayerPosition
        {
            get { return playerPosition; }
            set { playerPosition = value; }
        }

        public Weapon(Texture2D textureRight, Texture2D textureLeft, Texture2D bulletTexture, List<Bullet> Cargador, Vector2 playerPosition)
            : base(textureRight) // Comenzamos con la textura de la derecha por defecto
        {
            this.textureRight = textureRight;
            this.textureLeft = textureLeft;
            texture = textureRight;
            Position = new Vector2(-22, 14);
            this.Cargador = Cargador;
            this.bulletTexture = bulletTexture;
            this.playerPosition = playerPosition;

            // Dirección predeterminada (hacia la derecha)
            Direction = new Vector2(1, 0);
            for (int i = 0; i < ammo; i++)
            {
                Cargador.Add(new Bullet(bulletTexture, Position, playerPosition));
            }
        }

        public override void Update(GameTime gameTime)
        {
            _previousKey = _currentKey;
            _currentKey = Keyboard.GetState();
            if (_currentKey.IsKeyDown(Keys.Space) && _previousKey.IsKeyUp(Keys.Space) && Cargador.Count > 0)
            {
                ShootBullet();
            }
            foreach (Bullet b in disparadas)
            {
                b.Update(gameTime);
            }
        }

        public void ShootBullet()
        {
            var bullet = new Bullet(bulletTexture, Position, playerPosition);

            Vector2 weaponTipPosition = this.Position;
            int offset = 160;

            if (Direction == new Vector2(1, 0)) // Derecha
            {
                weaponTipPosition.X += _texture.Width; // Suma el ancho del arma
            }
            else if (Direction == new Vector2(-1, 0)) // Izquierda
            {
                weaponTipPosition.X -= _texture.Width - offset; // Resta el ancho del arma
            }

            bullet.Direction = this.Direction;
            bullet.Position = weaponTipPosition;
            bullet.LinearVelocity = this.LinearVelocity * 2;
            bullet.LifeSpan = 2f;
            bullet.Parent = this;

            disparadas.Add(bullet);
            Cargador.RemoveAt(Cargador.Count - 1);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, playerPosition + Position, Color.White);
            foreach (Bullet bullet in disparadas)
            {
                bullet.Draw(spriteBatch);
            }
        }
    }
}
