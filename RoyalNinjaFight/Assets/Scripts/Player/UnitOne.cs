
using UnityEngine;

public class UnitOne : PlayerUnit
{
    
    private void OnEnable()
    {
        _baseHarm = 11;
        _baseAccuracy = 0.2f;
        _baseAttackSpeed = 1.5f;

        setStartProperties();
    }

    public override void updatePropertiesToLevel()
    {
        base.updatePropertiesToLevel();
    }
}
