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
    public class GameState3 : State
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
        private int enemiesLeft;
        private int enemiesKilled;
        private SpriteFont font;
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
        Texture2D rocketTexture;
        Texture2D explosionTexture;
        Texture2D textureRight;
        Texture2D textureLeft;
        Turret _turret;
        Texture2D turretRight;
        Texture2D turretLeft;
        bool drawExplosion = false;
        Vector2 pos;
        Background _background;
        int _currentRound;
        bool isRound = false;
        Texture2D platformTexture;
        Rectangle platformRectangle;

        public GameState3(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
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

            turretLeft = content.Load<Texture2D>("turretLeft");
            turretRight = content.Load<Texture2D>("turretRight");

            textureRight = content.Load<Texture2D>("weaponRight");
            textureLeft = content.Load<Texture2D>("weaponLeft");
            bulletTexture = content.Load<Texture2D>("Bullet");
            rocketTexture = content.Load<Texture2D>("rocket");
            explosionTexture = content.Load<Texture2D>("explosion");

            platformTexture = content.Load<Texture2D>("Controls/Button");

            //_turret = new Turret(turretRight, turretLeft, new Vector2(1000, 700), rocketTexture);

            position = new Vector2(400, 875);
            //position2 = new Vector2(200, 875);

            _player = new Player(playerTextures, position, textureRight, textureLeft, bulletTexture, Cargador);

            //_player2 = new Player2(playerTextures, position2, textureRight, textureLeft, bulletTexture, Cargador);
            /*_enemy = new Enemy(enemyTextures, new Vector2(900, 875), textureRight, textureLeft, bulletTexture);
            enemies.Add(_enemy);*/
            random = new Random();

            _sprites = new List<Sprite>();

            font = content.Load<SpriteFont>("Fonts/Font");

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

            /*var nextLevel = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((_game._graphics.PreferredBackBufferWidth / 2) - buttonTexture.Width / 2, (_game._graphics.PreferredBackBufferHeight / 2) - 200),
                Text = "Next Level",
            };*/

            //nextLevel.Click += NextLevelButton_Click;

            /*_componentsWin = new List<Component>()
            {
                nextLevel,
            };*/

            Round1();
        }

        private void ReturnMenuButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content, this._game._graphics));
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
        }

        /*private void NextLevelButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState2(_game, _graphicsDevice, _content));
        }*/

        public override void Update(GameTime gameTime)
        {
            if (!gameOver)
            {
                _player.Update(gameTime, _sprites);

                LimitPlayerPosition();

                foreach (var sprite in _sprites.ToArray())
                    sprite.Update(gameTime, _sprites);

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
                    e.Update(gameTime, _player.Position);
                }

                _turret.Update(gameTime, _player.Position);
                _background.Update(gameTime);
                CheckCollisionPlayer();
                CheckCollisionEnemy();
                MissileCollisionPlayer();
                CheckCollisionPlatform();
                TurretCollisionPlayer();
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
            enemiesLeft = 5;
            for (int i = 0; i < enemiesLeft; i++)
            {
                enemies.Add(new Enemy(enemyTextures, new Vector2(_player.Position.X + random.Next(300, 900), 875), textureRight, textureLeft, bulletTexture));
                timer = 0;
            }
            _turret = new Turret(turretRight, turretLeft, new Vector2(1000, 700), rocketTexture);
            turrets.Add(_turret);
        }

        private void Round2()
        {
            gameWon = false;
            _currentRound = 2;
            enemiesLeft = 6;
            for (int i = 0; i < enemiesLeft; i++)
            {
                enemies.Add(new Enemy(enemyTextures, new Vector2(_player.Position.X + random.Next(300, 900), 875), textureRight, textureLeft, bulletTexture));
                timer = 0;
            }
        }

        private void Round3()
        {
            gameWon = false;
            _currentRound = 3;
            enemiesLeft = 8;
            for (int i = 0; i < enemiesLeft; i++)
            {
                enemies.Add(new Enemy(enemyTextures, new Vector2(_player.Position.X + random.Next(300, 900), 875), textureRight, textureLeft, bulletTexture));
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
                        if (!_player.IsDodging) // Si no lo esquivo pierdo vida
                        {
                            enemy.Weapon.Disparadas.RemoveAt(i);
                            ReduceHealth();
                        }
                        else
                        {
                            enemy.Weapon.Disparadas.RemoveAt(i);
                        }
                    }
                }
            }
        }
        
        public void CheckCollisionPlatform()
        {
            if (platformRectangle.Intersects(_player.CollisionRectangle))
            {
                _player.HasJumped = false;
                _player.Position = new Vector2(_player.Position.X, platformRectangle.Top - 82);
                if (_player.FacingDirection == Direction.Right)
                {
                    _player.Weapon.Position = new Vector2(-22, -90);
                }
                else if(_player.FacingDirection == Direction.Left)
                {
                    _player.Weapon.Position = new Vector2(-64, -90);
                }
            }
        }
        
        public void TurretCollisionPlayer()
        {
            for (int t = turrets.Count - 1; t >= 0; t--)
            {
                for (int i = _player.Weapon.Disparadas.Count - 1; i >= 0; i--)
                {
                    if (turrets[t].CollisionRectangle.Intersects(_player.Weapon.Disparadas[i].CollisionRectangle))
                    {
                        turrets[t].Health--;
                        _player.Weapon.Disparadas.RemoveAt(i);
                        if (turrets[t].Health <= 0)
                        {
                            turrets.RemoveAt(t);
                        }
                    }
                }
            }
        }

        public void MissileCollisionPlayer()
        {
            for (int t = turrets.Count - 1; t >= 0; t--)
            {
                for (int i = turrets[t].Disparadas.Count - 1; i >= 0; i--)
                {
                    if (turrets[t].Disparadas[i].CollisionRectangle.Intersects(_player.CollisionRectangle))
                    {
                        if (!_player.IsDodging) // Si no lo esquivo pierdo vida
                        {
                            drawExplosion = true;
                            Vector2 offset = new Vector2(100, 80);
                            pos = turrets[t].Disparadas[i].PositionAir - offset;
                            turrets[t].Disparadas.RemoveAt(i);
                            ReduceHealthMissile();
                        }
                        else
                        {
                            turrets[t].Disparadas.RemoveAt(i);
                        }
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
                        _player.Weapon.Disparadas.RemoveAt(i);
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
        public override void PostUpdate(GameTime gameTime)
        {

        }

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

            spriteBatch.Draw(platformTexture, new Vector2(500, 850), Color.White);

            platformRectangle = new Rectangle(500, 850, platformTexture.Width, platformTexture.Height);

            Texture2D rectTexture = new Texture2D(_graphicsDevice, 1, 1);
            rectTexture.SetData(new[] { Color.Red });
            spriteBatch.Draw(rectTexture, platformRectangle, Color.Red * 0.5f);


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
            spriteBatch.DrawString(font, $"Health Points: {_player.Health}", new Vector2(300, 10), Color.Black);
            spriteBatch.DrawString(font, $"Current Round: {_currentRound}", new Vector2(450, 10), Color.Black);
            spriteBatch.DrawString(font, $"Ammo: {_player.Weapon._Cargador.Count}", new Vector2(590, 10), Color.Black);

            //spriteBatch.DrawString(font, $"Player Position: {_player.Position}", new Vector2(300, 10), Color.Black);

            /*foreach (Enemy enemy in enemies)
            {
                spriteBatch.DrawString(font, $"Enemy Position: {enemy.Position}", new Vector2(600, 10), Color.Black);
            }*/

            if (gameOver)
            {
                foreach (var component in _components)
                    component.Draw(gameTime, spriteBatch);
            }
            if (_currentRound == 3 && enemiesLeft == 0 && !_turret.IsAlive)
            {
                spriteBatch.DrawString(font, "You Won!", new Vector2(700, 500), Color.Black);
            }

            spriteBatch.End();
        }
    }
}