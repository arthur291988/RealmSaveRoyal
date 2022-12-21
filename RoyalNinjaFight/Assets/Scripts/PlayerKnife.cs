using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnife : Knife
{
    public int _harm;
    public override void OnCollisionEnter2D(Collision2D collision)
    {
        
        //if (collision.gameObject.TryGetComponent<EnemyKnife>(out EnemyKnife knife)) _gameObject.SetActive(false);
        if (collision.gameObject.TryGetComponent<EnemyUnit>(out EnemyUnit enemyUnit)) _gameObject.SetActive(false);
        base.OnCollisionEnter2D(collision);
    }
    private void OnDisable()
    {
        _harm = 0;
    }
    //public override void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.TryGetComponent<EnemyUnit>(out EnemyUnit unit))
    //    {
    //        _gameObject.SetActive(false);
    //        base.OnTriggerEnter2D(collision);
    //    }
    //}

}
