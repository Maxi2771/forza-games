using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ParaTesting;
using System.Collections.Generic;

namespace ParaTesting
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Texturas
        private Texture2D playerTexture;
        private Texture2D weaponTexture;
        private Texture2D projectileTexture;

        // Jugador
        private Player player;

        // Lista de proyectiles
        private List<Projectile> projectiles;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Cargar las texturas
            playerTexture = Content.Load<Texture2D>("playerTexture");
            weaponTexture = Content.Load<Texture2D>("weaponTexture");
            projectileTexture = Content.Load<Texture2D>("projectileTexture");

            // Crear el arma con un offset ajustable
            Weapon weapon = new Weapon(weaponTexture, new Vector2(30, 20));

            // Crear el jugador con la posición inicial y el arma
            player = new Player(new Vector2(100, 100), playerTexture, weapon);

            // Inicializar la lista de proyectiles
            projectiles = new List<Projectile>();
        }

        protected override void Update(GameTime gameTime)
        {
            // Obtener el estado del teclado
            KeyboardState keyboardState = Keyboard.GetState();

            // Actualizar al jugador
            player.Update(gameTime, keyboardState);

            // Actualizar los proyectiles
            for (int i = 0; i < projectiles.Count; i++)
            {
                projectiles[i].Update(gameTime);

                // Eliminar proyectiles fuera de la pantalla
                if (projectiles[i].Position.X > _graphics.PreferredBackBufferWidth || projectiles[i].Position.X < 0)
                {
                    projectiles.RemoveAt(i);
                    i--;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // Dibujar al jugador y el arma
            player.Draw(_spriteBatch);

            // Dibujar los proyectiles
            foreach (var projectile in projectiles)
            {
                projectile.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}