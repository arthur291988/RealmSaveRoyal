
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBoss21 : MiniBoss
{
    private GameObject ObjectPulled;
    private List<GameObject> ObjectPulledList;
    [SerializeField]
    private int HPOfWall;
    [SerializeField]
    private int CountOfWalls;
    
    

    public override void startSettings()
    {
        base.startSettings();
        resetStartSuperHitTimer();
        resetSuperHitTimer();
    }

    private void superHitWall()
    {
        int sideMultiplier = _enemySide == 0 ? 1 : -1;
        if (CountOfWalls == 1)
        {
            ObjectPulledList = EnemyPull.current.GetEnemyWallPullPullList();
            ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
            ObjectPulled.transform.position = new Vector2(0, 15 * sideMultiplier);
            EnemyWall enemyWall = ObjectPulled.GetComponent<EnemyWall>();
            enemyWall.startSettings(HPOfWall, ObjectPulled);
            ObjectPulled.SetActive(true);
        }
        else if (CountOfWalls == 3)
        {
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

        }
        else
        {
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

    }
    private void superHitShot()
    {
        Vector2 shotPoint;
        for (int i = 0; i < countOfContinuousSuperHits; i++)
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
        if (superHitTimer > 0)
        {
            superHitTimer -= Time.deltaTime;
            if (superHitTimer <= 0) superHitShot();
        }
        if (startSuperHitTimer > 0)
        {
            startSuperHitTimer -= Time.deltaTime;
            if (startSuperHitTimer <= 0) superHitWall();
        }
    }
}
