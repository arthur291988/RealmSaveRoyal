using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFour : PlayerUnit
{
    //fire elf, index of unit 4
    //index of super hit particle system 2
    //index of super hit weapon none
    //aimed attack

    private int superHitEffectOnEnemyIndex;

    private int indexOnSuperShotsObjectPuller;

    private void OnEnable()
    {
        _baseHarm = 15;
        _baseAccuracy = 0.18f;
        _baseAttackSpeed = 1.2f;

        _baseSuperHitHarm = 3; //2
        _baseSuperHitTime = 5.5f; //3.5
        superHitEffectOnEnemyIndex = 0; //fire on enemy effect index
        indexOnSuperShotsObjectPuller = 2; //fireball index

        superHitsCount = 3;
        superHitsCounter = 0;

        setStartProperties();
    }

    public override void updatePropertiesToLevel(bool powerUp)
    {
        base.updatePropertiesToLevel(powerUp);
    }


    public override void superHit()
    {
        if (!isBlocked)
        {
            if (CommonData.instance.enemyUnits[unitSide].Count > 0)
            {
                ObjectPulledList = ObjectPuller.current.GetSuperShot(indexOnSuperShotsObjectPuller);//0 is fire circle effect
                ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
                ObjectPulled.transform.position = _unitStartPosition;
                ObjectPulled.GetComponent<SuperHitBase>().setPropertiesOfSuperHitEffect(_superHitHarm, _superHitTime, superHitEffectOnEnemyIndex);

                EnemyUnit unitToAttack = CommonData.instance.enemyUnits[unitSide].Count == 1 ? CommonData.instance.enemyUnits[unitSide][0] :
                        CommonData.instance.enemyUnits[unitSide][UnityEngine.Random.Range(0, CommonData.instance.enemyUnits[unitSide].Count)];

                attackDirection = new Vector2(unitToAttack._transform.position.x, unitToAttack._transform.position.y);
                attackDirection -= _unitStartPosition;
                attackDirection = RotateAttackVector(attackDirection, UnityEngine.Random.Range(-_accuracy, _accuracy));

                ObjectPulled.SetActive(true);
                ObjectPulled.GetComponent<Rigidbody2D>().AddForce(attackDirection.normalized * 30, ForceMode2D.Impulse);


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
}
