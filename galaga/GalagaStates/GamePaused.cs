using System;
using DIKUArcade.State;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.EventBus;
namespace galaga.GalagaStates
{
    public class GamePaused : IGameState
    {
        private Entity backGroundImage;
        private Text[] menuButtons;
        private int activeMenuButton;
        private int maxMenuButtons;

        private static GamePaused instance = null;

        public GamePaused(){
            InitializeGameState();
        }

        public static GamePaused GetInstance()
        {
            return GamePaused.instance  ?? (GamePaused.instance = new GamePaused());
        }
        
        public void GameLoop()
        {
            throw new NotImplementedException();
        }

        public void InitializeGameState()
        {
            menuButtons = new Text[]
            {
                new Text("Continue", new Vec2F(0.2f, 0.6f), new Vec2F(0.3f, 0.3f)),
                new Text("Main Menu", new Vec2F(0.2f, 0.3f), new Vec2F(0.3f, 0.3f))
            
            };     
            Console.WriteLine("Main_menu");
   
            
        }

        public void UpdateGameLogic()
        {
        }

        public void RenderState()
        {
            for (int i = 0; i <= menuButtons.Length -1; i++)
            {
                if (i == activeMenuButton)
                {
                    menuButtons[i].SetColor(new Vec3I(200, 0, 0));
                }
                else
                {
                    menuButtons[i].SetColor(new Vec3I(0, 0, 200));

                }
                menuButtons[i].RenderText();
            }
        }

        public void HandleKeyEvent(string keyValue, string keyAction)
        {
        if (keyAction == "KEY_PRESS")
            {
                switch (keyValue)
                {
                    case "KEY_UP":
                        activeMenuButton = 0;
                        break;
                    case "KEY_DOWN":
                        activeMenuButton = 1;
                        break;
                    case "KEY_ENTER":
                        Enter();
                        break;
                }
            }
        }
        public void Enter(){
                switch (activeMenuButton)
                {
                    case 0:
                        GalagaBus.GetBus().RegisterEvent(GameEventFactory<object>.CreateGameEventForAllProcessors(GameEventType.GameStateEvent, 
                            this, "CHANGE_STATE", "GAME_RUNNING", ""));
                        break;
                    case 1:
                        GalagaBus.GetBus().RegisterEvent(GameEventFactory<object>.CreateGameEventForAllProcessors(GameEventType.GameStateEvent, 
                            this, "CHANGE_STATE", "MAIN_MENU", ""));

                       break;
            }
        } 
    }
}