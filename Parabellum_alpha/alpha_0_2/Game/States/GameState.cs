using alpha_0_2.Game.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using alpha_0_2.Sprites;
using System.Collections.Generic;

namespace alpha_0_2.Game.States
{
    public class GameState : State
    {
        private Player _player;
        private List<Sprite> _sprites;

        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            // Cargar spritesheets para cada direcciÃ³n del jugador
            Texture2D[] playerTextures = new Texture2D[]
            {
            content.Load<Texture2D>("man_back"),
            content.Load<Texture2D>("man_front"),
            content.Load<Texture2D>("pj_izq"),
            content.Load<Texture2D>("pj_der"),
            };

            // Cargar la textura del arma y del proyectil (bala)
            var textureRight = content.Load<Texture2D>("weaponRight");
            var textureLeft = content.Load<Texture2D>("weaponLeft");
            var bulletTexture = content.Load<Texture2D>("Bullet");

            // Crear el jugador y asignarle el arma y la textura de la bala
            _player = new Player(playerTextures, new Vector2(400, 400), textureRight, textureLeft, bulletTexture);

            // Inicializar la lista de sprites (solo contendra las balas y otros elementos del juego)
            _sprites = new List<Sprite>();
        }

        public override void Update(GameTime gameTime)
        {
            // Actualizar al jugador
            _player.Update(gameTime, _sprites);

            // Actualizar los sprites (balas y otros)
            foreach (var sprite in _sprites.ToArray())
                sprite.Update(gameTime, _sprites);

            PostUpdate(gameTime);
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

            // Dibujar al jugador
            _player.Draw(spriteBatch);

            // Dibujar los sprites (incluyendo balas)
            foreach (var sprite in _sprites)
            {
                sprite.Draw(spriteBatch);
            }

            spriteBatch.End();
        }
    }
}