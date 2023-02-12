
using UnityEngine;

public class UnitThree : PlayerUnit
{
    //archer, index of unit 3
    //index of super hit particle system none
    //index of super hit weapon 5
    //non aimed attack

    //private int superHitEffectOnEnemyIndex;

    private int indexOnSuperShotsObjectPuller;

    private float XSpreadOfSimpleShot;

    private float XSpreadOfSuperShot1;
    private float XSpreadOfSuperShot2;
    private float XSpreadOfSuperShot3;
    private float XSpreadOfSuperShot4;

    private void OnEnable()
    {
        _baseHarm = 12;
        _baseAccuracy = 0.1f;
        _baseAttackSpeed = 1f;
        _baseSuperHitHarm = 10;
        //_baseSuperHitTime = 4;
        indexOnSuperShotsObjectPuller = 1; //frost circle onject puller index
                                           
        XSpreadOfSimpleShot = 4f;


        simpleShotsCounter = 0;
        simpleShotsCount = 2;

        superHitsCount = 9;
        superHitsCounter = 0;

        XSpreadOfSuperShot1 = 16f;
        XSpreadOfSuperShot2 = 8;
        XSpreadOfSuperShot3 = 4;
        XSpreadOfSuperShot4 = 2;

        setStartProperties();

    }


    public override void attackSimple()
    {
        ObjectPulledList = ObjectPuller.current.GetPlayerShotPullList(_unitType);
        ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
        ObjectPulled.transform.position = _transform.position;
        ObjectPulled.GetComponent<PlayerShot>()._harm = _harm;

        int sideMultiplier = unitSide == 0 ? 1 : -1; //0 is up

        //if (simpleShotsCounter == 0) attackDirection = new Vector2(_unitStartPosition.x, GameController.instance.topShotLine * sideMultiplier) - _unitStartPosition;
        if (simpleShotsCounter == 0) attackDirection = new Vector2(_unitStartPosition.x + XSpreadOfSimpleShot, GameController.instance.topShotLine * sideMultiplier) - _unitStartPosition;
        else if (simpleShotsCounter == 1) attackDirection = new Vector2(_unitStartPosition.x - XSpreadOfSimpleShot, GameController.instance.topShotLine * sideMultiplier) - _unitStartPosition;


        ObjectPulled.transform.rotation = Quaternion.FromToRotation(new Vector2(_unitStartPosition.x, _unitStartPosition.y + 1) - _unitStartPosition, attackDirection);

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

    public override void updatePropertiesToLevel()
    {
        base.updatePropertiesToLevel();
    }
    public override void superHit()
    {
        if (!isBlocked)
        {
            if (CommonData.instance.enemyUnits[unitSide].Count > 0)
            {
                ObjectPulledList = ObjectPuller.current.GetPlayerShotPullList(_unitType);
                ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
                ObjectPulled.transform.position = _transform.position;
                ObjectPulled.GetComponent<PlayerShot>()._harm = _superHitHarm;

                int sideMultiplier = unitSide == 0 ? 1 : -1; //0 is up

                if (superHitsCounter == 0) attackDirection = new Vector2(_unitStartPosition.x, GameController.instance.topShotLine * sideMultiplier) - _unitStartPosition;
                else if (superHitsCounter == 1) attackDirection = new Vector2(_unitStartPosition.x + XSpreadOfSuperShot1, GameController.instance.topShotLine * sideMultiplier) - _unitStartPosition;
                else if (superHitsCounter == 2) attackDirection = new Vector2(_unitStartPosition.x - XSpreadOfSuperShot1, GameController.instance.topShotLine * sideMultiplier) - _unitStartPosition;
                else if (superHitsCounter == 3) attackDirection = new Vector2(_unitStartPosition.x + XSpreadOfSuperShot2, GameController.instance.topShotLine * sideMultiplier) - _unitStartPosition;
                else if (superHitsCounter == 4) attackDirection = new Vector2(_unitStartPosition.x - XSpreadOfSuperShot2, GameController.instance.topShotLine * sideMultiplier) - _unitStartPosition;
                else if (superHitsCounter == 5) attackDirection = new Vector2(_unitStartPosition.x + XSpreadOfSuperShot3, GameController.instance.topShotLine * sideMultiplier) - _unitStartPosition;
                else if (superHitsCounter == 6) attackDirection = new Vector2(_unitStartPosition.x - XSpreadOfSuperShot3, GameController.instance.topShotLine * sideMultiplier) - _unitStartPosition;
                else if (superHitsCounter == 5) attackDirection = new Vector2(_unitStartPosition.x + XSpreadOfSuperShot4, GameController.instance.topShotLine * sideMultiplier) - _unitStartPosition;
                else if (superHitsCounter == 6) attackDirection = new Vector2(_unitStartPosition.x - XSpreadOfSuperShot4, GameController.instance.topShotLine * sideMultiplier) - _unitStartPosition;


                ObjectPulled.transform.rotation = Quaternion.FromToRotation(new Vector2(_unitStartPosition.x, _unitStartPosition.y + 1) - _unitStartPosition, attackDirection);

                ObjectPulled.SetActive(true);
                ObjectPulled.GetComponent<Rigidbody2D>().AddForce(attackDirection.normalized * _shotImpulse, ForceMode2D.Impulse);


                //repeat the method to make additional shots according the features of unit
                superHitsCounter++;
                if (superHitsCounter < superHitsCount) superHit();
                else superHitsCounter = 0;
            }
        }

    }
}
