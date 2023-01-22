using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitZero : PlayerUnit
{
    //dwarf, index of unit 0
    //index of super hit particle system none
    //index of super hit weapon 0

    private void OnEnable()
    {
        _baseHarm = 17;
        _baseAccuracy = 0.22f;
        _baseAttackSpeed = 1.8f;
        _baseSuperHitHarm = _baseHarm - 4;
        superHitsCount = 3;
        superHitsCounter = 0;
        setStartProperties();
    }

    public override void updatePropertiesToLevel()
    {
        base.updatePropertiesToLevel();
    }

    public override void superHit()
    {
        float attacPointX = Random.Range(CommonData.instance.leftEdgeofCastleTiles, CommonData.instance.rightEdgeofCastleTiles);
        float attacPointY = unitSide==0? Random.Range(10, GameController.instance.topShotLine): Random.Range(-10, GameController.instance.bottomShotLine);
        ObjectPulledList = ObjectPuller.current.GetSuperShot(0); //zero is peak
        ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
        ObjectPulled.transform.position = _unitStartPosition;
        ObjectPulled.GetComponent<PeakFly>().setProperties(_unitStartPosition, new Vector2(attacPointX, attacPointY), _superHitHarm);
        ObjectPulled.SetActive(true);
        //repeat the method to make additional shots according the features of unit
        superHitsCounter++;
        if (superHitsCounter < superHitsCount) superHit();
        else superHitsCounter = 0;
    }
}
