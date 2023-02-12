using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MiniBoss00 : MiniBoss
{
    public override void startSettings()
    {
        base.startSettings();
        miniBossSuperHitTime = 5f;
        playerUnitUnderSuperHit = 2;
        resetSuperHitTimer();
    }

    //private void superHit()
    //{
    //    int index = 0;
    //    List<PlayerUnit> enemySideUnits = new List<PlayerUnit>();
    //    for (int i = 0; i < CommonData.instance.playerUnits.Count; i++)
    //    {
    //        if (CommonData.instance.playerUnits[i].unitSide == _enemySide) enemySideUnits.Add(CommonData.instance.playerUnits[i]);
    //    }

    //    if (enemySideUnits.Count == 1)
    //    {
    //        if (!enemySideUnits[0].isMoved) enemySideUnits[0].blockTheUnit(4);
    //    }
    //    else if (enemySideUnits.Count == 2)
    //    {
    //        if (!enemySideUnits[0].isMoved) enemySideUnits[0].blockTheUnit(4);
    //        if (!enemySideUnits[1].isMoved) enemySideUnits[1].blockTheUnit(4);
    //    }
    //    else if (enemySideUnits.Count > 2)
    //    {
    //        index = Random.Range(0, enemySideUnits.Count);
    //        if (!enemySideUnits[index].isMoved) enemySideUnits[index].blockTheUnit(4);
    //        index = index < enemySideUnits.Count - 1 ? index + 1 : index - 1;
    //        if (!enemySideUnits[index].isMoved) enemySideUnits[index].blockTheUnit(4);

    //    }
    //    resetSuperHitTimer();
    //}

    //public override void Update()
    //{
    //    base.Update();
    //    if (miniBossSuperHitTimer > 0)
    //    {
    //        miniBossSuperHitTimer -= Time.deltaTime;
    //        if (miniBossSuperHitTimer <= 0) superHit();
    //    }
    //}
}
