using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetI320
{
    internal class Ennemy
    {
        private Texture2D _texture;
        private Vector2 _position;
        private Vector2 _vitesse;
        private Rectangle _rectangle;

        private int largeur = 40;
        private int hauteur = 40;

        private bool estVivant = true;
        public bool EstVivant
        {
            get { return estVivant; }
        }
        public void Kill()
        {
            estVivant = false;
        }

        public Rectangle Rectangle
        {
            get { return _rectangle; }
        }

        public Ennemy(Texture2D texture, Vector2 positionInitiale, Vector2 direction)
        {
            _texture = texture;
            _position = positionInitiale;
            _vitesse = direction * 1;
            _rectangle = new Rectangle((int)_position.X, (int)_position.Y, largeur, hauteur);
        }

        public void Update()
        {
            _position += _vitesse;
            _rectangle.X = (int)_position.X;
            _rectangle.Y = (int)_position.Y;

            if (_position.X < -largeur || _position.X > 800 || _position.Y < -hauteur || _position.Y > 600)
                estVivant = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _rectangle, Color.White);
        }
    }
}
