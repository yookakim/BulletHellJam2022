using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
	public Transform TooltipAnchorPosition { get => tooltipAnchorPosition; }
	public string TooltipContent { get => tooltipContent; set => tooltipContent = value; }
	public string TooltipHeader { get => tooltipHeader; set => tooltipHeader = value; }
	public SpriteRenderer InteractableSprite { get => interactableSprite; set => interactableSprite = value; }

	[SerializeField] private Transform tooltipAnchorPosition;

	[TextArea(15, 20)]
	[SerializeField] protected string tooltipContent;
	[TextArea(15, 5)]
	[SerializeField] protected string tooltipHeader;

	protected SpriteRenderer interactableSprite;

	protected virtual void Awake()
	{
		interactableSprite = GetComponent<SpriteRenderer>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Hitbox hitbox = collision.gameObject.GetComponent<Hitbox>();

		if (hitbox != null)
		{
			hitbox.TriggerHitbox(gameObject);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		Player player = collision.transform.parent.gameObject.GetComponent<Player>();

		if (player != null)
		{
			player.OnInteractableTriggerExit(this);
		}
	}

	public virtual void OnInteract(Player player)
	{

	}

	public virtual void OnInteractEnter()
	{
		// maybe apply outline, glow, etc.
	}

	public virtual void OnInteractExit()
	{

	}
}
