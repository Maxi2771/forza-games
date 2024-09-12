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
        private Weapon _weapon;
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

            /*Texture2D[] weaponTexture = new Texture2D[]
            {
                content.Load<Texture2D>("weapon_texture"),
            };*/

            // Cargar la textura del arma y del proyectil (bala)
            var weaponTexture = content.Load<Texture2D>("weapon_texture");
            var bulletTexture = content.Load<Texture2D>("proyectil");
            // Crear el jugador y asignarle el arma
            _player = new Player(playerTextures, new Vector2(400, 400), weaponTexture);

            _sprites = new List<Sprite>();
            /*{
                new Weapon(weaponTexture, Position)
                {
                    //Origin = new Vector2(200, 200),
                    Position = new Vector2 (_player.Position.X + 100, _player.Position.Y + 50),
                    Bullet = new Bullet(content.Load<Texture2D>("Bullet")),
                },
            };*/
        }

        public override void Update(GameTime gameTime)
        {
            // Actualizar al jugador
            _player.Update(gameTime, _sprites);

            // Actualizar los sprites (balas y otros)
            foreach (var sprite in _sprites)
            {
                sprite.Update(gameTime, _sprites);
            }

            // Limpiar balas eliminadas (IsRemoved = true)
            _sprites.RemoveAll(s => s.IsRemoved);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.CornflowerBlue);

            // Llamar a Begin una sola vez
            

            // Dibujar al jugador
            _player.Draw(spriteBatch);

            // Dibujar los sprites (incluyendo el arma)
            foreach (var sprite in _sprites)
            {
                sprite.Draw(spriteBatch);
            }

            // Terminar el spriteBatch después de dibujar todo
            spriteBatch.End();
        }



        public override void PostUpdate(GameTime gameTime)
        {
            // Lógica posterior a la actualización si es necesaria
        }
    }
}
