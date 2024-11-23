using alpha_0_2.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace alpha_0_2.Sprites
{
    public class Enemy : Sprite
    {
        private float _shootTimer;
        private float _shootInterval = 1.5f; // Intervalo de tiempo entre disparos
        private Bullet _bullet;
        private Texture2D[] textures;
        private bool _isAlive = true;
        private Vector2 _direction; // Dirección del enemigo
        private float _speed = 1.0f; // Velocidad del enemigo
        private Dictionary<Direction, List<Rectangle>> animationFrames;
        private int currentFrame;
        private float frameTimer;
        private Weapon weapon;
        new enum Direction
        {
            Left,
            Right
        }

        public Enemy(Texture2D[] textures, Vector2 position, Texture2D texture, Texture2D weaponRight, Texture2D weaponLeft, Texture2D bulletTexture) 
            : base(texture)
        {
            _texture = texture;
            this.textures = textures;
            this.Position = position;
            _direction = new Vector2(-1, 0);
            int frameWidth = 180 / 4; // Ancho del frame del spritesheet
            int frameHeight = 87; // Alto del frame del spritesheet

            animationFrames = new Dictionary<Direction, List<Rectangle>>();
            animationFrames[Direction.Left] = new List<Rectangle>();
            animationFrames[Direction.Right] = new List<Rectangle>();

            frameWidth = 180 / 4;
            frameHeight = 87;

            for (int i = 0; i < 3; i++)
            {
                animationFrames[Direction.Left].Add(new Rectangle(i * frameWidth, 0, frameWidth, frameHeight));
                animationFrames[Direction.Right].Add(new Rectangle(i * frameWidth, 0, frameWidth, frameHeight));
            }

            weapon = new Weapon(weaponRight, weaponLeft, bulletTexture);
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            if (!_isAlive) return;

            // Mover enemigo
            Position += _direction * _speed;
        }


        public void Hit()
        {
            _isAlive = false;
            IsRemoved = true; // Marca el enemigo como eliminado
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_isAlive)
            {
                base.Draw(spriteBatch); // Dibuja el enemigo solo si está vivo
            }
        }
    }
}