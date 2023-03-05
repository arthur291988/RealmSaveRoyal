using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBoss01 : MiniBoss
{

    private GameObject ObjectPulled;
    private List<GameObject> ObjectPulledList;

    public override void startSettings()
    {
        base.startSettings();
        resetSuperHitTimer();
    }

    private void superHit()
    {
        Vector2 shotPoint;
        for (int i=0;i< countOfContinuousSuperHits; i++)
        {
            shotPoint = new Vector2(Random.Range(-8, 8), 0);
            ObjectPulledList = EnemyPull.current.GetEnemyShotPullPullList();
            ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
            ObjectPulled.transform.position = _transform.position;
            EnemyShot enemyShot = ObjectPulled.GetComponent<EnemyShot>();
            enemyShot.startSettings(HP/2, 0.3f, shotPoint, ObjectPulled);
            ObjectPulled.SetActive(true);
        }

        resetSuperHitTimer();
    }

    public override void Update()
    {
        base.Update();
        if (superHitTimer > 0)
        {
            superHitTimer -= Time.deltaTime;
            if (superHitTimer <= 0) superHit();
        }
    }
}
