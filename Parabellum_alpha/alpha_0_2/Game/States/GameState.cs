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
        private Player _player;
        private Enemy _enemy;
        private List<Enemy> enemies = new List<Enemy>();
        private List<Sprite> _sprites;
        private int _lives = 3;
        private bool _keyPreviouslyPressed = false;
        private List<Bullet> Cargador = new List<Bullet>();
        private int points = 0;
        private SpriteFont _scoreFont;
        private SpriteFont _healthFont;
        private Random random;

        // Variables para el fondo
        private Texture2D _fondoTexture;
        private float _fondoPosX1;
        private float _fondoPosX2;
        private float _velocidad = 2.4f;
        private const float _limiteIzquierdo = 0;
        private const float _limiteDerecho = 1920;
        private Vector2 position;
        float timer = 0f;
        Texture2D enemyRight;
        Texture2D enemyLeft;
        Texture2D[] enemyTextures;
        Texture2D bulletTexture;
        Texture2D textureRight;
        Texture2D textureLeft;
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

            textureRight = content.Load<Texture2D>("weaponRight");
            textureLeft = content.Load<Texture2D>("weaponLeft");
            bulletTexture = content.Load<Texture2D>("Bullet");

            position = new Vector2(400, 875);

            _player = new Player(playerTextures, position, textureRight, textureLeft, bulletTexture, Cargador);
            /*_enemy = new Enemy(enemyTextures, new Vector2(900, 875), textureRight, textureLeft, bulletTexture);
            enemies.Add(_enemy);*/
            random = new Random();

            _sprites = new List<Sprite>();

            _scoreFont = content.Load<SpriteFont>("Fonts/Font");
            _healthFont = content.Load<SpriteFont>("Fonts/Font");

            // Cargar la textura del fondo
            _fondoTexture = content.Load<Texture2D>("fondo");
            _fondoPosX1 = 0;
            _fondoPosX2 = _fondoTexture.Width;
        }

        public override void Update(GameTime gameTime)
        {
            _player.Update(gameTime, _sprites);

            foreach (Enemy enemy in enemies)
            {
                enemy.Update(gameTime, _player.Position);
            }

            LimitPlayerPosition();

            foreach (var sprite in _sprites.ToArray())
                sprite.Update(gameTime, _sprites);

            UpdateBackground();

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            int X = random.Next(600, 1920);

            if (timer > 3.0f)
            {
                SpawnEnemies(X);
            }

            foreach (Enemy e in enemies)
            {
                e.Update(gameTime, position);
            }

            CheckCollisionPlayer();
            CheckCollisionEnemy();
        }

        private void SpawnEnemies(int X)
        {
            if (enemies.Count < 2)
            {
                enemies.Add(new Enemy(enemyTextures, new Vector2(X, 875), textureRight, textureLeft, bulletTexture));
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
                _game.ChangeState(new MenuState(_game, _game.GraphicsDevice, _game.Content));
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

        private void UpdateBackground()
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
        }

        public override void PostUpdate(GameTime gameTime)
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

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Draw(_fondoTexture, new Rectangle((int)_fondoPosX1, 0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height), Color.White);
            spriteBatch.Draw(_fondoTexture, new Rectangle((int)_fondoPosX2, 0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height), Color.White);
            spriteBatch.End();

            spriteBatch.Begin();

            _player.Draw(spriteBatch);

            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(spriteBatch);
            }

            foreach (var sprite in _sprites)
            {
                sprite.Draw(spriteBatch);
            }

            spriteBatch.DrawString(_scoreFont, $"Score: {points}", new Vector2(10, 10), Color.Black);
            spriteBatch.DrawString(_healthFont, $"Health Points: {_player.Health}", new Vector2(100, 10), Color.Black);

            spriteBatch.End();
        }
    }
}