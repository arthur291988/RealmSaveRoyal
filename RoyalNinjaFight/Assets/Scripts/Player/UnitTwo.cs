
using UnityEngine;

public class UnitTwo : PlayerUnit
{

    private void OnEnable()
    {
        _baseHarm = 30;
        _baseAccuracy = 0.3f;
        _baseAttackSpeed = 2f;

        setStartProperties();

    }

    public override void updatePropertiesToLevel()
    {
        base.updatePropertiesToLevel();
    }


    public override void attackSimple()
    {
        base.attackSimple();
        //randomnessOfSimpleAttackDirection = Random.Range(0, 2.5f);
        //if (Random.Range(0, 2) == 0) randomnessOfSimpleAttackDirection *= -1;

        //ObjectPulledList = ObjectPuller.current.GetPlayerShotPullList(_unitType);
        //ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
        //ObjectPulled.transform.position = _transform.position;
        //ObjectPulled.GetComponent<PlayerShot>()._harm = _harm;
        //ObjectPulled.SetActive(true);

        //EnemyUnit unitToAttack = CommonData.instance.enemyUnits[unitSide].Count == 1 ? CommonData.instance.enemyUnits[unitSide][0] :
        //        CommonData.instance.enemyUnits[unitSide][Random.Range(0, CommonData.instance.enemyUnits[unitSide].Count)];

        //attackDirection = new Vector2(unitToAttack._transform.position.x, unitToAttack._transform.position.y);

        //attackDirection -= _unitStartPosition;
        //attackDirection = RotateAttackVector(attackDirection, Random.Range(-_accuracy, _accuracy));
        //ObjectPulled.GetComponent<Rigidbody2D>().AddForce(attackDirection.normalized * _shotImpulse, ForceMode2D.Impulse);
    }
}
