using alpha_0_2.Game.States;
using alpha_0_2.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace alpha_0_2.Game
{
    public class Player
    {
        private Texture2D[] textures; // Array de texturas para cada dirección
        private Vector2 position; // Posición en X e Y
        private Vector2 velocity; // Velocidad en X e Y
        private float speed;
        private Direction facingDirection;
        private KeyboardState _currentKey;
        private KeyboardState _previousKey;
        private int _health = 6;

        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }

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

        public int Width => textures[(int)facingDirection].Width;

        Dictionary<Direction, List<Rectangle>> animationFrames = new Dictionary<Direction, List<Rectangle>>
        {
            { Direction.Right, new List<Rectangle>() },
            { Direction.Left, new List<Rectangle>() },
        };
        private int currentFrame = 0;
        private float frameTimer;
        private float frameInterval = 0.1f;

        // Salto
        private bool isJumping;
        private float jumpSpeed;
        private float gravity;
        private float initialJumpVelocity;
        private bool isOnGround;

        private Weapon weapon;

        public Weapon Weapon
        {
            get { return weapon; }
            set { weapon = value; }
        }
        public Rectangle CollisionRectangle
        {
            get
            {
                var currentFrameRect = animationFrames[facingDirection][currentFrame];
                return new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    currentFrameRect.Width,
                    currentFrameRect.Height
                );
            }
        }

        public Player(Texture2D[] textures, Vector2 position, Texture2D textureRight, Texture2D textureLeft, Texture2D bulletTexture, List<Bullet> Cargador)
        {
            this.textures = textures;
            this.position = position;
            speed = 1.2f;
            facingDirection = Direction.Right;

            animationFrames[Direction.Right] = new List<Rectangle>();
            animationFrames[Direction.Left] = new List<Rectangle>();

            int frameWidth = 180 / 4;
            int frameHeight = 87;

            for (int i = 0; i < 2; i++)
            {
                animationFrames[Direction.Right].Add(new Rectangle(i * frameWidth, 0, frameWidth, frameHeight));
                animationFrames[Direction.Left].Add(new Rectangle(i * frameWidth, 0, frameWidth, frameHeight));
            }

            currentFrame = 0;
            frameTimer = 0;

            isJumping = false;
            isOnGround = true;
            jumpSpeed = -10f;
            gravity = 0.6f;
            initialJumpVelocity = jumpSpeed;

            weapon = new Weapon(textureRight, textureLeft, bulletTexture, Cargador, position);
        }

        public void Update(GameTime gameTime, List<Sprite> sprites)
        {
            _previousKey = _currentKey;
            _currentKey = Keyboard.GetState();

            HandleInput(gameTime);
            UpdateMovement();
            weapon.Update(gameTime);
            weapon.EntityPosition = position;

            if (_currentKey.IsKeyDown(Keys.Space) && _previousKey.IsKeyUp(Keys.Space))
            {
                weapon.ShootBullet(gameTime);
            }
        }

        private void HandleInput(GameTime gameTime)
        {
            velocity.X = 0;

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                facingDirection = Direction.Left;
                velocity.X -= 1;
                weapon.Texture = weapon.TextureLeft;
                weapon.Direction = new Vector2(-1, 0);
                weapon.Position = new Vector2(-60, 14);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                facingDirection = Direction.Right;
                velocity.X += 1;
                weapon.Texture = weapon.TextureRight;
                weapon.Direction = new Vector2(1, 0);
                weapon.Position = new Vector2(-22, 14);
            }

            // Manejar salto
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && isOnGround)
            {
                isJumping = true;
                isOnGround = false;
                velocity.Y = initialJumpVelocity;
            }

            if (velocity.X != 0)
                velocity.Normalize();

            UpdateAnimation(gameTime);
        }

        private void UpdateAnimation(GameTime gameTime)
        {
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
                currentFrame = 0;
            }
        }

        private void UpdateMovement()
        {
            if (isJumping || !isOnGround)
            {
                velocity.Y += gravity;
            }

            position += velocity * speed;

            if (position.Y >= 875)
            {
                position.Y = 875;
                isJumping = false;
                isOnGround = true;
                velocity.Y = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textures[(int)facingDirection], position, animationFrames[facingDirection][currentFrame], Color.White);
            weapon.Draw(spriteBatch);
        }
    }
}