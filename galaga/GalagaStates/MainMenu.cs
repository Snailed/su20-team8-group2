using System;
using DIKUArcade.Entities;
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

        public static MainMenu GetInstance()
        {
            return MainMenu.instance  ?? (MainMenu.instance = new MainMenu());
        }

        public void GameLoop()
        {
            throw new NotImplementedException();
        }

        public void InitializeGameState()
        {
            backGroundImage = new Entity(new StationaryShape(new Vec2F(0.250f, 0.250f), new Vec2F(0.500f, 0.500f)), new Image("../Assets/Images/TitleImage.png"));
            menuButtons = new Text[]
            {
                new Text("New game", new Vec2F(0.2f, 0.2f), new Vec2F(0.3f, 0.3f)),
                new Text("Quit", new Vec2F(0.2f, 0.2f), new Vec2F(0.3f, 0.3f)),

            };
            
            
        }

        public void UpdateGameLogic()
        {
            throw new NotImplementedException();
        }

        public void RenderState()
        {
            backGroundImage.RenderEntity();
           
            for (int i = 0; i < menuButtons.Length -1; i++)
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
                        break;
                }
            }
        }
        
    }
}