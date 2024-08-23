using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace alpha_0_2.Game
{
    public class Weapon
    {
        private Texture2D texture; // Guarda textura del arma
        private Vector2 position; // Posición del arma
        private Direction facingDirection; // Direción a la que apunta
        private List<Projectile> projectiles; // Lista de proyectiles
        private float fireRate; // Tasa de disparo
        private float fireTimer; // Temporizador del disparo

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
        // Método de disparar
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
            // Se añaden proyectiles para que se produzca el disparo
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
}
