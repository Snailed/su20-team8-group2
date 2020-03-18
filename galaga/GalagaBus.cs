using DIKUArcade.EventBus;

namespace galaga {
    public static class GalagaBus {
        private static GameEventBus<object> _eventBus;

        public static GameEventBus<object> GetBus() {
            return _eventBus ?? (GalagaBus._eventBus = new GameEventBus<object>());
        }
    }
}