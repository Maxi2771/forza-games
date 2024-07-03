using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace alpha_0_2.Game
{
    public class Weapon
    {
        private Texture2D texture;
        private Vector2 position;
        private Direction facingDirection;
        private List<Projectile> projectiles;
        private float fireRate;
        private float fireTimer;

        public Weapon(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
            this.facingDirection = Direction.Right;
            this.projectiles = new List<Projectile>();
            this.fireRate = 0.5f; // Un disparo cada 0.5 segundos
            this.fireTimer = 0f;
        }

        public void Update(GameTime gameTime, Direction facingDirection)
        {
            this.facingDirection = facingDirection;
            fireTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && fireTimer >= fireRate)
            {
                Fire();
                fireTimer = 0f;
            }

            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                projectiles[i].Update(gameTime);
                if (!projectiles[i].IsActive)
                {
                    projectiles.RemoveAt(i);
                }
            }
        }

        private void Fire()
        {
            Vector2 projectilePosition = position;
            switch (facingDirection)
            {
                case Direction.Up:
                    projectilePosition.Y -= texture.Height;
                    break;
                case Direction.Down:
                    projectilePosition.Y += texture.Height;
                    break;
                case Direction.Left:
                    projectilePosition.X -= texture.Width;
                    break;
                case Direction.Right:
                    projectilePosition.X += texture.Width;
                    break;
            }

            projectiles.Add(new Projectile(texture, projectilePosition, facingDirection));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var projectile in projectiles)
            {
                projectile.Draw(spriteBatch);
            }
        }
    }

    public class Projectile
    {
        private Texture2D texture;
        private Vector2 position;
        private Vector2 velocity;
        private float speed = 10f;
        public bool IsActive { get; private set; }

        public Projectile(Texture2D texture, Vector2 position, Direction direction)
        {
            this.texture = texture;
            this.position = position;
            this.IsActive = true;

            switch (direction)
            {
                case Direction.Up:
                    velocity = new Vector2(0, -speed);
                    break;
                case Direction.Down:
                    velocity = new Vector2(0, speed);
                    break;
                case Direction.Left:
                    velocity = new Vector2(-speed, 0);
                    break;
                case Direction.Right:
                    velocity = new Vector2(speed, 0);
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            position += velocity;

            if (position.X < 0 || position.X > 800 || position.Y < 0 || position.Y > 600) // Suponiendo una pantalla de 800x600
            {
                IsActive = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
