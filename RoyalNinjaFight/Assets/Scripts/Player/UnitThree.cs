
using UnityEngine;

public class UnitThree : PlayerUnit
{
    private void OnEnable()
    {
        _baseHarm = 6;
        _baseAccuracy = 0.15f;
        _baseAttackSpeed = 0.8f;
        setStartProperties();
    }

    public override void updatePropertiesToLevel()
    {
        base.updatePropertiesToLevel();
    }


    public override void attackSimple()
    {
        base.attackSimple();
        ////randomnessOfSimpleAttackDirection = Random.Range(0, 2.5f);
        ////if (Random.Range(0, 2) == 0) randomnessOfSimpleAttackDirection *= -1;

        //ObjectPulledList = ObjectPuller.current.GetPlayerShotPullList(_unitType);
        //ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
        //ObjectPulled.transform.position = _transform.position;
        //ObjectPulled.GetComponent<PlayerShot>()._harm = _harm;
        //ObjectPulled.SetActive(true);

        ////attackDirection = new Vector2(unitToAttack._transform.position.x + randomnessOfSimpleAttackDirection, unitToAttack._transform.position.y);
        ////attackDirection -= new Vector2(_transform.position.x, _transform.position.y);

        //EnemyUnit unitToAttack = CommonData.instance.enemyUnits[unitSide].Count == 1 ? CommonData.instance.enemyUnits[unitSide][0] :
        //        CommonData.instance.enemyUnits[unitSide][Random.Range(0, CommonData.instance.enemyUnits[unitSide].Count)];

        //attackDirection = new Vector2(unitToAttack._transform.position.x, unitToAttack._transform.position.y);
        //attackDirection -= _unitStartPosition;
        //attackDirection = RotateAttackVector(attackDirection, Random.Range(-0.2f, 0.2f));

        ////attackDirection = RotateAttackVector(CommonData.instance.shotDirection - _unitStartPosition, Random.Range(-0.2f, 0.2f));
        //ObjectPulled.GetComponent<Rigidbody2D>().AddForce(attackDirection.normalized * _shotImpulse, ForceMode2D.Impulse);

    }
}
