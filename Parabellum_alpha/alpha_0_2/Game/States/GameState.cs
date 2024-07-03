using alpha_0_2.Game.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace alpha_0_2.Game.States
{
    public class GameState : State
    {
        private Player _player;

        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            // Cargar spritesheets para cada dirección del jugador
            Texture2D[] playerTextures = new Texture2D[]
            {
                content.Load<Texture2D>("man_back"),    // Texture for facing Up
                content.Load<Texture2D>("man_front"),   // Texture for facing Down
                content.Load<Texture2D>("man_left"),    // Texture for facing Left
                content.Load<Texture2D>("man_right")    // Texture for facing Right
            };

            // Inicializar jugador en el centro de la pantalla
            _player = new Player(playerTextures, new Vector2(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2));
        }

        public override void Update(GameTime gameTime)
        {
            _player.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Limpiar la pantalla antes de dibujar
            _graphicsDevice.Clear(Color.CornflowerBlue);

            // Dibujar al jugador
            _player.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // Lógica posterior a la actualización si es necesaria
        }
    }
}
