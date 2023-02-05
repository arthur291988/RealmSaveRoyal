
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UnitTen : PlayerUnit
{
    //engineer, index of unit 10
    //index of super hit particle system 1
    //index of super hit weapon none
    //non aimed attack


    private float XSpreadOfSimpleShot;
    private float towerFixTime;
    private float towerFixTimer;

    private const int BASE_FIX_COUNT = 2;

    private void OnEnable()
    {
        _baseHarm = 14;
        //_baseAccuracy = 0.2f;
        _baseAttackSpeed = 3f;
        _baseSuperHitHarm = 0;
        _baseSuperHitTime = 4;
        XSpreadOfSimpleShot = 13f;

        _shotImpulse -= 10; //shot impulse is slower for engineer

        simpleShotsCount = 3;
        towerFixTime = 7;
        towerFixTimer = 7;

        unitTalantLevel = 0; //to assign player talent leve on metho TODO

        setStartProperties();
        setUnitTalentLevel(0); //TO DO to delet from here and assign talent level while setting battle sene according to player settings
    }


    public override void setUnitTalentLevel(int talentLvl)
    {
        unitTalantLevel = talentLvl;
    }

    private void fixCastleTile()
    {
        for (int i = 0; i < CommonData.instance.castleTiles.Count; i++)
        {
            if (CommonData.instance.castleTiles[i]._playerUnit = this)
            {
                if (CommonData.instance.castleTiles[i].HP < CommonData.instance.HPOfTile)
                {
                    addHPToCastleTile(CommonData.instance.castleTiles[i], 1);
                    break;
                }
            }
        }
    }

    private void addHPToCastleTile(CastleTiles castleTile, int HPToAdd)
    {
        castleTile.playEffect(0); //0 is power up effect
        castleTile.HP+= HPToAdd;
        castleTile.setTileSprite();

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
        else if (simpleShotsCounter == 1) attackDirection = new Vector2(_unitStartPosition.x + XSpreadOfSimpleShot, GameController.instance.topShotLine * sideMultiplier) - _unitStartPosition;
        else if (simpleShotsCounter == 2) attackDirection = new Vector2(_unitStartPosition.x - XSpreadOfSimpleShot, GameController.instance.topShotLine * sideMultiplier) - _unitStartPosition;
        

        ObjectPulled.SetActive(true);
        ObjectPulled.GetComponent<Rigidbody2D>().AddForce(attackDirection.normalized * _shotImpulse, ForceMode2D.Impulse);

        if (simpleShotsCounter==0) animationPlay(); // play animation only once
        //repeat the method to make additional shots according the features of unit
        simpleShotsCounter++;
        if (simpleShotsCounter < simpleShotsCount) attackSimple();
        else
        {
            simpleShotsCounter = 0;
        }
    }

    public override void updatePropertiesToLevel()
    {
        base.updatePropertiesToLevel();
    }
    public override void superHit()
    {
        //setting fix counts, base fix count + upgrade level + merge level
        int castleTileFixCount = BASE_FIX_COUNT+_unitMergeLevel + CommonData.instance.playerUnitTypesOnScenePowerUpLevel[Array.IndexOf(CommonData.instance.playerUnitTypesOnScene, _unitType)];

        //0-up side unit 1-is down side unit
        for (int i = 0; i < CommonData.instance.castleTiles.Count; i++)
        {
            if (CommonData.instance.castleTiles[i]._tileSide == unitSide)
            {
                if (CommonData.instance.castleTiles[i].HP < CommonData.instance.HPOfTile)
                {
                    int HPToAdd = CommonData.instance.HPOfTile - CommonData.instance.castleTiles[i].HP;
                    if (HPToAdd <castleTileFixCount)
                    {
                        addHPToCastleTile(CommonData.instance.castleTiles[i], HPToAdd);
                        castleTileFixCount -= HPToAdd;
                    }
                    else {
                        addHPToCastleTile(CommonData.instance.castleTiles[i], castleTileFixCount);
                        castleTileFixCount = 0;
                    }
                    if (castleTileFixCount==0) break;// stop if on fix counts left
                }
            }
        }
    }


    public override void Update()
    {
        base.Update();

        //talent 
        if (unitTalantLevel > 0 && !isMoved && GameController.instance.gameIsOn)
        {
            towerFixTimer -= Time.deltaTime;
            if (towerFixTimer <= 0)
            {
                fixCastleTile();
                towerFixTimer = towerFixTime;
            }
        }

    }
}
