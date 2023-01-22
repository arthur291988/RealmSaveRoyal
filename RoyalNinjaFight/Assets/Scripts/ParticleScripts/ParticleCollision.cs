using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    [NonSerialized]
    public int _HPReduce;
    [NonSerialized]
    public float _effectTime;
    [NonSerialized]
    public float _effectTimer;
    [NonSerialized]
    public int _onEnemyEffectIndex;

    public virtual void OnParticleCollision(GameObject other)
    {
    }

    public virtual void setPropertiesOfSuperHitEffect(int HPReduce, float effectTime, int onEnemyEffectIndex) { 
    
    }
}

