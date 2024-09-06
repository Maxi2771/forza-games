using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParaTesting;
using System.Collections.Generic;

public class Weapon
{
    public Texture2D Texture;
    public Vector2 Offset;  // Desplazamiento respecto a la posición del jugador
    public bool IsFiring;
    private Vector2 playerPosition;

    public Weapon(Texture2D texture, Vector2 offset)
    {
        Texture = texture;
        Offset = offset;
        IsFiring = false;
    }

    // Método para disparar proyectiles
    public void Fire(List<Projectile> projectiles, Texture2D projectileTexture, Vector2 playerPosition)
    {
        if (IsFiring)
        {
            // Posicionar el proyectil en la punta del arma
            Vector2 projectilePosition = playerPosition + Offset;

            // Crear un nuevo proyectil y añadirlo a la lista
            Projectile newProjectile = new Projectile(projectilePosition, projectileTexture, new Vector2(10, 0)); // Velocidad en X
            projectiles.Add(newProjectile);
        }
    }

    // Actualiza la posición del arma en relación con el jugador
    public void Update(Vector2 playerPosition)
    {
        // La posición del arma se ajusta al jugador
    }

    // Dibujar el arma
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, playerPosition + Offset, Color.White); // Dibuja el arma con el offset
    }
}
