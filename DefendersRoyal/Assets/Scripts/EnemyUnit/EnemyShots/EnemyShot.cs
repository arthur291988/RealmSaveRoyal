using System;
using UnityEngine;

public class EnemyShot : MonoBehaviour
{
    [NonSerialized]
    public float _moveSpeed;

    [NonSerialized]
    public int _HP;

    [NonSerialized]
    public Transform _transform;

    [NonSerialized]
    public Vector2 _movePoint;

    private GameObject _gameObject;

    public virtual void startSettings(int HP, float moveSpeed, Vector2 moveToPoint, GameObject gameObject)
    {
        _HP = HP;
        _transform = transform;
        _moveSpeed = moveSpeed;
        _movePoint = moveToPoint;
        _gameObject = gameObject;
    }

    public void reduceHP(int harm)
    {
        _HP -= harm;

        if (_HP <= 0) disactivateThis();
    }


    public void disactivateThis()
    {
        _gameObject.SetActive(false);
    }

    private void Update()
    {
        //enemy shot move towards player castle
        if (GameController.instance.gameIsOn) _transform.position = Vector2.MoveTowards(_transform.position, _movePoint, _moveSpeed);
        if (_transform.position.y > CommonData.instance.vertScreenSize / 2 + 1 || _transform.position.y < -CommonData.instance.vertScreenSize / 2 - 1)
        {
            gameObject.SetActive(false);
        }
    }
}
