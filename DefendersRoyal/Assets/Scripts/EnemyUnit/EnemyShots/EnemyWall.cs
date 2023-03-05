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

    private GameObject ObjectPulled;
    private List<GameObject> ObjectPulledList;

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

    private void pullDestroyEffect()
    {
        ObjectPulledList = ObjectPuller.current.GetWallDestroyPullList();
        ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
        ObjectPulled.transform.position = _transform.position;
        ObjectPulled.SetActive(true);
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
        pullDestroyEffect();
        _gameObject.SetActive(false);
        animationFalse();
    }

}
