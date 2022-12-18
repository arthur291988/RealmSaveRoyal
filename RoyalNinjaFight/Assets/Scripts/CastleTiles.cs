using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CastleTiles : MonoBehaviour
{
    private int HP;
    private GameObject _gameObject;
    private Vector2 _position;

    private void Start()
    {
        _gameObject = gameObject;
        CommonData.platformPoints.Add(_position);
        CommonData.platformPointsWithNoUnits.Add(_position);
    }

    private void OnEnable()
    {
        _position = transform.position;
        HP = CommonData.HPOfTile;
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
        for (int i = 0; i < CommonData.playerUnits.Count; i++) {
            if (CommonData.playerUnits[i]._unitStartPosition == _position)
            {
                CommonData.playerUnits[i].disactivateUnit();
                return;
            }
        }
    }

    private void disactivateTile()
    {
        CommonData.platformPointsWithNoUnits.Remove(_position);
        CommonData.platformPoints.Remove(_position);
        destroyPlayerUnitStayingOnThisPlatform();
        _gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
