using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BombData : ScriptableObject
{
    public float bombExplosionRadius;
    public float bombExplosionStrength;
    public float bombTickDownTime;
    public float timeToDestroyAfterExplosion;
}
