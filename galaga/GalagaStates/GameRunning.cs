using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.State;
using DIKUArcade.Timers;
using galaga;
using galaga.Squadron;
using galaga.MovementStrategy;

namespace galaga.GalagaStates{

    public class GameRunning : IGameState
    {
        private static GameRunning instance = null;

        private  readonly Player player;
        private List<Image> enemyStrides;

        private  readonly Score score;
        private List<PlayerShot> playerShots;
        private SquiggleSquadron squiggleSquadron;

        private List<Image> explosionStrides;

        private AnimationContainer explosions;
        private int explosiveLength = 500;

        private NoMove noMove;
        private Down down;
        private ZigZagDown zigZagDown;
        private bool isGameOver;

    private Image bullet;

        public GameRunning(){

        isGameOver = false;
    
        player = new Player(new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
            new Image(Path.Combine("Assets", "Images", "Player.png")));
        score = new Score(new Vec2F(0.02f, 0.7f), new Vec2F(0.3f, 0.3f));
        enemyStrides = ImageStride.CreateStrides(4,
            Path.Combine("Assets", "Images", "BlueMonster.png"));
        squiggleSquadron = new SquiggleSquadron(6);
        AddEnemies();

        noMove = new NoMove();
        down = new Down();
        zigZagDown = new ZigZagDown();

        playerShots = new List<PlayerShot>();
        // Preloads the bullet image
        bullet = new Image(Path.Combine("Assets", "Images", "BulletRed2.png"));
        explosionStrides = ImageStride.CreateStrides(8, Path.Combine("Assets", "Images", "Explosion.png"));
        // Here the constructor is given the argument 6 since that is the total amount of enemies.  
        explosions = new AnimationContainer(6);
        //backGroundImage = new Entity(new StationaryShape(new Vec2F(0.2f, 0.2f), new Vec2F(0.5f, 0.5f)), new Image("Assets/Images/TitleImage.png"));
        GalagaBus.GetBus().Subscribe(GameEventType.MovementEvent, player);


        }

        public static GameRunning GetInstance()
        {
            return GameRunning.instance  ?? (GameRunning.instance = new GameRunning());
        }
        public void GameLoop()
        {
            throw new NotImplementedException();
        }

        public void InitializeGameState()
        {
            player.Entity.Shape.SetPosition(new Vec2F(0.45f, 0.1f));
            squiggleSquadron.Enemies.ClearContainer();
            score.resetScore();
            Console.WriteLine("GameRunning");

        }

        public void UpdateGameLogic()
        {
            player.Move();
            // Check if enemy has won
            squiggleSquadron.Enemies.Iterate(CheckIfEnemyHasWon);

            // See if difficulty should be increased and enemies created
            if (!isGameOver && squiggleSquadron.Enemies.CountEntities() <= 0) {
                zigZagDown.IncreaseSpeedBy(0.0001f);
                AddEnemies();
                }

                // Moves the shot
                IterateShot();
                //GalagaBus.GetBus().ProcessEvents();

        }

        public void RenderState()
        {
            // Render gameplay entities here
            // Render player object if game is not over
            if (!isGameOver)
                player.Entity.RenderEntity();
            score.RenderScore();
                
            foreach (var shot in playerShots) {
                shot.Image.Render(shot.Shape);
            }

            // Render all enemy objects
            zigZagDown.MoveEnemies(squiggleSquadron.Enemies);
            squiggleSquadron.Enemies.RenderEntities();
            explosions.RenderAnimations();
        }

        public void HandleKeyEvent(string keyValue, string keyAction)
        {

            switch (keyAction){
                case "KEY_PRESS":
                    switch(keyValue){
                        case "KEY_ESCAPE":
                            GalagaBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(GameEventType.WindowEvent, this,
                            "CLOSE_WINDOW", "", ""));
                            break;
                        case "KEY_H":
                            GalagaBus.GetBus().RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForAllProcessors(GameEventType.MovementEvent, this,
                                    "MOVE_RIGHT", "", ""));
                            break;
                        case "KEY_L":
                            GalagaBus.GetBus().RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForAllProcessors(GameEventType.MovementEvent, this,
                                    "MOVE_LEFT", "", ""));
                            break;
                        case "KEY_SPACE":
                            playerShots.Add(new PlayerShot(
                                new DynamicShape(
                                    new Vec2F(player.Entity.Shape.Position.X + 0.05f, player.Entity.Shape.Position.Y + 0.1f),
                                    new Vec2F(0.008f, 0.027f),
                                    new Vec2F(0.0f, 0.01f)
                                ),
                                bullet));
                            break;
                        case "KEY_P":
                            GalagaBus.GetBus().RegisterEvent(GameEventFactory<object>.CreateGameEventForAllProcessors(GameEventType.GameStateEvent, 
                            this, "CHANGE_STATE", "GAME_PAUSED", ""));
                            break;
                            }
                    break;
                case "KEY_RELEASE":
                    switch(keyValue){
                            case "KEY_H":
                                GalagaBus.GetBus().RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForAllProcessors(GameEventType.MovementEvent, this,
                                "MOVE_STOP", "", ""));
                                break;
                            case "KEY_L":
                                GalagaBus.GetBus().RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForAllProcessors(GameEventType.MovementEvent, this,
                                "MOVE_STOP", "", ""));
                            break;
                                } 
                            break;

            }
        }

            public void IterateShot() {
            foreach (var shot in playerShots) {
                shot.Shape.Move();
                if (shot.Shape.Position.Y > 1.0f) {
                    shot.DeleteEntity();
                }
                else {
                    foreach (Enemy enemy in squiggleSquadron.Enemies) {
                        var collision = CollisionDetection.Aabb(shot.Shape.AsDynamicShape(), enemy.Shape);
                        if (collision.Collision) {
                            AddExplosion(enemy.Shape.Position.X, enemy.Shape.Position.Y, enemy.Shape.Extent.X,
                                enemy.Shape.Extent.Y);
                            enemy.DeleteEntity();
                            shot.DeleteEntity();
                            score.AddPoint(1);
                        }
                    }
                }
            }

            List<PlayerShot> newPlayerShots = new List<PlayerShot>();
            foreach (var shot in playerShots) {
                if (!shot.IsDeleted()) {
                    newPlayerShots.Add(shot);
                }
            }

            playerShots = newPlayerShots;
        }


        public void AddExplosion(float posX, float posY, float extentX, float extentY) {
            explosions.AddAnimation(new StationaryShape(posX, posY, extentX, extentY), explosiveLength,
                new ImageStride(explosiveLength / 8, explosionStrides));
        }

    
    // Check if enemy has won by checking if enemy input
    // has reached the bottom of the screen, Position.Y <= 0
    private void CheckIfEnemyHasWon(Enemy enemy) {
        if (enemy.Shape.Position.Y <= 0) {
            GalagaBus.GetBus().RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(GameEventType.StatusEvent, this,
                    "GAME_OVER", "", ""));
            isGameOver = true;
            squiggleSquadron.Enemies.ClearContainer();
            playerShots.Clear();
        }
    }    
        private void AddEnemies() {
            squiggleSquadron.CreateEnemies(enemyStrides);
        }
    }
}