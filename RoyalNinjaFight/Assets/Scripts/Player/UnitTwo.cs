﻿
using UnityEngine;

public class UnitTwo : PlayerUnit
{

    private void OnEnable()
    {
        _baseHarm = 30;
        _baseAccuracy = 0.3f;
        _baseAttackSpeed = 2f;

        setStartProperties();

    }

    public override void updatePropertiesToLevel()
    {
        base.updatePropertiesToLevel();
    }

}
