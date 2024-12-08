using alpha_0_2.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alpha_0_2.Game
{
    public class Turret : Sprite
    {
        Texture2D textureRight;
        Texture2D textureLeft;
        Vector2 position;
        Vector2 _direction;
        Direction facingDirection = Game.Direction.Left;
        Texture2D currentTexture;
        bool isDestroyed = false;
        List<Bullet> disparadas = new List<Bullet>();
        Texture2D bulletTexture;
        float shootTimer = 0f;
        float shootInterval = 1f;

        public List<Bullet> Disparadas
        {
            get { return disparadas; }
            set { disparadas = value; }
        }

        public Turret(Texture2D textureRight, Texture2D textureLeft, Vector2 position, Texture2D bulletTexture) : base(textureRight)
        {
            this.textureRight = textureRight;
            this.textureLeft = textureLeft;
            this.position = position;
            this.bulletTexture = bulletTexture;
            _direction = new Vector2(-1, 0);
            currentTexture = textureLeft;
        }

        public void UpdateBullets(GameTime gameTime)
        {
            foreach (var bullet in disparadas.ToList())
            {
                bullet.Update(gameTime);
                if (bullet.LifeSpan <= 0)
                {
                    disparadas.Remove(bullet);
                }
            }
        }


        private void AimPlayer(Vector2 playerPosition, GameTime gameTime)
        {
            _direction = playerPosition - position;
            _direction.Normalize();

            Vector2 turretTipPosition = position;
            turretTipPosition.Y -= currentTexture.Height;
            if (_direction.X < 0)
            {
                facingDirection = Game.Direction.Left;
                currentTexture = textureLeft;
                turretTipPosition.X -= currentTexture.Width;
            }
            else
            {
                facingDirection = Game.Direction.Right;
                currentTexture = textureRight;
                turretTipPosition.X += currentTexture.Width;
            }

            var bullet = new Bullet(bulletTexture, position, true);

            bullet.Direction = this.Direction;
            bullet.Position = turretTipPosition;
            bullet.LinearVelocity = this.LinearVelocity * 2;
            bullet.LifeSpan = 2f;
            bullet.Parent = this;

            disparadas.Add(bullet);
        }

        public void Update(GameTime gameTime, Vector2 playerPosition)
        {
            // Actualizar la posición de las balas
            UpdateBullets(gameTime);

            // Controlar la dirección hacia el jugador
            AimPlayer(playerPosition, gameTime);

            Position += Direction * LinearVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            LifeSpan -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Reducir el temporizador
            shootTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Disparar si el temporizador ha llegado a 0
            if (shootTimer <= 0f)
            {
                Fire(playerPosition);
                shootTimer = shootInterval; // Reiniciar el temporizador
            }
        }

        private void Fire(Vector2 playerPosition)
        {
            Vector2 turretTipPosition = position;

            // Ajustar la posición de la punta según la dirección de disparo
            turretTipPosition.Y += 40; // Ajuste vertical
            if (facingDirection == Game.Direction.Left)
            {
                turretTipPosition.X -= 30; // Ajuste hacia la izquierda
            }
            else
            {
                turretTipPosition.X += 300; // Ajuste hacia la derecha
            }

            // Crear una bala
            var bullet = new Bullet(bulletTexture, turretTipPosition, true)
            {
                Direction = _direction,
                LinearVelocity = 4f,
                LifeSpan = 4f,
                Parent = this
            };

            // Añadir a la lista de balas disparadas
            disparadas.Add(bullet);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!isDestroyed)
            {
                spriteBatch.Draw(currentTexture, position, Color.White);
                foreach(Bullet bullet in disparadas)
                {
                    bullet.Draw(spriteBatch);
                }
            }
        }
    }
}
