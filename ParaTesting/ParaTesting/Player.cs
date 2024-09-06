using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Player
{
    public Vector2 Position;
    public Texture2D PlayerTexture;
    public Weapon Weapon;

    public Player(Vector2 startPosition, Texture2D playerTexture, Weapon weapon)
    {
        Position = startPosition;
        PlayerTexture = playerTexture;
        Weapon = weapon;
    }

    // Actualizar el jugador: manejar el movimiento y el arma
    public void Update(GameTime gameTime, KeyboardState keyboardState)
    {
        // Movimiento simple del jugador
        if (keyboardState.IsKeyDown(Keys.Right))
        {
            Position.X += 5f;  // Mueve a la derecha
        }
        if (keyboardState.IsKeyDown(Keys.Left))
        {
            Position.X -= 5f;  // Mueve a la izquierda
        }

        // Actualizar la posición del arma
        Weapon.Update(Position);
    }

    // Dibujar al jugador y su arma
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(PlayerTexture, Position, Color.White);  // Dibuja al jugador
        Weapon.Draw(spriteBatch);  // Dibuja el arma
    }
}
