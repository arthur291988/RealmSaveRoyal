using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleFire : CastleTiles
{
    // Start is called before the first frame update
    void Start()
    {
        HP = CommonData.instance.HPOfTile;
    }

    public override void disactivateTile()
    {
        GameController.instance.gameIsOn = false;
        _gameObject.SetActive(false);
    }
}
