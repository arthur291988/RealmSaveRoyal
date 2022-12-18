using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Unit : MonoBehaviour
{
    [HideInInspector]
    public GameObject ObjectPulled;
    [HideInInspector]
    public List<GameObject> ObjectPulledList;
    [HideInInspector]
    public Transform _transform;
    [HideInInspector]
    public const float ATTACK_TIME = 3;
    [HideInInspector]
    public float attackTimer;
    [HideInInspector]
    public int HP;
    [HideInInspector]
    public Vector2 attackDirection;
    [HideInInspector]
    public float randomnessOfSimpleAttackDirection;
    [HideInInspector]
    public Vector2 _unitStartPosition;
    [HideInInspector]
    public SpriteRenderer _unitSpriteRenderer;
    public Vector2 _movePoint;


    // Start is called before the first frame update
    public virtual void Start()
    {
        _transform = transform;
        _unitStartPosition = new Vector2(_transform.position.x, _transform.position.y);
        _unitSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void removeFromCommonData()
    {

    }

    public virtual void attackSimple()
    {
        randomnessOfSimpleAttackDirection = Random.Range(0,2.5f);
        if (Random.Range(0, 2) == 0) randomnessOfSimpleAttackDirection *= -1;
    }

    //public virtual void OnTriggerEnter2D(Collider2D collision)
    //{
        
    //}


    public void reduceHP()
    {
        HP--;
        
        //if (HP > 0) GameManager.current.ninjaOuchSound.Play();
        //GameManager.current.enemiesDestroyedInOneAir++;
        //GameManager.current.incrementScoreBasis();
        //GameManager.current.countTheScore();

        if (HP < 1) disactivateUnit(); //argument is for using with backToMenu function
        //else
        //{
        //    changeTheSpriteOfEnemy(enemyLevelString + (enemyLevel - enemyHP).ToString());
        //}
    }

    public virtual void disactivateUnit()
    {
        //GameManager.current.ninjaDestroySound.Play();
        //GameManager.current.allEnemiesWithPositionIndexes.Remove(indexOfThisEnemy);
        //if (enemyLevel == 2) GameManager.current.all2LevelEnemies.Remove(this);
        //else if (enemyLevel == 3) GameManager.current.all3LevelEnemies.Remove(this);
        //else if (enemyLevel == 4) GameManager.current.all4LevelEnemies.Remove(this);
        //else if (enemyLevel == 5) GameManager.current.all5LevelEnemies.Remove(this);
        //GameManager.current.checkIfWin();
        //nearEnemyIndexes.Clear();
        //thisGo.SetActive(false);
        //ObjectPulledList = ObjectPuller.current.GetDestroyEffectPullList();
        //ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
        //ObjectPulled.transform.position = positionOfThisEnemy;
        //MainModule main = ObjectPulled.GetComponent<ParticleSystem>().main;
        //main.startColor = colorOfEnemy;
        //if (enemyLevel == 4)
        //{
        //    thisGo.layer = 6;
        //    enemySpriteRenderer.color = colorOfEnemyNormal;
        //}
        //if (enemyLevel == 5)
        //{
        //    isInGoldReflectionMode = false;
        //    goldEnemyBackPicture.SetActive(false);
        //}
        //if (enemyLevel >= 4) StopAllCoroutines();
        //ObjectPulled.SetActive(true);
    }

    //private void Update()
    //{
    //    if (attackTimer > 0)
    //    {
    //        attackTimer -= Time.deltaTime;
    //        if (attackTimer <= 0)
    //        {
    //            attackSimple();
    //            attackTimer = ATTACK_TIME;
    //        }

    //    }
    //}


}
