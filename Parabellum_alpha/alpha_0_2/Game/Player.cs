using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace alpha_0_2.Game
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public class Player
    {
        private Texture2D[] textures; // Array de texturas para cada dirección
        private Vector2 position;
        private Vector2 velocity;
        private float speed;
        private Direction facingDirection; // Dirección actual del jugador

        // Animación
        private Dictionary<Direction, List<Rectangle>> animationFrames; // Diccionario de rectángulos de animación por dirección
        private int currentFrame;
        private float frameTimer;
        private float frameInterval = 0.1f; // Intervalo de tiempo entre frames (ajustable según la velocidad deseada)

        public Player(Texture2D[] textures, Vector2 position)
        {
            this.textures = textures;
            this.position = position;
            speed = 4;
            facingDirection = Direction.Down; // Dirección inicial por defecto

            // Inicializar frames de animación para cada dirección
            animationFrames = new Dictionary<Direction, List<Rectangle>>();
            animationFrames[Direction.Up] = new List<Rectangle>();
            animationFrames[Direction.Down] = new List<Rectangle>();
            animationFrames[Direction.Left] = new List<Rectangle>();
            animationFrames[Direction.Right] = new List<Rectangle>();

            // Añadir rectángulos de animación según las dimensiones de cada spritesheet
            int frameWidth = textures[(int)Direction.Down].Width / 4; // Suponiendo que cada spritesheet tiene 4 frames en horizontal
            int frameHeight = textures[(int)Direction.Down].Height / 4; // Suponiendo que cada spritesheet tiene 4 frames en vertical

            for (int i = 0; i < 4; i++)
            {
                animationFrames[Direction.Up].Add(new Rectangle(i * frameWidth, 0, frameWidth, frameHeight));
                animationFrames[Direction.Down].Add(new Rectangle(i * frameWidth, 0, frameWidth, frameHeight));
                animationFrames[Direction.Left].Add(new Rectangle(i * frameWidth, 0, frameWidth, frameHeight));
                animationFrames[Direction.Right].Add(new Rectangle(i * frameWidth, 0, frameWidth, frameHeight));
            }

            currentFrame = 0;
            frameTimer = 0;
        }

        public void Update(GameTime gameTime)
        {
            HandleInput(); // Manejar entrada para actualizar la dirección y la animación
            UpdateAnimation(gameTime); // Actualizar la animación
            UpdateMovement(); // Actualizar la posición según la velocidad
        }

        private void HandleInput()
        {
            velocity = Vector2.Zero;

            // Manejar entrada del teclado para cambiar la dirección
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                facingDirection = Direction.Up;
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
                facingDirection = Direction.Down;
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
                facingDirection = Direction.Left;
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
                facingDirection = Direction.Right;
        }

        private void UpdateAnimation(GameTime gameTime)
        {
            // Actualizar la animación basada en la dirección actual
            frameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (frameTimer >= frameInterval)
            {
                currentFrame = (currentFrame + 1) % animationFrames[facingDirection].Count;
                frameTimer = 0;
            }
        }

        private void UpdateMovement()
        {
            // Actualizar la posición del jugador basado en la velocidad
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                velocity.Y -= 1;
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
                velocity.Y += 1;
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
                velocity.X -= 1;
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
                velocity.X += 1;

            if (velocity != Vector2.Zero)
                velocity.Normalize();

            position += velocity * speed;
            velocity = Vector2.Zero;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textures[(int)facingDirection], position, animationFrames[facingDirection][currentFrame], Color.White);
        }
    }
}
