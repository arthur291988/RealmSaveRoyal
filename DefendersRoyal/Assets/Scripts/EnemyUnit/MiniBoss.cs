using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MiniBoss : EnemyUnit
{
    [NonSerialized]
    public float superHitTimer;
    public float superHitTime; //to assign on editor to each boss separately
    public int countOfContinuousSuperHits;

    [NonSerialized]
    public float startSuperHitTimer;
    public float startSuperHitTime;

    public virtual void resetSuperHitTimer()
    {
        superHitTimer = superHitTime;
    }
    public virtual void resetStartSuperHitTimer()
    {
        startSuperHitTimer = startSuperHitTime;
    }

    public override void setEnemyLevel(int level, int attackWaveCount)
    {
        base.setEnemyLevel(level, attackWaveCount);
        setMiniBossFeatures(attackWaveCount);
    }
    public override void pullDestroyEffect()
    {
        ObjectPulledList = ObjectPuller.current.GetMiniBossDestroyPullList();
        ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
        ObjectPulled.transform.position = _transform.position;
        ObjectPulled.SetActive(true);
    }

    private void setMiniBossFeatures(int waveNumber)
    {
        if (_unitSpriteRenderer == null) _unitSpriteRenderer = GetComponent<SpriteRenderer>();
        //1010 - 10 is mini boss, 1 wave count, location 0 mini boss: 1011 - 10 is mini boss, 1 wave count, location 1 mini boss 
        _unitSpriteRenderer.sprite = GameController.instance.enemyAtlas.GetSprite(_enemyLevel.ToString() + waveNumber.ToString() + CommonData.instance.location.ToString() + CommonData.instance.subLocation.ToString());

        int intHolder = 0;
        for (int i = 0; i < 4; i++)
        {
            intHolder += CommonData.instance.regularEnemyHPForAllLocations[CommonData.instance.location, CommonData.instance.subLocation, waveNumber, i] * CommonData.instance.HPMultiplierForMiniBoss;
        }
        HP = intHolder / 4;
        maxHP = HP;

        _energyOnDestroy = CommonData.instance.regularEnemyEnergy[waveNumber] * CommonData.instance.energyMultiplierForMiniBoss;

        float floatHolder = 0;
        for (int i = 0; i < 4; i++)
        {
            floatHolder += CommonData.instance.regularEnemySpeed[i];
        }
        _moveSpeed = floatHolder / 4;
    }

    
}
