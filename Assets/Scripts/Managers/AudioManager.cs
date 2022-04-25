using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance { get; private set; }

	// public Sound[] sounds;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Debug.LogError("Multiple instances of singleton AudioManager");
			Destroy(this);
		}
		else
		{
			Instance = this;
		}
	}
}
