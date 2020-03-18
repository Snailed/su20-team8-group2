using System;
using NUnit.Framework;
using galaga;
using DIKUArcade;
using galaga.GalagaStates;

namespace galagaTests {
    public class StateTransformerTests {
        [TestCase(GameStateType.GameRunning, "GAME_RUNNING")]
        [TestCase(GameStateType.GamePaused, "GAME_PAUSED")]
        [TestCase(GameStateType.MainMenu, "MAIN_MENU")]
        public void TestStateTransformerCorrect(GameStateType x, string y) {
            Assert.AreEqual(x, StateTransformer.TransformStringToState(y));
            Assert.AreEqual(StateTransformer.TransformStateToString(x), y);
        }
        
        [Test] 
        public void TestStateTransformerThrows() {
            Assert.Throws<ArgumentException>(() => { StateTransformer.TransformStringToState("NOT_A_REAL_STATE"); });
            // We cannot test if TransformStateToString throws, since there only are three possible cases for the enum,
            // and they are all covered.
        }
    }
}