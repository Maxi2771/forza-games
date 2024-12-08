using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alpha_0_2.Sprites
{
    public class Sprite : Component, ICloneable
    {
        protected Texture2D _texture;

        protected Texture2D[] _textureArr;

        protected float _rotation;

        protected KeyboardState _currentKey;

        protected KeyboardState _previousKey;

        public Vector2 Position { get; set; }

        public Vector2 Origin;

        public Vector2 Direction;

        public float RotationVelocity = 3f;

        public float LinearVelocity = 4f;

        public Sprite Parent;

        public float LifeSpan = 0f;

        public bool IsRemoved = false;



        public Rectangle Rectangle
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height); }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Color.White);
        }

        public Sprite(Texture2D texture)
        {
            _texture = texture;

            // The default origin in the centre of the sprite
            Origin = new Vector2(128, 64 / 2);
        }

        //public Sprite() { }

        public Sprite(Texture2D[] texture)
        {
            _textureArr = texture;

            // The default origin in the centre of the sprite
            Origin = new Vector2(128, 64 / 2);
        }

        public virtual void Update(GameTime gameTime, List<Sprite> sprites)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Color.White, _rotation, Origin, 1, SpriteEffects.None, 0);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}