using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackWaves : MonoBehaviour
{
    [HideInInspector]
    public GameObject ObjectPulled;
    [HideInInspector]
    public List<GameObject> ObjectPulledList;
    private float attackTimeBase;
    private float attackWaveTimeBase;

    private const int maxEnemyCountOnOneLine = 7;
    private int enemyCountOnOneLine;
    private float VerticalLineGap;
    private float HorizontalLineGap;
    private float horizontalLineGapMultiplier;

    private Dictionary<int, List<List<int>>> _attackWaves;
    private int sideOfWave;
    private int firstLevelWavesOfSubLocation;
    private int secondLevelWavesOfSubLocation;
    private bool attackWaveIsOn;
    private bool bossAttackPassed;
    private bool finalBossAttackPassed;

    // Start is called before the first frame update
    void Start()
    {
        horizontalLineGapMultiplier = 1;
        HorizontalLineGap = 0;
        attackTimeBase = 30;
        attackWaveTimeBase = 20;
        //StartCoroutine (initiateAttackWave(CommonData.instance.locationWaves[new Vector2(CommonData.instance.location, CommonData.instance.subLocation)]));
        attackWaveIsOn = false;
        bossAttackPassed = false;
        finalBossAttackPassed = false;
        setAttackWaves(CommonData.instance.locationWaves[new Vector2(CommonData.instance.location, CommonData.instance.subLocation)]);

    }

    private Vector2 pointOfEnemy(int side)
    {
        if (enemyCountOnOneLine != 0) {
            if (enemyCountOnOneLine % 2!= 0) HorizontalLineGap += 3;
        }
        
        if (HorizontalLineGap != 0) horizontalLineGapMultiplier = horizontalLineGapMultiplier < 0 ? 1 : -1;


        if (enemyCountOnOneLine >=maxEnemyCountOnOneLine)
        {
            enemyCountOnOneLine=0;
            VerticalLineGap += 3f;
            HorizontalLineGap = 0;
        }

        //to set lines with gap when too many enemy waves
        enemyCountOnOneLine++;

        //if (side == 0) return new Vector2(Random.Range(-CommonData.instance.horisScreenSize / 2, CommonData.instance.horisScreenSize / 2), CommonData.instance.vertScreenSize / 2+ VerticalLineGap + Random.Range(2, 4f));
        //else return new Vector2(Random.Range(-CommonData.instance.horisScreenSize / 2, CommonData.instance.horisScreenSize / 2), -CommonData.instance.vertScreenSize / 2 - VerticalLineGap - Random.Range(2, 4.5f));


        if (side == 0) return new Vector2(HorizontalLineGap * horizontalLineGapMultiplier, CommonData.instance.vertScreenSize / 2 + VerticalLineGap + Random.Range(0, 1f));
        else return new Vector2(HorizontalLineGap * horizontalLineGapMultiplier, -CommonData.instance.vertScreenSize / 2 - VerticalLineGap - Random.Range(0, 1f));


        //if (side == 0) return new Vector2(Random.Range(-CommonData.instance.horisScreenSize / 2, CommonData.instance.horisScreenSize / 2), CommonData.instance.vertScreenSize / 2 + Random.Range(2, 3.5f));
        //else if (side == 1) return new Vector2(CommonData.instance.horisScreenSize / 2 + Random.Range(2, 3.5f), Random.Range(-CommonData.instance.vertScreenSize / 2, CommonData.instance.vertScreenSize / 2));
        //else if (side == 2) return new Vector2(Random.Range(-CommonData.instance.horisScreenSize / 2, CommonData.instance.horisScreenSize / 2), -CommonData.instance.vertScreenSize / 2 - Random.Range(2, 3.5f));
        //else return new Vector2(-CommonData.instance.horisScreenSize / 2 - Random.Range(2, 3.5f), Random.Range(-CommonData.instance.vertScreenSize / 2, CommonData.instance.vertScreenSize / 2));
    }


    //public void initiateAttackWave(Dictionary<int, List<int>> attackWavesForLevel1)
    //{
    //    //int sideOfWave = Random.Range(0, 4); 
    //    int sideOfWave = Random.Range(0, 2); //top or bottom
    //    enemyCountOnOneLine = 0;
    //    lineGap = 0;
    //    //first index of attackWaves list is side counts feature 
    //    for (int i = 0; i < attackWavesForLevel1[attackWaveCount][0]; i++)
    //    {
    //        //if (i != 0) {
    //        //    if (sideOfWave < 3) sideOfWave++;
    //        //    else sideOfWave = 0;
    //        //}
    //        if (i != 0)
    //        {
    //            sideOfWave = sideOfWave==0? 1 : 0; //change attack side
    //        }
    //        for (int j = 0; j < attackWavesForLevel1[attackWaveCount][1]; j++)
    //        {
    //            ObjectPulledList = ObjectPuller.current.GetEnemyUnitsPullList();
    //            ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
    //            ObjectPulled.transform.position = pointOfEnemy(sideOfWave);
    //            EnemyUnit enemyUnit = ObjectPulled.GetComponent<EnemyUnit>();
    //            enemyUnit.setEnemySide(sideOfWave);
    //            enemyUnit.setEnemyLevel(0, attackWaveCount);
    //            ObjectPulled.SetActive(true);

    //        }
    //        for (int x = 0; x < attackWavesForLevel1[attackWaveCount][2]; x++)
    //        {
    //            ObjectPulledList = ObjectPuller.current.GetEnemyUnitsPullList();
    //            ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
    //            ObjectPulled.transform.position = pointOfEnemy(sideOfWave);
    //            EnemyUnit enemyUnit = ObjectPulled.GetComponent<EnemyUnit>();
    //            enemyUnit.setEnemySide(sideOfWave);
    //            enemyUnit.setEnemyLevel(1, attackWaveCount);
    //            ObjectPulled.SetActive(true);

    //        }
    //    }

    //}

    //bossIndex 1-miniBoss, 2-bigBoss  
    private void instantiateBosses(int enemyLevel, int waveCount, int bossIndex) {
        int side = 0;
        ObjectPulledList = ObjectPuller.current.GetEnemyUnitsPullList(bossIndex);
        ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
        ObjectPulled.transform.position = pointOfEnemy(side);
        EnemyUnit enemyUnit = ObjectPulled.GetComponent<EnemyUnit>();
        enemyUnit.setEnemySide(side);
        enemyUnit.setEnemyLevel(enemyLevel, waveCount);
        enemyUnit.addToCommonDataOnEnableaAndSetParameters();
        ObjectPulled.SetActive(true);

        side++;

        ObjectPulledList = ObjectPuller.current.GetEnemyUnitsPullList(bossIndex);
        ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
        ObjectPulled.transform.position = pointOfEnemy(side);
        enemyUnit = ObjectPulled.GetComponent<EnemyUnit>();
        enemyUnit.setEnemySide(side);
        enemyUnit.setEnemyLevel(enemyLevel, waveCount);
        enemyUnit.addToCommonDataOnEnableaAndSetParameters();
        ObjectPulled.SetActive(true);
    }

    public IEnumerator initiateAttackWave(Dictionary<int, List<List<int>>> attackWaves)
    {
        int sideOfWave = Random.Range(0, 2); //top or bottom

        //big waves of subLocation each finishes with mini boss 
        for (int i = 0; i< attackWaves.Count; i++)
        {
            //mini waves of subLocation big waves (no mini boss btw these ones)
            for (int j = 0; j < attackWaves[i].Count; j++) {

                //each side attack troops [0] is the holder of side counts (there are only two sides max)
                for (int y = 0; y < attackWaves[i][j][0]; y++) {
                    VerticalLineGap = 0;
                    HorizontalLineGap = 0;
                    sideOfWave = sideOfWave == 0 ? 1 : 0; //change attack side

                    //1 - index that holds the count of first level enemies 
                    for (int x = 0; x < attackWaves[i][j][1]; x++)
                    {
                        ObjectPulledList = ObjectPuller.current.GetEnemyUnitsPullList(0); //0-is index that pulls the regular enemy prefab from pull, not boss
                        ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
                        ObjectPulled.transform.position = pointOfEnemy(sideOfWave);
                        EnemyUnit enemyUnit = ObjectPulled.GetComponent<EnemyUnit>();
                        enemyUnit.setEnemySide(sideOfWave);
                        enemyUnit.setEnemyLevel(0, i);
                        ObjectPulled.SetActive(true);
                    }
                    //2 - index that holds the count of second level enemies
                    for (int x = 0; x < attackWaves[i][j][2]; x++)
                    {
                        ObjectPulledList = ObjectPuller.current.GetEnemyUnitsPullList(0);
                        ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
                        ObjectPulled.transform.position = pointOfEnemy(sideOfWave);
                        EnemyUnit enemyUnit = ObjectPulled.GetComponent<EnemyUnit>();
                        enemyUnit.setEnemySide(sideOfWave);
                        enemyUnit.setEnemyLevel(1, i);
                        ObjectPulled.SetActive(true);

                    }
                    //3 - index that holds the count of third level enemies
                    for (int x = 0; x < attackWaves[i][j][3]; x++)
                    {
                        ObjectPulledList = ObjectPuller.current.GetEnemyUnitsPullList(0);
                        ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
                        ObjectPulled.transform.position = pointOfEnemy(sideOfWave);
                        EnemyUnit enemyUnit = ObjectPulled.GetComponent<EnemyUnit>();
                        enemyUnit.setEnemySide(sideOfWave);
                        enemyUnit.setEnemyLevel(2, i);
                        ObjectPulled.SetActive(true);

                    }
                    //4 - index that holds the count of forth level enemies
                    for (int x = 0; x < attackWaves[i][j][4]; x++)
                    {
                        ObjectPulledList = ObjectPuller.current.GetEnemyUnitsPullList(0);
                        ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
                        ObjectPulled.transform.position = pointOfEnemy(sideOfWave);
                        EnemyUnit enemyUnit = ObjectPulled.GetComponent<EnemyUnit>();
                        enemyUnit.setEnemySide(sideOfWave);
                        enemyUnit.setEnemyLevel(3, i);
                        ObjectPulled.SetActive(true);

                    }
                }

                //gap btwn mini wave attacks
                yield return new WaitForSeconds(attackTimeBase);
            }
            VerticalLineGap = 0;
            HorizontalLineGap = 0;
            instantiateBosses(10, i,1);

            yield return new WaitForSeconds(attackWaveTimeBase);

            if (i == attackWaves.Count - 1)
            {
                VerticalLineGap = 0; 
                HorizontalLineGap = 0;
                instantiateBosses(100, i, 2);
                
            }
        }

    }


    private void setAttackWaves(Dictionary<int, List<List<int>>> attackWaves)
    {
        _attackWaves = attackWaves;
        firstLevelWavesOfSubLocation = 0;
        secondLevelWavesOfSubLocation = 0;// attackWaves[firstLevelWavesOfSubLocation].Count;
        sideOfWave = Random.Range(0, 2); //top or bottom
        initiateSecondLevelAttackWaves();
    }

    //private void initiateFirstLevelAttackWaves() {
    //    initiateSecondLevelAttackWaves();
    //}

    public void initiateSecondLevelAttackWaves()
    {
        //each side attack troops [0] is the holder of side counts (there are only two sides max)
        for (int y = 0; y < _attackWaves[firstLevelWavesOfSubLocation][secondLevelWavesOfSubLocation][0]; y++)
        {
            VerticalLineGap = 0;
            HorizontalLineGap = 0;
            sideOfWave = sideOfWave == 0 ? 1 : 0; //change attack side

            //1 - index that holds the count of first level enemies 
            for (int x = 0; x < _attackWaves[firstLevelWavesOfSubLocation][secondLevelWavesOfSubLocation][1]; x++)
            {
                ObjectPulledList = ObjectPuller.current.GetEnemyUnitsPullList(0); //0-is index that pulls the regular enemy prefab from pull, not boss
                ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
                ObjectPulled.transform.position = pointOfEnemy(sideOfWave);
                EnemyUnit enemyUnit = ObjectPulled.GetComponent<EnemyUnit>();
                enemyUnit.setEnemySide(sideOfWave);
                enemyUnit.setEnemyLevel(0, firstLevelWavesOfSubLocation);
                enemyUnit.addToCommonDataOnEnableaAndSetParameters();
                ObjectPulled.SetActive(true);
            }
            
            //2 - index that holds the count of second level enemies
            for (int x = 0; x < _attackWaves[firstLevelWavesOfSubLocation][secondLevelWavesOfSubLocation][2]; x++)
            {
                ObjectPulledList = ObjectPuller.current.GetEnemyUnitsPullList(0);
                ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
                ObjectPulled.transform.position = pointOfEnemy(sideOfWave);
                EnemyUnit enemyUnit = ObjectPulled.GetComponent<EnemyUnit>();
                enemyUnit.setEnemySide(sideOfWave);
                enemyUnit.setEnemyLevel(1, firstLevelWavesOfSubLocation);
                enemyUnit.addToCommonDataOnEnableaAndSetParameters();
                ObjectPulled.SetActive(true);
            }

            //3 - index that holds the count of third level enemies
            for (int x = 0; x < _attackWaves[firstLevelWavesOfSubLocation][secondLevelWavesOfSubLocation][3]; x++)
            {
                ObjectPulledList = ObjectPuller.current.GetEnemyUnitsPullList(0);
                ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
                ObjectPulled.transform.position = pointOfEnemy(sideOfWave);
                EnemyUnit enemyUnit = ObjectPulled.GetComponent<EnemyUnit>();
                enemyUnit.setEnemySide(sideOfWave);
                enemyUnit.setEnemyLevel(2, firstLevelWavesOfSubLocation);
                enemyUnit.addToCommonDataOnEnableaAndSetParameters();
                ObjectPulled.SetActive(true);

            }
            //4 - index that holds the count of forth level enemies
            for (int x = 0; x < _attackWaves[firstLevelWavesOfSubLocation][secondLevelWavesOfSubLocation][4]; x++)
            {
                ObjectPulledList = ObjectPuller.current.GetEnemyUnitsPullList(0);
                ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
                ObjectPulled.transform.position = pointOfEnemy(sideOfWave);
                EnemyUnit enemyUnit = ObjectPulled.GetComponent<EnemyUnit>();
                enemyUnit.setEnemySide(sideOfWave);
                enemyUnit.setEnemyLevel(3, firstLevelWavesOfSubLocation);
                enemyUnit.addToCommonDataOnEnableaAndSetParameters();
                ObjectPulled.SetActive(true);

            }
        }
        if (bossAttackPassed) bossAttackPassed = false;
        attackWaveIsOn = true;
        secondLevelWavesOfSubLocation++;
    }

    private void Update()
    {
        if (attackWaveIsOn) {
            if (CommonData.instance.enemyUnitsAll.Count == 0) {
                attackWaveIsOn = false;
                if (secondLevelWavesOfSubLocation < _attackWaves[firstLevelWavesOfSubLocation].Count)
                {
                    initiateSecondLevelAttackWaves();
                }
                else if (!bossAttackPassed)
                {
                    VerticalLineGap = 0;
                    HorizontalLineGap = 0;
                    instantiateBosses(10, firstLevelWavesOfSubLocation, 1); //100 - location 0 mini boss, 101-location 1 mini boss. It works as following 10+0 where 0 is location count
                    if (firstLevelWavesOfSubLocation < _attackWaves.Count-1) firstLevelWavesOfSubLocation++;
                    attackWaveIsOn = true;
                    bossAttackPassed = true;
                }
                //else if (firstLevelWavesOfSubLocation < _attackWaves.Count)
                //{
                //    initiateFirstLevelAttackWaves();
                //}
                else if (!finalBossAttackPassed)
                {
                    VerticalLineGap = 0;
                    HorizontalLineGap = 0;
                    instantiateBosses(100, firstLevelWavesOfSubLocation, 2); //1000 - location 0 mini boss, 1001-location 1 mini boss. It works as following 10+0 where 0 is location count
                    attackWaveIsOn = true;
                    finalBossAttackPassed = true;
                }
                else {
                    Debug.Log("Victory");
                }
            }
        }
    }


    // Update is called once per frame
    //void Update()
    //{
    //    //if (attackWaveTime > 0)
    //    //{
    //    //    attackWaveTime -= Time.deltaTime;
    //    //    if (attackWaveTime <= 0 && attackWaveCount < CommonData.instance.attackWavesForLevel1.Count)
    //    //    {
    //    //        initiateAttackWave(CommonData.instance.attackWavesForLevel1);
    //    //        attackWaveCount++;
    //    //        attackWaveTime = 20;
    //    //    }

    //    //}
    //}
}
