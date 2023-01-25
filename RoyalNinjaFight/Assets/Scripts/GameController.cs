
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
    private Image bonusHitButtonImage;

    public Slider bonusFillSlider;
    private int bonusHitSliderMaxValue;
    private int bonusHitSliderMaxValueBase;
    private int bonusHitSliderMinValueMultiplier;
    //private int bonusHitSliderValueStep;

    private Text energyToNextUnitAddTxt;
    private int energyToNextUnitAdd;

    private Text energyToNextCastleTileAddTxt;
    private int energyToNextCastleTileAdd;

    public List<Button> powerUpButtonsList;
    public List<Image> powerUpButtonsListImg;
    public List<Text> powerUpButtonTextsList;
    public List<int> energyToNextPowerUpList;

    //public ParticleSystem bonusHitEffect;

    private int indexOfNextMegaHitUnit;

    public float unitYShift; //is used to put unit little bit up on castle tile. because castle tile is isometric

    private void Awake()
    {
        unitYShift = 0.5f;
        instance = this;
        gameIsOn = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        bonusHitButton.interactable = false;
        bonusHitButtonImage = bonusHitButton.image; 
        bonusHitSliderMinValueMultiplier = 1;
        bonusHitSliderMaxValue = 100;
        bonusHitSliderMaxValueBase = bonusHitSliderMaxValue;
        //bonusHitSliderValueStep = 10;
        bonusFillSlider.maxValue = bonusHitSliderMaxValue;
        bonusFillSlider.value = 0;
        energyToNextUnitAddTxt = addUnitButton.GetComponentInChildren<Text>();
        energyToNextCastleTileAddTxt = addCastleTileButton.GetComponentInChildren<Text>();
        CommonData.instance.energy = CommonData.instance.energyOnStart;
        updateEneryText();
        setStartPowerUpSettings();
        energyToNextUnitAdd = CommonData.instance.energyToNextUnitAddStep;
        energyToNextCastleTileAdd = CommonData.instance.energyToNextUnitAddStep;

        setNextSuperHIt();

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
        updateBonusSliderFill(energy);
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
                if (energyToNextPowerUpList[i] <= CommonData.instance.energyToPowerUpMax)
                {
                    energyToNextPowerUpList[i] *= 2;
                    if(energyToNextPowerUpList[i]<= CommonData.instance.energyToPowerUpMax) powerUpButtonTextsList[i].text = energyToNextPowerUpList[i].ToString();
                    else powerUpButtonTextsList[i].text = "Max";
                }
            }
        }
    }
    public void updatePowerUpButtonsUI()
    {
        for (int i = 0; i < energyToNextPowerUpList.Count; i++) {
            if (energyToNextPowerUpList[i] > CommonData.instance.energyToPowerUpMax) powerUpButtonsList[i].interactable = false;
            else { 
                if (energyToNextPowerUpList[i]<= CommonData.instance.energy) powerUpButtonsList[i].interactable = true;
                else powerUpButtonsList[i].interactable = false;
            }
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
            if (CommonData.instance.castleTiles[i]._position == new Vector2(position.x, position.y-unitYShift))
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
        ObjectPulled.transform.position = new Vector2 (CommonData.instance.castlePointsWithNoUnits[positionIndex].x, CommonData.instance.castlePointsWithNoUnits[positionIndex].y+ unitYShift);
        CommonData.instance.castlePointsWithNoUnits.RemoveAt(positionIndex);
        PlayerUnit unit = ObjectPulled.GetComponent<PlayerUnit>();
        unit.setUnitMergeLevel(0);
        unit.SetUnitType(CommonData.instance.playerUnitTypesOnScene[unitTypeIndex]);
        unit.setSpriteOfUnit();
        unit.addToCommonData();


        ObjectPulled.SetActive(true);
        unit.setUnitPosition();
        getAndSetTileToUnit(unit._unitStartPosition,unit); 
        unit.updatePropertiesToLevel();
        //unit.setUnitFeatures(unit._baseHarm, unit._baseAttackSpeed, unit._baseAccuracy);

        consumeTheEnergy(energyToNextUnitAdd);
        incrementEnergyNeedToNextUnitAdd();
        updateEneryToNextUnitAddText();
        updateUnitsAndCastleTileAddButtonsUI();
        updatePowerUpButtonsUI();

        //foreach (CastleTiles ct in CommonData.instance.castleTiles) Debug.Log(ct._playerUnit == null);
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
            updateUnitsAndCastleTileAddButtonsUI();
            updatePowerUpButtonsUI();
        }
    }

    private void addInitialCastleTiles()
    {
        for (int j = 0; j < 6; j++)
        {
            addNewCastleTile(true);
        }


        //foreach (CastleTiles ct in CommonData.instance.castleTiles) Debug.Log(ct._playerUnit == null);
    }

    public void updateBonusSliderFill(int energy) {
        bonusFillSlider.value += energy;
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
        for (int i=0;i<CommonData.instance.playerUnits.Count;i++) {
            if (CommonData.instance.playerUnits[i]._unitType == indexOfNextMegaHitUnit) CommonData.instance.playerUnits[i].superHit();
        }
        setNextSuperHIt();
    }

    private void setNextSuperHIt()
    {
        indexOfNextMegaHitUnit = CommonData.instance.playerUnitTypesOnScene[Random.Range(0, CommonData.instance.playerUnitTypesOnScene.Length)];
        bonusHitButtonImage.sprite = CommonData.instance.playerSpriteAtlases.GetSprite(indexOfNextMegaHitUnit.ToString());
    }

    public void poweUpUnits(int index) {
        consumeTheEnergy(energyToNextPowerUpList[index]);
        incrementEnergyNeedToNextPowerUp(index);
        updateUnitsAndCastleTileAddButtonsUI();
        updatePowerUpButtonsUI();
        CommonData.instance.playerUnitTypesOnScenePowerUpLevel[index]++;
        for (int i = 0; i < CommonData.instance.playerUnits.Count; i++) {
            if (CommonData.instance.playerUnits[i]._unitType == CommonData.instance.playerUnitTypesOnScene[index])
            {
                //CommonData.instance.playerUnits[i].setUnitPoweUpLevel(CommonData.instance.playerUnitTypesOnScenePowerUpLevel[i]);
                CommonData.instance.playerUnits[i].updatePropertiesToLevel();
            }
        }
    }
}
