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
using galaga.GalagaStates;

public class Game : IGameEventProcessor<object>
{
    
    private readonly Window win;

    private StateMachine stateMachine;
    
    private readonly GameTimer gameTimer;
    //private readonly Player player;
    private readonly GameEventBus<object> eventBus;

    public Game() {
        win = new Window("Galaga", 500, 500);
        gameTimer = new GameTimer(60, 60);
        
        GalagaBus.GetBus().InitializeEventBus(new List<GameEventType> {
            GameEventType.InputEvent,
            GameEventType.WindowEvent,
            GameEventType.MovementEvent,
            GameEventType.StatusEvent,
            GameEventType.GameStateEvent
            
        });
        win.RegisterEventBus(GalagaBus.GetBus());
        GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, this);
        GalagaBus.GetBus().Subscribe(GameEventType.WindowEvent, this);
        GalagaBus.GetBus().Subscribe(GameEventType.StatusEvent, this);

        stateMachine = new StateMachine();

    }
    
    public void GameLoop() {
        while (win.IsRunning()) {
            gameTimer.MeasureTime();
            while (gameTimer.ShouldUpdate()) {
                win.PollEvents();
                stateMachine.ActiveState.UpdateGameLogic();
                GalagaBus.GetBus().ProcessEvents();

            }
            if (gameTimer.ShouldRender()) {
                win.Clear();
                stateMachine.ActiveState.RenderState();
                win.SwapBuffers();
            }
            if (gameTimer.ShouldReset()) {
                win.Title = "Galaga | UPS: " + gameTimer.CapturedUpdates + ", FPS: " + gameTimer.CapturedFrames;
        }
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
            
            stateMachine.ActiveState.HandleKeyEvent(gameEvent.Message, gameEvent.Parameter1);

    
    }
}
