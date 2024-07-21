using alpha_0_2.Game.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace alpha_0_2.Game.States
{
    public class GameState : State
    {
        private Player _player;
        private Weapon _weapon;
        private Texture2D _weaponTexture;
        private Texture2D _projectileTexture;

        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            // Cargar spritesheets para cada dirección del jugador
            Texture2D[] playerTextures = new Texture2D[]
            {
                content.Load<Texture2D>("man_back"),
                content.Load<Texture2D>("man_front"),
                content.Load<Texture2D>("izquierda"),
                content.Load<Texture2D>("derecha")
            };

            // Cargar textura del arma y del proyectil
            _weaponTexture = content.Load<Texture2D>("weapon_texture");
            //_projectileTexture = content.Load<Texture2D>("projectile_texture");

            // Inicializar jugador en el centro de la pantalla
            _player = new Player(playerTextures, new Vector2(400, 400));

            // Inicializar arma del jugador en la posición del jugador
            _weapon = new Weapon(_weaponTexture, _player.Position);
        }

        public override void Update(GameTime gameTime)
        {
            _player.Update(gameTime);
            _weapon.Update(gameTime, _player.FacingDirection);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Limpiar la pantalla antes de dibujar
            _graphicsDevice.Clear(Color.CornflowerBlue);

            // Dibujar al jugador y al arma
            _player.Draw(spriteBatch);
            _weapon.Draw(spriteBatch);

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // Lógica posterior a la actualización si es necesaria
        }
    }
}
