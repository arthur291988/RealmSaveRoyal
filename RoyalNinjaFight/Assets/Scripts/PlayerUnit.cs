using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit:MonoBehaviour
{
    [HideInInspector]
    public int harm;

    [HideInInspector]
    public GameObject ObjectPulled;
    [HideInInspector]
    public List<GameObject> ObjectPulledList;

    [HideInInspector]
    public Transform _transform;
    [HideInInspector]
    public Vector2 _unitStartPosition;

    [HideInInspector]
    public float attackTimer;
    [HideInInspector]
    public Vector2 attackDirection;
    [HideInInspector]
    public float randomnessOfSimpleAttackDirection;

    [HideInInspector]
    public SpriteRenderer _unitSpriteRenderer;

    public void Start()
    {
        _transform = transform;
        _unitStartPosition = new Vector2(_transform.position.x, _transform.position.y);
        _unitSpriteRenderer = GetComponent<SpriteRenderer>();
        attackTimer = Random.Range(ATTACK_TIME - 2.5f, ATTACK_TIME - 1f);
        addToCommonData();
    }

    public void addToCommonData()
    {
        CommonData.instance.playerUnits.Add(this);
    }
    public void removeFromCommonData()
    {
        CommonData.instance.playerUnits.Remove(this);
    }
    public void disactivateUnit()
    {
        removeFromCommonData();
        gameObject.SetActive(false);
    }
    public void attackSimple()
    {
        randomnessOfSimpleAttackDirection = Random.Range(0, 2.5f);
        if (Random.Range(0, 2) == 0) randomnessOfSimpleAttackDirection *= -1;

        ObjectPulledList = ObjectPuller.current.GetPlayerSurikenPullList();
        ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
        ObjectPulled.transform.position = _transform.position;
        ObjectPulled.SetActive(true);

        EnemyUnit unitToAttack = CommonData.instance.enemyUnits.Count == 1 ? CommonData.instance.enemyUnits[0] : CommonData.instance.enemyUnits[Random.Range(0, CommonData.instance.enemyUnits.Count)];
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
