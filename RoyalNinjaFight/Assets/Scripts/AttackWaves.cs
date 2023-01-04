using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackWaves : MonoBehaviour
{
    [HideInInspector]
    public GameObject ObjectPulled;
    [HideInInspector]
    public List<GameObject> ObjectPulledList;
    private float attackWaveTime;
    private int attackWaveCount;


    private const int maxEnemyCountOnOneLine = 12;
    private int enemyCountOnOneLine;
    private float lineGap;

    // Start is called before the first frame update
    void Start()
    {
        attackWaveCount = 0;
        attackWaveTime = 20;
        initiateAttackWave(CommonData.instance.attackWavesForLevel1);
    }

    private Vector2 pointOfEnemy(int side)
    {
        //to set lines with gap when too many enemy waves 
        enemyCountOnOneLine++;
        if (enemyCountOnOneLine >= maxEnemyCountOnOneLine)
        {
            enemyCountOnOneLine=0;
            lineGap +=1.5f;
        }

        if (side == 0) return new Vector2(Random.Range(-CommonData.instance.horisScreenSize / 2, CommonData.instance.horisScreenSize / 2), CommonData.instance.vertScreenSize / 2+ lineGap + Random.Range(2, 4f));
        else return new Vector2(Random.Range(-CommonData.instance.horisScreenSize / 2, CommonData.instance.horisScreenSize / 2), -CommonData.instance.vertScreenSize / 2 - lineGap - Random.Range(2, 4.5f));

        //if (side == 0) return new Vector2(Random.Range(-CommonData.instance.horisScreenSize / 2, CommonData.instance.horisScreenSize / 2), CommonData.instance.vertScreenSize / 2 + Random.Range(2, 3.5f));
        //else if (side == 1) return new Vector2(CommonData.instance.horisScreenSize / 2 + Random.Range(2, 3.5f), Random.Range(-CommonData.instance.vertScreenSize / 2, CommonData.instance.vertScreenSize / 2));
        //else if (side == 2) return new Vector2(Random.Range(-CommonData.instance.horisScreenSize / 2, CommonData.instance.horisScreenSize / 2), -CommonData.instance.vertScreenSize / 2 - Random.Range(2, 3.5f));
        //else return new Vector2(-CommonData.instance.horisScreenSize / 2 - Random.Range(2, 3.5f), Random.Range(-CommonData.instance.vertScreenSize / 2, CommonData.instance.vertScreenSize / 2));
    }


    public void initiateAttackWave(Dictionary<int, List<int>> attackWavesForLevel1)
    {
        //int sideOfWave = Random.Range(0, 4); 
        int sideOfWave = Random.Range(0, 2); //top or bottom
        enemyCountOnOneLine = 0;
        lineGap = 0;
        //first index of attackWaves list is side counts feature 
        for (int i = 0; i < attackWavesForLevel1[attackWaveCount][0]; i++)
        {
            //if (i != 0) {
            //    if (sideOfWave < 3) sideOfWave++;
            //    else sideOfWave = 0;
            //}
            if (i != 0)
            {
                sideOfWave = sideOfWave==0? 1 : 0; //change attack side
            }
            for (int j = 0; j < attackWavesForLevel1[attackWaveCount][1]; j++)
            {
                ObjectPulledList = ObjectPuller.current.GetEnemyUnitsPullList();
                ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
                ObjectPulled.transform.position = pointOfEnemy(sideOfWave);
                EnemyUnit enemyUnit = ObjectPulled.GetComponent<EnemyUnit>();
                CommonData.instance.enemyUnits[sideOfWave].Add(enemyUnit);
                enemyUnit.setEnemySide(sideOfWave);
                enemyUnit.setEnemyLevel(0, attackWaveCount);
                ObjectPulled.SetActive(true);

            }
            for (int x = 0; x < attackWavesForLevel1[attackWaveCount][2]; x++)
            {
                ObjectPulledList = ObjectPuller.current.GetEnemyUnitsPullList();
                ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
                ObjectPulled.transform.position = pointOfEnemy(sideOfWave);
                EnemyUnit enemyUnit = ObjectPulled.GetComponent<EnemyUnit>();
                CommonData.instance.enemyUnits[sideOfWave].Add(enemyUnit);
                enemyUnit.setEnemySide(sideOfWave);
                enemyUnit.setEnemyLevel(1, attackWaveCount);
                ObjectPulled.SetActive(true);

            }
        }
        
    }

    

    // Update is called once per frame
    void Update()
    {
        if (attackWaveTime > 0)
        {
            attackWaveTime -= Time.deltaTime;
            if (attackWaveTime <= 0 && attackWaveCount < CommonData.instance.attackWavesForLevel1.Count)
            {
                initiateAttackWave(CommonData.instance.attackWavesForLevel1);
                attackWaveCount++;
                attackWaveTime = 20;
            }

        }
    }
}
