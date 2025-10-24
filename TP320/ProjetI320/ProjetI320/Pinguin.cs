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
        public int PV = 1;
        private Vector2 _position = new Vector2(100, 200);
        private Vector2 _vitesse = new Vector2(2, 0);
        private Texture2D _texture; //image du pinguin
        private Rectangle _rectangle;
        private int _largeur = 30;
        private int _hauteur = 30;

        //attibuts du pinguin pour le projetile
        public List<Projectile> projectileList = new List<Projectile>();
        public Texture2D textureProjetctile;
        private double tempsDepuisDernierTir = 0;
        public double delaiEntreTirs = 700;//temps en millisecondes

        //direction actuelle du pingouin
        private Vector2 derniereDirection = new Vector2(0, -1); //par defaut vers le haut

        public Rectangle Rectangle
        {
            get { return _rectangle; }
        }

        //constructeur du pinguin
        public Pinguin(Texture2D texture, Vector2 positionInitiale)
        {
            _texture = texture;
            _position = positionInitiale;
            _vitesse = Vector2.Zero;

            _rectangle = new Rectangle((int)_position.X, (int)_position.Y, _largeur, _hauteur);
        }

        //methode update qui gere le deplacement et les limites
        public void Update (GameTime frame)
        {
            KeyboardState clavier = Keyboard.GetState();
            _vitesse = Vector2.Zero;

            //déplacement du pinguin
            if (clavier.IsKeyDown(Keys.W)) { _vitesse.Y = -2; derniereDirection = new Vector2(0, -1); }
            if (clavier.IsKeyDown(Keys.S)) { _vitesse.Y = +2; derniereDirection = new Vector2(0, 1); }
            if (clavier.IsKeyDown(Keys.A)) { _vitesse.X = -2; derniereDirection = new Vector2(-1, 0); }
            if (clavier.IsKeyDown(Keys.D)) { _vitesse.X = +2; derniereDirection = new Vector2(1, 0); }


            //gestion des limites de l'ecran
            _position += _vitesse;
            _position.X = MathHelper.Clamp(_position.X, 0, 800 - _largeur);
            _position.Y = MathHelper.Clamp(_position.Y, 0, 600 - _largeur);

            _rectangle.X = (int)_position.X;
            _rectangle.Y = (int)_position.Y;

            //tir
            tempsDepuisDernierTir += frame.ElapsedGameTime.TotalMilliseconds;

            if (clavier.IsKeyDown(Keys.Space) && tempsDepuisDernierTir >= delaiEntreTirs)
            {
                //calculer la position initiale selon la direction
                Vector2 posProjectile = _position; // valeur par défaut
                int projectileLargeur = 25;
                int projectileHauteur = 25;

                if (derniereDirection == new Vector2(0, -1)) //haut
                    posProjectile = new Vector2(_position.X + _largeur / 2 - projectileLargeur / 2, _position.Y - projectileHauteur);
                else if (derniereDirection == new Vector2(0, 1)) //bas
                    posProjectile = new Vector2(_position.X + _largeur / 2 - projectileLargeur / 2, _position.Y + _hauteur);
                else if (derniereDirection == new Vector2(-1, 0)) //gauche
                    posProjectile = new Vector2(_position.X - projectileLargeur, _position.Y + _hauteur / 2 - projectileHauteur / 2);
                else if (derniereDirection == new Vector2(1, 0)) //droite
                    posProjectile = new Vector2(_position.X + _largeur, _position.Y + _hauteur / 2 - projectileHauteur / 2);

                Projectile nouveauProjectile = new Projectile(textureProjetctile, posProjectile, derniereDirection);
                projectileList.Add(nouveauProjectile);
                tempsDepuisDernierTir = 0;
            }

            //mettre a jour tous les projectiles
            foreach (Projectile projectile in projectileList)
            {
                projectile.Update();
            }

            //supprimer les projectiles qui sont hors de l'ecran
            projectileList.RemoveAll(p => !p.EstActif);


        }

        //methode pour afficher le pinguin a l'ecran
        public void Draw(SpriteBatch spriteBatch)
        {
            //dessiner le pinguin
            spriteBatch.Draw(_texture, _rectangle, Color.White);

            //dessiner tous les projectiles
            foreach (Projectile projectile in projectileList)
            {
                projectile.Draw(spriteBatch);
            }
        }

        //methode pour afficher le projectile
        public void SetProjectileTexture(Texture2D texture)
        {
            textureProjetctile = texture;
        }

    }
}
