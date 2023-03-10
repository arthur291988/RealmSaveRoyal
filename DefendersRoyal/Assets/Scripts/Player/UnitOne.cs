
using System.Runtime.CompilerServices;
using UnityEngine;

public class UnitOne : PlayerUnit
{
    //fire wizard, index of unit 1
    //index of super hit particle system 0
    //index of super hit weapon none
    //non aimed attack

    private int superHitEffectOnEnemyIndex;

    private int indexOnSuperShotsObjectPuller;


    private float XSpreadOfSimpleShot1;
    private float XSpreadOfSimpleShot2;

    private void OnEnable()
    {
        _baseHarm = 12;
        _baseAccuracy = 0.2f;
        _baseAttackSpeed = 2.9f;
        _baseSuperHitHarm = 3; //1
        _baseSuperHitTime = 3f; //2.5f

        superHitEffectOnEnemyIndex = 0; //fire on enemy effect index
        indexOnSuperShotsObjectPuller = 0; //fire circle index on object puller


        XSpreadOfSimpleShot1 = 14f;
        XSpreadOfSimpleShot2 = 7;

        setStartProperties();
        setUnitTalentLevel(0); //TO DO to delet from here and assign talent level while setting battle sene according to player settings
    }

    public override void setUnitTalentLevel(int talentLvl)
    {
        if (talentLvl == 0) simpleShotsCount = 3;
        else if (talentLvl == 1) simpleShotsCount = 4;
        else simpleShotsCount = 5;
    }

    public override void updatePropertiesToLevel(bool powerUp)
    {
        base.updatePropertiesToLevel(powerUp);
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
        else if (simpleShotsCounter == 1) attackDirection = new Vector2(_unitStartPosition.x+ XSpreadOfSimpleShot1, GameController.instance.topShotLine * sideMultiplier) - _unitStartPosition;
        else if(simpleShotsCounter == 2) attackDirection = new Vector2(_unitStartPosition.x - XSpreadOfSimpleShot1, GameController.instance.topShotLine * sideMultiplier) - _unitStartPosition;
        else if(simpleShotsCounter == 3) attackDirection = new Vector2(_unitStartPosition.x - XSpreadOfSimpleShot2, GameController.instance.topShotLine * sideMultiplier) - _unitStartPosition;
        else attackDirection = new Vector2(_unitStartPosition.x+ XSpreadOfSimpleShot2, GameController.instance.topShotLine * sideMultiplier) - _unitStartPosition;

        ObjectPulled.SetActive(true);
        ObjectPulled.GetComponent<Rigidbody2D>().AddForce(attackDirection.normalized * _shotImpulse, ForceMode2D.Impulse);

        if (simpleShotsCounter == 0) animationPlay(); // play animation only once
        //repeat the method to make additional shots according the features of unit
        simpleShotsCounter++;
        if (simpleShotsCounter < simpleShotsCount) attackSimple();
        else
        {
            simpleShotsCounter = 0;
        }
    }

    public override void superHit()
    {
        if (!isBlocked)
        {
            base.superHit();
            float attacPointX = _unitStartPosition.x;
            //float attacPointY = unitSide == 0 ? Random.Range(13, 21/*GameController.instance.topShotLine*/) : Random.Range(-13, -21 /*GameController.instance.bottomShotLine*/);

            float attacPointY = unitSide == 0 ? _unitStartPosition.y + 10 : _unitStartPosition.y - 10;
            ObjectPulledList = ObjectPuller.current.GetSuperShot(indexOnSuperShotsObjectPuller);//0 is fire circle effect
            ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
            ObjectPulled.transform.position = new Vector2(attacPointX, attacPointY);
            ObjectPulled.GetComponent<SuperHitBase>().setPropertiesOfSuperHitEffect(_superHitHarm, _superHitTime, superHitEffectOnEnemyIndex);
            ObjectPulled.SetActive(true);
        }
    }
}
