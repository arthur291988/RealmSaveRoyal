
using UnityEngine;

public class UnitThree : PlayerUnit
{
    private void OnEnable()
    {
        SetAttackSpeed(CommonData.instance.attackSpeed3);
        SetAttackHarm(CommonData.instance.harmOfUnit3);
        attackTimer = Random.Range(0.5f, 1);
        //addToCommonData();
        _transform = transform;
    }

    public override void updatePropertiesToLevel()
    {
        if (_unitSpriteRenderer == null) _unitSpriteRenderer = GetComponent<SpriteRenderer>();
        _unitSpriteRenderer.sprite = _unitSpriteAtlas.GetSprite(_unitType.ToString() + _unitLevel.ToString());
        _attackSpeed -= CommonData.instance.attackSpeedIncreaseStep3 * _unitLevel;
        _harm += CommonData.instance.attackHarmIncreaseStep3 * _unitLevel;
    }


    public override void attackSimple()
    {
        //randomnessOfSimpleAttackDirection = Random.Range(0, 2.5f);
        //if (Random.Range(0, 2) == 0) randomnessOfSimpleAttackDirection *= -1;

        ObjectPulledList = ObjectPuller.current.GetPlayerShotPullList(3);
        ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
        ObjectPulled.transform.position = _transform.position;
        ObjectPulled.GetComponent<PlayerShot>()._harm = _harm;
        ObjectPulled.SetActive(true);

        //attackDirection = new Vector2(unitToAttack._transform.position.x + randomnessOfSimpleAttackDirection, unitToAttack._transform.position.y);
        //attackDirection -= new Vector2(_transform.position.x, _transform.position.y);

        EnemyUnit unitToAttack = CommonData.instance.enemyUnits[unitSide].Count == 1 ? CommonData.instance.enemyUnits[unitSide][0] :
                CommonData.instance.enemyUnits[unitSide][Random.Range(0, CommonData.instance.enemyUnits[unitSide].Count)];

        attackDirection = new Vector2(unitToAttack._transform.position.x, unitToAttack._transform.position.y);
        attackDirection -= _unitStartPosition;
        attackDirection = RotateAttackVector(attackDirection, Random.Range(-0.2f, 0.2f));

        //attackDirection = RotateAttackVector(CommonData.instance.shotDirection - _unitStartPosition, Random.Range(-0.2f, 0.2f));
        ObjectPulled.GetComponent<Rigidbody2D>().AddForce(attackDirection.normalized * _shotImpulse, ForceMode2D.Impulse);
    }
}
