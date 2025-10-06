using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetI320
{
    internal class Pinguin
    {
        //attributs proppre au pinguin
        public int PV = 3;
        private Vector2 position = new Vector2(100, 200);
        private Vector2 vitesse = new Vector2(2, 0);
        private Texture2D _texture; //image du pinguin
        private Rectangle rectangle;
        private int largeur = 50;
        private int hauteur = 50;

        //attibuts du pinguin pour le projetile
        public List<Projectile> points = new List<Projectile>();
        public Texture2D textureProjetctile;
        private double tempsDepuisDernierTir = 0;
        public double delaiEntreTirs = 300;//temps en millisecondes

        // Direction actuelle du pingouin
        private Vector2 derniereDirection = new Vector2(0, -1); // Par défaut vers le haut

        public Pinguin(Texture2D texture, Vector2 positionInitiale)
        {
            _texture = texture;
            position = positionInitiale;
            vitesse = Vector2.Zero;

            rectangle = new Rectangle((int)position.X, (int)position.Y, largeur, hauteur);
        }

        public void Update (GameTime frame)
        {
            KeyboardState clavier = Keyboard.GetState();
            vitesse = Vector2.Zero;

            //Déplacement du pinguin
            if (clavier.IsKeyDown(Keys.W)) vitesse.Y = -2;
            if (clavier.IsKeyDown(Keys.S)) vitesse.Y = +2;
            if (clavier.IsKeyDown(Keys.A)) vitesse.X = -2;
            if (clavier.IsKeyDown(Keys.D)) vitesse.X = +2;

            //Gestion des limites de l'écran
            position += vitesse;
            position.X = MathHelper.Clamp(position.X, 0, 800 - largeur);
            position.Y = MathHelper.Clamp(position.Y, 0, 600 - largeur);

            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y;

            //Tir

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, rectangle, Color.White);
        }

        public void SetProjectileTexture(Texture2D texture)
        {
            textureProjetctile = texture;
        }

    }
}
