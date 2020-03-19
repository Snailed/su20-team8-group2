using System;
using System.IO;
using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;

namespace galaga.GalagaStates
{
    public class MainMenu : IGameState
    {
        private static MainMenu instance = null;

        private Entity backGroundImage;
        private Text[] menuButtons;
        private int activeMenuButton;
        private int maxMenuButtons;


        public MainMenu (){
            InitializeGameState();
            
        }
        

        public static MainMenu GetInstance()
        {
            return MainMenu.instance ?? (MainMenu.instance = new MainMenu());
        }

        public void GameLoop()
        {
            throw new NotImplementedException();
        }

        public void InitializeGameState()
        {
            var path = Path.Combine("Assets", "Images", "TitleImage.png");
            Console.WriteLine("menu1");

            backGroundImage = new Entity(new StationaryShape(new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 1.0f)), new Image(path));
    
            menuButtons = new Text[]
            {
                new Text("New game", new Vec2F(0.2f, 0.6f), new Vec2F(0.3f, 0.3f)),
                new Text("Quit", new Vec2F(0.2f, 0.3f), new Vec2F(0.3f, 0.3f))
            
            };
            
            
            Console.WriteLine("menu2");

            activeMenuButton = 0;
        }

        public void UpdateGameLogic()
        {
            
        }

        public void RenderState()
        {

            
            backGroundImage.RenderEntity();
           
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
                        GalagaBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(GameEventType.WindowEvent, this,
                                "CLOSE_WINDOW", "", ""));
                       break;
            }
        } 
    }
}
