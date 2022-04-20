using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public GameObject Owner { get; set; }
    public DamageComponent.Alignment MeleeAlignment { get => damageComponent.DamageAlignment; }
	public MeleeData MeleeData { get; set; }

    [SerializeField] private DamageComponent damageComponent;
    [SerializeField] private float hitboxLifetime = 0.1f;
	[SerializeField] private float objectLifetime = 0.25f;

	private float elapsedLifetime = 0f;
	private Collider2D meleeHitbox;

	private void Awake()
	{
		meleeHitbox = GetComponent<Collider2D>();
	}

	private void Update()
	{
		elapsedLifetime += Time.deltaTime;
		if (elapsedLifetime >= hitboxLifetime && meleeHitbox.isActiveAndEnabled)
		{
			meleeHitbox.enabled = false;
		}
		if (elapsedLifetime >= objectLifetime)
		{
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// pass own gameobject into hitbox right here
		Hitbox hitbox = collision.gameObject.GetComponent<Hitbox>();
		hitbox.TriggerHitbox(gameObject);
	}

	public void InitializeMeleeData(MeleeData meleeData)
	{
		MeleeData = meleeData;

		damageComponent.DamageAlignment = meleeData.alignment;
		damageComponent.DamageAmount = meleeData.meleeDamage;
		damageComponent.KnockbackForce = meleeData.knockbackForce;
	}
}
