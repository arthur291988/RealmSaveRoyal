using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : EnemyUnit
{
    [NonSerialized]
    public float superHitTimer;
    public float superHitTime;
    [NonSerialized]
    public float startSuperHitTimer;
    public float startSuperHitTime;
    public int countOfContinuousSuperHits;


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
        setBigBossFeatures(attackWaveCount);
    }

    public override void pullDestroyEffect()
    {
        ObjectPulledList = ObjectPuller.current.GetFinalBossDestroyPullList();
        ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
        ObjectPulled.transform.position = _transform.position;
        ObjectPulled.SetActive(true);
    }

    private void setBigBossFeatures(int waveNumber)
    {
        if (_unitSpriteRenderer == null) _unitSpriteRenderer = GetComponent<SpriteRenderer>();


        //10000 - 100 is location boss, 0 wave count (always zero), location 0 mini boss: 10001 - 100 is location boss, 0 wave count (always zero), location 1 mini boss 
        _unitSpriteRenderer.sprite = GameController.instance.enemyAtlas.GetSprite(_enemyLevel.ToString() + "0" + CommonData.instance.location.ToString() + CommonData.instance.subLocation.ToString());

        int intHolder = 0;
        for (int i = 0; i < 4; i++)
        {
            intHolder +=
                CommonData.instance.regularEnemyHPForAllLocations[CommonData.instance.location, CommonData.instance.subLocation, waveNumber, i] * CommonData.instance.HPMultiplierForBigBoss;
        }
        HP = intHolder / 4;
        maxHP = HP;

        _energyOnDestroy = CommonData.instance.regularEnemyEnergy[waveNumber] * CommonData.instance.energyMultiplierForBigBoss;

        float floatHolder = 0;
        for (int i = 0; i < 4; i++)
        {
            floatHolder += CommonData.instance.regularEnemySpeed[i];
        }
        _moveSpeed = floatHolder / 4;
    }
}
