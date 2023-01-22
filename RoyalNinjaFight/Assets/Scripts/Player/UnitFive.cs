using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFive : PlayerUnit
{
    private void OnEnable()
    {
        _baseHarm = 10;
        _baseAccuracy = 0.2f;
        _baseAttackSpeed = 1.1f;

        setStartProperties();
    }

    public override void updatePropertiesToLevel()
    {
        base.updatePropertiesToLevel();
    }
    public override void superHit()
    {
        Debug.Log("SuperHitOfFive");
    }
}
