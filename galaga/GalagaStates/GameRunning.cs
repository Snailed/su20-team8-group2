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