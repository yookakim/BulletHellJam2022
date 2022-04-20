using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScanRaycast : MonoBehaviour
{
    [SerializeField] private TransformReference playerTransform;
    [SerializeField] private float scanRange;
    [SerializeField] private float giveUpTimeAfterLosingSight;
    public bool HasSightOfPlayer { get; private set; }
    public Vector2 VectorToPlayer { get => playerTransform.reference.position - transform.position; }

    public Vector2 CurrentPlayerPosition { get => playerTransform.reference.position; }

    private LayerMask hitboxLayer;

    private float timeLastSeenPlayer;

	private void Awake()
	{
        // raycastIgnore |= LayerMask.GetMask("Destructible")
        hitboxLayer = LayerMask.GetMask("Hitbox");

    }

	void Update()
    {
        ScanForPlayer();
        // Debug.DrawLine(transform.position, playerTransform.reference.position, Color.red);
    }

    private void ScanForPlayer()
	{
        Vector2 normalizedVectorToPlayer = (playerTransform.reference.position - transform.position).normalized;
        RaycastHit2D[] raycast = Physics2D.RaycastAll(transform.position, normalizedVectorToPlayer, scanRange, hitboxLayer);


		if (Time.time - timeLastSeenPlayer >= giveUpTimeAfterLosingSight)
		{
            HasSightOfPlayer = false;
		}
        
		foreach (RaycastHit2D raycastHit in raycast)
		{
			if (LayerMask.LayerToName(raycastHit.collider.transform.parent.gameObject.layer).Equals("Destructible") &&
                !ReferenceEquals(raycastHit.collider.transform.parent.gameObject, gameObject))
			{
                return;
			}
            else if (LayerMask.LayerToName(raycastHit.collider.transform.parent.gameObject.layer).Equals("Player"))
			{
                HasSightOfPlayer = true;
                timeLastSeenPlayer = Time.time;
                return;
			}
		}
	}
}
