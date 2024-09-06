using alpha_0_2.Game;
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
    public class Weapon : Sprite
    {
        public Bullet Bullet;

        public Weapon(Texture2D texture)
          : base(texture)
        {
            // Establecemos una dirección predeterminada (ejemplo: hacia la derecha)
            Direction = new Vector2(1, 0); // Cambia esto según la dirección que quieras
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            _previousKey = _currentKey;
            _currentKey = Keyboard.GetState();

            // Solo disparamos al presionar la tecla Espacio
            if (_currentKey.IsKeyDown(Keys.Space) &&
                _previousKey.IsKeyUp(Keys.Space))
            {
                AddBullet(sprites);
            }
        }

        private void AddBullet(List<Sprite> sprites)
        {
            var bullet = Bullet.Clone() as Bullet;

            // Calculamos la posición de la punta del arma
            Vector2 bulletStartPosition = Position + Direction * (_texture.Width / 2);

            // Configuramos la dirección y posición inicial de la bala
            bullet.Direction = this.Direction; // Usamos la dirección del arma
            bullet.Position = bulletStartPosition; // Posición inicial desde la punta del arma
            bullet.LinearVelocity = 8f; // Velocidad de la bala (ajusta según sea necesario)
            bullet.LifeSpan = 2f; // Tiempo de vida de la bala
            bullet.Parent = this;

            sprites.Add(bullet);
        }
    }
}
