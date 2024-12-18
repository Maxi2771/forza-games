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
        private SpriteFont font;
        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphicsManager) : base(game, graphicsDevice, content)
        {
            _graphicsManager = graphicsManager;
            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            font = _content.Load<SpriteFont>("Fonts/Font");

            var startButton = new Button(buttonTexture, font)
            {
                Position = new Vector2((_graphicsManager.PreferredBackBufferWidth / 2) - buttonTexture.Width / 2, (_graphicsManager.PreferredBackBufferHeight / 2) - 300),
                Text = "Start",
            };

            startButton.Click += StartButton_Click;

            var pickLevelButton = new Button(buttonTexture, font)
            {
                Position = new Vector2((_graphicsManager.PreferredBackBufferWidth / 2) - buttonTexture.Width / 2, (_graphicsManager.PreferredBackBufferHeight / 2) - 200),
                Text = "Pick a level",
            };

            pickLevelButton.Click += PickLevelButton_Click;

            var TwoPlayerGameButton = new Button(buttonTexture, font)
            {
                Position = new Vector2((_graphicsManager.PreferredBackBufferWidth / 2) - buttonTexture.Width / 2, (_graphicsManager.PreferredBackBufferHeight / 2) - 100),
                Text = "2 Players (Experimental)",
            };

            TwoPlayerGameButton.Click += TwoPlayerGameButton_Click;

            var quitGameButton = new Button(buttonTexture, font)
            {
                Position = new Vector2((_graphicsManager.PreferredBackBufferWidth / 2) - buttonTexture.Width / 2, (_graphicsManager.PreferredBackBufferHeight / 2) - 0),
                Text = "Quit",
            };

            quitGameButton.Click += QuitGameButton_Click;

            _components = new List<Component>()
            {
                startButton,
                pickLevelButton,
                TwoPlayerGameButton,
                quitGameButton,
            };
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }

        private void TwoPlayerGameButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameStateP(_game, _graphicsDevice, _content));
        }

        private void PickLevelButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MenuState2(_game, _graphicsDevice, _content, _graphicsManager));
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            foreach(var component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.DrawString(font, "Parabellum", new Vector2(10, 10), Color.Black);

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
            foreach(var component in _components)
                component.Update(gameTime);
        }
    }
}
