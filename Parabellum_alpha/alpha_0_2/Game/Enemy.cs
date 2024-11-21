using alpha_0_2.Game.States;
using alpha_0_2.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alpha_0_2.Game
{
    public class Enemy
    {
        private GameState root;
        private Vector2 position;
        private Vector2 velocity;
        private Texture2D spriteImage;
        //private float frameWidth = 180 / 4;
        private int health = 1;

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        private Dictionary<Direction, List<Rectangle>> animationFrames;
        private int frameWidth;
        private int frameHeight;
        private int currentFrame;
        private float frameTimer;

        public Rectangle PositionRectangle
        {
            get { return new Rectangle((int)position.X, (int)position.Y, (int)frameWidth, (int)frameHeight); }
        }

        public Enemy(GameState root, Vector2 position)
        {
            this.root = root;
            this.position = position;
            //this.frameWidth = 300f;
            this.velocity = new Vector2(-1.0f, 5.0f);

            animationFrames = new Dictionary<Direction, List<Rectangle>>();
            animationFrames[Direction.Up] = new List<Rectangle>();
            animationFrames[Direction.Down] = new List<Rectangle>();
            animationFrames[Direction.Left] = new List<Rectangle>();
            animationFrames[Direction.Right] = new List<Rectangle>();

            frameWidth = 180 / 4; // Ancho del frame del spritesheet
            frameHeight = 87; // Alto del frame del spritesheet

            for (int i = 0; i < 3; i++)
            {
                animationFrames[Direction.Up].Add(new Rectangle(i * frameWidth, 0, frameWidth, frameHeight));
                animationFrames[Direction.Down].Add(new Rectangle(i * frameWidth, 0, frameWidth, frameHeight));
                animationFrames[Direction.Left].Add(new Rectangle(i * frameWidth, 0, frameWidth, frameHeight));
                animationFrames[Direction.Right].Add(new Rectangle(i * frameWidth, 0, frameWidth, frameHeight));
            }
            currentFrame = 0;
            frameTimer = 0;

            LoadContent();
        }

        public void LoadContent()
        {
            this.spriteImage = root.Content.Load<Texture2D>("EnemyLeft");
            this.spriteImage = root.Content.Load<Texture2D>("EnemyRight");
        }

        public void Update(GameTime gameTime)
        {
            position += velocity;
            if(position.Y < 0 || position.Y > (root.ScreenHeight - frameHeight))
            {
                velocity.Y *= -1;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteImage, PositionRectangle, Color.White);
        }
    }
}
