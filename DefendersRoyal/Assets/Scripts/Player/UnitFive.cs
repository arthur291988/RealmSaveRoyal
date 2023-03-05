using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFive : PlayerUnit
{
    //frost elf, index of unit 5
    //index of super hit particle system 3
    //index of super hit weapon none
    //aimed attack


    private int superHitEffectOnEnemyIndex;

    private int indexOnSuperShotsObjectPuller;

    private void OnEnable()
    {
        _baseHarm = 14;
        _baseAccuracy = 0.17f;
        _baseAttackSpeed = 1.1f;

        _baseSuperHitHarm = 0;
        _baseSuperHitTime = 7f; // 4f;
        superHitEffectOnEnemyIndex = 1; //frost on enemy effect index
        indexOnSuperShotsObjectPuller = 3; //frost ball index

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
                ObjectPulledList = ObjectPuller.current.GetSuperShot(indexOnSuperShotsObjectPuller);
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
