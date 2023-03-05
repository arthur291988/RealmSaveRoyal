
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [SerializeField]
    private GameObject groundTile;
    [SerializeField]
    private SpriteAtlas groundSpritesAtlas;


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
    public TextMeshProUGUI bonusHitsAvailableTMP;
    private int bonusHitSliderMaxValue;
    private int bonusHitSliderMaxValueBase;
    private int bonusHitSliderMinValueMultiplier;
    private int bounsHitsAvailable;
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

    [NonSerialized]
    public float unitYShift; //is used to put unit little bit up on castle tile. because castle tile is isometric

    public GameObject messagePanel;
    public TextMeshProUGUI messagePanleText;

    public SpriteAtlas enemyAtlas;
    public SpriteAtlas playerSpriteAtlases;
    public SpriteAtlas playerRangeSpriteAtlases;
    public SpriteAtlas castleTileSpriteAtlases;
    public SpriteAtlas enemyWallSpriteAtlases;

    private bool victory;


    private void Awake()
    {
        unitYShift = 0.5f;
        instance = this;
        gameIsOn = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        bounsHitsAvailable = 0;
        updateBonusHitsAvailable();
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
        updateEneryToNextUnitAddText();
        updateEneryToNextCastleTileAddText();

        setNextSuperHit();

        Vector2 worldPosition = CommonData.instance.cameraOfGame.ScreenToWorldPoint(new Vector3(0, lowerPanelRect.anchoredPosition.y + lowerPanelRect.sizeDelta.y* Screen.height/canvasScaler.referenceResolution.y, 0));
        bottomShotLine = worldPosition.y;
        topShotLine = -worldPosition.y;
        addGroundTilesOnStart();
        addPlatformTilesOnStart();
        addInitialCastleTiles();

    }

    public void backToMenu() {
        if (victory) {
            //TODO in case of success of MVP test
            GameParams.achievedLocationStatic = CommonData.instance.location;
            if (GameParams.achievedSubLocationStatic<= CommonData.instance.subLocation) GameParams.achievedSubLocationStatic = CommonData.instance.subLocation+1;
        }
        SaveAndLoad.instance.saveGameData();
        SceneSwitchMngr.LoadMenuScene();
    }

    public void EndGame(bool win)
    {
        gameIsOn = false;
        if (win)
        {

            showMessage(CommonData.instance.getWinText(), CommonData.instance.greenColor);
            victory = true;
        }
        else
        {
            showMessage(CommonData.instance.getLoseText(), CommonData.instance.redColor);
            victory = false; 
        }
    }


    private void addPlatformTilesOnStart()
    {
        foreach (Vector2 positionOfPlatform in CommonData.instance.platformTilesUp.Keys)
        {
            ObjectPulledList = ObjectPuller.current.GetPlatformTilePullList();
            ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
            ObjectPulled.transform.position = positionOfPlatform;
            ObjectPulled.SetActive(true);
        }
        foreach (Vector2 positionOfPlatform in CommonData.instance.platformTilesDown.Keys)
        {
            ObjectPulledList = ObjectPuller.current.GetPlatformTilePullList();
            ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
            ObjectPulled.transform.position = positionOfPlatform;
            ObjectPulled.SetActive(true);
        }
    }

    private void addGroundTilesOnStart() {
        GameObject groundTileLocal;
        float xPoint;
        float yPoint = 0;
        float xStep;
        float yStep = 15;
        float yMult = 1;
        float xMult = 1;
        for (int i = 0; i < 5; i++) {
            xPoint = 0;
            xStep = 15;
            if (i != 0)
            {
                if (i % 2 != 0)
                {
                    yMult = 1;
                    yPoint += yStep;
                }
                else
                {
                    yMult = -1;
                }
            }
            for (int j = 0; j < 5; j++) {
                if (j != 0)
                {
                    if (j % 2 != 0)
                    {
                        xMult = 1;
                        xPoint += xStep;
                    }
                    else xMult = -1;
                }
                groundTileLocal = Instantiate(groundTile);
                groundTileLocal.GetComponent<SpriteRenderer>().sprite = groundSpritesAtlas.GetSprite(CommonData.instance.locationString+CommonData.instance.subLocationString);
                groundTileLocal.transform.position = new Vector2(xPoint* xMult, yPoint*yMult);
            }
        }
    }

    public void showMessage(string message, Color colorOfMessage)
    {
        messagePanleText.text = message;
        messagePanleText.color = colorOfMessage; 
        messagePanel.SetActive(true);
    }

    private void setStartPowerUpSettings()
    {
        energyToNextPowerUpList = new List<int>();
        for (int i = 0; i < powerUpButtonsList.Count; i++) { 
            energyToNextPowerUpList.Add(CommonData.instance.energyToPowerUpBase); 
            powerUpButtonTextsList[i].text = energyToNextPowerUpList[i].ToString();
            powerUpButtonsListImg[i].sprite = playerSpriteAtlases.GetSprite(CommonData.instance.playerUnitTypesOnScene[i].ToString());
        }
    }

    public void updateEneryText() => energyText.text = CommonData.instance.energy.ToString();
    public void updateEneryToNextUnitAddText() => energyToNextUnitAddTxt.text = energyToNextUnitAdd.ToString();
    public void updateEneryToNextCastleTileAddText() => energyToNextCastleTileAddTxt.text = energyToNextCastleTileAdd.ToString();
    private void incrementEnergyNeedToNextUnitAdd() => energyToNextUnitAdd += CommonData.instance.energyToNextUnitAddStep;
    private void incrementEnergyNeedToNextCatleTileAdd() => energyToNextCastleTileAdd += CommonData.instance.energyToNextUnitAddStep;
    public void incrementEnergy(int energy) {
        CommonData.instance.energy += energy;
        updateEneryText();
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
                if (CommonData.instance.energy < energyToNextCastleTileAdd)
                {
                    addCastleTileButton.interactable = false;
                    //lightenPlatformTiles(false);
                }
            }
            else if (CommonData.instance.energy >= energyToNextCastleTileAdd)
            {
                addCastleTileButton.interactable = true;
                //lightenPlatformTiles(true);
            }
        }
    }

    //private void lightenPlatformTiles(bool light) {

    //    foreach (PlatformTile platform in CommonData.instance.allPlatformTiles) {
    //        if (platform.side == 0) {
    //            //0 means platform is empty
    //            if (CommonData.instance.platformTilesUp[platform._position] == 0) {
    //                platform.lightenTheTile(light);
    //            }
    //        }
    //        if (platform.side == 1)
    //        {
    //            //0 means platform is empty
    //            if (CommonData.instance.platformTilesDown[platform._position] == 0)
    //            {
    //                platform.lightenTheTile(light);
    //            }
    //        }
    //    }
    //}

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
        int unitTypeIndex = UnityEngine.Random.Range(0, CommonData.instance.playerUnitTypesOnScene.Length);

        //position index on all castle points with no units (randome if there more than ont empty castle tile and first one if only one empty tile left)
        if (CommonData.instance.castlePointsWithNoUnits.Count > 1)
            positionIndex = UnityEngine.Random.Range(0, CommonData.instance.castlePointsWithNoUnits.Count);
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
        unit.updatePropertiesToLevel(false);
        //unit.setUnitFeatures(unit._baseHarm, unit._baseAttackSpeed, unit._baseAccuracy);

        consumeTheEnergy(energyToNextUnitAdd);
        incrementEnergyNeedToNextUnitAdd();
        updateEneryToNextUnitAddText();
        updateUnitsAndCastleTileAddButtonsUI();
        updatePowerUpButtonsUI();

        //foreach (CastleTiles ct in CommonData.instance.castleTiles) Debug.Log(ct._playerUnit == null);
    }


    //public void addNewCastleTileByPoint(Vector2 positionPointed, PlatformTile plaform)
    //{
    //    if (CommonData.instance.energy >= energyToNextCastleTileAdd)
    //    {
    //        if (positionPointed.y > 0)
    //        {
    //            if (CommonData.instance.platformTilesUp[positionPointed] == 0)
    //            {
    //                ObjectPulledList = ObjectPuller.current.GetCastleTilePullList();
    //                ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
    //                ObjectPulled.transform.position = positionPointed;

    //                CastleTiles castleTile = ObjectPulled.GetComponent<CastleTiles>();
    //                castleTile.addCastleTilePropertiesToCommonData(0, positionPointed);

    //                ObjectPulled.SetActive(true);

    //                consumeTheEnergy(energyToNextCastleTileAdd);
    //                incrementEnergyNeedToNextCatleTileAdd();
    //                updateEneryToNextCastleTileAddText();
    //                updateUnitsAndCastleTileAddButtonsUI();
    //                updatePowerUpButtonsUI();
    //            }
    //        }
    //        else
    //        {
    //            if (CommonData.instance.platformTilesDown[positionPointed] == 0)
    //            {
    //                ObjectPulledList = ObjectPuller.current.GetCastleTilePullList();
    //                ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
    //                ObjectPulled.transform.position = positionPointed;

    //                CastleTiles castleTile = ObjectPulled.GetComponent<CastleTiles>();
    //                castleTile.addCastleTilePropertiesToCommonData(1, positionPointed);

    //                ObjectPulled.SetActive(true);

    //                consumeTheEnergy(energyToNextCastleTileAdd);
    //                incrementEnergyNeedToNextCatleTileAdd();
    //                updateEneryToNextCastleTileAddText();
    //                updateUnitsAndCastleTileAddButtonsUI();
    //                updatePowerUpButtonsUI();
    //            }
    //        }
    //        plaform.lightenTheTile(false);
    //    }
    //}


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
        sideOfCastleTile = upEmptyPlatformTiles == downEmptyPlatformTiles? UnityEngine.Random.Range(0,2) : upEmptyPlatformTiles < downEmptyPlatformTiles?1:0;

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
            bounsHitsAvailable++;
            updateBonusHitsAvailable();
            bonusHitSliderMinValueMultiplier++;
            bonusHitSliderMaxValue = bonusHitSliderMaxValueBase*bonusHitSliderMinValueMultiplier;
            bonusFillSlider.maxValue = bonusHitSliderMaxValue;
            bonusFillSlider.value = 0;
            updateBonusHitButton();
        }
    }

    public void updateBonusHitButton() {
        if (bounsHitsAvailable > 0) {
            if (!bonusHitButton.interactable) bonusHitButton.interactable = true;
        }
        else if (bonusHitButton.interactable) bonusHitButton.interactable = false;
    }

    private void updateBonusHitsAvailable()
    {
        bonusHitsAvailableTMP.text = bounsHitsAvailable.ToString();
    }  

    public void bonusHit() {

        bounsHitsAvailable--;
        updateBonusHitsAvailable();
        updateBonusHitButton();
        for (int i=0;i<CommonData.instance.playerUnits.Count;i++) {
            if (CommonData.instance.playerUnits[i]._unitType == indexOfNextMegaHitUnit) CommonData.instance.playerUnits[i].superHit();
        }
        setNextSuperHit();
    }

    private void setNextSuperHit()
    {
        indexOfNextMegaHitUnit = CommonData.instance.playerUnitTypesOnScene[UnityEngine.Random.Range(0, CommonData.instance.playerUnitTypesOnScene.Length)];
        bonusHitButtonImage.sprite = playerSpriteAtlases.GetSprite(indexOfNextMegaHitUnit.ToString());
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
                CommonData.instance.playerUnits[i].updatePropertiesToLevel(true);
            }
        }
    }
}
