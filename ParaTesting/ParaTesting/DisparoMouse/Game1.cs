using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ParaTesting.Sprites;
using System.Collections.Generic;

namespace ParaTesting
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private List<Sprite> _sprites;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var weaponTexture = Content.Load<Texture2D>("Weapon");
            _sprites = new List<Sprite>()
            {
                new Weapon(weaponTexture)
                {
                    Position = new Vector2(100, 100),
                    Bullet = new Bullet(Content.Load<Texture2D>("Bullet")),
                }
            };
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (var sprite in _sprites.ToArray())
                sprite.Update(gameTime, _sprites);

            PostUpdate();

            base.Update(gameTime);
        }

        private void PostUpdate()
        {
            for (int i = 0; i < _sprites.Count; i++)
            {
                if (_sprites[i].IsRemoved)
                {
                    _sprites.RemoveAt(i);
                    i--;
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            foreach (var sprite in _sprites)
                sprite.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
