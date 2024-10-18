using alpha_0_2.Game;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace alpha_0_2.Sprites
{
    public class Weapon : Sprite
    {
        public Bullet Bullet;
        private Texture2D _textureRight;
        private Texture2D _textureLeft;
        private int ammo = 15; // Munición inicial

        public Weapon(Texture2D textureRight, Texture2D textureLeft)
            : base(textureRight) // Comenzamos con la textura de la derecha por defecto
        {
            _textureRight = textureRight;
            _textureLeft = textureLeft;

            // Dirección predeterminada (hacia la derecha)
            Direction = new Vector2(1, 0);
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
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

            // Solo disparamos al presionar la tecla Espacio y si hay munición
            if (_currentKey.IsKeyDown(Keys.Space) &&
                _previousKey.IsKeyUp(Keys.Space) &&
                ammo > 0) // Verificar que haya munición
            {
                // Verificamos que el objeto Bullet no sea null antes de disparar
                if (Bullet != null)
                {
                    AddBullet(sprites);
                }
            }
        }

        private void AddBullet(List<Sprite> sprites)
        {
            if (ammo <= 0) // Si no hay munición, no hacemos nada
                return;

            var bullet = Bullet.Clone() as Bullet;

            // Verificar si el clon de bullet no es null
            if (bullet == null)
                return;

            // Calcular la posición de la punta del arma
            Vector2 weaponTipPosition = this.Position;

            // Ajustar la posición de la punta del arma según la dirección en la que está mirando
            if (Direction == new Vector2(1, 0)) // Derecha
            {
                weaponTipPosition.X += _texture.Width; // Suma el ancho del arma
            }
            else if (Direction == new Vector2(-1, 0)) // Izquierda
            {
                weaponTipPosition.X -= 1; // Resta el ancho del arma
            }
            else if (Direction == new Vector2(0, -1)) // Arriba
            {
                weaponTipPosition.Y -= _texture.Height; // Resta el alto del arma
            }
            else if (Direction == new Vector2(0, 1)) // Abajo
            {
                weaponTipPosition.Y += _texture.Height; // Suma el alto del arma
            }

            // Configuramos la dirección y posición inicial de la bala
            bullet.Direction = this.Direction; // Usamos la dirección del arma
            bullet.Position = weaponTipPosition; // Posición inicial desde la punta del arma
            bullet.LinearVelocity = this.LinearVelocity * 2; // Velocidad de la bala (ajusta según sea necesario)
            bullet.LifeSpan = 2f; // Tiempo de vida de la bala
            bullet.Parent = this;

            sprites.Add(bullet);
            ammo--; // Resta la munición
        }
    }
}
