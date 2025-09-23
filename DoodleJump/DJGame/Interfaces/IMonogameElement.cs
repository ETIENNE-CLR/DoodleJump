using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DJGame.Interfaces
{
    /// <summary>
    /// Interface qui me permet d'implémenter un élement monogame
    /// </summary>
    public interface IMonogameElement
    {
        /// <summary>
        /// Méthode qui permet de charger le contenu de la classe
        /// </summary>
        /// <param name="content">Content qui chargera les textures</param>
        public void LoadContent(ContentManager content);

        /// <summary>
        /// Méthode Update de la logique de l'élement
        /// </summary>
        /// <param name="gameTime">Element gametime</param>
        public void Update(GameTime gameTime);

        /// <summary>
        /// Méthode pour dessiner l'élement
        /// </summary>
        /// <param name="spriteBatch">Element SpriteBatch pour dessiner</param>
        /// <param name="gameTime">Element gametime</param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
