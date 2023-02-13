using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleFire : CastleTiles
{
    // Start is called before the first frame update
    void Start()
    {
        HP = CommonData.instance.HPOfTile;
        addToCommonData();
    }

    private void addToCommonData() {
        CommonData.instance.fireTiles.Add(this);
    }


    public override void reduceHP()
    {
        HP--;
        if (HP < 1) disactivateTile();
    }

    public override void disactivateTile()
    {
        GameController.instance.EndGame(false);
        CommonData.instance.fireTiles.Remove(this);
        _gameObject.SetActive(false);
    }
}
