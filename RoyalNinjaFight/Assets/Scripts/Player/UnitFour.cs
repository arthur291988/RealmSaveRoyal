using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFour : PlayerUnit
{
    private void OnEnable()
    {
        _baseHarm = 17;
        _baseAccuracy = 0.22f;
        _baseAttackSpeed = 1.7f;

        setStartProperties();
    }

    public override void updatePropertiesToLevel()
    {
        base.updatePropertiesToLevel();
    }
}
