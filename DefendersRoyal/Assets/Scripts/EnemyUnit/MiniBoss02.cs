using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBoss02 : MiniBoss

{

    private GameObject ObjectPulled;
    private List<GameObject> ObjectPulledList;

    public override void startSettings()
    {
        base.startSettings();
        miniBossSuperHitTime = 8f;
        playerUnitUnderSuperHit = 2;
        resetSuperHitTimer();
    }

    private void superHit()
    {
        Vector2 shotPoint;
        for (int i = 0; i < 2; i++)
        {
            shotPoint = new Vector2(Random.Range(-8, 8), 0);
            ObjectPulledList = EnemyPull.current.GetEnemyShotPullPullList();
            ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
            ObjectPulled.transform.position = _transform.position;
            EnemyShot enemyShot = ObjectPulled.GetComponent<EnemyShot>();
            enemyShot.startSettings(HP / 2, 0.3f, shotPoint, ObjectPulled);
            ObjectPulled.SetActive(true);
        }

        resetSuperHitTimer();
    }

    public override void Update()
    {
        base.Update();
        if (miniBossSuperHitTimer > 0)
        {
            miniBossSuperHitTimer -= Time.deltaTime;
            if (miniBossSuperHitTimer <= 0) superHit();
        }
    }
}
