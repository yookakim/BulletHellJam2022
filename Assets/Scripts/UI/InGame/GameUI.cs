using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameEvent requestMenuLoadEvent;
	[SerializeField] private TMP_Text coinAmountText;
	[SerializeField] private TMP_Text testbuttontext;

	private void Awake()
	{
		Debug.Log("awake in GameUI");
	}

	public void OnReturnToMenuButtonClicked()
	{
        requestMenuLoadEvent.Raise();
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

	public void RefreshCoinAmount(int newAmount)
	{
		Debug.Log("laser focused");


		coinAmountText.text = ("x" + newAmount).ToString();
		coinAmountText.ClearMesh();
		coinAmountText.SetAllDirty();
		coinAmountText.ForceMeshUpdate(true, true);

		testbuttontext.text = "lol wtf";
	}
}
