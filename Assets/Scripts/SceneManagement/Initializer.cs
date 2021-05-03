using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

/// <summary>
/// This class is responsible for starting the game by loading the persistent managers scene
/// and raising the event to load the Main Menu
/// </summary>
public class Initializer : MonoBehaviour
{
    [SerializeField] private GameSceneSO managerScene;
    [SerializeField] private GameSceneSO menuScene;

    [Header("Broadcasting On")]
    [SerializeField] private AssetReference menuLoadChannel;
    
    private void Start()
    {
        // Load persistent managers scene
        managerScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += LoadEventChannel;
    }

    private void LoadEventChannel(AsyncOperationHandle<SceneInstance> obj)
    {
        menuLoadChannel.LoadAssetAsync<LoadEventChannelSO>().Completed += LoadMainMenu;
    }

    private void LoadMainMenu(AsyncOperationHandle<LoadEventChannelSO> obj)
    {
        LoadEventChannelSO loadEventChannelSO = (LoadEventChannelSO)menuLoadChannel.Asset;
        loadEventChannelSO.RaiseEvent(menuScene);

        SceneManager.UnloadSceneAsync(0); // Initialization is the only scene in Build Setting --> index = 0
    }
}
