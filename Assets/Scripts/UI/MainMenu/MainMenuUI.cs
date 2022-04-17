using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
	[SerializeField] private GameEvent startGameSceneEvent;

    public void StartGame()
	{
		startGameSceneEvent.Raise();
	}

	private void Update()
	{
		
	}
}
