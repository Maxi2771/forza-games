using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParaTesting
{
    public class Projectile
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public Texture2D Texture;
        public float speed;

        public Projectile(Texture2D texture, Vector2 startPosition, Vector2 direction)
        {
            Texture = texture;
            Position = startPosition;
            Velocity = direction * speed; // Velocidad del proyectil
        }

        public Projectile(Vector2 projectilePosition, Texture2D projectileTexture, Vector2 vector2)
        {
        }

        public void Update(GameTime gameTime)
        {
            Position += Velocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
