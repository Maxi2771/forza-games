using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alpha_0_2.Game
{
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
