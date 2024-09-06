using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace ParaTesting.Sprites
{
    public class Weapon : Sprite
    {
        public Bullet Bullet;

        public Weapon(Texture2D texture)
            : base(texture)
        {

        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            // Seguimiento del mouse
            Vector2 mousePosition = new Vector2(_currentMouseState.X, _currentMouseState.Y);

            // Calcula la dirección desde el arma hacia el mouse
            Direction = mousePosition - this.Position;
            Direction.Normalize();

            // Calcula la rotación del arma hacia el mouse
            _rotation = (float)Math.Atan2(Direction.Y, Direction.X);

            // Solo se dispara si se presiona el botón izquierdo del mouse
            if (_currentMouseState.LeftButton == ButtonState.Pressed &&
                _previousMouseState.LeftButton == ButtonState.Released)
            {
                AddBullet(sprites);
            }
        }

        private void AddBullet(List<Sprite> sprites)
        {
            var bullet = Bullet.Clone() as Bullet;
            bullet.Direction = this.Direction;
            bullet.Position = this.Position;
            bullet.LinearVelocity = this.LinearVelocity * 2;
            bullet.LifeSpan = 2f;
            bullet.Parent = this;
            //bullet.Rotation = this._rotation;  // Alinea la rotación del proyectil con el arma

            sprites.Add(bullet);
        }

        private MouseState _previousMouseState, _currentMouseState;
    }
}
