
using System.Runtime.CompilerServices;
using UnityEngine;

public class UnitOne : PlayerUnit
{
    //fire wizard, index of unit 1
    //index of super hit particle system 0
    //index of super hit weapon none

    private int superHitEffectIndex;

    private void OnEnable()
    {
        _baseHarm = 12;
        _baseAccuracy = 0.2f;
        _baseAttackSpeed = 1.4f;
        _baseSuperHitHarm = 1;
        _baseSuperHitTime = 2.5f;
        superHitEffectIndex=0; //fire circle effect index
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
        ObjectPulledList = ObjectPuller.current.GetSuperShotParticleEffects(superHitEffectIndex);//0 is fire circle effect
        ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
        ObjectPulled.transform.position = new Vector2 (attacPointX, attacPointY);
        ObjectPulled.GetComponent<ParticleCollision>().setPropertiesOfSuperHitEffect(_superHitHarm,_superHitTime, superHitEffectIndex);
        ObjectPulled.SetActive(true);
    }
}
