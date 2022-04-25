using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameEvent requestMenuLoadEvent;
	[SerializeField] private GameEvent requestGameReloadEvent;
	[SerializeField] private TMP_Text coinAmountText;
	[SerializeField] private TMP_Text testbuttontext;
	[SerializeField] private GameObject victoryPanel;
	[SerializeField] private GameObject defeatPanel;
	[SerializeField] private GameObject darkOverlay;

	private void Awake()
	{
		Debug.Log("awake in GameUI");
	}

	public void OnReturnToMenuButtonClicked()
	{
        requestMenuLoadEvent.Raise();
	}

	public void OnRestartGameButtonClicked()
	{
		requestGameReloadEvent.Raise();
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
			victoryPanel.SetActive(false);
			defeatPanel.SetActive(false);
			darkOverlay.SetActive(false);
		}
		if (newState == GameManager.GameState.DefeatState)
		{
			defeatPanel.SetActive(true);
			darkOverlay.SetActive(true);
		}
		if (newState == GameManager.GameState.VictoryState)
		{
			victoryPanel.SetActive(true);
			darkOverlay.SetActive(true);
		}
	}

	public void RefreshCoinAmount(int newAmount)
	{
		coinAmountText.text = ("x" + newAmount).ToString();
	}
}
