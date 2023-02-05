
using UnityEngine;

public class UnitThree : PlayerUnit
{
    private void OnEnable()
    {
        _baseHarm = 6;
        _baseAccuracy = 0.15f;
        _baseAttackSpeed = 1f;
        setStartProperties();
    }

    public override void updatePropertiesToLevel()
    {
        base.updatePropertiesToLevel();
    }

    public override void superHit()
    {
        Debug.Log("SuperHitOfThree");
    }
}
