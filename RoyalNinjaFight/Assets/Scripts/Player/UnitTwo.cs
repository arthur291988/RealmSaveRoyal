
using UnityEngine;

public class UnitTwo : PlayerUnit
{
    //frost wizard, index of unit 2
    //index of super hit particle system 1
    //index of super hit weapon none

    private int superHitEffectOnEnemyIndex;

    private int superHitIndexOnObjectPuller;

    private void OnEnable()
    {
        _baseHarm = 12;
        _baseAccuracy = 0.2f;
        _baseAttackSpeed = 1.25f;
        _baseSuperHitHarm = 0;
        _baseSuperHitTime = 4;
        superHitEffectOnEnemyIndex = 1; //frost effect on enemy
        superHitIndexOnObjectPuller = 1; //frost circle onject puller index 

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
        ObjectPulled.transform.position = new Vector2(attacPointX, attacPointY);
        ObjectPulled.GetComponent<ParticleCollision>().setPropertiesOfSuperHitEffect(_superHitHarm, _superHitTime, superHitEffectOnEnemyIndex);
        ObjectPulled.SetActive(true);
    }
}
