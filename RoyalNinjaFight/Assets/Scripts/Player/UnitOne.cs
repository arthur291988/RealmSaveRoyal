
using UnityEngine;

public class UnitOne : PlayerUnit
{
    private void OnEnable()
    {
        SetAttackSpeed(CommonData.instance.attackSpeed1);
        SetAttackHarm(CommonData.instance.harmOfUnit1);
        attackTimer = Random.Range(0.5f, 1);
        _transform = transform;
    }

    public override void updatePropertiesToLevel()
    {
        if (_unitSpriteRenderer==null) _unitSpriteRenderer = GetComponent<SpriteRenderer>();
        _unitSpriteRenderer.sprite = _unitSpriteAtlas.GetSprite(_unitType.ToString()+_unitLevel.ToString());
        _attackSpeed -= CommonData.instance.attackSpeedIncreaseStep1 * _unitLevel;
        _harm += CommonData.instance.attackHarmIncreaseStep1 * _unitLevel;
    }


    public override void attackSimple()
    {
        //randomnessOfSimpleAttackDirection = Random.Range(0, 2.5f);
        //if (Random.Range(0, 2) == 0) randomnessOfSimpleAttackDirection *= -1;

        ObjectPulledList = ObjectPuller.current.GetPlayerShotPullList(1);
        ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
        ObjectPulled.transform.position = _transform.position;
        ObjectPulled.GetComponent<PlayerShot>()._harm = _harm;
        ObjectPulled.SetActive(true);

        EnemyUnit unitToAttack = CommonData.instance.enemyUnits[unitSide].Count == 1 ? CommonData.instance.enemyUnits[unitSide][0] :
                CommonData.instance.enemyUnits[unitSide][Random.Range(0, CommonData.instance.enemyUnits[unitSide].Count)];

        attackDirection = new Vector2(unitToAttack._transform.position.x, unitToAttack._transform.position.y);
        attackDirection -= _unitStartPosition;
        attackDirection = RotateAttackVector(attackDirection, Random.Range(-0.2f, 0.2f));

        ObjectPulled.GetComponent<Rigidbody2D>().AddForce(attackDirection.normalized * _shotImpulse, ForceMode2D.Impulse);
    }
}
