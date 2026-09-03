using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetI320
{
    internal class Obstacle
    {
        private Texture2D _texture;
        private Vector2 _position;
        private Rectangle _rectangle;
        private int _largeur = 50;
        private int _hauteur = 50;

        public Rectangle Rectangle
        {
            get { return _rectangle; }
        }

        public Obstacle(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            _position = position;
            _rectangle = new Rectangle((int)_position.X, (int)_position.Y, _largeur, _hauteur);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _rectangle, Color.White);
        }
    }
}
