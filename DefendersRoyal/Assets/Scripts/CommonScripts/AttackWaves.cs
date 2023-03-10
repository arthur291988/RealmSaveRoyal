using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttackWaves : MonoBehaviour
{
    [HideInInspector]
    public GameObject ObjectPulled;
    [HideInInspector]
    public List<GameObject> ObjectPulledList;
    private float attackWavePauseTime;
    private float attackWavePauseTimer;

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

    [SerializeField]
    private GameObject attackPauseAlertButtonUp;
    [SerializeField]
    private GameObject attackPauseAlertButtonDown;
    [SerializeField]
    private Image pauseAlerImageUp;
    [SerializeField]
    private Image pauseAlerImageDown;
    [SerializeField]
    private TextMeshProUGUI pauseAlertMessageUp;
    [SerializeField]
    private TextMeshProUGUI pauseAlertMessageDown;
    private bool wavePause;

    [SerializeField]
    private TextMeshProUGUI wavesCountTextText;
    private int totalWavesLeft;

    // Start is called before the first frame update
    void Start()
    {
        horizontalLineGapMultiplier = 1;
        HorizontalLineGap = 0;

        attackWavePauseTime = CommonData.instance.wavePauseTimeBase;
        resetPauseTimer();
        pauseAlertMessageDown.text = CommonData.instance.getTheyAreComingText();
        pauseAlertMessageUp.text = CommonData.instance.getTheyAreComingText();

        attackWaveIsOn = false;
        bossAttackPassed = false;
        finalBossAttackPassed = false;
        setAttackWaves(CommonData.instance.locationWaves[new Vector2(CommonData.instance.location, CommonData.instance.subLocation)]);
        setTotalWavesCountOnStart();
        updateWavesText();
    }

    private void setTotalWavesCountOnStart() {
        for (int i = 0; i < _attackWaves.Count; i++) {
            for (int y = 0; y < _attackWaves[i].Count; y++) {
                totalWavesLeft++;
            }
            totalWavesLeft++;
        }
    }

    private void updateWavesText()
    {
        wavesCountTextText.text = totalWavesLeft.ToString();
    }

    private Vector2 pointOfEnemy(int side)
    {
        if (enemyCountOnOneLine != 0) {
            if (enemyCountOnOneLine % 2 != 0) HorizontalLineGap += 3;
        }

        if (HorizontalLineGap != 0) horizontalLineGapMultiplier = horizontalLineGapMultiplier < 0 ? 1 : -1;


        if (enemyCountOnOneLine >= maxEnemyCountOnOneLine)
        {
            enemyCountOnOneLine = 0;
            VerticalLineGap += 3f;
            HorizontalLineGap = 0;
        }

        //to set lines with gap when too many enemy waves
        enemyCountOnOneLine++;

        if (side == 0) return new Vector2(HorizontalLineGap * horizontalLineGapMultiplier, CommonData.instance.vertScreenSize / 2 + VerticalLineGap + Random.Range(0, 1f));
        else return new Vector2(HorizontalLineGap * horizontalLineGapMultiplier, -CommonData.instance.vertScreenSize / 2 - VerticalLineGap - Random.Range(0, 1f));

    }

    private Vector2 pointOfBoss(int side)
    {
        if (side == 0) return new Vector2(0, CommonData.instance.vertScreenSize / 2 + VerticalLineGap + 6);
        else return new Vector2(0, -CommonData.instance.vertScreenSize / 2 - VerticalLineGap - 6);
    }

    //bossIndex 1-miniBoss, 2-bigBoss  
    private void instantiateBosses(int enemyLevel, int waveCount, int bossIndex) {
        int side = 0;

        if (bossIndex == 1) ObjectPulled = EnemyPull.current.LocationMiniBossesPull[waveCount]; //1 is mini boss index 
        //it is necessary to instantiate final boss, because it is not instantiated on enemyPull
        else ObjectPulled = Instantiate(EnemyPull.current.allFinalBosses[new Vector2(CommonData.instance.location, CommonData.instance.subLocation)]); //2 is final boss index 

        //ObjectPulledList = ObjectPuller.current.GetEnemyUnitsPullList(bossIndex);
        //ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
        ObjectPulled.transform.position = pointOfBoss(side);
        EnemyUnit enemyUnit = ObjectPulled.GetComponent<EnemyUnit>();
        commonEnemyUnitSettings(enemyUnit, enemyLevel, waveCount, side);
        ObjectPulled.SetActive(true);

        side++;

        //for now i make it through instabtiation cause there only one instance in pull because the index of list serves as key to exact wave count boss
        if (bossIndex == 1) ObjectPulled = Instantiate(EnemyPull.current.LocationMiniBossesPull[waveCount]); //1 is mini boss index 
        else ObjectPulled = Instantiate(EnemyPull.current.allFinalBosses[new Vector2(CommonData.instance.location, CommonData.instance.subLocation)]); //2 is final boss index 
        //ObjectPulledList = ObjectPuller.current.GetEnemyUnitsPullList(bossIndex);
        //ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
        ObjectPulled.transform.position = pointOfBoss(side);
        enemyUnit = ObjectPulled.GetComponent<EnemyUnit>();
        commonEnemyUnitSettings(enemyUnit, enemyLevel, waveCount, side);
        ObjectPulled.SetActive(true);
    }

    private void commonEnemyUnitSettings(EnemyUnit _enemyUnit, int enemyLevel, int waveCount, int side) {
        _enemyUnit.setEnemySide(side);
        _enemyUnit.setEnemyLevel(enemyLevel, waveCount);
        _enemyUnit.addToCommonDataOnEnableaAndSetParameters();
        _enemyUnit.startSettings();
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
        //each side attack troops [0] index is the holder of side counts (there are only two sides max) so it is 1 or 2
        for (int y = 0; y < _attackWaves[firstLevelWavesOfSubLocation][secondLevelWavesOfSubLocation][0]; y++)
        {
            VerticalLineGap = 0;
            HorizontalLineGap = 0;
            sideOfWave = sideOfWave == 0 ? 1 : 0; //change attack side

            //1 - index that holds the count of first level enemies 
            for (int x = 0; x < _attackWaves[firstLevelWavesOfSubLocation][secondLevelWavesOfSubLocation][1]; x++)
            {
                ObjectPulledList = ObjectPuller.current.GetEnemyUnitsPullList();
                ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
                ObjectPulled.transform.position = pointOfEnemy(sideOfWave);
                EnemyUnit enemyUnit = ObjectPulled.GetComponent<EnemyUnit>();
                commonEnemyUnitSettings(enemyUnit, 0, firstLevelWavesOfSubLocation, sideOfWave);
                ObjectPulled.SetActive(true);
            }

            //2 - index that holds the count of second level enemies
            for (int x = 0; x < _attackWaves[firstLevelWavesOfSubLocation][secondLevelWavesOfSubLocation][2]; x++)
            {
                ObjectPulledList = ObjectPuller.current.GetEnemyUnitsPullList();
                ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
                ObjectPulled.transform.position = pointOfEnemy(sideOfWave);
                EnemyUnit enemyUnit = ObjectPulled.GetComponent<EnemyUnit>();

                commonEnemyUnitSettings(enemyUnit, 1, firstLevelWavesOfSubLocation, sideOfWave);
                ObjectPulled.SetActive(true);
            }

            //3 - index that holds the count of third level enemies
            for (int x = 0; x < _attackWaves[firstLevelWavesOfSubLocation][secondLevelWavesOfSubLocation][3]; x++)
            {
                ObjectPulledList = ObjectPuller.current.GetEnemyUnitsPullList();
                ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
                ObjectPulled.transform.position = pointOfEnemy(sideOfWave);
                EnemyUnit enemyUnit = ObjectPulled.GetComponent<EnemyUnit>();

                commonEnemyUnitSettings(enemyUnit, 2, firstLevelWavesOfSubLocation, sideOfWave);
                ObjectPulled.SetActive(true);

            }

            //4 - index that holds the count of forth level enemies
            for (int x = 0; x < _attackWaves[firstLevelWavesOfSubLocation][secondLevelWavesOfSubLocation][4]; x++)
            {
                ObjectPulledList = ObjectPuller.current.GetEnemyUnitsPullList();
                ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
                ObjectPulled.transform.position = pointOfEnemy(sideOfWave);
                EnemyUnit enemyUnit = ObjectPulled.GetComponent<EnemyUnit>();

                commonEnemyUnitSettings(enemyUnit, 3, firstLevelWavesOfSubLocation, sideOfWave);
                ObjectPulled.SetActive(true);

            } 
            
        }
        if (bossAttackPassed) bossAttackPassed = false;
        attackWaveIsOn = true;
        secondLevelWavesOfSubLocation++;
    }

    private void instantiateSomeSoldersAroundBoss(int firstLevelWave)
    {
        int level1EnemyCount=0;
        int level2EnemyCount=0;
        int level3EnemyCount = 0;
        int level4EnemyCount = 0;

        //calculating the summ of all enemyes on first level wave, by their level
        for (int i = 0; i < _attackWaves[firstLevelWavesOfSubLocation].Count; i++) {
            level1EnemyCount += _attackWaves[firstLevelWavesOfSubLocation][i][1];
            level2EnemyCount += _attackWaves[firstLevelWavesOfSubLocation][i][2];
            level3EnemyCount += _attackWaves[firstLevelWavesOfSubLocation][i][3];
            level4EnemyCount += _attackWaves[firstLevelWavesOfSubLocation][i][4];
        }

        //setting the avarage count of enemies by their level
        level1EnemyCount /= _attackWaves[firstLevelWavesOfSubLocation].Count;
        level2EnemyCount /= _attackWaves[firstLevelWavesOfSubLocation].Count;
        level3EnemyCount /= _attackWaves[firstLevelWavesOfSubLocation].Count;
        level4EnemyCount /= _attackWaves[firstLevelWavesOfSubLocation].Count;

        for (int y = 0; y < 2; y++)
        {
            VerticalLineGap = 0;
            HorizontalLineGap = 0;
            sideOfWave = sideOfWave == 0 ? 1 : 0; //change attack side

            //first level enemies 
            //for (int x = 0; x < level1EnemyCount; x++)
            //{
            //    ObjectPulledList = ObjectPuller.current.GetEnemyUnitsPullList(); 
            //    ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
            //    ObjectPulled.transform.position = pointOfEnemy(sideOfWave);
            //    EnemyUnit enemyUnit = ObjectPulled.GetComponent<EnemyUnit>();

            //    commonEnemyUnitSettings(enemyUnit, 0, firstLevelWavesOfSubLocation, sideOfWave);
            //    //enemyUnit.setEnemySide(sideOfWave);
            //    //enemyUnit.setEnemyLevel(0, firstLevelWavesOfSubLocation);
            //    //enemyUnit.addToCommonDataOnEnableaAndSetParameters();
            //    ObjectPulled.SetActive(true);
            //}

            //second level enemies
            for (int x = 0; x < level2EnemyCount; x++)
            {
                ObjectPulledList = ObjectPuller.current.GetEnemyUnitsPullList();
                ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
                ObjectPulled.transform.position = pointOfEnemy(sideOfWave);
                EnemyUnit enemyUnit = ObjectPulled.GetComponent<EnemyUnit>();

                commonEnemyUnitSettings(enemyUnit, 1, firstLevelWavesOfSubLocation, sideOfWave);
                ObjectPulled.SetActive(true);
            }

            //third level enemies
            for (int x = 0; x < level3EnemyCount; x++)
            {
                ObjectPulledList = ObjectPuller.current.GetEnemyUnitsPullList();
                ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
                ObjectPulled.transform.position = pointOfEnemy(sideOfWave);
                EnemyUnit enemyUnit = ObjectPulled.GetComponent<EnemyUnit>();

                commonEnemyUnitSettings(enemyUnit, 2, firstLevelWavesOfSubLocation, sideOfWave);
                ObjectPulled.SetActive(true);

            }

            //forth level enemies
            for (int x = 0; x < level4EnemyCount; x++)
            {
                ObjectPulledList = ObjectPuller.current.GetEnemyUnitsPullList();
                ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
                ObjectPulled.transform.position = pointOfEnemy(sideOfWave);
                EnemyUnit enemyUnit = ObjectPulled.GetComponent<EnemyUnit>();

                commonEnemyUnitSettings(enemyUnit, 3, firstLevelWavesOfSubLocation, sideOfWave);
                ObjectPulled.SetActive(true);

            } 
            

           
        }
    }

    private void resetPauseTimer() {
        attackWavePauseTimer = attackWavePauseTime;
        pauseAlerImageUp.fillAmount = 0;
        pauseAlerImageDown.fillAmount = 0;
    }

    private void attackWavePauseTimeCounter() {
        if (attackWavePauseTimer > 0) {
            //double sided regular attack
            if (secondLevelWavesOfSubLocation < _attackWaves[firstLevelWavesOfSubLocation].Count && _attackWaves[firstLevelWavesOfSubLocation][secondLevelWavesOfSubLocation][0] == 2)
            {
                if (!attackPauseAlertButtonUp.activeInHierarchy) attackPauseAlertButtonUp.SetActive(true);
                if (!attackPauseAlertButtonDown.activeInHierarchy) attackPauseAlertButtonDown.SetActive(true);
                attackWavePauseTimer -= Time.deltaTime;
                pauseAlerImageUp.fillAmount = 1-attackWavePauseTimer / attackWavePauseTime;
                pauseAlerImageDown.fillAmount = pauseAlerImageUp.fillAmount;
                if (attackWavePauseTimer <= 0)
                {
                    attackWavePauseTimer = 0;
                    attackPauseAlertButtonUp.SetActive(false);
                    attackPauseAlertButtonDown.SetActive(false);
                    resetPauseTimer();
                    startAttackWave();
                    wavePause = false;
                }
            }
            //double sided boss attack
            else if (secondLevelWavesOfSubLocation == _attackWaves[firstLevelWavesOfSubLocation].Count) {
                if (!attackPauseAlertButtonUp.activeInHierarchy) attackPauseAlertButtonUp.SetActive(true);
                if (!attackPauseAlertButtonDown.activeInHierarchy) attackPauseAlertButtonDown.SetActive(true);
                attackWavePauseTimer -= Time.deltaTime;
                pauseAlerImageUp.fillAmount = 1 - attackWavePauseTimer / attackWavePauseTime;
                pauseAlerImageDown.fillAmount = pauseAlerImageUp.fillAmount;
                if (attackWavePauseTimer <= 0)
                {
                    attackWavePauseTimer = 0;
                    attackPauseAlertButtonUp.SetActive(false);
                    attackPauseAlertButtonDown.SetActive(false);
                    resetPauseTimer();
                    startAttackWave();
                    wavePause = false;
                }
            }
            //one sided regular attack
            else
            { //side of wave is taken into account in reverse, because if the side of wave is 0 (up), then it will be changed on for () loop to 1 (down)

                attackWavePauseTimer -= Time.deltaTime;
                if (sideOfWave == 0) {
                    if (!attackPauseAlertButtonDown.activeInHierarchy) attackPauseAlertButtonDown.SetActive(true);
                    pauseAlerImageDown.fillAmount = 1 - attackWavePauseTimer / attackWavePauseTime;
                    if (attackWavePauseTimer <= 0)
                    {
                        attackWavePauseTimer = 0;
                        attackPauseAlertButtonDown.SetActive(false);
                        resetPauseTimer();
                        startAttackWave();
                        wavePause = false;
                    }
                }
                else {
                    if (!attackPauseAlertButtonUp.activeInHierarchy) attackPauseAlertButtonUp.SetActive(true);
                    pauseAlerImageUp.fillAmount = 1 - attackWavePauseTimer / attackWavePauseTime;
                    if (attackWavePauseTimer <= 0)
                    {
                        attackWavePauseTimer = 0;
                        attackPauseAlertButtonUp.SetActive(false);
                        resetPauseTimer();
                        startAttackWave();
                        wavePause = false;
                    }
                }
            }
        }
    }

    public void interruptAttackWavePause() {
        attackWavePauseTimer = 0;
        attackPauseAlertButtonDown.SetActive(false);
        attackPauseAlertButtonUp.SetActive(false);
        bonusEnergyForInteruption();
        resetPauseTimer();
        startAttackWave();
        wavePause = false;
    }

    private void startAttackWave() {
        if (secondLevelWavesOfSubLocation < _attackWaves[firstLevelWavesOfSubLocation].Count)
        {
            initiateSecondLevelAttackWaves();
        }
        else if (!bossAttackPassed)
        {
            VerticalLineGap = 0;
            HorizontalLineGap = 0;
            //1000 - location 0 mini boss, 1011-location 1 mini boss of 2-nd wave. It works as following 10 is mini boss,+0 is main wave count, 0 is location count
            instantiateBosses(10, firstLevelWavesOfSubLocation, 1);
            instantiateSomeSoldersAroundBoss(firstLevelWavesOfSubLocation);
            if (firstLevelWavesOfSubLocation < _attackWaves.Count - 1)
            {
                secondLevelWavesOfSubLocation = 0;
                firstLevelWavesOfSubLocation++;
            }
            attackWavePauseTime--; //each MiniBoss attack reduces the pause time 
            attackWaveIsOn = true;
            bossAttackPassed = true;
        }
        else if (!finalBossAttackPassed)
        {
            VerticalLineGap = 0;
            HorizontalLineGap = 0;
            //10000 - location 0 final boss, 10001-location 1 final boss. It works as following 100 is final boss, 0 wave count tha is alwais 0 for final boss, 0 is location count
            instantiateBosses(100, firstLevelWavesOfSubLocation, 2);
            instantiateSomeSoldersAroundBoss(firstLevelWavesOfSubLocation);
            attackWaveIsOn = true;
            finalBossAttackPassed = true;

        }
        totalWavesLeft--;
        updateWavesText(); 
    }

    private void bonusEnergyForInteruption() {
        GameController.instance.incrementEnergy((int)(attackWavePauseTime-attackWavePauseTimer));
    }

    private int bonusEnergyBeforeWave() {
        int energy = 0;
        foreach (CastleFire fireTile in CommonData.instance.fireTiles) {
            energy += fireTile.HP;
        }
        energy *= CommonData.instance.energyMultiplyerBaseFromAttackWave+ secondLevelWavesOfSubLocation;
        return energy;

    }

    private void Update()
    {
        if (attackWaveIsOn) {
            if (CommonData.instance.enemyUnitsAll.Count == 0) {
                attackWaveIsOn = false;

                GameController.instance.incrementEnergy(bonusEnergyBeforeWave());
                if (!finalBossAttackPassed)
                {
                    wavePause = true;
                    if (GameParams.isTutor) GameController.instance.tutorProcessor();
                }
                else GameController.instance.EndGame(true);
                
            }
        }
        if (wavePause) attackWavePauseTimeCounter();
    }
}
