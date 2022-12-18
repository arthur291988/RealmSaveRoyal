﻿using System.Collections;
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
        CommonData.instance.platformPoints.Add(_position);
        CommonData.instance.platformPointsWithNoUnits.Add(_position);
    }

    private void OnEnable()
    {
        _position = transform.position;
        HP = CommonData.instance.HPOfTile;
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
            if (CommonData.instance.playerUnits[i]._unitStartPosition == _position)
            {
                CommonData.instance.playerUnits[i].disactivateUnit();
                return;
            }
        }
    }

    private void disactivateTile()
    {
        CommonData.instance.platformPointsWithNoUnits.Remove(_position);
        CommonData.instance.platformPoints.Remove(_position);
        destroyPlayerUnitStayingOnThisPlatform();
        _gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
