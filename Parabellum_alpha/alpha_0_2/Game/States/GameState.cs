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
            // Cargar textura del jugador
            var playerTexture = content.Load<Texture2D>("ball");

            // Inicializar jugador en el centro de la pantalla con velocidad
            _player = new Player(playerTexture, new Vector2(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2), 200f);
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

            // Finalizar el dibujo
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // Aquí podrías realizar lógica posterior a la actualización si es necesario
        }
    }
}
