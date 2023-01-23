
using System.Runtime.CompilerServices;
using UnityEngine;

public class UnitOne : PlayerUnit
{
    //fire wizard, index of unit 1
    //index of super hit particle system 0
    //index of super hit weapon none

    private int superHitEffectOnEnemyIndex;

    private int superHitIndexOnObjectPuller;

    private void OnEnable()
    {
        _baseHarm = 12;
        _baseAccuracy = 0.2f;
        _baseAttackSpeed = 1.4f;
        _baseSuperHitHarm = 1;
        _baseSuperHitTime = 2.5f;
        superHitEffectOnEnemyIndex = 0; //fire on enemy effect index
        superHitIndexOnObjectPuller = 0; //fire circle index on object puller
        setStartProperties();
    }

    public override void updatePropertiesToLevel()
    {
        base.updatePropertiesToLevel();
    }
    public override void superHit()
    {
        float attacPointX = _unitStartPosition.x;
        float attacPointY = unitSide == 0 ? Random.Range(10, GameController.instance.topShotLine) : Random.Range(-10, GameController.instance.bottomShotLine);
        ObjectPulledList = ObjectPuller.current.GetSuperShotParticleEffects(superHitIndexOnObjectPuller);//0 is fire circle effect
        ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
        ObjectPulled.transform.position = new Vector2 (attacPointX, attacPointY);
        ObjectPulled.GetComponent<ParticleCollision>().setPropertiesOfSuperHitEffect(_superHitHarm,_superHitTime, superHitEffectOnEnemyIndex);
        ObjectPulled.SetActive(true);
    }
}
