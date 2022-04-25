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
			if (value <= 0)
			{
				currentCoinAmount = 0;
			}
			else
			{
				currentCoinAmount = value;
			}
			coinAmountChangedEvent.Raise(currentCoinAmount);
		} 
	}

	public Interactable CurrentlyInteractedWith
	{
		get => currentlyInteractedWith;
		set
		{
			currentlyInteractedWith = value;
			interactableEnteredEvent.Raise(currentlyInteractedWith.gameObject);
		}
	}

	[SerializeField] private TransformReference playerTransformReference;
	[SerializeField] private HealthEvent playerHealthEvent;
	[SerializeField] private IntEvent coinAmountChangedEvent;
	[SerializeField] private GameObjectEvent interactableEnteredEvent;
	[SerializeField] private GameEvent interactableExitedEvent;
	[SerializeField] private GameObjectEvent gameEndZoneReachedEvent;
	[SerializeField] private GameEvent playerDiedEvent;

	public SoundEffectData coinPickupSound;

	private PlayerInputController inputController;
	private Movement movement;
	private Health health;
	private PlayerBombLauncher bombLauncher;
	private WeaponController weaponController;
	private EntityTweenEffects entityTweenFX;
	private MeleeController meleeController;
	private Hitbox hitbox;

	private int currentCoinAmount;

	private Interactable currentlyInteractedWith;

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

		if (inputController.CursorInputHeld)
		{
			weaponController.useInputHeld = true;
		}
		else
		{
			weaponController.useInputHeld = false;
		}

		if (inputController.MeleeInputPressed)
		{
			// for detecting single click actions like double click abilities
		}

		if (inputController.MeleeInputHeld)
		{
			meleeController.AttemptUse();
		}

		if (inputController.InteractInputPressed)
		{
			if (currentlyInteractedWith != null)
			{
				currentlyInteractedWith.OnInteract(this);
			}
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

	public void OnInteractableTriggerExit(Interactable interactable)
	{
		// if the interactable we exited isn't the one we are currently interacted with, don't cancel
		if (ReferenceEquals(interactable, currentlyInteractedWith))
		{
			interactableExitedEvent.Raise();
			currentlyInteractedWith = null;
		}
	}

	public void OnShopItemPurchased(ShopItemData shopItem)
	{
		WeaponShopItemData weaponItem = shopItem as WeaponShopItemData;
		BombShopItemData bombItem = shopItem as BombShopItemData;

		if (weaponItem != null)
		{
			weaponController.SwapWeapon(weaponItem.weaponData);
		}

		if (bombItem != null)
		{
			bombLauncher.SwapBombType(bombItem.bombData);
		}
	}

	private void OnHealthZero(GameObject deadPlayerObject)
	{
		// put player into dead state later
		entityTweenFX.CancelAllTweens();
		weaponController.CancelTweens();
		playerDiedEvent.Raise();
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
			coinPickupSound.Play();
		}

		Interactable interactable = objectHitBy.GetComponent<Interactable>();

		if (interactable != null)
		{
			CurrentlyInteractedWith = interactable;
		}

		GameEndZone gameEndZone = objectHitBy.GetComponent<GameEndZone>();

		if (gameEndZone != null)
		{
			// emit end game event or something
			gameEndZoneReachedEvent.Raise(gameObject);
		}
	}
}
