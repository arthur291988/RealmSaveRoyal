using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWall : MonoBehaviour
{
    [NonSerialized]
    public int _HP;

    [NonSerialized]
    public Transform _transform;

    private GameObject _gameObject;
    private Animator _animator; //animator is the same as castleTile
    [NonSerialized]
    public SpriteRenderer _spriteRenderer;

    private void OnEnable()
    {
        if (_animator == null) _animator = _gameObject.GetComponent<Animator>();
        if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();
        setTileSprite();
    }

    public void startSettings(int HP, GameObject gameObject)
    {
        _HP = HP;
        _transform = transform;
        _gameObject = gameObject;
    }

    public void reduceHP()
    {
        _HP--;

        if (_HP <= 0) disactivateThis();
        else
        {
            setTileSprite();
            _animator.SetBool("Play", true);
            Invoke("animationFalse", 0.1f);
        }
    }

    public void animationFalse()
    {
        _animator.SetBool("Play", false);
    }

    public void setTileSprite()
    {
        if (_HP > 6) _spriteRenderer.sprite = GameController.instance.enemyWallSpriteAtlases.GetSprite("0");
        else if (_HP > 4) _spriteRenderer.sprite = GameController.instance.enemyWallSpriteAtlases.GetSprite("1");
        else if (_HP > 2) _spriteRenderer.sprite = GameController.instance.enemyWallSpriteAtlases.GetSprite("2");
    }

    public void disactivateThis()
    {
        _gameObject.SetActive(false);
        animationFalse();
    }

}
