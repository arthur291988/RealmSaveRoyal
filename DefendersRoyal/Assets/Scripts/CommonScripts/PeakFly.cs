using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeakFly : MonoBehaviour
{
    //public Transform Target;
    private Vector2 targetPosition;
    private Vector2 startPosition;

    private GameObject _gameObject;
    private Transform _transform;
    //private CircleCollider2D _circleCollider;

    private float speed;
    private float arcHeight;
    private int harm;

    private Vector3 nextPos;
    private float x0;
    private float x1;
    private float dist;
    private float nextX;
    private float baseY;
    private float arc;

    private bool isMoving;

    private float timeToWithdraw;
    private float withdrawTimer;

    void Awake()
    {
        if (_gameObject==null) _gameObject = gameObject;
        if (_transform == null) _transform = transform;
        //if (_circleCollider == null) _circleCollider = GetComponent<CircleCollider2D>();
        arcHeight = 3;
        speed = 25;
        timeToWithdraw = 0.7f;
    }



    public void setProperties(Vector2 startPos, Vector2 targetPos, int _harm) {
        isMoving = true;
        targetPosition = targetPos;
        startPosition = startPos;
        harm = _harm;
        nextPos = startPos;
        withdrawTimer = 0;
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<EnemyUnit>(out EnemyUnit enemyUnit))
        {
            enemyUnit.reduceHP(harm);
            disactivateThisObject();
        }
    }

    private void disactivateThisObject()
    {
        //_circleCollider.enabled = false;
        _gameObject.SetActive(false);
    }

    void Update()
    {
        if (isMoving)
        {  // Compute the next position, with arc added in
            x0 = startPosition.y;
            x1 = targetPosition.y;
            dist = x1 - x0;
            nextX = Mathf.MoveTowards(_transform.position.y, x1, speed * Time.deltaTime);
            baseY = Mathf.Lerp(startPosition.x, targetPosition.x, (nextX - x0) / dist);
            arc = arcHeight * (nextX - x0) * (nextX - x1) / (-0.25f * dist * dist);
            nextPos = new Vector3(baseY + arc, nextX, 0);

            // Rotate to face the next position, and then move there
            _transform.rotation = LookAt2D(nextPos - _transform.position);
            _transform.position = nextPos;
        }
        else {
            withdrawTimer += Time.deltaTime;
            if (withdrawTimer >= timeToWithdraw) disactivateThisObject();
        }
       

        // Do something when we reach the target
        if ((Vector2)nextPos == targetPosition && isMoving)
        {
            isMoving = false;
            //_circleCollider.enabled = true;
        } 

    }


    static Quaternion LookAt2D(Vector2 forward)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg-90);
    }
}


