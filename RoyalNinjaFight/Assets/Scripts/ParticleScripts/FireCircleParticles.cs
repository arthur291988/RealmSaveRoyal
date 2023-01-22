using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCircleParticles : ParticleCollision
{
    private GameObject _gameObject;

    private void OnEnable()
    {
        if (_gameObject == null) _gameObject = gameObject;
    }

    public override void OnParticleCollision(GameObject other)
    {
        EnemyUnit enemyUnit = other.GetComponent<EnemyUnit>();
        if (!enemyUnit.underLastingFireInjure) enemyUnit.lastingInjure(_effectTime, _HPReduce, _onEnemyEffectIndex);
    }

    public override void setPropertiesOfSuperHitEffect(int HPReduce, float effectTime, int onEnemyEffectIndex)
    {
        _HPReduce = HPReduce;
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
