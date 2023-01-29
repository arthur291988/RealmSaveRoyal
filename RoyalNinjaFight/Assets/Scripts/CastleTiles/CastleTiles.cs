using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CastleTiles : MonoBehaviour
{
    [NonSerialized]
    public int HP;
    [NonSerialized]
    public GameObject _gameObject;
    [NonSerialized]
    public Vector2 _position;
    [NonSerialized]
    public Vector2 _playerPosition;
    [NonSerialized]
    public SpriteRenderer _spriteRenderer;
    [NonSerialized]
    public PlayerUnit _playerUnit;

    [NonSerialized]
    public int _tileSide; //0 up 1 down

    private Animator _animator;


    private void OnEnable()
    {
        _gameObject = gameObject;
        if (_animator == null) _animator = _gameObject.GetComponent<Animator>();
        setCastleTileLayer();
    }

    public void addCastleTilePropertiesToCommonData(int tileSide, Vector2 position)
    {
        HP = CommonData.instance.HPOfTile;
        _tileSide = tileSide;
        _position = position;
        _playerPosition = new Vector2(position.x, position.y+GameController.instance.unitYShift);
        if (tileSide == 0)
        {
            CommonData.instance.castlePointsUp.Add(_position);
            CommonData.instance.platformTilesUp[_position] = 1;//set platform to "filled" mode, cause it is not empty any more
        }
        else
        {
            CommonData.instance.castlePointsDown.Add(_position);
            CommonData.instance.platformTilesDown[_position] = 1;//set platform to "filled" mode, cause it is not empty any more
        }

        CommonData.instance.castlePointsWithNoUnits.Add(_position);
        CommonData.instance.castleTiles.Add(this);

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerUnit = null;
        setTileSprite();
    }

    public void setCastleTileLayer() {
        if (_position.y==-8) _spriteRenderer.sortingOrder = 6;
        else if (_position.y == -4) _spriteRenderer.sortingOrder = 5;
        else if(_position.y == 4) _spriteRenderer.sortingOrder = 3;
        else if(_position.y == 8) _spriteRenderer.sortingOrder = 2;
    }

    private void setTileSprite() {
        if (HP > 4) _spriteRenderer.sprite = CommonData.instance.castleTileSpriteAtlases.GetSprite("0");
        else if (HP > 3) _spriteRenderer.sprite = CommonData.instance.castleTileSpriteAtlases.GetSprite("1");
        else if (HP > 2) _spriteRenderer.sprite = CommonData.instance.castleTileSpriteAtlases.GetSprite("2");
        else _spriteRenderer.sprite = CommonData.instance.castleTileSpriteAtlases.GetSprite("3");
    }
   
    //tile destruction by enemy hit
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<EnemyUnit>(out EnemyUnit unit))
        {
            unit.disactivateUnit();
            reduceHP();
        }
    }
    public virtual void reduceHP()
    {
        HP--;
        if (HP < 1) disactivateTile();
        else
        {
            setTileSprite();
            _animator.SetBool("Play",true);
        }
    }

    public void animationFalse()
    {
        _animator.SetBool("Play", false);
    }


    private void destroyPlayerUnitStayingOnThisPlatform() {
        for (int i = 0; i < CommonData.instance.playerUnits.Count; i++) {
            if (CommonData.instance.playerUnits[i]._unitStartPosition == _playerPosition)
            {
                CommonData.instance.playerUnits[i].disactivateUnit();
                return;
            }
        }
    }

    public virtual void disactivateTile()
    {
        if (_playerUnit==null) CommonData.instance.castlePointsWithNoUnits.Remove(_position);
        if (_tileSide == 0)
        {
            CommonData.instance.castlePointsUp.Remove(_position);
            CommonData.instance.platformTilesUp[_position] = 0;//set platform to empty mode
        }
        else
        {
            CommonData.instance.castlePointsDown.Remove(_position);
            CommonData.instance.platformTilesDown[_position] = 0;//set platform to empty mode
        }



        CommonData.instance.castleTiles.Remove(this);
        _playerUnit = null;
        destroyPlayerUnitStayingOnThisPlatform();
        _gameObject.SetActive(false);
        GameController.instance.updateUnitsAndCastleTileAddButtonsUI();
    }
}
