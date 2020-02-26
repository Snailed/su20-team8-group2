using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Timers;
using galaga;

public class Game : IGameEventProcessor<object> {
    private readonly Window win;

    private readonly GameTimer gameTimer;
    private readonly Player player;
    private readonly GameEventBus<object> eventBus;
    private readonly Score score;

    public Game() {
        win = new Window("Galaga", 500, 500);
        gameTimer = new GameTimer(60, 60);
        player = new Player(new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
            new Image(Path.Combine("Assets", "Images", "Player.png")));
        score = new Score(new Vec2F(0.0f, 0.95f), new Vec2F(0.3f, 0.1f));
        eventBus = new GameEventBus<object>();
        eventBus.InitializeEventBus(new List<GameEventType> {
            GameEventType.InputEvent,
            GameEventType.WindowEvent
        });
        win.RegisterEventBus(eventBus);
        eventBus.Subscribe(GameEventType.InputEvent, this);
        eventBus.Subscribe(GameEventType.WindowEvent, this);
    }

    public void GameLoop() {
        while (win.IsRunning()) {
            gameTimer.MeasureTime();
            while (gameTimer.ShouldUpdate()) {
                win.PollEvents();
                // Update game logic here 
                eventBus.ProcessEvents();
            }


            if (gameTimer.ShouldRender()) {
                win.Clear();
                // Render gameplay entities here
                player.Entity.RenderEntity();
                score.RenderScore();
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
        }
    }

    public void KeyRelease(string key) {
        throw new NotImplementedException();
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
}