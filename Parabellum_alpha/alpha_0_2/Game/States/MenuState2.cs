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
    public class MenuState2 : State
    {
        private List<Component> _components;
        private GraphicsDeviceManager _graphicsManager;
        public MenuState2(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphicsManager) : base(game, graphicsDevice, content)
        {
            _graphicsManager = graphicsManager;
            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var buttonFont = _content.Load<SpriteFont>("Fonts/Font");

            var levelOneButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((_graphicsManager.PreferredBackBufferWidth / 2) - buttonTexture.Width / 2, (_graphicsManager.PreferredBackBufferHeight / 2) - 300),
                Text = "Level 1",
            };

            levelOneButton.Click += LevelOneButton_Click;

            var levelTwoButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((_graphicsManager.PreferredBackBufferWidth / 2) - buttonTexture.Width / 2, (_graphicsManager.PreferredBackBufferHeight / 2) - 200),
                Text = "Level 2",
            };

            levelTwoButton.Click += LevelTwoButton_Click;

            var levelThreeButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((_graphicsManager.PreferredBackBufferWidth / 2) - buttonTexture.Width / 2, (_graphicsManager.PreferredBackBufferHeight / 2) - 100),
                Text = "Level 3",
            };

            levelThreeButton.Click += LevelThreeButton_Click;

            /*var backButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((_graphicsManager.PreferredBackBufferWidth / 2) - buttonTexture.Width / 2, (_graphicsManager.PreferredBackBufferHeight / 2) - 100),
                Text = "Back",
            };*/

            //backButton.Click += BackButton_Click;

            _components = new List<Component>()
            {
                levelOneButton,
                levelTwoButton,
                levelThreeButton,
                //backButton,
            };
        }

        /*private void BackButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }*/

        private void LevelThreeButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState3(_game, _graphicsDevice, _content));
        }

        private void LevelTwoButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState2(_game, _graphicsDevice, _content));
        }

        private void LevelOneButton_Click(object sender, EventArgs e)
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
        }

        public override void Update(GameTime gameTime)
        {
            foreach(var component in _components)
                component.Update(gameTime);
        }
    }
}
