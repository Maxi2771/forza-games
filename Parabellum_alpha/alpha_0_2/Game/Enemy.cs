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
        private Direction facingDirection;
        private Texture2D[] textures;
        private bool _isAlive = true;
        private Vector2 _direction; // Dirección del enemigo
        private float _speed = 35.0f; // Velocidad del enemigo
        private Dictionary<Direction, List<Rectangle>> animationFrames;
        private int currentFrame;
        private float frameTimer;
        private Weapon weapon;
        private List<Bullet> Cargador = new List<Bullet>();
        private List<Bullet> disparadas = new List<Bullet>();
        private Vector2 position;
        private float frameInterval = 0.1f;
        private float _shootTimer;
        private float _nextShootInterval;
        private Random _random;
        private Texture2D bulletTexture;

        public bool IsAlive
        {
            get { return _isAlive; }
            set { _isAlive = value; }
        }

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

        public Enemy(Texture2D[] textures, Vector2 position, Texture2D weaponRight, Texture2D weaponLeft, Texture2D bulletTexture)
        {
            this.textures = textures;
            this.position = position;
            this.bulletTexture = bulletTexture;
            _direction = new Vector2(-1, 0);
            int frameWidth = 176 / 4;
            int frameHeight = 87;

            animationFrames = new Dictionary<Direction, List<Rectangle>>();
            animationFrames[Direction.Left] = new List<Rectangle>();
            animationFrames[Direction.Right] = new List<Rectangle>();

            _random = new Random();

            for (int i = 0; i < 3; i++)
            {
                animationFrames[Direction.Left].Add(new Rectangle(i * frameWidth, 0, frameWidth, frameHeight));
                animationFrames[Direction.Right].Add(new Rectangle(i * frameWidth, 0, frameWidth, frameHeight));
            }

            weapon = new Weapon(weaponRight, weaponLeft, bulletTexture, Cargador, position);
            RandomInterval();
        }

        private void RandomInterval()
        {
            _nextShootInterval = (float)_random.NextDouble() * 2;
        }

        private void Shoot(Vector2 playerPosition, GameTime gameTime)
        {
            if (_shootTimer >= _nextShootInterval)
            {
                _shootTimer = 0;
                RandomInterval();

                weapon.ShootBullet(gameTime);
            }
        }


        private void FollowPlayer(Vector2 playerPosition, GameTime gameTime)
        {
            _direction = playerPosition - position;
            _direction.Normalize();

            position += _direction * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            position.Y = 875;

            if(_direction.X < 0)
            {
                facingDirection = Direction.Left;
                weapon.Texture = weapon.TextureLeft;
                weapon.Direction = new Vector2(-1, 0);
                weapon.Position = new Vector2(-65, 14);
            }
            else
            {
                facingDirection = Direction.Right;
                weapon.Texture = weapon.TextureRight;
                weapon.Direction = new Vector2(1, 0);
                weapon.Position = new Vector2(-15, 14);
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
            weapon.Update(gameTime);
            weapon.EntityPosition = position;
            _shootTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if ((_direction.X < 0 && facingDirection == Direction.Left) ||
            (_direction.X > 0 && facingDirection == Direction.Right))
            {
                Shoot(playerPosition, gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_isAlive)
            {
                var frame = animationFrames[facingDirection][currentFrame];
                spriteBatch.Draw(textures[(int)facingDirection], position, frame, Color.White);
                weapon.Draw(spriteBatch);
            }
        }
    }
}