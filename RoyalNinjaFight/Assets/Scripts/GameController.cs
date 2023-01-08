﻿
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public static GameController instance;

    public bool gameIsOn;

    public RectTransform lowerPanelRect;
    public CanvasScaler canvasScaler;
    [HideInInspector]
    public float bottomShotLine;
    [HideInInspector]
    public float topShotLine;
    [HideInInspector]
    public GameObject ObjectPulled;
    [HideInInspector]
    public List<GameObject> ObjectPulledList;
    public Text energyText;

    public Button addUnitButton;
    public Button addCastleTileButton;
    public Button bonusHitButton;

    public Slider bonusFillSlider;
    private int bonusHitSliderMaxValue;
    private int bonusHitSliderMaxValueBase;
    private int bonusHitSliderMinValueMultiplier;
    private int bonusHitSliderValueStep;

    private Text energyToNextUnitAddTxt;
    private int energyToNextUnitAdd;

    private Text energyToNextCastleTileAddTxt;
    private int energyToNextCastleTileAdd;

    public List<Button> powerUpButtonsList;
    public List<Image> powerUpButtonsListImg;
    public List<Text> powerUpButtonTextsList;
    public List<int> energyToNextPowerUpList;

    private void Awake()
    {
        instance = this;
        gameIsOn = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        bonusHitButton.interactable = false;
        bonusHitSliderMinValueMultiplier = 1;
        bonusHitSliderMaxValue = 100;
        bonusHitSliderMaxValueBase = bonusHitSliderMaxValue;
        bonusHitSliderValueStep = 10;
        bonusFillSlider.maxValue = bonusHitSliderMaxValue;
        bonusFillSlider.value = 0;
        energyToNextUnitAddTxt = addUnitButton.GetComponentInChildren<Text>();
        energyToNextCastleTileAddTxt = addCastleTileButton.GetComponentInChildren<Text>();
        CommonData.instance.energy = CommonData.instance.energyOnStart;
        updateEneryText();
        setStartPowerUpSettings();
        energyToNextUnitAdd = CommonData.instance.energyToNextUnitAddStep;
        energyToNextCastleTileAdd = CommonData.instance.energyToNextUnitAddStep;



        Vector2 worldPosition = CommonData.instance.cameraOfGame.ScreenToWorldPoint(new Vector3(0, lowerPanelRect.anchoredPosition.y + lowerPanelRect.sizeDelta.y* Screen.height/canvasScaler.referenceResolution.y, 0));
        bottomShotLine = worldPosition.y;
        topShotLine = -worldPosition.y;
        addInitialCastleTiles();
        //gameTurn = 0;
        //timerText.text = "0";
    }

    private void setStartPowerUpSettings()
    {
        energyToNextPowerUpList = new List<int>();
        for (int i = 0; i < powerUpButtonsList.Count; i++) { 
            energyToNextPowerUpList.Add(CommonData.instance.energyToPowerUpBase); 
            powerUpButtonTextsList[i].text = energyToNextPowerUpList[i].ToString();
            powerUpButtonsListImg[i].sprite = CommonData.instance.playerSpriteAtlases.GetSprite(CommonData.instance.playerUnitTypesOnScene[i].ToString());
        }
    }

    public void updateEneryText() => energyText.text = CommonData.instance.energy.ToString();
    public void updateEneryToNextUnitAddText() => energyToNextUnitAddTxt.text = energyToNextUnitAdd.ToString();
    public void updateEneryToNextCastleTileAddText() => energyToNextCastleTileAddTxt.text = energyToNextCastleTileAdd.ToString();
    private void incrementEnergyNeedToNextUnitAdd() => energyToNextUnitAdd += CommonData.instance.energyToNextUnitAddStep;
    private void incrementEnergyNeedToNextCatleTileAdd() => energyToNextCastleTileAdd += CommonData.instance.energyToNextUnitAddStep;
    public void incrementEnergy(int energy) {
        CommonData.instance.energy += energy;
        updateUnitsAndCastleTileAddButtonsUI();
        updateBonusSliderFill();
        updatePowerUpButtonsUI();
    }

    public void updateUnitsAndCastleTileAddButtonsUI()
    {
        if (!emptyCastleTilesLeft()) addUnitButton.interactable = false;
        else {
            if (addUnitButton.interactable) {
                if (CommonData.instance.energy < energyToNextUnitAdd) addUnitButton.interactable = false;
            }
            else if (CommonData.instance.energy >= energyToNextUnitAdd) addUnitButton.interactable = true;
        }

        if (!emptyPlatformTilesLeft()) addCastleTileButton.interactable = false;
        else
        {
            if (addCastleTileButton.interactable)
            {
                if (CommonData.instance.energy < energyToNextCastleTileAdd) addCastleTileButton.interactable = false;
            }
            else if (CommonData.instance.energy >= energyToNextCastleTileAdd) addCastleTileButton.interactable = true;
        }

    }

    public void incrementEnergyNeedToNextPowerUp(int index) {
        for (int i = 0; i < energyToNextPowerUpList.Count; i++) {
            if (index == i) {
                if (energyToNextPowerUpList[i] < CommonData.instance.energyToPowerUpMax)
                {
                    energyToNextPowerUpList[i] *= 2;
                    powerUpButtonTextsList[i].text = energyToNextPowerUpList[i].ToString();
                }
                else powerUpButtonTextsList[i].text = "Max";
            }
        }
    }
    public void updatePowerUpButtonsUI()
    {
        for (int i = 0; i < energyToNextPowerUpList.Count; i++) {
            if (energyToNextPowerUpList[i] == CommonData.instance.energyToPowerUpMax) powerUpButtonsList[i].interactable = false;
            else { 
                if (energyToNextPowerUpList[i]<= CommonData.instance.energy) powerUpButtonsList[i].interactable = true;
                else powerUpButtonsList[i].interactable = false;
            }
        } 
    }

    private void consumeTheEnergy(int consumeAmount) {
        CommonData.instance.energy -=consumeAmount;
        updateEneryText();
        updateUnitsAndCastleTileAddButtonsUI();
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

    private bool emptyCastleTilesLeft() {
        if (CommonData.instance.castlePointsWithNoUnits.Count > 0) return true;
        else return false;
    }
    private bool emptyPlatformTilesLeft()
    {
        foreach (int x in CommonData.instance.platformTilesUp.Values) if (x==0) return true;
        foreach (int x in CommonData.instance.platformTilesDown.Values) if (x == 0) return true;
        return false;
    }

    public void addNewUnit() {
        int positionIndex = 0;
        int unitTypeIndex = Random.Range(0, CommonData.instance.playerUnitTypesOnScene.Length);

        //position index on all castle points with no units (randome if there more than ont empty castle tile and first one if only one empty tile left)
        if (CommonData.instance.castlePointsWithNoUnits.Count > 1)
            positionIndex = Random.Range(0, CommonData.instance.castlePointsWithNoUnits.Count);
        else if (CommonData.instance.castlePointsWithNoUnits.Count == 1) positionIndex = 0;

        ObjectPulledList = ObjectPuller.current.GetPlayerUnitsPullList(CommonData.instance.playerUnitTypesOnScene[unitTypeIndex]);
        ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
        ObjectPulled.transform.position = CommonData.instance.castlePointsWithNoUnits[positionIndex];
        CommonData.instance.castlePointsWithNoUnits.RemoveAt(positionIndex);
        PlayerUnit unit = ObjectPulled.GetComponent<PlayerUnit>();
        unit.setUnitMergeLevel(0);
        unit.setUnitPoweUpLevel(0);
        unit.SetUnitType(CommonData.instance.playerUnitTypesOnScene[unitTypeIndex]);
        unit.setSpriteOfUnit();
        CommonData.instance.playerUnits.Add(unit);


        ObjectPulled.SetActive(true);
        unit.setUnitPosition();
        getAndSetTileToUnit(unit._unitStartPosition,unit);
        unit.setUnitFeatures(unit._baseHarm, unit._baseAttackSpeed, unit._baseAccuracy);

        consumeTheEnergy(energyToNextUnitAdd);
        incrementEnergyNeedToNextUnitAdd();
        updateEneryToNextUnitAddText();
    }

    public void addNewCastleTile(bool start) {
        int upEmptyPlatformTiles = 0;
        int downEmptyPlatformTiles = 0;
        int sideOfCastleTile = 0;
        Vector2 position = Vector2.zero;

        foreach (int x in CommonData.instance.platformTilesUp.Values) {
            if (x==0) upEmptyPlatformTiles++;
        }
        foreach (int x in CommonData.instance.platformTilesDown.Values)
        {
            if (x == 0) downEmptyPlatformTiles++;
        }
        sideOfCastleTile = upEmptyPlatformTiles == downEmptyPlatformTiles? Random.Range(0,2) : upEmptyPlatformTiles < downEmptyPlatformTiles?1:0;

        ObjectPulledList = ObjectPuller.current.GetCastleTilePullList();
        ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);

        if (sideOfCastleTile == 0)
        {
            foreach (Vector2 key in CommonData.instance.platformTilesUp.Keys)
            {
                if (CommonData.instance.platformTilesUp[key] == 0) { position = key; break; }
                
            }
        }
        else
        {
            foreach (Vector2 key in CommonData.instance.platformTilesDown.Keys)
            {
                if (CommonData.instance.platformTilesDown[key] == 0) { position = key; break; };
            }
        }

        ObjectPulled.transform.position = position;
        CastleTiles castleTile = ObjectPulled.GetComponent<CastleTiles>();
        castleTile.addCastleTilePropertiesToCommonData(sideOfCastleTile,position);

        ObjectPulled.SetActive(true);

        if (!start)
        {
            consumeTheEnergy(energyToNextCastleTileAdd);
            incrementEnergyNeedToNextCatleTileAdd();
            updateEneryToNextCastleTileAddText();
        }
    }

    private void addInitialCastleTiles()
    {
        for (int j = 0; j < 6; j++)
        {
            addNewCastleTile(true);
        }
    }

    public void updateBonusSliderFill() {
        bonusFillSlider.value += bonusHitSliderValueStep;
        if (bonusFillSlider.value >= bonusHitSliderMaxValue) {
            bonusHitSliderMinValueMultiplier++;
            bonusHitSliderMaxValue = bonusHitSliderMaxValueBase*bonusHitSliderMinValueMultiplier;
            bonusFillSlider.maxValue = bonusHitSliderMaxValue;
            bonusFillSlider.value = 0;
            updateBonusHitButton();
        }
    }

    public void updateBonusHitButton() {
        if (!bonusHitButton.interactable) bonusHitButton.interactable = true;
    }

    public void bonusHit() {
        bonusHitButton.interactable = false;
    }

    public void poweUpUnits(int index) {

        consumeTheEnergy(energyToNextPowerUpList[index]);
        incrementEnergyNeedToNextPowerUp(index);
        updatePowerUpButtonsUI();
        for (int i = 0; i < CommonData.instance.playerUnits.Count; i++) {
            if (CommonData.instance.playerUnits[i]._unitType == CommonData.instance.playerUnitTypesOnScene[index])
            {
                CommonData.instance.playerUnits[i].setUnitPoweUpLevel(CommonData.instance.playerUnits[i]._unitPowerUpLevel + 1);
                CommonData.instance.playerUnits[i].updatePropertiesToLevel();
            }
        }
    }
}
