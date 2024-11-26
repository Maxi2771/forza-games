using alpha_0_2.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace alpha_0_2.Sprites
{
    public class Enemy
    {
        private Vector2 velocity;
        private Direction facingDirection;
        private float _shootTimer;
        private float _shootInterval = 1.5f; // Intervalo de tiempo entre disparos
        private Bullet _bullet;
        private Texture2D[] textures;
        private bool _isAlive = true;
        private Vector2 _direction; // Dirección del enemigo
        private float _speed = 50.0f; // Velocidad del enemigo
        private Dictionary<Direction, List<Rectangle>> animationFrames;
        private int currentFrame;
        private float frameTimer;
        private Weapon weapon;
        private List<Bullet> Cargador = new List<Bullet>();
        private List<Bullet> disparadas = new List<Bullet>();
        private Vector2 position;
        private bool IsRemoved = false;
        private float frameInterval = 0.1f;

        public Enemy(Texture2D[] textures, Vector2 position, Texture2D weaponRight, Texture2D weaponLeft, Texture2D bulletTexture)
        {
            this.textures = textures;
            this.position = position;
            _direction = new Vector2(-1, 0);
            int frameWidth = 176 / 4; // Ancho del frame del spritesheet
            int frameHeight = 87; // Alto del frame del spritesheet

            animationFrames = new Dictionary<Direction, List<Rectangle>>();
            animationFrames[Direction.Left] = new List<Rectangle>();
            animationFrames[Direction.Right] = new List<Rectangle>();


            for (int i = 0; i < 3; i++)
            {
                animationFrames[Direction.Left].Add(new Rectangle(i * frameWidth, 0, frameWidth, frameHeight));
                animationFrames[Direction.Right].Add(new Rectangle(i * frameWidth, 0, frameWidth, frameHeight));
            }

            //weapon = new Weapon(weaponRight, weaponLeft, bulletTexture, Cargador, position);
        }

        private void FollowPlayer(Vector2 playerPosition, GameTime gameTime)
        {
            _direction = playerPosition - position;

            if (_direction != Vector2.Zero)
                _direction.Normalize();

            position += _direction * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(_direction.X < 0)
            {
                facingDirection = Direction.Left;
            }
            else
            {
                facingDirection = Direction.Right;
            }

            UpdateAnimation(gameTime);
        }


        private void UpdateAnimation(GameTime gameTime)
        {
            if (Math.Abs(_direction.X) > 0.01f)
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

        public void Update(GameTime gameTime, Vector2 playerPosition)
        {
            if (!_isAlive) return;

            FollowPlayer(playerPosition, gameTime);
        }


        public void Hit()
        {
            /*if ()
            {
                _isAlive = false;
            }*/
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_isAlive)
            {
                var frame = animationFrames[facingDirection][currentFrame];
                spriteBatch.Draw(textures[(int)facingDirection], position, frame, Color.White);
            }
        }
    }
}