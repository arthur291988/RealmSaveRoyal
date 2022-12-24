﻿using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyUnit : MonoBehaviour
{
    private int _enemyLevel;
    private int _energyOnDestroy;
    private float _moveSpeed;

    [HideInInspector]
    public int HP;

    [HideInInspector]
    public Transform _transform;
    [HideInInspector]
    public Vector2 _unitStartPosition;
    [HideInInspector]
    public Vector2 _movePoint;


    public void Start()
    {
        _transform = transform;
        _unitStartPosition = new Vector2(_transform.position.x, _transform.position.y);
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
            _energyOnDestroy = CommonData.instance.energyFromEnemy1;
        }
        HP = _enemyLevel;
    }

    private void setMoveToPoint() =>
        _movePoint = CommonData.instance.platformPoints.Count > 0 ? CommonData.instance.platformPoints[Random.Range(0, CommonData.instance.platformPoints.Count)] : Vector2.zero;

    public void reduceHP(int harm)
    {
        HP -= harm;
        if (HP < 1) disactivateUnit();
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.TryGetComponent<PlayerShot>(out PlayerShot shot))
    //    {
    //        reduceHP();
    //    }
    //}
    

    public void removeFromCommonData()
    {
        CommonData.instance.enemyUnits.Remove(this);
    }

    public void disactivateUnit()
    {
        GameController.instance.incrementEnergy(_energyOnDestroy);
        GameController.instance.updateEneryText();
        removeFromCommonData();
        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        _transform.position = Vector2.MoveTowards(_transform.position, _movePoint, 0.02f);
    }
}
