using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DJGame.Models.Agents;
using Microsoft.Xna.Framework;

namespace DJGame.Models.Game
{
    /// <summary>
    /// Classe généré par ChatGPT pour la gestion de la caméra
    /// </summary>
    public class Camera2D
    {
        public Matrix Transform { get; private set; }
        public Vector2 Position { get; private set; }

        private float upperLimit; // Y de la ligne du haut
        private float lowerLimit; // Y de la ligne du bas

        public Camera2D(int screenHeight)
        {
            // On définit la zone safe (par ex. 1/3 haut et bas de l’écran)
            upperLimit = screenHeight *1/4;
            lowerLimit = screenHeight * 2/3;
        }

        public void Follow(Player player)
        {
            float playerScreenY = player.Position.Y - Position.Y;

            // Joueur dépasse en haut → on monte la caméra
            if (playerScreenY < upperLimit)
            {
                Position = new Vector2(Position.X, player.Position.Y - upperLimit);
            }
            // Joueur dépasse en bas → on descend la caméra
            else if (playerScreenY > lowerLimit)
            {
                Position = new Vector2(Position.X, player.Position.Y - lowerLimit);
            }

            // Met à jour la matrice de transformation
            Transform = Matrix.CreateTranslation(new Vector3(-Position, 0));
        }
    }

}
