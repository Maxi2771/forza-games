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
    public class GameStateP : State
    {
        private List<Component> _components = new List<Component>();
        private List<Component> _componentsWin = new List<Component>();
        private Enemy _enemy;
        private List<Enemy> enemies = new List<Enemy>();
        private List<Turret> turrets = new List<Turret>();
        private List<Sprite> _sprites;
        private int _lives = 3;
        private bool _keyPreviouslyPressed = false;
        private List<Bullet> Cargador = new List<Bullet>();
        private int enemiesLeft;
        private int enemiesKilled;
        private SpriteFont font;
        private Random random;
        private bool gameOver = false;
        private bool gameWon = false;

        private float _velocidad = 2.4f;
        private const float _limiteIzquierdo = 0;
        private const float _limiteDerecho = 1920;
        float timer = 0f;
        float timerExplosion = 0f;
        Texture2D enemyRight;
        Texture2D enemyLeft;
        Texture2D[] enemyTextures;
        Texture2D bulletTexture;
        Texture2D rocketTexture;
        Texture2D explosionTexture;
        Texture2D textureRight;
        Texture2D textureLeft;
        Turret _turret;
        bool drawExplosion = false;
        Vector2 pos;
        Background _background;
        int _currentRound;
        bool isRound = false;
        Vector2 playerPosition1;
        int playerWidth1;
        Vector2 playerPosition2;
        int playerWidth2;
        Player _player1;
        Player2 _player2;

        public GameStateP(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            Texture2D[] playerTextures = new Texture2D[]
            {
                content.Load<Texture2D>("Right"),
                content.Load<Texture2D>("Left"),
            };

            enemyTextures = new Texture2D[]
            {
                enemyRight = content.Load<Texture2D>("enemyRight"),
                enemyLeft = content.Load<Texture2D>("enemyLeft"),
            };

            var turretLeft = content.Load<Texture2D>("turretLeft");
            var turretRight = content.Load<Texture2D>("turretRight");

            textureRight = content.Load<Texture2D>("weaponRight");
            textureLeft = content.Load<Texture2D>("weaponLeft");
            bulletTexture = content.Load<Texture2D>("Bullet");
            rocketTexture = content.Load<Texture2D>("rocket");
            explosionTexture = content.Load<Texture2D>("explosion");

            _player1 = new Player(playerTextures, new Vector2(400, 875), textureRight, textureLeft, bulletTexture, Cargador);
            _player2 = new Player2(playerTextures, new Vector2(200, 875), textureRight, textureLeft, bulletTexture, Cargador);

            random = new Random();

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

            var nextLevel = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((_game._graphics.PreferredBackBufferWidth / 2) - buttonTexture.Width / 2, (_game._graphics.PreferredBackBufferHeight / 2) - 200),
                Text = "Next Level",
            };

            nextLevel.Click += NextLevelButton_Click;

            _componentsWin = new List<Component>()
            {
                nextLevel,
            };

            Round1();
        }

        private void ReturnMenuButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content, this._game._graphics));
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameStateP(_game, _graphicsDevice, _content));
        }

        private void NextLevelButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState2(_game, _graphicsDevice, _content));
        }

        public override void Update(GameTime gameTime)
        {
            if (!gameOver)
            {
                _player1.Update(gameTime, _sprites);
                _player2.Update(gameTime, _sprites);

                LimitPlayerPosition();

                if (drawExplosion)
                {
                    timerExplosion += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (timerExplosion >= 1f)
                    {
                        drawExplosion = false;
                        timerExplosion = 0f;
                    }
                }

                enemiesLeft = enemies.Count;

                if (gameWon)
                {
                    timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    switch (_currentRound)
                    {
                        case 1:
                            Round2();
                            break;
                        case 2:
                            Round3();
                            break;
                    }
                    foreach(Component component in _componentsWin)
                    {
                        component.Update(gameTime);
                    }
                }

                foreach (Enemy e in enemies)
                {
                    e.Update(gameTime, _player1.Position);
                    e.Update(gameTime, _player2.Position);
                }

                _background.Update(gameTime);
                CheckCollisionPlayer();
                CheckCollisionEnemy();
            }
            else
            {
                foreach (var component in _components)
                    component.Update(gameTime);
            }
        }

        private void Round1()
        {
            gameWon = false;
            _currentRound = 1;
            enemiesLeft = 2;
            for (int i = 0; i < enemiesLeft; i++)
            {
                enemies.Add(new Enemy(enemyTextures, new Vector2(_player1.Position.X + random.Next(300, 900), 875), textureRight, textureLeft, bulletTexture));
                enemies.Add(new Enemy(enemyTextures, new Vector2(_player2.Position.X + random.Next(300, 900), 875), textureRight, textureLeft, bulletTexture));
                timer = 0;
            }
        }

        private void Round2()
        {
            gameWon = false;
            _currentRound = 2;
            enemiesLeft = 4;
            for (int i = 0; i < enemiesLeft; i++)
            {
                enemies.Add(new Enemy(enemyTextures, new Vector2(_player1.Position.X + random.Next(300, 900), 875), textureRight, textureLeft, bulletTexture));
                enemies.Add(new Enemy(enemyTextures, new Vector2(_player2.Position.X + random.Next(300, 900), 875), textureRight, textureLeft, bulletTexture));
                timer = 0;
            }
        }

        private void Round3()
        {
            gameWon = false;
            _currentRound = 3;
            enemiesLeft = 6;
            for (int i = 0; i < enemiesLeft; i++)
            {
                enemies.Add(new Enemy(enemyTextures, new Vector2(_player1.Position.X + random.Next(300, 900), 875), textureRight, textureLeft, bulletTexture));
                enemies.Add(new Enemy(enemyTextures, new Vector2(_player2.Position.X + random.Next(300, 900), 875), textureRight, textureLeft, bulletTexture));
                timer = 0;
            }
        }

        public void CheckCollisionPlayer()
        {
            for (int e = enemies.Count - 1; e >= 0; e--)
            {
                var enemy = enemies[e];
                for (int i = enemy.Weapon.Disparadas.Count - 1; i >= 0; i--)
                {
                    try
                    {
                        if (!_player1.IsDodging) // Si no lo esquivo pierdo vida
                        {
                            enemy.Weapon.Disparadas.RemoveAt(i);
                            ReduceHealthPlayer1();
                        }
                        if (!_player2.IsDodging) // Si no lo esquivo pierdo vida
                        {
                            enemy.Weapon.Disparadas.RemoveAt(i);
                            ReduceHealthPlayer2();
                        }
                        else
                        {
                            enemy.Weapon.Disparadas.RemoveAt(i);
                        }
                    }
                    catch { }
                }
            }
        }

        public void CheckCollisionEnemy()
        {
            for (int i = _player1.Weapon.Disparadas.Count - 1; i >= 0; i--)
            {
                var bullet = _player1.Weapon.Disparadas[i];
                for (int e = enemies.Count - 1; e >= 0; e--)
                {
                    var enemy = enemies[e];
                    if (bullet.CollisionRectangle.Intersects(enemy.CollisionRectangle) && enemy.IsAlive)
                    {
                        _player1.Weapon.Disparadas.RemoveAt(i);
                        enemy.IsAlive = false;
                        RemoveEnemies();
                        gameWon = (enemies.Count == 0);
                        break;
                    }
                }
            }
            for (int i = _player2.Weapon.Disparadas.Count - 1; i >= 0; i--)
            {
                var bullet = _player2.Weapon.Disparadas[i];
                for (int e = enemies.Count - 1; e >= 0; e--)
                {
                    var enemy = enemies[e];
                    if (bullet.CollisionRectangle.Intersects(enemy.CollisionRectangle) && enemy.IsAlive)
                    {
                        _player2.Weapon.Disparadas.RemoveAt(i);
                        enemy.IsAlive = false;
                        RemoveEnemies();
                        gameWon = (enemies.Count == 0);
                        break;
                    }
                }
            }
        }


        public void RemoveEnemies()
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (!enemies[i].IsAlive)
                {
                    enemiesKilled++;
                    enemies.RemoveAt(i);
                    i--;
                }
            }
        }

        public void ReduceHealthPlayer1()
        {
            _player1.Health--;
            if (_player1.Health <= 0)
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

        private void LimitPlayerPosition()
        {
            playerPosition1 = _player1.Position;
            playerWidth1 = _player1.Width;

            playerPosition2 = _player2.Position;
            playerWidth2 = _player2.Width;

            if (playerPosition1.X < 0)
            {
                playerPosition1.X = 0;
            }
            else if (playerPosition1.X > _limiteDerecho - playerWidth1)
            {
                playerPosition1.X = _limiteDerecho - playerWidth1;
            }

            if (playerPosition2.X < 0)
            {
                playerPosition2.X = 0;
            }
            else if (playerPosition2.X > _limiteDerecho - playerWidth2)
            {
                playerPosition2.X = _limiteDerecho - playerWidth2;
            }

            _player1.Position = playerPosition1;
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

            _player1.Draw(spriteBatch);
            _player2.Draw(spriteBatch);

            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(spriteBatch);
            }

            foreach (var sprite in _sprites)
            {
                sprite.Draw(spriteBatch);
            }

            if (drawExplosion)
            {
                spriteBatch.Draw(explosionTexture, pos, Color.White);
            }

            spriteBatch.DrawString(font, $"Enemies Left: {enemiesLeft}", new Vector2(10, 10), Color.Black);
            spriteBatch.DrawString(font, $"Enemies Killed: {enemiesKilled}", new Vector2(150, 10), Color.Black);
            spriteBatch.DrawString(font, $"Current Round: {_currentRound}", new Vector2(450, 10), Color.Black);

            spriteBatch.DrawString(font, $"Player 1 Ammo: {_player1.Weapon._Cargador.Count}", new Vector2(590, 10), Color.Black);
            spriteBatch.DrawString(font, $"Player 2 Ammo: {_player2.Weapon._Cargador.Count}", new Vector2(700, 10), Color.Black);

            spriteBatch.DrawString(font, $"Player 1 Health Points: {_player1.Health}", new Vector2(900, 10), Color.Black);
            spriteBatch.DrawString(font, $"Player 2 Health Points: {_player2.Health}", new Vector2(1100, 10), Color.Black);

            if (gameOver)
            {
                foreach (var component in _components)
                    component.Draw(gameTime, spriteBatch);
            }
            if (_currentRound == 3 && enemiesLeft == 0)
            {
                foreach (var component in _componentsWin)
                    component.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }
    }
}