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
    public Text energyText;
    public Button gameTurnButton;
    private Text energyToNextUnitAddTxt;
    private int energyToNextUnitAdd;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        energyToNextUnitAddTxt = gameTurnButton.GetComponentInChildren<Text>();
        CommonData.instance.energy = CommonData.instance.energyOnStart;
        updateEneryText();
        energyToNextUnitAdd = CommonData.instance.energyToNextUnitAddStep;
        populatePlayerUnitsTypesArray();
        //gameTurn = 0;
        //timerText.text = "0";
    }

    private void populatePlayerUnitsTypesArray() {
        for (int i = 0; i < CommonData.instance.playerUnitTypesOnScene.Length; i++) {
            CommonData.instance.playerUnitTypesOnScene[i] = i + 1; //TO DO with more than 3 types (for now there only 3 types)
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

    private void getAndSetTileToUnit(Vector2 position, PlayerUnit unit)
    {
        for (int i = 0; i < CommonData.instance.castleTiles.Count; i++)
        {
            if (CommonData.instance.castleTiles[i]._position == position)
            {
                CommonData.instance.castleTiles[i]._playerUnit = unit;
                break;
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
        unit.setUnitLevel(0);
        unit.SetUnitType(CommonData.instance.playerUnitTypesOnScene[unitTypeIndex]);
        unit.setSpriteOfUnit();
        CommonData.instance.playerUnits.Add(unit);


        ObjectPulled.SetActive(true);
        unit.setUnitPosition();
        getAndSetTileToUnit(unit._unitStartPosition,unit);


        consumeTheEnergy(energyToNextUnitAdd);
        incrementEnergyNeedToNextUnitAdd();
        updateEneryToNextUnitAddText();
        updateUnitsAddButtonUI();
    }

    
}
