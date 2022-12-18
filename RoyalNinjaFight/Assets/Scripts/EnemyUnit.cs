using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyUnit : Unit
{
    private int _enemyLevel;
    private int _energyOnDestroy;
    private float _moveSpeed;

    public override void Start()
    {
        base.Start();
        attackTimer = Random.Range(ATTACK_TIME + 4, ATTACK_TIME + 10f);
    }

    private void OnEnable()
    {
        setEnemyFeatures();
        Invoke("setMoveToPoint", 1f);
    }

    public void setEnemyLevel(int level) =>_enemyLevel=level;

    private void setEnemyFeatures() {
        if (_enemyLevel == 1)
        {
            _moveSpeed = CommonData.instance.speedOfEnemy1;
            _energyOnDestroy = CommonData.instance.energyOfEnemy1;
        }
        HP = _enemyLevel;
    }

    private void setMoveToPoint() =>
        _movePoint = CommonData.instance.platformPoints.Count > 0 ? CommonData.instance.platformPoints[Random.Range(0, CommonData.instance.platformPoints.Count)] : Vector2.zero;

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.TryGetComponent<PlayerKnife>(out PlayerKnife knife))
    //    {
    //        reduceHP();
    //    }
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.TryGetComponent<PlayerKnife>(out PlayerKnife knife))
    //    {
    //        reduceHP();
    //    }
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerKnife>(out PlayerKnife knife))
        {
            reduceHP();
        }
    }

    public override void removeFromCommonData()
    {
        CommonData.instance.enemyUnits.Remove(this);
    }


    public override void disactivateUnit()
    {
        GameController.instance.incrementEnergy(_energyOnDestroy);
        GameController.instance.updateEneryText();
        removeFromCommonData();
        gameObject.SetActive(false);
    }

    //public override void attackSimple()
    //{
    //    base.attackSimple();

    //    ObjectPulledList = ObjectPuller.current.GetEnemySurikenPullList();
    //    ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
    //    ObjectPulled.transform.position = _transform.position;
    //    ObjectPulled.SetActive(true);
    //    Unit unitToAttack = CommonData.playerUnits.Count == 1 ? CommonData.playerUnits[0] : CommonData.playerUnits[Random.Range(0, CommonData.playerUnits.Count)];
    //    attackDirection = new Vector2 (unitToAttack._transform.position.x+randomnessOfSimpleAttackDirection, unitToAttack._transform.position.y);
    //    attackDirection -= new Vector2 (_transform.position.x, _transform.position.y);
    //    ObjectPulled.GetComponent<Rigidbody2D>().AddForce(attackDirection.normalized * 10, ForceMode2D.Impulse);

    //}

    


    private void Update()
    {
        //if (attackTimer > 0 && CommonData.playerUnits.Count > 0 /*&& CommonData.gameIsOn*/)
        //{
        //    attackTimer -= Time.deltaTime;
        //    if (attackTimer <= 0)
        //    {
        //        //attackSimple();
        //        attackTimer = Random.Range(ATTACK_TIME + 4, ATTACK_TIME + 10f);
        //    }

        //}

    }

    private void FixedUpdate()
    {
        _transform.position = Vector2.MoveTowards(_transform.position, _movePoint, 0.02f);
    }
}
