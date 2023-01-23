using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBallParticle : ParticleCollision
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
        _gameObject.SetActive(false);

    }

    public override void setPropertiesOfSuperHitEffect(int HPReduce, float effectTime, int onEnemyEffectIndex)
    {
        _HPReduce = HPReduce;
        _effectTime = effectTime;
        _onEnemyEffectIndex = onEnemyEffectIndex;
    }
}
