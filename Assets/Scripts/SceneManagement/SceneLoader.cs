using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

/// <summary>
/// This class manages the scene loading and unloading.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameSceneSO gameplayScene;

    [Header("Load Events")] 
    [SerializeField] private LoadEventChannelSO loadLocation;
    [SerializeField] private LoadEventChannelSO loadMenu;
    [SerializeField] private LoadEventChannelSO coldStartupLocation;

    [Header("Broadcasting On")] 
    [SerializeField] private BoolEventChannelSO toggleLoadingScreen;
    [SerializeField] private VoidEventChannelSO onSceneReady;

    private AsyncOperationHandle<SceneInstance> loadingOperationHandle;
    private AsyncOperationHandle<SceneInstance> gameplayManagerLoadingOpHandle;

    // Parameters coming from scenes loading requests
    private GameSceneSO sceneToLoad;
    private GameSceneSO currentlyLoadedScene;
    private bool showLoadingScreen;

    private SceneInstance gameplayManagerSceneInstance = new SceneInstance();
    #endregion

    #region Events
    private void OnEnable()
    {
        // loadLocation.OnLoadingRequested += LoadLocation;
        loadMenu.OnLoadingRequested += LoadMenu;
    }
    #endregion
    
    #region Load/Unload Methods
    /// <summary>
    /// Prepares to load the main menu scene, first we need to unload the Gameplay scene in case the game is coming back from the gameplay to menu
    /// </summary>
    private void LoadMenu(GameSceneSO menuToLoad, bool showLoadingScreen)
    {
        sceneToLoad = menuToLoad;
        this.showLoadingScreen = showLoadingScreen;
        
        // In case we are coming back from a Location back to the main menu, we need to remove the persistent gameplay managers scene
        if (gameplayManagerSceneInstance.Scene != null && gameplayManagerSceneInstance.Scene.isLoaded)
        {
            Addressables.UnloadSceneAsync(gameplayManagerLoadingOpHandle, true);
        }
        UnloadPreviousScene();
    }

    /// <summary>
    /// In both Location and Menu loading, this functions takes care of removing previously loaded scenes.
    /// </summary>
    private void UnloadPreviousScene()
    {
        if (currentlyLoadedScene != null) // would be null if the game was started in Initialisation
        {
            if (currentlyLoadedScene.sceneReference.OperationHandle.IsValid())
            {
                // Unload the scene through its Asset Reference, i.e through the Addressable system
                currentlyLoadedScene.sceneReference.UnLoadScene();
            }
#if UNITY_EDITOR
            else
            {
                // Only use when after a "cold start", the player moves to a new scene.
                // Since the AsyncOperationHandle has not been used (the scene was already open in the editor),
                // the scene needs to be unloaded using regular SceneManager instead of Addressable system
                SceneManager.UnloadSceneAsync(currentlyLoadedScene.sceneReference.editorAsset.name);
            }
#endif
        }
        LoadNewScene();
    }
    
    /// <summary>
    /// Kicks off the asynchronous loading of a scene, either Menu or Location.
    /// </summary>
    private void LoadNewScene()
    {
        if (showLoadingScreen)
        {
            toggleLoadingScreen.RaiseEvent(true);
        }

        loadingOperationHandle = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0);
        loadingOperationHandle.Completed += OnNewSceneLoaded;
    }

    private void OnNewSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
    {
        // Save loaded scenes (to be unloaded at next load request)
        currentlyLoadedScene = sceneToLoad;
        SetActiveScene();

        if (showLoadingScreen)
        {
            toggleLoadingScreen.RaiseEvent(false);
        }
    }

    /// <summary>
    /// This function is called when all the scenes have been loaded
    /// </summary>
    private void SetActiveScene()
    {
        Scene s = ((SceneInstance) loadingOperationHandle.Result).Scene;
        SceneManager.SetActiveScene(s);
        
        LightProbes.TetrahedralizeAsync();

        StartGameplay();
    }

    private void StartGameplay()
    {
        onSceneReady.RaiseEvent(); // Spawn player and player model
    }

    private void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit!");
    }
    #endregion
}
