using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostField : SuperHitBase
{
    private GameObject _gameObject;

    private void OnEnable()
    {
        if (_gameObject == null) _gameObject = gameObject;
    }

    //public override void OnCollisionEnter2D(Collision2D collision)
    //{
        
    //}

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<EnemyUnit>(out EnemyUnit enemyUnit))
        {
            if (!enemyUnit.underLastingFrost) enemyUnit.lastingInjure(_effectTime, _HPReduce, _onEnemyEffectIndex);
        }
    }

    public override void setPropertiesOfSuperHitEffect(int HPReduce, float effectTime, int onEnemyEffectIndex)
    {
        _effectTime = effectTime;
        _effectTimer = _effectTime;
        _onEnemyEffectIndex = onEnemyEffectIndex;
    }

    private void Update()
    {
        _effectTimer -= Time.deltaTime;
        if (_effectTimer <= 0) _gameObject.SetActive(false);
    }
}
