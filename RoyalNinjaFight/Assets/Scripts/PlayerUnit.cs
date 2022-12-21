using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit
{
    public int harm;

    public override void Start()
    {
        base.Start();
        attackTimer = Random.Range(ATTACK_TIME - 2.5f, ATTACK_TIME - 1f);
        HP = 4;
        addToCommonData();
    }

    //public override void OnTriggerEnter2D(Collider2D collision) {
    //    if (collision.gameObject.TryGetComponent<EnemyKnife>(out EnemyKnife knife))
    //    {
    //        reduceHP();
    //    }
    //}
    public void addToCommonData()
    {
        CommonData.instance.playerUnits.Add(this);
    }
    public override void removeFromCommonData()
    {
        CommonData.instance.playerUnits.Remove(this);
    }
    public override void disactivateUnit()
    {
        removeFromCommonData();
        gameObject.SetActive(false);
    }
    public override void attackSimple()
    {
        base.attackSimple();

        ObjectPulledList = ObjectPuller.current.GetPlayerSurikenPullList();
        ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
        ObjectPulled.transform.position = _transform.position;
        ObjectPulled.SetActive(true);

        Unit unitToAttack = CommonData.instance.enemyUnits.Count == 1 ? CommonData.instance.enemyUnits[0] : CommonData.instance.enemyUnits[Random.Range(0, CommonData.instance.enemyUnits.Count)];
        //attackDirection = new Vector2(unitToAttack._transform.position.x + randomnessOfSimpleAttackDirection, unitToAttack._transform.position.y);
        //attackDirection -= new Vector2(_transform.position.x, _transform.position.y);
        attackDirection = Rotate(CommonData.instance.shotDirection-_unitStartPosition, Random.Range(-0.2f,0.2f));
        ObjectPulled.GetComponent<Rigidbody2D>().AddForce(attackDirection.normalized * 60, ForceMode2D.Impulse);
    }

    public Vector2 Rotate(Vector2 v, float delta)
    {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }

    private void Update()
    {
        if (attackTimer > 0 && CommonData.instance.enemyUnits.Count>0 /*&& CommonData.gameIsOn*/)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                attackSimple();
                attackTimer = Random.Range(ATTACK_TIME - 2.5f, ATTACK_TIME - 1f);
            }

        }
    }

}
