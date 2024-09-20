using alpha_0_2.Game.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using alpha_0_2.Sprites;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace alpha_0_2.Game.States
{
    public class GameState : State
    {
        private Player _player;
        private Weapon _weapon;
        private Bullet _bullet;
        private Texture2D weaponTexture;
        private Texture2D _projectileTexture;
        private List<Sprite> _sprites;
        private Vector2 Position;


        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            // Cargar spritesheets para cada dirección del jugador
            Texture2D[] playerTextures = new Texture2D[]
            {
                content.Load<Texture2D>("man_back"),
                content.Load<Texture2D>("man_front"),
                content.Load<Texture2D>("pj_izq"),
                content.Load<Texture2D>("pj_der"),
            };

            // Cargar la textura del arma y del proyectil (bala)
            var weaponTexture = content.Load<Texture2D>("Weapon");
            // Crear el jugador y asignarle el arma
            _player = new Player(playerTextures, new Vector2(400, 400), weaponTexture);

            _sprites = new List<Sprite>();
            {
                new Weapon(weaponTexture)
                {
                    Position = new Vector2(100, 100),
                    Bullet = new Bullet(content.Load<Texture2D>("Bullet")),
                };
            };
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

            // Llamar a Begin una sola vez
            

            // Dibujar al jugador
            _player.Draw(spriteBatch);

            //_bullet.Draw(spriteBatch);

            // Dibujar los sprites (incluyendo el arma)
            

            // Terminar el spriteBatch después de dibujar todo
            spriteBatch.End();
        }
    }
}
