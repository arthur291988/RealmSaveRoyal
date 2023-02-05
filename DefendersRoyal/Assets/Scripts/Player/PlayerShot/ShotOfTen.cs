using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotOfTen : PlayerShot
{

    // Update is called once per frame
    void Update()
    {
        if (_shotTransform.position.y > CommonData.instance.vertScreenSize / 2 - 15 || _shotTransform.position.y < -CommonData.instance.vertScreenSize / 2 + 15)
        {
            _gameObject.SetActive(false);
        }
    }
}
