using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{
	[SerializeField] private SceneDataSO gameplayScene;
	[SerializeField] private SceneDataSO gameManagersScene;
	[SerializeField] private SceneDataSO menuScene;
	[SerializeField] private GameEvent gameplaySceneLoadCompletedEvent;

	private SceneDataSO sceneToLoad;
	private SceneDataSO currentSceneData; // could either be the MainMenu or the GameplayScene
	private SceneInstance gameplayManagersSceneInstance = new SceneInstance();

	private AsyncOperationHandle<SceneInstance> gameManagersSceneLoadingHandle;

	public void OnMenuSceneLoadRequested()
	{
		sceneToLoad = menuScene;
		// Two situations from where menu load is requested - 
		// on startup from initialization, and from the game scene.

		if (gameplayManagersSceneInstance.Scene != null
			|| gameplayManagersSceneInstance.Scene.isLoaded)
		{
			if (gameManagersSceneLoadingHandle.IsValid())
			{
				gameManagersScene.sceneReference.UnLoadScene();
			}
		}

		UnloadCurrentScene();
	}

	public void OnGameSceneLoadRequested()
	{
		// Called from UnityEvent in GameEventListener on Start Game button click
		Debug.Log("Game Scene Load Request Received");
		sceneToLoad = gameplayScene;
		// two situations for loading game scene: from menu, and from instant reload in game scene

		// if managers scene doesn't exist, then we are loading from the main menu
		if (gameplayManagersSceneInstance.Scene == null
			|| !gameplayManagersSceneInstance.Scene.isLoaded)
		{
			gameManagersSceneLoadingHandle = gameManagersScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
			gameManagersSceneLoadingHandle.Completed += OnGameManagersReady;
		}
		else // the gamemanager scene exists, so we unload and reload the gameplay scene
		{
			UnloadCurrentScene();
		}
	}

	private void OnGameManagersReady(AsyncOperationHandle<SceneInstance> handle)
	{
		// GameManagers scene completely loaded at this point


		UnloadCurrentScene(); // unloads the Main Menu
	}

	private void UnloadCurrentScene()
	{
		if (currentSceneData != null)
		{
			if (currentSceneData.sceneReference.OperationHandle.IsValid())
			{
				currentSceneData.sceneReference.UnLoadScene();
			}
		}

		LoadNewScene();
	}

	private void LoadNewScene()
	{
		sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += OnNewSceneLoaded;
	}

	private void OnNewSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
	{
		currentSceneData = sceneToLoad;
		Scene s = obj.Result.Scene;
		SceneManager.SetActiveScene(s);

		// Since this gets called for menu load too, technically we should check if the new scene is 
		// actually a gameplay scene before we call this, but since nothing is listening to this for
		// menu load it should be ok
		gameplaySceneLoadCompletedEvent.Raise();
	}
}
