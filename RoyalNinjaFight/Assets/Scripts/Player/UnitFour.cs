using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFour : PlayerUnit
{
    private void OnEnable()
    {
        _baseHarm = 11;
        _baseAccuracy = 0.22f;
        _baseAttackSpeed = 1.2f;

        setStartProperties();
    }

    public override void updatePropertiesToLevel()
    {
        base.updatePropertiesToLevel();
    }

    public override void superHit()
    {
        Debug.Log("SuperHitOfFour");
    }
}
