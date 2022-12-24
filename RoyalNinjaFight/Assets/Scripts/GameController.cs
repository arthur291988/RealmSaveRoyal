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


    // Start is called before the first frame update
    void Start()
    {
        energyToNextUnitAddTxt = gameTurnButton.GetComponentInChildren<Text>();
        instance =this;
        attackWaveTime = 7;
        initiateAttackWave(5,1);
        CommonData.instance.energy = CommonData.instance.energyOnStart;
        updateEneryText();
        energyToNextUnitAdd = CommonData.instance.energyToNextUnitAddStep;
        populatePlayerUnitsTypesArray();
        //gameTurn = 0;
        //timerText.text = "0";
    }

    private void populatePlayerUnitsTypesArray() {
        for (int i = 0; i < CommonData.instance.playerUnitTypesOnScene.Length; i++) {
            CommonData.instance.playerUnitTypesOnScene[i] = i + 1; //TO DO 
        } 
    }

    public void updateEneryText() => energyText.text = CommonData.instance.energy.ToString();
    public void updateEneryToNextUnitAddText() => energyToNextUnitAddTxt.text = energyToNextUnitAdd.ToString();
    private void incrementEnergyNeedToNextUnitAdd() => energyToNextUnitAdd += CommonData.instance.energyToNextUnitAddStep;
    public void incrementEnergy(int energy) {
        CommonData.instance.energy += energy;
        updateUnitsAddButtonUI();
    }
    public void updateUnitsAddButtonUI()
    {
        if (!emptyPlatformTilesLeft()) gameTurnButton.interactable = false;
        else {
            if (gameTurnButton.interactable) {
                if (CommonData.instance.energy < energyToNextUnitAdd) gameTurnButton.interactable = false;
            }
            else if (CommonData.instance.energy >= energyToNextUnitAdd) gameTurnButton.interactable = true;
        }
    }
    private void consumeTheEnergy(int consumeAmount) {
        CommonData.instance.energy -=consumeAmount;
        updateEneryText();
    }

    private Vector2 pointOfEnemy (int side) { 
        if (side==0) return new Vector2(Random.Range(-CommonData.instance.horisScreenSize / 2, CommonData.instance.horisScreenSize /2), CommonData.instance.vertScreenSize / 2 + Random.Range(2, 3.5f));
        else if (side == 1) return new Vector2(CommonData.instance.horisScreenSize / 2 + Random.Range(2, 3.5f), Random.Range(-CommonData.instance.vertScreenSize / 2, CommonData.instance.vertScreenSize / 2));
        else if (side == 2) return new Vector2(Random.Range(-CommonData.instance.horisScreenSize / 2, CommonData.instance.horisScreenSize/2), -CommonData.instance.vertScreenSize / 2 - Random.Range(2, 3.5f));
        else return new Vector2(-CommonData.instance.horisScreenSize / 2 - Random.Range(2, 3.5f), Random.Range(-CommonData.instance.vertScreenSize / 2, CommonData.instance.vertScreenSize/2));
    }

    public void initiateAttackWave(int counOfEnemiesForEachSide, int level) {
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < counOfEnemiesForEachSide; j++)
            {
                ObjectPulledList = ObjectPuller.current.GetEnemyUnitsPullList();
                ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
                ObjectPulled.transform.position = pointOfEnemy(i);
                EnemyUnit enemyUnit = ObjectPulled.GetComponent<EnemyUnit>();
                CommonData.instance.enemyUnits.Add(enemyUnit);
                enemyUnit.setEnemyLevel(level);
                ObjectPulled.SetActive(true);
            }
        }
    }

    private bool emptyPlatformTilesLeft() {
        if (CommonData.instance.platformPointsWithNoUnits.Count > 0) return true;
        else return false;
    }


    public void addNewUnit() {
        int positionIndex = 0;
        int unitTypeIndex = Random.Range(0, CommonData.instance.playerUnitTypesOnScene.Length);
        if (CommonData.instance.platformPointsWithNoUnits.Count > 1)
            positionIndex = Random.Range(0, CommonData.instance.platformPointsWithNoUnits.Count);
        else if (CommonData.instance.platformPointsWithNoUnits.Count == 1) positionIndex = 0;

        ObjectPulledList = ObjectPuller.current.GetPlayerUnitsPullList(CommonData.instance.playerUnitTypesOnScene[unitTypeIndex]);
        ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
        ObjectPulled.transform.position = CommonData.instance.platformPointsWithNoUnits[positionIndex];
        CommonData.instance.platformPointsWithNoUnits.RemoveAt(positionIndex);
        PlayerUnit unit = ObjectPulled.GetComponent<PlayerUnit>();
        unit.setUnitLevel(1);
        unit.SetUnitType(CommonData.instance.playerUnitTypesOnScene[unitTypeIndex]);
        unit.setSpriteOfUnit();
        CommonData.instance.playerUnits.Add(unit);


        ObjectPulled.SetActive(true);
        unit.setUnitPosition();


        consumeTheEnergy(energyToNextUnitAdd);
        incrementEnergyNeedToNextUnitAdd();
        updateEneryToNextUnitAddText();
        updateUnitsAddButtonUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (attackWaveTime > 0)
        {
            attackWaveTime -= Time.deltaTime;
            if (attackWaveTime <= 0)
            {
                initiateAttackWave(4,1);
                attackWaveTime = 7;
            }

        }
    }
}
