using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineCameraController : MonoBehaviour
{
	public enum FollowTarget
	{
		Player,
		Worm
	}

	public Transform PlayerTransform { get; set; }

    [SerializeField] private Transform giantWormCameraFollowTransform;
	[SerializeField] private Transform giantWormTransform;

	private CinemachineVirtualCamera cinemachine;

	private FollowTarget followMode;

	private void Awake()
	{
		cinemachine = GetComponent<CinemachineVirtualCamera>();
		followMode = FollowTarget.Player;
	}

	private void Update()
	{
		if (PlayerTransform == null)
		{
			return;
		}

		float playerWormYDifference = PlayerTransform.position.y - giantWormTransform.position.y;

		if (playerWormYDifference <= cinemachine.m_Lens.OrthographicSize && followMode != FollowTarget.Worm)
		{
			followMode = FollowTarget.Worm;
			cinemachine.Follow = giantWormCameraFollowTransform;
		}
		else if (playerWormYDifference >= cinemachine.m_Lens.OrthographicSize && followMode != FollowTarget.Player)
		{
			followMode = FollowTarget.Player;
			cinemachine.Follow = PlayerTransform;
		}
	}
}
