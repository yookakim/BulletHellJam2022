using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class InitializationLoader : MonoBehaviour
{
    [SerializeField] private SceneDataSO mainMenuScene;
    [SerializeField] private SceneDataSO persistentManagersScene;
    // [SerializeField] private GameEvent mainMenuLoadRequestEvent;
    [SerializeField] private AssetReference mainMenuLoadRequestEvent;

    AsyncOperationHandle<SceneInstance> persistentManagersSceneLoadHandle;

    void Start()
    {
        persistentManagersSceneLoadHandle =
            persistentManagersScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);

        persistentManagersSceneLoadHandle.Completed += OnPersistentManagersLoadComplete;

        Screen.SetResolution(900, 900, false);
    }


    private void OnPersistentManagersLoadComplete(AsyncOperationHandle<SceneInstance> handle)
    {
        // SceneManager.UnloadSceneAsync(0); //Initialization is the only scene in BuildSettings, thus it has index 0
        // mainMenuScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += OnMainMenuLoadComplete;
        mainMenuLoadRequestEvent.LoadAssetAsync<GameEvent>().Completed += OnEventAssetLoaded;
        // SceneLoader is now ready, so call the event for menu load which SceneLoader is listening to

    }

    private void OnEventAssetLoaded(AsyncOperationHandle<GameEvent> handle)
	{
        handle.Result.Raise();
        SceneManager.UnloadSceneAsync(0);
    }

/*    private void OnMainMenuLoadComplete(AsyncOperationHandle<SceneInstance> handle)
	{
        SceneManager.UnloadSceneAsync(0);
    }*/
}
