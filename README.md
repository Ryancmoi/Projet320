# ProjetI320

Petit jeu vidéo en 2D fait avec **MonoGame** (C#), dans le cadre du cours I320.

Le joueur contrôle un pingouin au centre de l'écran. Des orques (ennemis) apparaissent depuis les bords de l'écran et avancent vers le centre. Le pingouin peut se déplacer et lancer des boules de neige pour les éliminer. Si un ennemi touche le pingouin, la partie se termine (Game Over), et on peut relancer une nouvelle partie.

## Contrôles

- **W / A / S / D** : déplacer le pingouin
- **Espace** : tirer une boule de neige (dans la dernière direction empruntée)
- **Escape** : quitter le jeu
- **R** : relancer la partie (à l'écran Game Over)

## Ce qui a été mis en pratique

- **Programmation orientée objet** : le jeu est divisé en classes qui représentent chacune un élément du jeu :
  - `Game1` : la classe principale qui gère la boucle de jeu (chargement, mise à jour, affichage)
  - `Pinguin` : le personnage du joueur (déplacement, tir, gestion des projectiles)
  - `Ennemy` : les orques qui se déplacent vers le pingouin
  - `Projectile` : les boules de neige tirées par le pingouin
- **Encapsulation** : les attributs (position, vitesse, état vivant/actif, etc.) sont privés et exposés au besoin via des propriétés (`Rectangle`, `EstVivant`, `EstActif`)
- **Utilisation de vecteurs** (`Vector2`) : pour représenter les positions et les directions de déplacement de tous les objets du jeu (pingouin, ennemis, projectiles)
- **Listes génériques** (`List<T>`) : pour gérer dynamiquement le nombre d'ennemis et de projectiles présents à l'écran
- **Détection de collisions** : avec `Rectangle.Intersects()` pour détecter les impacts entre les projectiles et les ennemis, ainsi qu'entre les ennemis et le pingouin
- **Gestion d'un état de jeu** (`enum GameState`) : pour distinguer l'état "en jeu" de l'état "game over"

## Technologies

- C#
- [MonoGame](https://www.monogame.net/)

## Lancer le projet

Ouvrir `ProjetI320.sln` dans Visual Studio et lancer le projet (F5).
