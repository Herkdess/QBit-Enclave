using System;
using System.Threading.Tasks;
using Base.UI;
using Sirenix.OdinInspector;
namespace Base {
    [Serializable]
    public class B_Bridge {
        #region Managers

        [TabGroup("Game Manager")]
        public GameManagerFunctions gameManagerFunctions;
        [TabGroup("UI Manager")]
        public UIManagerFunctions UIManager;
        [TabGroup("Level Manager")]
        public LevelManagerFunctions levelManagerFunctions;
        [TabGroup("Coroutine Manager")]
        public CoroutineRunnerFunctions coroutineRunnerFunctions;
        [TabGroup("Camera Manager")]
        public CameraFunctions CameraFunctions;
        #endregion

        public Task SetupBridge(BaseEngine bootLoader) {

            B_CoroutineControl.Setup(bootLoader, coroutineRunnerFunctions);
            B_UIControl.Setup(UIManager);
            B_GameControl.Setup(gameManagerFunctions);
            B_LevelControl.Setup(levelManagerFunctions);
            B_CameraControl.Setup(CameraFunctions);

            return Task.CompletedTask;
        }
        
        #region Control Functions

        public void FlushBridgeData() {
            
        }

        #endregion
    }
}