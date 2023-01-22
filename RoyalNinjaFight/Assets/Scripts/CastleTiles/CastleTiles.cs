using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CastleTiles : MonoBehaviour
{
    [HideInInspector]
    public int HP;
    [HideInInspector]
    public GameObject _gameObject;
    [HideInInspector]
    public Vector2 _position;
    public Vector2 _playerPosition;
    [HideInInspector]
    public SpriteRenderer _spriteRenderer;
    [HideInInspector]
    public PlayerUnit _playerUnit;

    [HideInInspector]
    public int _tileSide; //0 up 1 down

    private void OnEnable()
    {
        _gameObject = gameObject;
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
    }

    public void setCastleTileLayer() {
        if (_position.y==-8) _spriteRenderer.sortingOrder = 6;
        else if (_position.y == -4) _spriteRenderer.sortingOrder = 5;
        else if(_position.y == 4) _spriteRenderer.sortingOrder = 3;
        else if(_position.y == 8) _spriteRenderer.sortingOrder = 2;
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
    private void reduceHP()
    {
        HP--;
        if (HP < 1) disactivateTile();
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
