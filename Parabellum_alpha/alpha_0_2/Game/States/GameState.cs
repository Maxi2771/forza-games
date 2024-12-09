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
    public class GameState : State
    {
        private List<Component> _components = new List<Component>();
        private List<Component> _componentsWin = new List<Component>();
        private Player _player;
        //private Player2 _player2;
        private Enemy _enemy;
        private List<Enemy> enemies = new List<Enemy>();
        private List<Turret> turrets = new List<Turret>();
        private List<Sprite> _sprites;
        private int _lives = 3;
        private bool _keyPreviouslyPressed = false;
        private List<Bullet> Cargador = new List<Bullet>();
        private int points = 0;
        private SpriteFont _scoreFont;
        private SpriteFont _healthFont;
        private Random random;
        private bool gameOver = false;
        private bool gameWon = false;

        // Variables para el fondo
        //private Texture2D _fondoTexture;
        /*private float _fondoPosX1;
        private float _fondoPosX2;*/
        private float _velocidad = 2.4f;
        private const float _limiteIzquierdo = 0;
        private const float _limiteDerecho = 1920;
        private Vector2 position;
        //private Vector2 position2;
        float timer = 0f;
        float timerExplosion = 0f;
        Texture2D enemyRight;
        Texture2D enemyLeft;
        Texture2D[] enemyTextures;
        Texture2D bulletTexture;
        Texture2D missileTexture;
        Texture2D explosionTexture;
        Texture2D textureRight;
        Texture2D textureLeft;
        Turret _turret;
        bool drawExplosion = false;
        Vector2 pos;
        Background _background;

        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
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
            missileTexture = content.Load<Texture2D>("misil");
            explosionTexture = content.Load<Texture2D>("explosion");

            _turret = new Turret(turretRight, turretLeft, new Vector2(1000, 700), missileTexture);

            position = new Vector2(400, 875);
            //position2 = new Vector2(200, 875);

            _player = new Player(playerTextures, position, textureRight, textureLeft, bulletTexture, Cargador);

            //_player2 = new Player2(playerTextures, position2, textureRight, textureLeft, bulletTexture, Cargador);
            /*_enemy = new Enemy(enemyTextures, new Vector2(900, 875), textureRight, textureLeft, bulletTexture);
            enemies.Add(_enemy);*/
            random = new Random();

            _sprites = new List<Sprite>();

            _scoreFont = content.Load<SpriteFont>("Fonts/Font");
            _healthFont = content.Load<SpriteFont>("Fonts/Font");

            // Cargar la textura del fondo
            var _fondoTexture = content.Load<Texture2D>("fondo");
            _background = new Background(_fondoTexture, graphicsDevice);

            /*_fondoPosX1 = 0;
            _fondoPosX2 = _fondoTexture.Width;*/

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
                menuButton,
            };
        }

        private void ReturnMenuButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content, this._game._graphics));
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
        }

        private void NextLevelButton_Click(object sender, EventArgs e)
        {
            //_game.ChangeLevel();
        }

        public override void Update(GameTime gameTime)
        {
            if (!gameWon)
            {
                if (!gameOver)
                {
                    _player.Update(gameTime, _sprites);

                    _turret.Update(gameTime, _player.Position);

                    LimitPlayerPosition();

                    foreach (var sprite in _sprites.ToArray())
                        sprite.Update(gameTime, _sprites);

                    timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (drawExplosion)
                    {
                        // Incrementar el timer basado en el tiempo transcurrido
                        timerExplosion += (float)gameTime.ElapsedGameTime.TotalSeconds;

                        // Cambiar el estado del booleano después de 3 segundos
                        if (timerExplosion >= 1f)
                        {
                            drawExplosion = false;
                            timerExplosion = 0f;
                        }
                    }

                    SpawnEnemies();

                    foreach (Enemy e in enemies)
                    {
                        e.Update(gameTime, position);
                    }

                    _background.Update(gameTime);
                    CheckCollisionPlayer();
                    CheckCollisionEnemy();
                    MissileCollisionPlayer();
                }
                else
                {
                    foreach (var component in _components)
                        component.Update(gameTime);
                }
            }
            else
            {
                foreach (var component in _componentsWin)
                    component.Update(gameTime);
            }
        }

        private void SpawnEnemies()
        {
            if (enemies.Count < 2 && timer > 1.0f)
            {
                enemies.Add(new Enemy(enemyTextures, new Vector2(_player.Position.X + 400, 875), textureRight, textureLeft, bulletTexture));
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
                    if (enemy.Weapon.Disparadas[i].CollisionRectangle.Intersects(_player.CollisionRectangle))
                    {
                        enemy.Weapon.Disparadas.RemoveAt(i);
                        ReduceHealth();
                    }
                }
            }
        }

        public void MissileCollisionPlayer()
        {
            /*for (int e = enemies.Count - 1; e >= 0; e--)
            {
                var enemy = enemies[e];
                for (int i = enemy.Weapon.Disparadas.Count - 1; i >= 0; i--)
                {
                    if (enemy.Weapon.Disparadas[i].CollisionRectangle.Intersects(_player.CollisionRectangle))
                    {
                        enemy.Weapon.Disparadas.RemoveAt(i);
                        ReduceHealth();
                    }
                }
            }*/
            for (int i = _turret.Disparadas.Count - 1; i >= 0; i--)
            {
                if (_turret.Disparadas[i].CollisionRectangle.Intersects(_player.CollisionRectangle))
                {
                    drawExplosion = true;
                    Vector2 offset = new Vector2(100, 80);
                    pos = _turret.Disparadas[i].PositionAir - offset;
                    _turret.Disparadas.RemoveAt(i);
                    ReduceHealthMissile();
                }
            }
        }
        
        public void CheckCollisionEnemy()
        {
            for (int i = _player.Weapon.Disparadas.Count - 1; i >= 0; i--)
            {
                var bullet = _player.Weapon.Disparadas[i];
                for (int e = enemies.Count - 1; e >= 0; e--)
                {
                    var enemy = enemies[e];
                    if (bullet.CollisionRectangle.Intersects(enemy.CollisionRectangle) && enemy.IsAlive)
                    {
                        points += 100;
                        _player.Weapon.Disparadas.RemoveAt(i);
                        enemy.IsAlive = false;
                        RemoveEnemies();
                        if (points == 500)
                            gameWon = true;
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
                    enemies.RemoveAt(i);
                    i--;
                }
            }
        }

        public void ReduceHealth()
        {
            _player.Health--;
            if (_player.Health <= 0)
            {
                gameOver = true;
            }
        }

        public void ReduceHealthMissile()
        {
            _player.Health -= 4;
            if (_player.Health <= 0)
            {
                gameOver = true;
            }
        }

        private void LimitPlayerPosition()
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

        /*private void UpdateBackground()
        {
            var state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Left))
            {
                if (_fondoPosX1 < _limiteIzquierdo)
                {
                    _fondoPosX1 += _velocidad;
                    _fondoPosX2 += _velocidad;
                }
            }
            else if (state.IsKeyDown(Keys.Right))
            {
                _fondoPosX1 -= _velocidad;
                _fondoPosX2 -= _velocidad;
            }

            if (_fondoPosX1 <= -_fondoTexture.Width)
                _fondoPosX1 = _fondoPosX2 + _fondoTexture.Width;

            if (_fondoPosX2 <= -_fondoTexture.Width)
                _fondoPosX2 = _fondoPosX1 + _fondoTexture.Width;
        }*/


        public override void PostUpdate(GameTime gameTime)
        {

        }

        /*public override void PostUpdate(GameTime gameTime)
        {
            for (int i = 0; i < _sprites.Count; i++)
            {
                if (_sprites[i].IsRemoved)
                {
                    _sprites.RemoveAt(i);
                    i--;
                }
            }
        }*/

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.CornflowerBlue);

            //spriteBatch.Draw(_fondoTexture, new Rectangle((int)_fondoPosX1, 0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height), Color.White);
            //spriteBatch.Draw(_fondoTexture, new Rectangle((int)_fondoPosX2, 0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height), Color.White);
            spriteBatch.End();

            spriteBatch.Begin();

            _background.Draw(spriteBatch);
            _player.Draw(spriteBatch);
            _turret.Draw(spriteBatch);
            //_player2.Draw(spriteBatch);

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

            spriteBatch.DrawString(_scoreFont, $"Score: {points}", new Vector2(10, 10), Color.Black);
            spriteBatch.DrawString(_healthFont, $"Health Points: {_player.Health}", new Vector2(100, 10), Color.Black);

            if (gameOver)
            {
                foreach (var component in _components)
                    component.Draw(gameTime, spriteBatch);
            }
            else if (gameWon)
            {
                foreach(var component in _componentsWin)
                    component.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }
    }
}