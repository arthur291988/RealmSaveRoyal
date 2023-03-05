using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFifteen : PlayerUnit
{
    //Spear woman, index of unit 15
    //index of super hit particle system none
    //index of super hit weapon 2
    //aimed attack

    private int indexOnSuperShotsObjectPuller;
    private int superHitEffectOnEnemyIndex;

    private void OnEnable()
    {
        _baseHarm = 13;
        _baseAccuracy = 0.17f;
        _baseAttackSpeed = 1.1f;

        _baseSuperHitHarm = 8;
        _baseSuperHitTime = 4.5f;

        superHitsCount = 3;
        superHitsCounter = 0;

        indexOnSuperShotsObjectPuller = 4; //4 is peak pit
        superHitEffectOnEnemyIndex = 2; //2 is blood effect
        setStartProperties();
    }

    public override void updatePropertiesToLevel(bool powerUp)
    {
        base.updatePropertiesToLevel(powerUp);
    }
    public override void attackSimple()
    {
        base.attackSimple();

        ObjectPulled.transform.rotation = Quaternion.FromToRotation(new Vector2(_unitStartPosition.x, _unitStartPosition.y + 1) - _unitStartPosition, attackDirection);
    }



    public override void superHit()
    {
        if (!isBlocked)
        {
            base.superHit();
            float attacPointX = _unitStartPosition.x;
            //float attacPointY = unitSide == 0 ? Random.Range(13, 21) : Random.Range(-13, -21);

            float attacPointY = unitSide == 0 ? _unitStartPosition.y + 10 : _unitStartPosition.y - 10;
            ObjectPulledList = ObjectPuller.current.GetSuperShot(indexOnSuperShotsObjectPuller);//0 is fire circle effect
            ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
            ObjectPulled.transform.position = new Vector2(attacPointX, attacPointY);
            ObjectPulled.GetComponent<SuperHitBase>().setPropertiesOfSuperHitEffect(_superHitHarm, _superHitTime, superHitEffectOnEnemyIndex); // no need to assign 
            ObjectPulled.SetActive(true);
        }
    }
}
