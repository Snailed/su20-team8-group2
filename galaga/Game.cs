using System;
using DIKUArcade;
using DIKUArcade.EventBus;
using DIKUArcade.Timers;

public class Game : IGameEventProcessor<object>
{
    private Window win;
    private DIKUArcade.Timers.GameTimer gameTimer;

    public Game()
    {
        win = new Window("Galaga", 500, 500);
        gameTimer = new GameTimer(30, 30);

    }

    public void GameLoop()
    {
        while (win.IsRunning())
        {
            gameTimer.MeasureTime();
            while (gameTimer.ShouldUpdate())
            {
                win.PollEvents();
                // Update game logic here 
            }



            if (gameTimer.ShouldUpdate())
            {
                win.Clear();
                // Render gameplay entities here
                win.SwapBuffers();
            }

            if (gameTimer.ShouldRender())
            {
                // 1 second has passed - display last captured ups and fps
                win.Title = "Galaga | UPS: " + gameTimer.CapturedUpdates + ", FPS: " + gameTimer.CapturedFrames;
            }
        }
    }

    public void keyPress(string key){
        throw new NotImplementedException();
    }

    public void KeyRelese(string key)
    {
        throw new NotImplementedException();
    }

    public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent)
    {
        throw new NotImplementedException();
    }
}
