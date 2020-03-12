using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.Timers;
using galaga;
using galaga.Squadron;
using galaga.MovementStrategy;

public class Game : IGameEventProcessor<object> {
    private readonly Window win;

    private readonly GameTimer gameTimer;
    private readonly Player player;
    private readonly GameEventBus<object> eventBus;
    
    
    private List<Image> enemyStrides;
    //private List<Enemy> enemies;
    private readonly Score score;

    private List<PlayerShot> playerShots;

    private SquiggleSquadron squiggleSquadron;
    
    private List<Image> explosionStrides;
    private AnimationContainer explosions;
    private int explosiveLength = 500;

    private NoMove noMove;
    private Down down;
    private ZigZagDown zigZagDown;
    
    
    private Image bullet;
    
    public Game() {
        win = new Window("Galaga", 500, 500);
        gameTimer = new GameTimer(60, 60);
        player = new Player(new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
            new Image(Path.Combine("Assets", "Images", "Player.png")));
        score = new Score(new Vec2F(0.02f, 0.7f), new Vec2F(0.3f, 0.3f));
        enemyStrides = ImageStride.CreateStrides(4,
            Path.Combine("Assets", "Images", "BlueMonster.png"));
        //enemies = new List<Enemy>();
        squiggleSquadron = new SquiggleSquadron(6);
        AddEnemies();
        
         noMove = new NoMove();
         down = new Down();
         zigZagDown = new ZigZagDown();
         
        eventBus = new GameEventBus<object>();
        eventBus.InitializeEventBus(new List<GameEventType> {
            GameEventType.InputEvent,
            GameEventType.WindowEvent,
            GameEventType.MovementEvent
        });
        win.RegisterEventBus(eventBus);
        eventBus.Subscribe(GameEventType.InputEvent, this);
        eventBus.Subscribe(GameEventType.WindowEvent, this);
        eventBus.Subscribe(GameEventType.MovementEvent, player);

        playerShots = new List<PlayerShot>();
        // Preloads the bullet image
        bullet = new Image(Path.Combine("Assets", "Images", "BulletRed2.png"));
        explosionStrides = ImageStride.CreateStrides(8, Path.Combine("Assets", "Images", "Explosion.png"));
        // Here the constructor is given the argument 6 since that is the total amount of enemies.  
        explosions = new AnimationContainer(6);
    }

    public void GameLoop() {
        while (win.IsRunning()) {
            gameTimer.MeasureTime();
            while (gameTimer.ShouldUpdate()) {
                win.PollEvents();
                // Update game logic here 
                player.Move();
                
                // Moves the shot
                IterateShot();
                
                eventBus.ProcessEvents();
            }


            if (gameTimer.ShouldRender()) {
                win.Clear();
                // Render gameplay entities here
                // Render player object
                player.Entity.RenderEntity();
                score.RenderScore();

                foreach (var shot in playerShots)
                {
                    shot.Image.Render(shot.Shape);
                } 
                
              // Render all enemy objects
              zigZagDown.MoveEnemies(squiggleSquadron.Enemies);
              squiggleSquadron.Enemies.RenderEntities();
              /*
              foreach (var enemy in enemies)
                {
                    enemy.Image.Render(enemy.Shape);
                }
                */
                
                explosions.RenderAnimations();
                win.SwapBuffers();
            }

            if (gameTimer.ShouldReset()) // 1 second has passed - display last captured ups and fps
                win.Title = "Galaga | UPS: " + gameTimer.CapturedUpdates + ", FPS: " + gameTimer.CapturedFrames;
        }
    }

    public void KeyPress(string key) {
        switch (key) {
            case "KEY_ESCAPE":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(GameEventType.WindowEvent, this,
                        "CLOSE_WINDOW", "", ""));
                break;
            case "KEY_H":
                // player.Direction(new Vec2F(-0.01f, 0.0f));
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(GameEventType.MovementEvent, this,
                        "MOVE_RIGHT", "", ""));
                break;
            case "KEY_L" :
                // player.Direction(new Vec2F(0.01f, 0.0f));
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(GameEventType.MovementEvent, this,
                        "MOVE_LEFT", "", ""));
                break;
            case "KEY_SPACE":
                playerShots.Add(new PlayerShot(
                    new DynamicShape(
                        new Vec2F(player.Entity.Shape.Position.X +0.05f, player.Entity.Shape.Position.Y + 0.1f),
                        new Vec2F(0.008f, 0.027f),
                        new Vec2F(0.0f, 0.01f)
                    ),
                    bullet));
                break;
                
        }
    }

    public void KeyRelease(string key) {
        switch (key) {
            case "KEY_H":
                // player.Direction(new Vec2F(0.0f, 0.0f));
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(GameEventType.MovementEvent, this,
                        "MOVE_STOP", "", ""));
                break;
            case "KEY_L" :
                // player.Direction(new Vec2F(0.0f, 0.0f));
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(GameEventType.MovementEvent, this,
                        "MOVE_STOP", "", ""));
                break;
            
        }
    }

    public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
        if (eventType == GameEventType.WindowEvent)
            switch (gameEvent.Message) {
                case "CLOSE_WINDOW":
                    win.CloseWindow();
                    break;
            }
        else if (eventType == GameEventType.InputEvent)
            switch (gameEvent.Parameter1) {
                case "KEY_PRESS":
                    KeyPress(gameEvent.Message);
                    break;
                case "KEY_RELEASE":
                    KeyRelease(gameEvent.Message);
                    break;
            }
    }

    private void AddEnemies()
    {
        //SquiggleSquadron squiggleSquadron = new SquiggleSquadron(6);
        squiggleSquadron.CreateEnemies(enemyStrides);
        /*
        foreach (Enemy enemy in squiggleSquadron.Enemies) {
            enemies.Add(enemy);
        }
        */
    }
    
    public void IterateShot()
    {
        foreach (var shot in playerShots)
        {
            shot.Shape.Move();
            if (shot.Shape.Position.Y > 1.0f )
            {
                shot.DeleteEntity();
            }
            else
            {
                foreach (Enemy enemy in  squiggleSquadron.Enemies)
                {
                    var collision = CollisionDetection.Aabb(shot.Shape.AsDynamicShape(),  enemy.Shape);
                    if (collision.Collision)
                    {
                        AddExplosion(enemy.Shape.Position.X, enemy.Shape.Position.Y, enemy.Shape.Extent.X, enemy.Shape.Extent.Y);
                        enemy.DeleteEntity();
                        shot.DeleteEntity();
                        score.AddPoint(1);
                    }
                }
            }
        }
        
        /*
        List<Enemy> newEnemies = new List<Enemy>();
        foreach (var enemy in enemies)
        {
            if (!enemy.IsDeleted())
            {
                newEnemies.Add(enemy);
            }                       
        }
        enemies = newEnemies;
        */
        
        List<PlayerShot> newPlayerShots = new List<PlayerShot>();
        foreach (var shot in playerShots)
        {
            if (!shot.IsDeleted())
            {
                newPlayerShots.Add(shot);
            }
        }
        playerShots = newPlayerShots;
    }

    public void AddExplosion(float posX, float posY, float extentX, float extentY)
    {
        explosions.AddAnimation(new StationaryShape(posX, posY, extentX, extentY), explosiveLength,
            new ImageStride(explosiveLength / 8, explosionStrides));
        
    }
}
