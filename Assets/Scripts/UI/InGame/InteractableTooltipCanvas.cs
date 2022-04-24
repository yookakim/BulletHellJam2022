using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractableTooltipCanvas : MonoBehaviour
{
    [SerializeField] private RectTransform tooltipTransform;
	[SerializeField] private TMP_Text tooltipHeader;
	[SerializeField] private TMP_Text tooltipContent;

    private Interactable currentInteractable;
	private Camera mainCamera;

	private void Awake()
	{
		mainCamera = Camera.main;
	}
	// Update is called once per frame
	void Update()
    {
		if (currentInteractable != null)
		{
			Vector2 interactableAnchorPosition = currentInteractable.TooltipAnchorPosition.position;

			Vector2 anchorPositionOnScreen = mainCamera.WorldToScreenPoint(interactableAnchorPosition);

			tooltipTransform.position = anchorPositionOnScreen;
		}

    }

    public void SetCurrentlyDisplayedInteractable(GameObject interactableObject)
	{
        Interactable newInteractable = interactableObject.GetComponent<Interactable>();

		if (newInteractable != null)
		{
			currentInteractable = newInteractable;
			tooltipHeader.text = newInteractable.TooltipHeader;
			tooltipContent.text = newInteractable.TooltipContent;
			tooltipTransform.gameObject.SetActive(true);
		}
		else
		{
            Debug.LogError("Expected interactable component in tooltip refresh but found none");
		}
	}

	public void DisableInteractable()
	{
		tooltipTransform.gameObject.SetActive(false);
		currentInteractable = null;
	}
}
