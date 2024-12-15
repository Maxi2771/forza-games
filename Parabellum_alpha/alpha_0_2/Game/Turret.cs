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
        List<Bullet> disparadas = new List<Bullet>();
        Texture2D bulletTexture;
        float shootTimer = 0f;
        float shootInterval = 3f;
        int health = 3;
        bool _isAlive = true;
        public Rectangle CollisionRectangle
        {
            get
            {
                return new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    currentTexture.Width,
                    currentTexture.Height
                );
            }
        }

        public bool IsAlive
        {
            get { return _isAlive; }
            set { _isAlive = value; }
        }

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

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
            if (!IsAlive) return;

            UpdateBullets(gameTime);

            AimPlayer(playerPosition, gameTime);

            Position += Direction * LinearVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            LifeSpan -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            shootTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(health <= 0)
            {
                _isAlive = false;
            }

            if (shootTimer <= 0f)
            {
                Fire(playerPosition);
                shootTimer = shootInterval;
            }
        }

        private void Fire(Vector2 playerPosition)
        {
            Vector2 turretTipPosition = position;

            turretTipPosition.Y += 40;
            if (facingDirection == Game.Direction.Left)
            {
                turretTipPosition.X -= 30;
            }
            else
            {
                turretTipPosition.X += 300;
            }

            var bullet = new Bullet(bulletTexture, turretTipPosition, true)
            {
                Direction = _direction,
                LinearVelocity = 4f,
                LifeSpan = 4f,
                Parent = this
            };

            disparadas.Add(bullet);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_isAlive)
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
