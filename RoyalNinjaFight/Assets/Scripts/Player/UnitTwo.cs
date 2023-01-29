﻿
using UnityEngine;

public class UnitTwo : PlayerUnit
{
    //frost wizard, index of unit 2
    //index of super hit particle system 1
    //index of super hit weapon none

    private int superHitEffectOnEnemyIndex;

    private int superHitIndexOnObjectPuller;

    private float XSpreadOfSimpleShot1;
    private float XSpreadOfSimpleShot2;

    private void OnEnable()
    {
        _baseHarm = 11;
        _baseAccuracy = 0.2f;
        _baseAttackSpeed = 2.75f;
        _baseSuperHitHarm = 0;
        _baseSuperHitTime = 4;
        superHitEffectOnEnemyIndex = 1; //frost effect on enemy
        superHitIndexOnObjectPuller = 1; //frost circle onject puller index 

        XSpreadOfSimpleShot1 = 14f;
        XSpreadOfSimpleShot2 = 7;

        playerTalantLevel = 0; //to assign player talent leve TODO

        if (playerTalantLevel == 0) simpleShotsCount = 3;
        else if (playerTalantLevel == 1) simpleShotsCount = 4;
        else simpleShotsCount = 5;

        setStartProperties();

    }
    public override void attackSimple()
    {
        ObjectPulledList = ObjectPuller.current.GetPlayerShotPullList(_unitType);
        ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
        ObjectPulled.transform.position = _transform.position;
        ObjectPulled.GetComponent<PlayerShot>()._harm = _harm;

        EnemyUnit unitToAttack = CommonData.instance.enemyUnits[unitSide].Count == 1 ? CommonData.instance.enemyUnits[unitSide][0] :
                CommonData.instance.enemyUnits[unitSide][UnityEngine.Random.Range(0, CommonData.instance.enemyUnits[unitSide].Count)];

        int sideMultiplier = unitSide == 0 ? 1 : -1; //0 is up

        if (simpleShotsCounter == 0) attackDirection = new Vector2(_unitStartPosition.x, GameController.instance.topShotLine * sideMultiplier) - _unitStartPosition;
        else if (simpleShotsCounter == 1) attackDirection = new Vector2(_unitStartPosition.x + XSpreadOfSimpleShot1, GameController.instance.topShotLine * sideMultiplier) - _unitStartPosition;
        else if (simpleShotsCounter == 2) attackDirection = new Vector2(_unitStartPosition.x - XSpreadOfSimpleShot1, GameController.instance.topShotLine * sideMultiplier) - _unitStartPosition;
        else if (simpleShotsCounter == 3) attackDirection = new Vector2(_unitStartPosition.x - XSpreadOfSimpleShot2, GameController.instance.topShotLine * sideMultiplier) - _unitStartPosition;
        else attackDirection = new Vector2(_unitStartPosition.x + XSpreadOfSimpleShot2, GameController.instance.topShotLine * sideMultiplier) - _unitStartPosition;

        ObjectPulled.SetActive(true);
        ObjectPulled.GetComponent<Rigidbody2D>().AddForce(attackDirection.normalized * _shotImpulse, ForceMode2D.Impulse);

        //repeat the method to make additional shots according the features of unit
        simpleShotsCounter++;
        if (simpleShotsCounter < simpleShotsCount) attackSimple();
        else simpleShotsCounter = 0;
    }

    public override void updatePropertiesToLevel()
    {
        base.updatePropertiesToLevel();
    }
    public override void superHit()
    {
        float attacPointX = _unitStartPosition.x;
        float attacPointY = unitSide == 0 ? Random.Range(15, GameController.instance.topShotLine) : Random.Range(-10, GameController.instance.bottomShotLine);
        ObjectPulledList = ObjectPuller.current.GetSuperShotParticleEffects(superHitIndexOnObjectPuller);//0 is fire circle effect
        ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
        ObjectPulled.transform.position = new Vector2(attacPointX, attacPointY);
        ObjectPulled.GetComponent<SuperHitBase>().setPropertiesOfSuperHitEffect(_superHitHarm, _superHitTime, superHitEffectOnEnemyIndex);
        ObjectPulled.SetActive(true);
    }
}
