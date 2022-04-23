using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthSlot : MonoBehaviour
{
	public Image SlotImage { get => slotImage; set => slotImage = value; }
	public Image HeartImage { get => heartImage; set => heartImage = value; }

    [SerializeField] private Image slotImage;
    [SerializeField] private Image heartImage;
}
