using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss01 : FinalBoss
{
    private GameObject ObjectPulled;
    private List<GameObject> ObjectPulledList;
    [SerializeField]
    private int HPOfWall;

    public override void startSettings()
    {
        base.startSettings();
        resetStartSuperHitTimer();
    }

    private void superHit()
    {
        int sideMultiplier = _enemySide == 0 ? 1 : -1;

        ObjectPulledList = EnemyPull.current.GetEnemyWallPullPullList();
        ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
        ObjectPulled.transform.position = new Vector2(0, 15 * sideMultiplier);
        EnemyWall enemyWall = ObjectPulled.GetComponent<EnemyWall>();
        enemyWall.startSettings(HPOfWall, ObjectPulled);
        ObjectPulled.SetActive(true);

        ObjectPulledList = EnemyPull.current.GetEnemyWallPullPullList();
        ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
        ObjectPulled.transform.position = new Vector2(-8, 15 * sideMultiplier);
        enemyWall = ObjectPulled.GetComponent<EnemyWall>();
        enemyWall.startSettings(HPOfWall, ObjectPulled);
        ObjectPulled.SetActive(true);

        ObjectPulledList = EnemyPull.current.GetEnemyWallPullPullList();
        ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
        ObjectPulled.transform.position = new Vector2(8, 15 * sideMultiplier);
        enemyWall = ObjectPulled.GetComponent<EnemyWall>();
        enemyWall.startSettings(HPOfWall, ObjectPulled);
        ObjectPulled.SetActive(true);

        ObjectPulledList = EnemyPull.current.GetEnemyWallPullPullList();
        ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
        ObjectPulled.transform.position = new Vector2(0, 18 * sideMultiplier);
        enemyWall = ObjectPulled.GetComponent<EnemyWall>();
        enemyWall.startSettings(HPOfWall, ObjectPulled);
        ObjectPulled.SetActive(true);

        ObjectPulledList = EnemyPull.current.GetEnemyWallPullPullList();
        ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
        ObjectPulled.transform.position = new Vector2(-8, 18 * sideMultiplier);
        enemyWall = ObjectPulled.GetComponent<EnemyWall>();
        enemyWall.startSettings(HPOfWall, ObjectPulled);
        ObjectPulled.SetActive(true);

        ObjectPulledList = EnemyPull.current.GetEnemyWallPullPullList();
        ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
        ObjectPulled.transform.position = new Vector2(8, 18 * sideMultiplier);
        enemyWall = ObjectPulled.GetComponent<EnemyWall>();
        enemyWall.startSettings(HPOfWall, ObjectPulled);
        ObjectPulled.SetActive(true);


    }

    public override void Update()
    {
        base.Update();
        if (startSuperHitTimer > 0)
        {
            startSuperHitTimer -= Time.deltaTime;
            if (startSuperHitTimer <= 0) superHit();
        }
    }
}
