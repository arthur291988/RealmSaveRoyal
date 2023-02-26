using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHits : SuperHitBase
{
    private GameObject _gameObject;

    private void OnEnable()
    {
        if (_gameObject == null) _gameObject = gameObject;
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<EnemyUnit>(out EnemyUnit enemyUnit))
        {
            if (!enemyUnit.underLastingFireInjure) enemyUnit.lastingInjure(_effectTime, _HPReduce, _onEnemyEffectIndex);
            _gameObject.SetActive(false);
        }

        if (collision.gameObject.TryGetComponent<EnemyShot>(out EnemyShot shot))
        {
            shot.reduceHP(10);
            _gameObject.SetActive(false);
        }
    }


    public override void setPropertiesOfSuperHitEffect(int HPReduce, float effectTime, int onEnemyEffectIndex)
    {
        _HPReduce = HPReduce;
        _effectTime = effectTime;
        _onEnemyEffectIndex = onEnemyEffectIndex;
    }
}
