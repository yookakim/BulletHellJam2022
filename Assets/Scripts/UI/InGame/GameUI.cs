using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameEvent RequestMenuLoadEvent;

	private void Awake()
	{
		Debug.Log("awake in GameUI");
	}

	public void OnReturnToMenuButtonClicked()
	{
        RequestMenuLoadEvent.Raise();
	}

    /// <summary>
    /// On level start, pause, end, etc.
    /// </summary>
    /// <param name="newState"></param>
    public void OnGameStateChanged(GameManager.GameState newState)
	{
        Debug.Log(newState);
		if (newState == GameManager.GameState.LoadLevelState)
		{

		}
	}
}
