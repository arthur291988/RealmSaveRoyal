using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTTwo : PlayerUnit
{
    //desert warrior, index of unit 22
    //index of super hit particle system none
    //index of super hit weapon 0 basic
    //aimed attack


    private int indexOnSuperShotsObjectPuller;

    private void OnEnable()
    {
        _baseHarm = 14;
        _baseAccuracy = 0.19f;
        _baseAttackSpeed = 1.4f;
        _baseSuperHitHarm = 20; //base peak harm
        superHitsCount = 3;
        indexOnSuperShotsObjectPuller = 5; //five is peak
        superHitsCounter = 0;
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
            float attacPointX = Random.Range(CommonData.instance.leftEdgeofCastleTiles, CommonData.instance.rightEdgeofCastleTiles);
            float attacPointY = unitSide == 0 ? Random.Range(22, GameController.instance.topShotLine) : Random.Range(-22, GameController.instance.bottomShotLine);
            ObjectPulledList = ObjectPuller.current.GetSuperShot(indexOnSuperShotsObjectPuller);
            ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
            ObjectPulled.transform.position = _unitStartPosition;
            ObjectPulled.GetComponent<PeakFly>().setProperties(_unitStartPosition, new Vector2(attacPointX, attacPointY), _superHitHarm);
            ObjectPulled.SetActive(true);
            //repeat the method to make additional shots according the features of unit
            superHitsCounter++;
            if (superHitsCounter < superHitsCount) superHit();
            else
            {
                superHitsCounter = 0;
                base.superHit();
            }
        }
    }
}
