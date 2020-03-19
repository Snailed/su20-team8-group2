using System;
using DIKUArcade.State;

namespace galaga.GalagaStates
{
    public class GamePaused : IGameState
    {
        private static GamePaused instance = null;

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
            throw new NotImplementedException();
        }

        public void UpdateGameLogic()
        {
            throw new NotImplementedException();
        }

        public void RenderState()
        {
            throw new NotImplementedException();
        }

        public void HandleKeyEvent(string keyValue, string keyAction)
        {
            throw new NotImplementedException();
        }
    }
}