using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFive : PlayerUnit
{
    private void OnEnable()
    {
        _baseHarm = 23;
        _baseAccuracy = 0.25f;
        _baseAttackSpeed = 1.9f;

        setStartProperties();
    }

    public override void updatePropertiesToLevel()
    {
        base.updatePropertiesToLevel();
    }
}
