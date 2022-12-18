using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [HideInInspector]
    public GameObject ObjectPulled;
    [HideInInspector]
    public List<GameObject> ObjectPulledList;
    private float attackWaveTime;
    public Text energyText;
    public Button gameTurnButton;
    private Text energyToNextUnitAddTxt;
    private int energyToNextUnitAdd;
    //private float gameTimer;
    //private const float FIRST_LVL_TIME = 15;
    //private const float SECOND_LVL_TIME = 15;
    //private const float THIRD_LVL_TIME = 30;

    //private int gameTurn;

    //public GameObject TestGo;

    // Start is called before the first frame update
    void Start()
    {
        energyToNextUnitAddTxt = gameTurnButton.GetComponentInChildren<Text>();
        instance =this;
        attackWaveTime = 7;
        initiateAttackWave(5);
        CommonData.energy = CommonData.energyOnStart;
        updateEneryText();
        energyToNextUnitAdd = CommonData.energyToNextUnitAddStep;
        //gameTurn = 0;
        //timerText.text = "0";
    }

    public void updateEneryText() => energyText.text = CommonData.energy.ToString();
    public void updateEneryToNextUnitAddText() => energyToNextUnitAddTxt.text = energyToNextUnitAdd.ToString();
    private void incrementEnergyNeedToNextUnitAdd() => energyToNextUnitAdd += CommonData.energyToNextUnitAddStep;
    public void incrementEnergy(int energy) {
        CommonData.energy += energy;
        updateUnitsAddButtonUI();
    }
    private void updateUnitsAddButtonUI()
    {
        if (!emptyPlatformTilesLeft()) gameTurnButton.interactable = false;
        else {
            if (gameTurnButton.interactable) {
                if (CommonData.energy < energyToNextUnitAdd) gameTurnButton.interactable = false;
            }
            else if (CommonData.energy >= energyToNextUnitAdd) gameTurnButton.interactable = true;
        }
    }
    private void consumeTheEnergy(int consumeAmount) {
        CommonData.energy-=consumeAmount;
        updateEneryText();
    }

    private Vector2 pointOfEnemy (int side) { 
        if (side==0) return new Vector2(Random.Range(-CommonData.horisScreenSize / 2, CommonData.horisScreenSize/2), CommonData.vertScreenSize / 2 + Random.Range(2, 3.5f));
        else if (side == 1) return new Vector2(CommonData.horisScreenSize / 2 + Random.Range(2, 3.5f), Random.Range(-CommonData.vertScreenSize / 2, CommonData.vertScreenSize / 2));
        else if (side == 2) return new Vector2(Random.Range(-CommonData.horisScreenSize / 2, CommonData.horisScreenSize/2), -CommonData.vertScreenSize / 2 - Random.Range(2, 3.5f));
        else return new Vector2(-CommonData.horisScreenSize / 2 - Random.Range(2, 3.5f), Random.Range(-CommonData.vertScreenSize / 2, CommonData.vertScreenSize/2));
    }

    public void initiateAttackWave(int counOfEnemiesForEachSide) {
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < counOfEnemiesForEachSide; j++)
            {
                ObjectPulledList = ObjectPuller.current.GetEnemyUnitsPullList();
                ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
                ObjectPulled.transform.position = pointOfEnemy(i);
                CommonData.enemyUnits.Add(ObjectPulled.GetComponent<EnemyUnit>());
                ObjectPulled.SetActive(true);
            }
        }
    }

    private bool emptyPlatformTilesLeft() {
        if (CommonData.platformPointsWithNoUnits.Count > 0) return true;
        else return false;
    }


    public void addNewUnit() {
        int positionIndex = 0;
        if (CommonData.platformPointsWithNoUnits.Count > 1)
            positionIndex = Random.Range(0, CommonData.platformPointsWithNoUnits.Count);
        else if (CommonData.platformPointsWithNoUnits.Count == 1) positionIndex = 0;

        ObjectPulledList = ObjectPuller.current.GetPlayerUnitsPullList();
        ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
        ObjectPulled.transform.position = CommonData.platformPointsWithNoUnits[positionIndex];
        CommonData.platformPointsWithNoUnits.RemoveAt(positionIndex);
        CommonData.playerUnits.Add(ObjectPulled.GetComponent<PlayerUnit>());
        ObjectPulled.SetActive(true);
        consumeTheEnergy(energyToNextUnitAdd);
        incrementEnergyNeedToNextUnitAdd();
        updateEneryToNextUnitAddText();
        updateUnitsAddButtonUI();
    }

    //public void nextTurnOfGame() {
    //    gameTurn++;
    //    if (gameTurn == 1) gameTimer = FIRST_LVL_TIME;
    //    if (gameTurn == 2) gameTimer = SECOND_LVL_TIME;
    //    if (gameTurn == 3) gameTimer = THIRD_LVL_TIME;
    //    CommonData.gameIsOn = true;
    //    gameTurnButton.interactable = false;
    //    timerText.text = gameTimer.ToString("0");
    //}

    // Update is called once per frame
    void Update()
    {
        if (attackWaveTime > 0)
        {
            attackWaveTime -= Time.deltaTime;
            if (attackWaveTime <= 0)
            {
                initiateAttackWave(4);
                attackWaveTime = 7;
            }

        }

        //if (CommonData.gameIsOn)
        //{
        //    gameTimer -= Time.deltaTime;
        //    if (gameTimer <= 0)
        //    {
        //        CommonData.gameIsOn = false;
        //        gameTurnButton.interactable = true;
        //    }

        //    timerText.text = gameTimer.ToString("0");
        //}
    }
}
