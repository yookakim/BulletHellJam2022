using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityTweenEffects : MonoBehaviour
{
	[SerializeField] EntityTweenEffectData effectData;
	[SerializeField] private SpriteRenderer sprite;
	[SerializeField] private Material entityOverlayMaterial;


	// private SpriteRenderer entitySpriteOverlay;

	private void Awake()
	{
		// entitySpriteOverlay = entitySpriteOverlayObject.GetComponent<SpriteRenderer>();
		sprite.material = new Material(entityOverlayMaterial);
	}

	public void OnDamageTween()
	{
		// Color colorToReset = entitySpriteOverlay.color;
		// entitySpriteOverlay.color = effectData.damageTweenColor;

		sprite.material.SetColor("_OverlayColor", effectData.damageTweenColor);
		sprite.material.SetFloat("_OverlayOpacity", 1);

		LeanTween.value(gameObject, 1f, 0f, effectData.damageTweenDuration
		).setEase(effectData.damageTweenCurve).setOnUpdate((float val) =>
		{
			sprite.material.SetFloat("_OverlayOpacity", val);
		}).setOnComplete(() =>
		{
			sprite.material.SetFloat("_OverlayOpacity", 0);
		});

		// LeanTween.can
	}

	public void CancelAllTweens()
	{
		LeanTween.cancel(gameObject);
	}
}
