using alpha_0_2.Game.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using alpha_0_2.Sprites;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace alpha_0_2.Game.States
{
    public class GameState : State
    {
        private Player _player;
        private List<Sprite> _sprites;
        private List<Enemy> _enemies; // Lista de enemigos
        private int _lives = 3;
        private bool _keyPreviouslyPressed = false;

        // Variables para el fondo
        private Texture2D _fondoTexture;
        private float _fondoPosX1;
        private float _fondoPosX2;
        private float _velocidad = 2.4f;
        private const float _limiteIzquierdo = 0;
        private const float _limiteDerecho = 1920;
        Texture2D textureRight;
        Texture2D textureLeft;
        Texture2D enemyTextureRight;
        Texture2D enemyTextureLeft;
        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            // Cargar spritesheets para cada dirección del jugador
            Texture2D[] playerTextures = new Texture2D[]
            {
                content.Load<Texture2D>("back"),
                content.Load<Texture2D>("front"),
                content.Load<Texture2D>("Left"),
                content.Load<Texture2D>("Right"),
            };

            /*Texture2D[] enemyTextures = new Texture2D[]
            {
                content.Load<Texture2D>("EnemyLeft"),
                content.Load<Texture2D>("EnemyRight"),
            };*/

            // Cargar la textura del arma y del proyectil (bala)
            textureRight = content.Load<Texture2D>("weaponRight");
            textureLeft = content.Load<Texture2D>("weaponLeft");
            var bulletTexture = content.Load<Texture2D>("Bullet");

            // Crear el jugador y asignarle el arma y la textura de la bala
            _player = new Player(playerTextures, new Vector2(800, 875), textureRight, textureLeft, bulletTexture);

            // Inicializar la lista de sprites (solo contendrá las balas y otros elementos del juego)
            _sprites = new List<Sprite>();

            // Cargar la textura del fondo
            _fondoTexture = content.Load<Texture2D>("fondo");
            _fondoPosX1 = 0;
            _fondoPosX2 = _fondoTexture.Width; // Comienza el segundo fondo justo a la derecha del primero

            // Cargar las texturas de los enemigos
            enemyTextureLeft = content.Load<Texture2D>("EnemyLeft"); // Enemigo que se mueve a la izquierda
            enemyTextureRight = content.Load<Texture2D>("EnemyRight"); // Enemigo que se mueve a la derecha

            // Crear enemigos y agregar a la lista
            /*_enemies = new List<Enemy>
            {
                new Enemy(enemyTextures, new Vector2(1200, 875), bulletTexture, textureRight, textureLeft, bulletTexture)
            };*/
        }

        public override void Update(GameTime gameTime)
        {
            // Actualizar al jugador
            _player.Update(gameTime, _sprites);

            // Limitar la posición del jugador para que no salga de la pantalla
            LimitPlayerPosition();

            // Actualizar los sprites (balas y otros)
            foreach (var sprite in _sprites.ToArray())
                sprite.Update(gameTime, _sprites);

            // Actualizar enemigos
            /*foreach (var enemy in _enemies.ToArray())
            {
                enemy.Update(gameTime, _sprites);
                if (!enemy.IsRemoved)
                {
                    // Verificar colisiones con balas del jugador
                    foreach (var bullet in _sprites.ToArray())
                    {
                        if (bullet is Bullet && bullet.Rectangle.Intersects(enemy.Rectangle))
                        {
                            enemy.Hit(); // El enemigo recibe daño
                            bullet.IsRemoved = true; // Elimina la bala
                        }
                    }
                }
            }*/

            // Actualizar la posición del fondo
            UpdateBackground();

            Lives();

            PostUpdate(gameTime);
        }

        private void LimitPlayerPosition()
        {
            // Obtener el tamaño del jugador
            var playerPosition = _player.Position;
            var playerWidth = _player.Width; // Utilizar la propiedad Width para obtener el ancho de la textura del jugador

            // Limitar la posición del jugador en el eje X
            if (playerPosition.X < 0)
            {
                playerPosition.X = 0; // No permitir que el jugador se salga por la izquierda
            }
            else if (playerPosition.X > _limiteDerecho - playerWidth)
            {
                playerPosition.X = _limiteDerecho - playerWidth; // No permitir que el jugador se salga por la derecha
            }

            _player.Position = playerPosition; // Actualiza la posición del jugador
        }

        private void UpdateBackground()
        {
            var state = Keyboard.GetState();

            // Mueve el fondo hacia la izquierda si se presiona la tecla izquierda
            if (state.IsKeyDown(Keys.Left))
            {
                // Solo mueve el fondo si no se ha alcanzado el límite izquierdo
                if (_fondoPosX1 < _limiteIzquierdo)
                {
                    _fondoPosX1 += _velocidad; // Mueve el fondo a la izquierda
                    _fondoPosX2 += _velocidad; // Mueve el segundo fondo a la izquierda
                }
            }
            // Mueve el fondo hacia la derecha si se presiona la tecla derecha
            else if (state.IsKeyDown(Keys.Right))
            {
                _fondoPosX1 -= _velocidad; // Mueve el fondo a la derecha
                _fondoPosX2 -= _velocidad; // Mueve el segundo fondo a la derecha
            }

            // Verifica si el primer fondo salió completamente de la pantalla
            if (_fondoPosX1 <= -_fondoTexture.Width)
                _fondoPosX1 = _fondoPosX2 + _fondoTexture.Width; // Reposiciona el fondo

            // Verifica si el segundo fondo salió completamente de la pantalla
            if (_fondoPosX2 <= -_fondoTexture.Width)
                _fondoPosX2 = _fondoPosX1 + _fondoTexture.Width; // Reposiciona el fondo
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
            // Limpiar el fondo con el color azul solo al principio
            _graphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.End();
            spriteBatch.Begin();
            spriteBatch.Draw(_fondoTexture, new Rectangle((int)_fondoPosX1, 0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height), Color.White);
            spriteBatch.Draw(_fondoTexture, new Rectangle((int)_fondoPosX2, 0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height), Color.White);

            // Comienza un nuevo spriteBatch para dibujar al jugador y otros sprites
            // Dibujar al jugador
            _player.Draw(spriteBatch);

            // Dibujar los sprites (incluyendo balas)
            foreach (var sprite in _sprites)
            {
            //    sprite.Draw(spriteBatch);
            }

            // Dibujar enemigos
            /*foreach (var enemy in _enemies)
            {
                spriteBatch.Draw(enemyTextureRight, enemy.Position, Color.White);
                //enemy.Draw(spriteBatch);
            }*/

            spriteBatch.End();
        }

        public void Lives()
        {
            var state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.K))
            {
                if (!_keyPreviouslyPressed && _lives > 0)
                {
                    _lives--;
                }
                _keyPreviouslyPressed = true;
            }
            else
            {
                _keyPreviouslyPressed = false;
            }

            if (_lives <= 0)
            {
                _game.Exit();
            }
        }
    }
}