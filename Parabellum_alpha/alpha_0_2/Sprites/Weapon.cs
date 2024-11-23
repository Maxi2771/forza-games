using alpha_0_2.Game;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace alpha_0_2.Sprites
{
    public class Weapon : Sprite
    {
        public Bullet[] Cargador;
        private Texture2D _textureRight;
        private Texture2D _textureLeft;

        public Weapon(Texture2D textureRight, Texture2D textureLeft, Texture2D bullet)
            : base(textureRight) // Comenzamos con la textura de la derecha por defecto
        {
            _textureRight = textureRight;
            _textureLeft = textureLeft;


            // Dirección predeterminada (hacia la derecha)
            Direction = new Vector2(1, 0);
        }

        public override void Update(GameTime gameTime)
        {
            _previousKey = _currentKey;
            _currentKey = Keyboard.GetState();

            // Cambiar la textura según la dirección del jugador
            if (_currentKey.IsKeyDown(Keys.Right)) // Derecha
            {
                _texture = _textureRight;
                Direction = new Vector2(1, 0);
            }
            else if (_currentKey.IsKeyDown(Keys.Left)) // Izquierda
            {
                _texture = _textureLeft;
                Direction = new Vector2(-1, 0);
            }
        }
    }
}
