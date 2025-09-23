using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DJGame.Models
{
    internal class Animation
    {
        // Champs de la classe
        private List<Rectangle> frames;
        private float frameDuration;
        private bool isLooping;
        private int currentFrameIndex;
        private double timer;
        private float speedEntity;

        // Propriétés de la classe
        public List<Rectangle> Frames { get => frames; }
        public float FrameDuration { get => frameDuration; }
        public Rectangle CurrentFrame => frames[currentFrameIndex];
        public float SpeedEntity { get => speedEntity; }

        /// <summary>
        /// Constructeur de la classe...
        /// </summary>
        /// <param name="frames">Le recueil de toutes les Rectangles (animations) de l'animation</param>
        /// <param name="frameDuration">Temps que prend le moteur de jeu pour changer d'animation</param>
        /// <param name="speedEntity">Le temps que met l'animation pour son mouvement (si on souhaite une syncronisation en fonction des mouvement de l'élement)</param>
        /// <param name="isLooping">Si l'animation doit être jouée en boucle</param>
        /// <exception cref="ArgumentNullException"></exception>
        public Animation(List<Rectangle> frames, float frameDuration, float speedEntity, bool isLooping = true)
        {
            this.frames = frames ?? throw new ArgumentNullException(nameof(frames));
            this.frameDuration = frameDuration;
            this.speedEntity = speedEntity;
            this.isLooping = isLooping;
        }

        // Méthodes de la classe...
        public void AjouterFrame(Rectangle frame, int indexToInsert = -1)
        {
            if (indexToInsert < 0)
                frames.Add(frame);
            else
                frames.Insert(indexToInsert, frame);
        }
        public void AjouterFrame(List<Rectangle> frames, int indexToInsert = -1)
        {
            if (indexToInsert < 0)
                this.frames.AddRange(frames);
            else
                this.frames.InsertRange(indexToInsert, frames);
        }
        // ---------------------------------------
        public void AjouterFrame(Rectangle frame, List<int> indexesToInsert)
        {
            foreach (int index in indexesToInsert)
            {
                this.frames.Insert(index, frame);
            }
        }
        public void AjouterFrame(List<Rectangle> frames, List<int> indexesToInsert)
        {
            foreach (int index in indexesToInsert)
            {
                this.frames.InsertRange(index, frames);
            }
        }

        public void Update(double deltaTime)
        {
            timer += deltaTime;
            if (timer >= frameDuration)
            {
                timer -= frameDuration;
                currentFrameIndex++;
                if (currentFrameIndex >= frames.Count)
                    currentFrameIndex = isLooping ? 0 : frames.Count - 1;
            }
        }

        public void Restart()
        {
            timer = 0;
            currentFrameIndex = 0;
        }

        /// <summary>
        /// Méthode qui permet de récupérer l'animation d'après plusieurs paramètres.
        /// Ces paramètres doivent être déterminés par ce site web : https://is.si/animator/
        /// </summary>
        /// <param name="spriteWidth">La largeur du sprite</param>
        /// <param name="spriteHeight">La hauteur du sprite</param>
        /// <param name="startX">Où est ce que ça commence dans le spritesheet en X</param>
        /// <param name="startY">Où est ce que ça commence dans le spritesheet en Y</param>
        /// <param name="spacingX">L'espacement des sprites horizontalement</param>
        /// <param name="spacingY">L'espacement des sprites verticalement</param>
        /// <param name="frameCount">Le nombre de frame de l'animation</param>
        /// <returns>La liste des frame de l'animation</returns>
        public static List<Rectangle> GenerateAnimation(int spriteWidth, int spriteHeight, int startX, int startY, int spacingX, int spacingY, int frameCount)
        {
            List<Rectangle> frames = new List<Rectangle>();

            for (int i = 0; i < frameCount; i++)
            {
                int x = startX + i * (spriteWidth + spacingX);
                int y = startY;
                frames.Add(new Rectangle(x, y, spriteWidth, spriteHeight));
            }
            return frames;
        }
    }
}
