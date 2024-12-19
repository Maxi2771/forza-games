using alpha_0_2.Game.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using alpha_0_2.Sprites;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System;

namespace alpha_0_2.Game.States
{
    public class GameState1v1 : State
    {
        private List<Component> _components = new List<Component>();
        private Player _player;
        private Player2 _player2;
        private List<Enemy> enemies = new List<Enemy>();
        private List<Sprite> _sprites;
        private List<Bullet> Cargador = new List<Bullet>();
        private SpriteFont font;
        private bool gameOver = false;

        private const float _limiteDerecho = 1920;
        private Vector2 position;
        private Vector2 position2;
        Texture2D bulletTexture;
        Texture2D explosionTexture;
        Texture2D textureRight;
        Texture2D textureLeft;
        bool drawExplosion = false;
        Vector2 pos;
        Background _background;

        public GameState1v1(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            Texture2D[] playerTextures = new Texture2D[]
            {
                content.Load<Texture2D>("Right"),
                content.Load<Texture2D>("Left"),
            };

            Texture2D[] player2Textures = new Texture2D[]
            {
                content.Load<Texture2D>("enemyRight"),
                content.Load<Texture2D>("enemyLeft"),
            };

            textureRight = content.Load<Texture2D>("weaponRight");
            textureLeft = content.Load<Texture2D>("weaponLeft");
            bulletTexture = content.Load<Texture2D>("Bullet");

            position = new Vector2(400, 875);
            position2 = new Vector2(800, 875);

            _player = new Player(playerTextures, position, textureRight, textureLeft, bulletTexture, Cargador);
            _player2 = new Player2(player2Textures, position2, textureRight, textureLeft, bulletTexture, Cargador);

            _player.Health = 20;
            _player2.Health = 20;

            _sprites = new List<Sprite>();

            font = content.Load<SpriteFont>("Fonts/Font");

            // Cargar la textura del fondo
            var _fondoTexture = content.Load<Texture2D>("fondo");
            _background = new Background(_fondoTexture, graphicsDevice);

            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var buttonFont = _content.Load<SpriteFont>("Fonts/Font");

            var restartButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((_game._graphics.PreferredBackBufferWidth / 2) - buttonTexture.Width / 2, (_game._graphics.PreferredBackBufferHeight / 2) - 200),
                Text = "Restart",
            };

            restartButton.Click += RestartButton_Click;

            var menuButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((_game._graphics.PreferredBackBufferWidth / 2) - buttonTexture.Width / 2, (_game._graphics.PreferredBackBufferHeight / 2) - 100),
                Text = "Return to Main Menu",
            };

            menuButton.Click += ReturnMenuButton_Click;

            _components = new List<Component>()
            {
                restartButton,
                menuButton,
            };
        }

        private void ReturnMenuButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content, this._game._graphics));
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState1v1(_game, _graphicsDevice, _content));
        }

        private void NextLevelButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState2(_game, _graphicsDevice, _content));
        }

        public override void Update(GameTime gameTime)
        {
            if (!gameOver)
            {
                _player.Update(gameTime, _sprites);
                _player2.Update(gameTime, _sprites);

                LimitPlayer1Position();
                LimitPlayer2Position();

                foreach (Enemy e in enemies)
                {
                    e.Update(gameTime, _player.Position);
                }

                _background.Update(gameTime);
                CheckCollisionPlayer1();
                CheckCollisionPlayer2();
            }
            else
            {
                foreach (var component in _components)
                    component.Update(gameTime);
            }
        }

        public void CheckCollisionPlayer1()
        {
            for(int i = _player2.Weapon.Disparadas.Count - 1; i >= 0; i--)
            {
                if (_player2.Weapon.Disparadas[i].CollisionRectangle.Intersects(_player.CollisionRectangle))
                {
                    _player2.Weapon.Disparadas.RemoveAt(i);
                    ReduceHealthPlayer1();
                }
            }
        }

        public void CheckCollisionPlayer2()
        {
            for (int i = _player.Weapon.Disparadas.Count - 1; i >= 0; i--)
            {
                if (_player.Weapon.Disparadas[i].CollisionRectangle.Intersects(_player2.CollisionRectangle))
                {
                    _player.Weapon.Disparadas.RemoveAt(i);
                    ReduceHealthPlayer2();
                }
            }
        }

        public void ReduceHealthPlayer1()
        {
            _player.Health--;
            if (_player.Health <= 0)
            {
                gameOver = true;
            }
        }

        public void ReduceHealthPlayer2()
        {
            _player2.Health--;
            if (_player2.Health <= 0)
            {
                gameOver = true;
            }
        }
        private void LimitPlayer1Position()
        {
            var playerPosition = _player.Position;
            var playerWidth = _player.Width;

            if (playerPosition.X < 0)
            {
                playerPosition.X = 0;
            }
            else if (playerPosition.X > _limiteDerecho - playerWidth)
            {
                playerPosition.X = _limiteDerecho - playerWidth;
            }

            _player.Position = playerPosition;
        }

        private void LimitPlayer2Position()
        {
            var playerPosition2 = _player2.Position;
            var playerWidth2 = _player2.Width;

            if (playerPosition2.X < 0)
            {
                playerPosition2.X = 0;
            }
            else if (playerPosition2.X > _limiteDerecho - playerWidth2)
            {
                playerPosition2.X = _limiteDerecho - playerWidth2;
            }

            _player2.Position = playerPosition2;
        }
        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.End();

            spriteBatch.Begin();

            _background.Draw(spriteBatch);
            _player.Draw(spriteBatch);
            _player2.Draw(spriteBatch);

            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(spriteBatch);
            }

            if (drawExplosion)
            {
                spriteBatch.Draw(explosionTexture, pos, Color.White);
            }

            spriteBatch.DrawString(font, $"Player 1 Health Points: {_player.Health}", new Vector2(100, 10), Color.Black);

            spriteBatch.DrawString(font, $"Player 2 Health Points: {_player2.Health}", new Vector2(500, 10), Color.Black);

            if (gameOver)
            {
                foreach (var component in _components)
                    component.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }
    }
}