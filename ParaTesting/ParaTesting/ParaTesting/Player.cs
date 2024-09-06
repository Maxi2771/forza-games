/*using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace alpha_0_2.Game
{
    public class Player
    {
        private Texture2D[] textures; // Array de texturas para cada dirección
        private Vector2 position; // Posición en X e Y
        private Vector2 velocity; // Velocidad en X e Y
        private float speed;
        private Direction facingDirection; // Dirección actual del jugador

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Direction FacingDirection
        {
            get { return facingDirection; }
            set { facingDirection = value; }
        }

        // Animación
        private Dictionary<Direction, List<Rectangle>> animationFrames; // Diccionario de rectángulos de animación por dirección
        private int currentFrame;
        private float frameTimer;
        private float frameInterval = 0.1f; // Intervalo de tiempo entre frames (ajustable según la velocidad deseada)

        // Salto
        private bool isJumping; // Booleano para saber si está saltando
        private float jumpSpeed; // Velocidad del salto
        private float gravity; // Gravedad que se le aplica al salto
        private float initialJumpVelocity; // Velocidad inicial del salto (al despegarse del suelo)
        private bool isOnGround; // Nueva variable para verificar si el jugador está en el suelo

        public Player(Texture2D[] textures, Vector2 position)
        {
            this.textures = textures;
            this.position = position;
            speed = 4;
            facingDirection = Direction.Right; // Dirección inicial por defecto

            // Inicializar frames de animación para cada dirección
            animationFrames = new Dictionary<Direction, List<Rectangle>>();
            animationFrames[Direction.Up] = new List<Rectangle>();
            animationFrames[Direction.Down] = new List<Rectangle>();
            animationFrames[Direction.Left] = new List<Rectangle>();
            animationFrames[Direction.Right] = new List<Rectangle>();

            // Añadir rectángulos de animación según las dimensiones de cada spritesheet
            int frameWidth = 118 / 3; // Ancho del frame del spritesheet
            int frameHeight = 64; // Alto del frame del spritesheet

            for (int i = 0; i < 3; i++)
            {
                animationFrames[Direction.Up].Add(new Rectangle(i * frameWidth, 0, frameWidth, frameHeight));
                animationFrames[Direction.Down].Add(new Rectangle(i * frameWidth, 0, frameWidth, frameHeight));
                animationFrames[Direction.Left].Add(new Rectangle(i * frameWidth, 0, frameWidth, frameHeight));
                animationFrames[Direction.Right].Add(new Rectangle(i * frameWidth, 0, frameWidth, frameHeight));
            }

            currentFrame = 0;
            frameTimer = 0;

            // Inicializar parámetros de salto
            isJumping = false;
            isOnGround = true;
            jumpSpeed = -7f; // Velocidad inicial hacia arriba
            gravity = 0.6f; // Gravedad
            initialJumpVelocity = jumpSpeed;
        }

        public void Update(GameTime gameTime)
        {
            HandleInput(); // Manejar entrada para actualizar la dirección y la animación
            UpdateMovement(); // Actualizar la posición según la velocidad
            UpdateAnimation(gameTime); // Actualizar la animación
        }

        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        private void HandleInput()
        {
            velocity.X = 0;

            // Manejar entrada del teclado para cambiar la dirección y establecer velocidad
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                facingDirection = Direction.Left;
                velocity.X -= 1;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                facingDirection = Direction.Right;
                velocity.X += 1;
            }

            // Manejar salto
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && isOnGround)
            {
                isJumping = true;
                isOnGround = false;
                velocity.Y = initialJumpVelocity;
            }

            // Normalizar la velocidad solo si se está moviendo horizontalmente
            if (velocity.X != 0)
                velocity.Normalize();
        }

        private void UpdateAnimation(GameTime gameTime)
        {
            // Solo actualizar la animación si el jugador se está moviendo
            if (velocity.X != 0)
            {
                frameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (frameTimer >= frameInterval)
                {
                    currentFrame = (currentFrame + 1) % animationFrames[facingDirection].Count;
                    frameTimer = 0;
                }
            }
            else
            {
                currentFrame = 0; // Mantener el primer frame si no se está moviendo
            }
        }

        private void UpdateMovement()
        {
            // Aplicar gravedad si el jugador está saltando o en el aire
            if (isJumping || !isOnGround)
            {
                velocity.Y += gravity;
            }

            // Actualizar la posición del jugador basado en la velocidad
            position += velocity * speed;

            // Detener el salto si el jugador toca el suelo (supongamos que el suelo está en y = 400)
            if (position.Y >= 400)
            {
                position.Y = 400;
                isJumping = false;
                isOnGround = true;
                velocity.Y = 0;
            }
        }
        // Método para dibujar al jugador
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textures[(int)facingDirection], position, animationFrames[facingDirection][currentFrame], Color.White);
        }
    }
}
*/