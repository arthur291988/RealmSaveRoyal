using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularEnemy : EnemyUnit
{
    public override void setEnemyLevel(int level, int attackWaveCount)
    {
        base.setEnemyLevel(level, attackWaveCount);
        setEnemyFeatures(attackWaveCount);
    }

    private void setEnemyFeatures(int waveNumber)
    {
        if (_unitSpriteRenderer == null) _unitSpriteRenderer = GetComponent<SpriteRenderer>();
        _unitSpriteRenderer.sprite = GameController.instance.enemyAtlas.GetSprite(_enemyLevel.ToString() + CommonData.instance.location.ToString() + CommonData.instance.subLocation.ToString());

        _energyOnDestroy = CommonData.instance.regularEnemyEnergy[waveNumber];
        _moveSpeed = CommonData.instance.regularEnemySpeed[_enemyLevel];
        HP = CommonData.instance.regularEnemyHPForAllLocations[CommonData.instance.location, CommonData.instance.subLocation, waveNumber, _enemyLevel];
        maxHP = HP;
    }
}
