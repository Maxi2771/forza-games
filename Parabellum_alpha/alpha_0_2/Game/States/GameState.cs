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
        private int _lives = 3;
        private bool _keyPreviouslyPressed = false;
        private List<Bullet> Cargador = new List<Bullet>();

        // Variables para el fondo
        private Texture2D _fondoTexture;
        private float _fondoPosX1;
        private float _fondoPosX2;
        private float _velocidad = 2.4f;
        private const float _limiteIzquierdo = 0;
        private const float _limiteDerecho = 800;

        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            Texture2D[] playerTextures = new Texture2D[]
            {
                content.Load<Texture2D>("Right"),
                content.Load<Texture2D>("Left"),
            };

            var textureRight = content.Load<Texture2D>("weaponRight");
            var textureLeft = content.Load<Texture2D>("weaponLeft");
            var bulletTexture = content.Load<Texture2D>("Bullet");

            _player = new Player(playerTextures, new Vector2(400, 875), textureRight, textureLeft, bulletTexture, Cargador);

            _sprites = new List<Sprite>();

            // Cargar la textura del fondo
            _fondoTexture = content.Load<Texture2D>("fondo");
            _fondoPosX1 = 0;
            _fondoPosX2 = _fondoTexture.Width;
        }

        public override void Update(GameTime gameTime)
        {
            _player.Update(gameTime, _sprites);

            LimitPlayerPosition();

            foreach (var sprite in _sprites.ToArray())
                sprite.Update(gameTime, _sprites);

            UpdateBackground();

            Lives();

            PostUpdate(gameTime);
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

            foreach (var sprite in _sprites)
            {
                sprite.Draw(spriteBatch);
            }

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