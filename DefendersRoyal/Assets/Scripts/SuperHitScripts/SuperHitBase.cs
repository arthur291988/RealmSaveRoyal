using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperHitBase : MonoBehaviour
{
    [NonSerialized]
    public int _HPReduce;
    [NonSerialized]
    public float _effectTime;
    [NonSerialized]
    public float _effectTimer;
    [NonSerialized]
    public int _onEnemyEffectIndex;

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    public virtual void setPropertiesOfSuperHitEffect(int HPReduce, float effectTime, int onEnemyEffectIndex)
    {

    }
}
