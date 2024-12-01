using alpha_0_2.Game.Controls;
using alpha_0_2.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alpha_0_2.Game.States
{
    public class MenuState : State
    {
        private List<Component> _components;
        private GraphicsDeviceManager _graphicsManager;
        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphicsManager) : base(game, graphicsDevice, content)
        {
            _graphicsManager = graphicsManager;
            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var buttonFont = _content.Load<SpriteFont>("Fonts/Font");

            var newGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((_graphicsManager.PreferredBackBufferWidth / 2) - buttonTexture.Width / 2, (_graphicsManager.PreferredBackBufferHeight / 2) - 300),
                Text = "New Game",
            };

            newGameButton.Click += NewGameButton_Click;

            var loadGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((_graphicsManager.PreferredBackBufferWidth / 2) - buttonTexture.Width / 2, (_graphicsManager.PreferredBackBufferHeight / 2) - 200),
                Text = "Load Game",
            };

            loadGameButton.Click += LoadGameButton_Click;

            var quitGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((_graphicsManager.PreferredBackBufferWidth / 2) - buttonTexture.Width / 2, (_graphicsManager.PreferredBackBufferHeight / 2) - 100),
                Text = "Quit",
            };

            quitGameButton.Click += QuitGameButton_Click;

            _components = new List<Component>()
            {
                newGameButton,
                loadGameButton,
                quitGameButton,
            };
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }

        private void LoadGameButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Load Game");
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            foreach(var component in _components)
                component.Draw(gameTime, spriteBatch);

            
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // remove sprites if they're not needed
        }

        public override void Update(GameTime gameTime)
        {
            foreach(var component in _components)
                component.Update(gameTime);
        }
    }
}
