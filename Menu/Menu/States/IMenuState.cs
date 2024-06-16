using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Menu.States
{
    public interface IMenuState
    {
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        void PostUpdate(GameTime gameTime);
        void Update(GameTime gameTime);
    }
}