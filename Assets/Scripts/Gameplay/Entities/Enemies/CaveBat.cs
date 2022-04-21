using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveBat : Entity
{
    public float TimeElapsedInAttackPhase { get; set; }
    public float TimeElapsedInRepositionPhase { get; set; }

    public float timeInAttackPhase;
    public float timeInRepositionPhase;
    public bool repositionDestinationReached;
    
    [SerializeField] private float randomMoveDistance;
    [SerializeField] private float repositionDistance;
    [SerializeField] private float randomMoveInterval;
    [SerializeField] private float randomRepositionInterval;
    [SerializeField] private float repositionSpeedMultiplier;
    [SerializeField] private float randomSearchRetryAngleInterval;

    private Vector2 currentDestination;
    private bool randomLocationReached;


    private float currentElapsedInRandomMoveInterval;
    private float currentElapsedInRandomRepositionInterval;

    private PlayerScanRaycast playerScanner;

	private void Awake()
	{
        playerScanner = GetComponent<PlayerScanRaycast>();

        randomLocationReached = true;
        repositionDestinationReached = true;
	}

    protected override void OnHit(GameObject gameObjectHitBy)
    {
        base.OnHit(gameObjectHitBy);
    }

    protected override void OnEntityHealthZero(GameObject deadObject)
    {
        // change state to dead or something later
        base.OnEntityHealthZero(deadObject);
        Destroy(deadObject);
    }

    public Vector2 GetRandomNearbyPosition(float distance)
	{
        Vector2 positionToReturn = Vector2.zero;



        bool randomPositionFound = false;
        int antiFreezeCounter = 0; // LMAO
        float searchAngleIncrement = 0;

        float randomRotateAngle = Random.Range(0, 360);
        Vector2 randomDirection = HelperFunctions.RotateVectorRad(Vector2.up, randomRotateAngle * Mathf.Deg2Rad);

        while (!randomPositionFound)
		{
            
            
            Vector2 incrementedSearchDirection = HelperFunctions.RotateVectorRad(randomDirection, searchAngleIncrement * Mathf.Deg2Rad);

            RaycastHit2D raycastCheck = Physics2D.Raycast(transform.position, incrementedSearchDirection, distance, LayerMask.GetMask("Destructible"));

			if (raycastCheck.collider == null)
			{
                positionToReturn = (Vector2)transform.position + (randomDirection * distance);
                randomPositionFound = true;
            }
			else
			{
                // Debug.Log("check ran into terrain, checking again: " + raycastCheck.collider.gameObject);
			}

            antiFreezeCounter++;

			if (antiFreezeCounter >= (360 / randomSearchRetryAngleInterval))
			{
                Debug.LogError("Could not find any suitable destination, exiting to avoid freeze");
                return transform.position;
			}

            searchAngleIncrement += randomSearchRetryAngleInterval;
        }

        return positionToReturn;
	}

    public void MoveToRandomNearbyLocation()
	{
		if (currentElapsedInRandomMoveInterval >= randomMoveInterval)
		{
            currentDestination = GetRandomNearbyPosition(randomMoveDistance);
            currentElapsedInRandomMoveInterval = 0;
		}

        movement.Move((currentDestination - (Vector2)transform.position).normalized);

        currentElapsedInRandomMoveInterval += Time.deltaTime;

        weaponController.CurrentWeaponTarget = playerScanner.CurrentPlayerPosition;
        weaponController.AttemptUse();
    }

    /// <summary>
    /// Choose a random location away from player and fly quickly towards it
    /// </summary>
    public void Reposition()
	{
        if (currentElapsedInRandomRepositionInterval >= randomRepositionInterval)
        {
            currentDestination = GetRandomNearbyPosition(repositionDistance);
            currentElapsedInRandomRepositionInterval = 0;
        }

        movement.Move((currentDestination - (Vector2)transform.position).normalized * repositionSpeedMultiplier);

        currentElapsedInRandomRepositionInterval += Time.deltaTime;
    }
}
