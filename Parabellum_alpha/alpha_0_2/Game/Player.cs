using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace alpha_0_2.Game
{
    public class Player
    {
        private Texture2D _texture;
        private Vector2 _position;
        private float _speed;

        public Player(Texture2D texture, Vector2 position, float speed)
        {
            _texture = texture;
            _position = position;
            _speed = speed;
        }

        public void Update(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Left))
            {
                _position.X -= _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (kstate.IsKeyDown(Keys.Right))
            {
                _position.X += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (kstate.IsKeyDown(Keys.Up))
            {
                _position.Y -= _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (kstate.IsKeyDown(Keys.Down))
            {
                _position.Y += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
        }
    }
}
