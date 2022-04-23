using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "VFX/Entity Tween Data")]
public class EntityTweenEffectData : ScriptableObject
{
    public float damageTweenDuration;
    [ColorUsage(true, true)]
    public Color damageTweenColor;
    public AnimationCurve damageTweenCurve;

}
