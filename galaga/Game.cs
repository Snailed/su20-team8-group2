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

public class Game : IGameEventProcessor<object> {
    private readonly Window win;

    private readonly GameTimer gameTimer;
    private readonly Player player;
    private readonly GameEventBus<object> eventBus;
    
    
    private List<Image> enemyStrides;
    private List<Enemy> enemies;

    private List<PlayerShot> playerShots;
    

    public Game() {
        win = new Window("Galaga", 500, 500);
        gameTimer = new GameTimer(60, 60);
        player = new Player(new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
            new Image(Path.Combine("Assets", "Images", "Player.png")));
        enemyStrides = ImageStride.CreateStrides(4,
            Path.Combine("Assets", "Images", "BlueMonster.png"));
        enemies = new List<Enemy>();
        AddEnemies();
        eventBus = new GameEventBus<object>();
        eventBus.InitializeEventBus(new List<GameEventType> {
            GameEventType.InputEvent,
            GameEventType.WindowEvent
        });
        win.RegisterEventBus(eventBus);
        eventBus.Subscribe(GameEventType.InputEvent, this);
        eventBus.Subscribe(GameEventType.WindowEvent, this);
        playerShots = new List<PlayerShot>();
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
                
                // Renders all the shots in the PlayerShots list
                foreach (var shot in playerShots)
                {
                    shot.Image.Render(shot.Shape);
                } 
                // Render all enemy objects
                foreach (var enemy in enemies)
                {
                    enemy.Image.Render(enemy.Shape);
                }
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
                player.Direction(new Vec2F(-0.01f, 0.0f));
                break;
            case "KEY_L" :
                player.Direction(new Vec2F(0.01f, 0.0f));
                break;
            case "KEY_SPACE":
                playerShots.Add(new PlayerShot(
                    new DynamicShape(
                        new Vec2F(player.Entity.Shape.Position.X +0.05f, player.Entity.Shape.Position.Y + 0.1f),
                        new Vec2F(0.008f, 0.027f),
                        new Vec2F(0.0f, 0.01f)
                    ),
                    new Image("Assets/Images/BulletRed2.png")));
                break;
                
        }
    }

    public void KeyRelease(string key) {
        switch (key) {
            case "KEY_H":
                player.Direction(new Vec2F(0.0f, 0.0f));
                break;
            case "KEY_L" :
                player.Direction(new Vec2F(0.0f, 0.0f));
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
        for (int i = 0; i < 6; i++)
        {
            enemies.Add(new Enemy(
                new DynamicShape(
                    // The +0.2f is for centering purposes
                    new Vec2F(i*0.1f + 0.2f, 0.9f), 
                    new Vec2F(0.1f, 0.1f)),
                new ImageStride(80, enemyStrides)));
        }
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
                foreach (var enemy in enemies)
                {
                    var collision = CollisionDetection.Aabb(shot.Shape.AsDynamicShape(),  enemy.Shape);
                    if (collision.Collision)
                    {
                        enemy.DeleteEntity();
                        shot.DeleteEntity();
                        Console.WriteLine("hit");
                    }
                }
            }
        }
        List<Enemy> newEnemies = new List<Enemy>();
        foreach (var enemy in enemies)
        {
            if (!enemy.IsDeleted())
            {
                newEnemies.Add(enemy);
            }                       
        }
        enemies = newEnemies;
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
}
