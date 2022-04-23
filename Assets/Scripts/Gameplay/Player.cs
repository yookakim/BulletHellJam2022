using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public int CurrentCoinAmount
	{
		get => currentCoinAmount;
		set 
		{
			currentCoinAmount = value;
			coinAmountChangedEvent.Raise(value);
		} 
	}

	[SerializeField] private TransformReference playerTransformReference;
	[SerializeField] private HealthEvent playerHealthEvent;
	[SerializeField] private IntEvent coinAmountChangedEvent;

	private PlayerInputController inputController;
	private Movement movement;
	private Health health;
	private PlayerBombLauncher bombLauncher;
	private WeaponController weaponController;
	private EntityTweenEffects entityTweenFX;
	private MeleeController meleeController;
	private Hitbox hitbox;

	private int currentCoinAmount;

	private void Awake()
	{
		inputController = new PlayerInputController(Camera.main);
		movement = GetComponent<Movement>();
		health = GetComponent<Health>();
		bombLauncher = GetComponent<PlayerBombLauncher>();
		weaponController = GetComponentInChildren<WeaponController>();
		entityTweenFX = GetComponent<EntityTweenEffects>();
		meleeController = GetComponent<MeleeController>();
		hitbox = GetComponentInChildren<Hitbox>();

		playerTransformReference.reference = transform;
	}

	private void Start()
	{
		playerHealthEvent.Raise(health);
		coinAmountChangedEvent.Raise(currentCoinAmount);
	}

	private void Update()
	{
		inputController.ReadInput();
		weaponController.CurrentWeaponTarget = inputController.CurrentWorldCursorPoint;
		meleeController.CurrentWeaponTarget = inputController.CurrentWorldCursorPoint;
		movement.Move(inputController.CurrentMoveInput);

		if (inputController.BombInputPressed)
		{
			bombLauncher.LaunchBomb(inputController.CurrentWorldCursorPoint);
		}

		if (inputController.CursorInputPressed)
		{
			weaponController.AttemptUse();
		}

		if (inputController.MeleeInputPressed)
		{
			// for detecting single click actions like double click abilities
		}

		if (inputController.MeleeInputHeld)
		{
			meleeController.AttemptUse();
		}
	}

	private void OnEnable()
	{
		hitbox.hitboxTriggeredEvent += OnHitboxTrigger;
		health.HealthZeroEvent += OnHealthZero;
		health.HealthChangedEvent += OnHealthChanged;
	}

	private void OnDisable()
	{
		hitbox.hitboxTriggeredEvent -= OnHitboxTrigger;
		health.HealthZeroEvent -= OnHealthZero;
		health.HealthChangedEvent -= OnHealthChanged;
	}

	private void OnHealthZero(GameObject deadPlayerObject)
	{
		// put player into dead state later
		entityTweenFX.CancelAllTweens();
		weaponController.CancelTweens();
		Time.timeScale = 0;
	}

	private void OnHealthChanged(Health health, int currentHealth)
	{
		playerHealthEvent.Raise(health);
	}

	private void OnHitboxTrigger(GameObject objectHitBy)
	{
		DamageComponent gameObjectDamageComponent = objectHitBy.GetComponent<DamageComponent>();

		if (gameObjectDamageComponent != null)
		{
			if (gameObjectDamageComponent.DamageAlignment != hitbox.ownerAlignment || gameObjectDamageComponent.HitsAllies)
			{
				int damageToDeal = 0;

				Bomb bomb = objectHitBy.GetComponent<Bomb>();

				if (bomb != null)
				{
					damageToDeal = (int)(gameObjectDamageComponent.DamageAmount * bomb.FriendlyDamageMultiplier);
				}
				else
				{
					damageToDeal = gameObjectDamageComponent.DamageAmount;
				}
				entityTweenFX.OnDamageTween();
				health.DealDamage(damageToDeal);
			}
		}

		Coin coin = objectHitBy.GetComponent<Coin>();

		if (coin != null)
		{
			CurrentCoinAmount++;
			coin.OnPickup();
		}
	}
}
