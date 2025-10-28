using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjetI320
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D pinguinTexture;
        private Pinguin pinguin1;
        private Texture2D snowBallTexture;

        private Texture2D enemyTexture;
        private List<Ennemy> enemies = new List<Ennemy>();
        private Random random = new Random();
        private double tempsDepuisDernierSpawn = 0;
        private double delaiEntreSpawns = 300;

        private int score = 0;
        private SpriteFont font;

        private Texture2D obstacleTexture;
        private List<Obstacle> obstacles = new List<Obstacle>();

        private void SpawnInitialEnemies(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Vector2 position;
                Vector2 direction;

                int cote = random.Next(4);
                int largeurEcran = 800;
                int hauteurEcran = 600;
                int spriteLargeur = 40;
                int spriteHauteur = 40;

                switch (cote)
                {
                    case 0: // haut
                        position = new Vector2(random.Next(0, largeurEcran), -spriteHauteur);
                        direction = new Vector2(0, 1);
                        break;
                    case 1: // bas
                        position = new Vector2(random.Next(0, largeurEcran), hauteurEcran - spriteHauteur);
                        direction = new Vector2(0, -1);
                        break;
                    case 2: // gauche
                        position = new Vector2(-spriteLargeur, random.Next(0, hauteurEcran));
                        direction = new Vector2(1, 0);
                        break;
                    default: // droite
                        position = new Vector2(largeurEcran - spriteLargeur, random.Next(0, hauteurEcran));
                        direction = new Vector2(-1, 0);
                        break;
                }

                enemies.Add(new Ennemy(enemyTexture, position, direction));
            }
        }

        enum GameState
        {
            Playing,
            GameOver
        }
        private GameState currentState = GameState.Playing;
        private void RestartGame()
        {
            score = 0;
            enemies.Clear();
            SpawnInitialEnemies(10);
            pinguin1.projectileList.Clear();
            pinguin1.ResetPosition();
            currentState = GameState.Playing;
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        //methode qui charge le contenu au lancement
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("Arial");

            Vector2 startPosition = new Vector2(385, 250);
            pinguinTexture = Content.Load<Texture2D>("PinguinPixelArt");
            pinguin1 = new Pinguin(pinguinTexture, startPosition);

            snowBallTexture = Content.Load<Texture2D>("SnowBall");
            pinguin1.SetProjectileTexture(snowBallTexture);

            enemyTexture = Content.Load<Texture2D>("Orca");

            obstacleTexture = Content.Load<Texture2D>("Iceberg");

            obstacles.Add(new Obstacle(obstacleTexture, new Vector2(150, 150)));
            obstacles.Add(new Obstacle(obstacleTexture, new Vector2(490, 80)));
            obstacles.Add(new Obstacle(obstacleTexture, new Vector2(600, 250)));
            obstacles.Add(new Obstacle(obstacleTexture, new Vector2(170, 320)));
            obstacles.Add(new Obstacle(obstacleTexture, new Vector2(430, 350)));


            //cree 10 ennemis au hasard des le début
            SpawnInitialEnemies(10);

        }

        //methode update qui s'execute chaque seconde
        protected override void Update(GameTime gameTime)
        {
            if (currentState == GameState.GameOver)
            {
                KeyboardState clavier = Keyboard.GetState();

                //relance le jeu si R est appuyé
                if (clavier.IsKeyDown(Keys.R))
                {
                    RestartGame();
                }

                //quitte le jeu si Escape est appuyé
                if (clavier.IsKeyDown(Keys.Escape))
                {
                    Exit();
                }

                return; //stoppe Update si GameOver
            }


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            pinguin1.Update(gameTime);

            tempsDepuisDernierSpawn += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (tempsDepuisDernierSpawn >= delaiEntreSpawns)
            {
                tempsDepuisDernierSpawn = 0;

                //choisir un coté au hasard 0=haut 1=bas 2=gauche 3=droite
                int cote = random.Next(4);
                Vector2 position = Vector2.Zero;
                Vector2 direction = Vector2.Zero;

                int largeurEcran = 800;
                int hauteurEcran = 600;

                int spriteLargeur = 40;
                int spriteHauteur = 40;

                switch (cote)
                {
                    case 0: //haut
                        position = new Vector2(random.Next(0, largeurEcran), -spriteHauteur);
                        direction = new Vector2(0, 1);
                        break;
                    case 1: //bas
                        position = new Vector2(random.Next(0, largeurEcran), hauteurEcran - spriteHauteur);
                        direction = new Vector2(0, -1);
                        break;
                    case 2: //gauche
                        position = new Vector2(-spriteLargeur, random.Next(0, hauteurEcran));
                        direction = new Vector2(1, 0);
                        break;
                    case 3: //droite
                        position = new Vector2(largeurEcran - spriteLargeur, random.Next(0, hauteurEcran));
                        direction = new Vector2(-1, 0);
                        break;
                }

                Ennemy nouvelEnnemy = new Ennemy(enemyTexture, position, direction);
                enemies.Add(nouvelEnnemy);
            }
            foreach (Ennemy ennemy in enemies)
            {
                ennemy.Update();
            }

            //collision projectile/ennemis
            foreach (Projectile projectile in pinguin1.projectileList.ToList())
            {
                foreach (Ennemy ennemy in enemies.ToList())
                {
                    if (projectile.Rectangle.Intersects(ennemy.Rectangle))
                    {
                        projectile.Kill();
                        ennemy.Kill();
                        score += 1;
                    }
                }
            }

            //collision ennemis/pinguin
            foreach (Ennemy ennemi in enemies)
            {
                foreach (var ennemy in enemies.ToList())
                {
                    if (pinguin1.Rectangle.Intersects(ennemy.Rectangle))
                    {
                        currentState = GameState.GameOver;
                        break;
                    }
                }
            }

            enemies.RemoveAll(e => !e.EstVivant);
            pinguin1.projectileList.RemoveAll(p => !p.EstActif);

            //xollision projectile/obstacle
            foreach (Obstacle obstacle in obstacles)
            {
                foreach (Projectile projectile in pinguin1.projectileList.ToList())
                {
                    if (projectile.Rectangle.Intersects(obstacle.Rectangle))
                    {
                        projectile.Kill();
                    }
                }
            }

            //collision ennemis/obstacle
            foreach (Obstacle obstacle in obstacles)
            {
                foreach (Ennemy ennemy in enemies.ToList())
                {
                    if (ennemy.Rectangle.Intersects(obstacle.Rectangle))
                    {
                        ennemy.Kill();
                    }
                }
            }

            //collision joueur/obstacle
            foreach (Obstacle obstacle in obstacles)
            {
                if (pinguin1.Rectangle.Intersects(obstacle.Rectangle))
                {
                    currentState = GameState.GameOver;
                }
            }

            base.Update(gameTime);
        }

        //methode pour afficher des elements a l'ecran
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            if (currentState == GameState.Playing)
            {
                //dessiner le jeu normalement
                pinguin1.Draw(_spriteBatch);

                foreach (Obstacle obstacle in obstacles)
                    obstacle.Draw(_spriteBatch);

                foreach (Ennemy ennemi in enemies)
                    ennemi.Draw(_spriteBatch);

                _spriteBatch.DrawString(font, "Score: " + score, new Vector2(10, 10), Color.White);
            }
            else if (currentState == GameState.GameOver)
            {
                string textScore = "Votre score : " + score;
                string textGameOver = "GAME OVER\nAppuyez sur R pour relancer\nAppuyez sur Escape pour quitter";

                //mesurer la taille totale pour centrer
                Vector2 sizeGameOver = font.MeasureString(textGameOver);
                Vector2 sizeScore = font.MeasureString(textScore);

                //positionner le score au dessus de GameOver
                Vector2 positionScore = new Vector2(400 - sizeScore.X / 2, 300 - sizeGameOver.Y / 2 - 30);
                Vector2 positionGameOver = new Vector2(400 - sizeGameOver.X / 2, 300 - sizeGameOver.Y / 2);

                _spriteBatch.DrawString(font, textScore, positionScore, Color.White);
                _spriteBatch.DrawString(font, textGameOver, positionGameOver, Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
