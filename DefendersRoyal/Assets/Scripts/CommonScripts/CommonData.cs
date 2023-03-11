
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using static UnityEditor.PlayerSettings;

public class CommonData : MonoBehaviour
{
    public static CommonData instance { get; private set; }

    public CommonData() {
        instance = this;
    }

    //public SpriteAtlas enemyAtlas;
    //public SpriteAtlas playerSpriteAtlases;
    //public SpriteAtlas playerRangeSpriteAtlases;
    //public SpriteAtlas castleTileSpriteAtlases;

    [NonSerialized]
    public Camera cameraOfGame;
    [NonSerialized]
    public float vertScreenSize;
    [NonSerialized]
    public float horisScreenSize;

    [NonSerialized]
    public Vector2 shotDirection;

    [NonSerialized]
    public bool gameIsOn;

    [NonSerialized]
    public List<EnemyUnit> enemyUnitsAll; //this one is used on attack wave sequence control

    [NonSerialized]
    public Dictionary<int, List<EnemyUnit>> enemyUnits; //0 - is up, 1 is down

    [NonSerialized]
    public List<PlayerUnit> playerUnits;

    [NonSerialized]
    public List<CastleTiles> castleTiles;

    [NonSerialized]
    public List<CastleFire> fireTiles;

    //value shows if tile is empty 0-empty 1 - not Empty 
    [NonSerialized]
    public Dictionary<Vector2, int> platformTilesUp;

    //value shows if tile is empty 0-empty 1 - not Empty 
    [NonSerialized]
    public Dictionary<Vector2, int> platformTilesDown;

    //[NonSerialized]
    //public List<PlatformTile> allPlatformTiles;

    [NonSerialized]
    public List<Vector2> castlePointsUp;
    [NonSerialized]
    public List<Vector2> castlePointsDown;

    [NonSerialized]
    public List<Vector2> castlePointsWithNoUnits;

    //[NonSerialized]
    //public List<Vector2> insideCastlePointsForEnemyUnits;

    [NonSerialized]
    public int[] playerUnitTypesOnScene;
    [NonSerialized]
    public int[] playerUnitTypesOnScenePowerUpLevel;

    [NonSerialized]
    public int playerUnitMaxLevel;

    [NonSerialized]
    public int HPOfTile;

    [NonSerialized]
    public int energy;

    [NonSerialized]
    public int energyOnStart;
    [NonSerialized]
    public int energyToNextUnitAddStep;

    [NonSerialized]
    public int energyToPowerUpBase;
    [NonSerialized]
    public int energyToPowerUpMax;

    [NonSerialized]
    public float shotImpulse;

    [NonSerialized]
    public int baseHPOfRegularEnemy;

    [NonSerialized]
    public float baseSpeedOfEnemy;

    [NonSerialized]
    public int energyFromEnemyBase;
    [NonSerialized]
    public int energyFromRegularEnemyIncreaser;

    [NonSerialized]
    public int subLocation;
    [NonSerialized]
    public int location;
    [NonSerialized]
    public string subLocationString;
    [NonSerialized]
    public string locationString;

    [NonSerialized]
    public float leftEdgeofCastleTiles;
    [NonSerialized]
    public float rightEdgeofCastleTiles;

    [NonSerialized]
    public int towerHPReduceAmount;

    [NonSerialized]
    public Color redColor;
    [NonSerialized]
    public Color greenColor;

    
    public string getLoseText()
    {
        if (GameParams.language == 0) return "Defeat...";
        else if (GameParams.language == 1) return "Поражение...";
        else return "Derrota...";
    }
    public string getWinText()
    {
        if (GameParams.language == 0) return "Victory!";
        else if (GameParams.language == 1) return "Победа!";
        else return "Victoria!";
    }
    public string getTheyAreComingText()
    {
        if (GameParams.language == 0) return "They are coming...";
        else if (GameParams.language == 1) return "Они идут...";
        else return "Ellos estan viniendo...";
    }
    public string getAvailableManaText()
    {
        if (GameParams.language == 0) return "Amount of mana";
        else if (GameParams.language == 1) return "Количество маны";
        else return "Сantidad de maná";
    }
    public string getAddHeroesText()
    {
        if (GameParams.language == 0) return "Summon heroes!";
        else if (GameParams.language == 1) return "Призови героев!";
        else return "Invocar héroes!";
    }
    public string getAddTowerText()
    {
        if (GameParams.language == 0) return "Build towers!";
        else if (GameParams.language == 1) return "Построй башни!";
        else return "Сonstruir torres!";
    }
    public string getEvilIsCloseAddHeroesText()
    {
        if (GameParams.language == 0) return "Evil is near! Summon more heroes!";
        else if (GameParams.language == 1) return "Зло близко! Призови больше героев!";
        else return "El mal está cerca! Invoca a más héroes!";
    }
    public string getTimeTilNextWaveText()
    {
        if (GameParams.language == 0) return "Time until next wave";
        else if (GameParams.language == 1) return "Время до следующей волны";
        else return "Tiempo hasta la próxima ola";
    }
    public string getPowerUpText()
    {
        if (GameParams.language == 0) return "Strengthen the heroes!";
        else if (GameParams.language == 1) return "Усиль героев!";
        else return "Fortalece a los héroes!";
    }
    public string getSuperAttackText()
    {
        if (GameParams.language == 0) return "Super attack!";
        else if (GameParams.language == 1) return "Супер атака!";
        else return "Super ataque!";
    }
    public string getMergeText()
    {
        if (GameParams.language == 0) return "Merge the same heroes!";
        else if (GameParams.language == 1) return "Соедини одинаковых героев!";
        else return "Combina los mismos héroes!";
    }
    public string getTutorFinishTextText()
    {
        if (GameParams.language == 0) return "You are ready! Save the Kingdom!";
        else if (GameParams.language == 1) return "Вы готовы! Спасите Королевство!";
        else return "Estás listo! Salvar el Reino!";
    }



    #region levelParameters
    //index explanation: 0 - attack side counts (2 means attack will go from up and down simultaniously); 1-2-3-4 first-second-third-forth level enemies count 
    //keys are the wave counts
    [HideInInspector]
    public Dictionary<int, List<List<int>>> attackWavesForLocation00 = new Dictionary<int, List<List<int>>>
    {
        [0] = new List<List<int>> {
            new List<int> {1, 20, 10, 0, 0}, //1, 20, 10, 0, 0
            new List<int> { 2, 25, 11, 0, 0 }, // 2, 25, 11, 0, 0 
            new List<int> { 1, 20, 22, 0, 0 }, //1, 20, 22, 0, 0 
        },
        [1] = new List<List<int>>
        {
            new List<int>() { 2, 5, 25, 5, 0 }, //2, 18, 15, 5, 0
            new List<int>() { 1, 10, 25, 15, 0 }, //1, 20, 20, 10, 0
            new List<int>() { 2, 5, 10, 20, 0 }, //2, 10, 20, 4, 0
            new List<int>() { 1, 15, 10, 25, 0 }, //1, 35, 20, 13, 0
        },
        [2] = new List<List<int>>
        {
            new List<int>() { 2, 30, 20,10, 0 }, //2, 30, 25, 5, 0
            new List<int>() { 1, 10, 20, 30, 0 }, //1, 40, 15, 8, 0
            new List<int>() { 1, 7, 25, 20, 0 }, //1, 30, 25, 10, 0
            new List<int>() { 2, 7, 30, 15, 0 }, //2, 30, 30, 7, 0
            new List<int>() { 2, 3, 40, 50, 0 }, //2, 30, 40, 8, 0
        }
    };

    public Dictionary<int, List<List<int>>> attackWavesForLocation01 = new Dictionary<int, List<List<int>>>
    {
        [0] = new List<List<int>> {
            new List<int> {2, 10, 20, 0, 0},
            new List<int> { 1, 20, 20, 20, 0 },
            new List<int> { 1, 5, 30, 25, 0 },
            new List<int> { 2, 4, 25, 20, 0 },
        },
        [1] = new List<List<int>>
        {
            new List<int>() { 2, 20, 25, 25, 0 },
            new List<int>() { 2, 0, 20, 30, 0 },
            new List<int>() { 1, 10, 10, 40, 0 },
            new List<int>() { 2, 0, 25, 35, 0 },
        },
        [2] = new List<List<int>>
        {
            new List<int>() { 2, 5, 25, 25, 0 },
            new List<int>() { 1, 10, 15, 30, 0 },
            new List<int>() { 2, 0, 15, 40, 0 },
            new List<int>() { 2, 10, 20, 30, 0 },
            new List<int>() { 1, 0, 30, 30, 0 },
        },
        [3] = new List<List<int>>
        {
            new List<int>() { 2, 5, 25, 25, 0 },
            new List<int>() { 2, 0, 10, 40, 0 },
            new List<int>() { 2, 10, 20, 30, 0 },
            new List<int>() { 2, 0, 10, 50, 0 },
        }
        //[0] = new List<List<int>> {
        //    new List<int> {2, 10, 20, 0, 0}
        //},
        //[1] = new List<List<int>>
        //{
        //    new List<int>() { 2, 20, 25, 25, 0 }
        //},
        //[2] = new List<List<int>>
        //{
        //    new List<int>() { 2, 5, 25, 25, 0 }
        //},
        //[3] = new List<List<int>>
        //{
        //    new List<int>() { 2, 5, 25, 25, 0 }
        //}
    }; 
    
    public Dictionary<int, List<List<int>>> attackWavesForLocation02 = new Dictionary<int, List<List<int>>>
    {
        [0] = new List<List<int>> {
            new List<int> {2, 20, 20, 0, 0},
            new List<int> { 1, 25, 15, 20, 0 },
            new List<int> { 1, 5, 30, 25, 0 },
            new List<int> { 2, 4, 25, 20, 0 },
        },
        [1] = new List<List<int>>
        {
            new List<int>() { 2, 20, 25, 25, 0 },
            new List<int>() { 2, 7, 20, 30, 0 },
            new List<int>() { 1, 10, 10, 40, 0 },
            new List<int>() { 2, 0, 25, 35, 0 },
        },
        [2] = new List<List<int>>
        {
            new List<int>() { 2, 7, 25, 25, 0 },
            new List<int>() { 1, 10, 15, 30, 0 },
            new List<int>() { 2, 0, 15, 40, 0 },
            new List<int>() { 2, 10, 20, 30, 0 },
            new List<int>() { 1, 0, 30, 30, 0 },
        },
        [3] = new List<List<int>>
        {
            new List<int>() { 2, 5, 25, 25, 0 },
            new List<int>() { 2, 0, 10, 50, 0 },
            new List<int>() { 2, 10, 30, 60, 0 },
            new List<int>() { 2, 0, 10, 50, 0 },
        }
    };

    [HideInInspector]
    public Dictionary<Vector2, Dictionary<int, List<List<int>>>> locationWaves; //Vector2 key determined by location and sub location, for example 0 - location and 1 is sub locatio new Vector2 (0,1);

    #endregion levelParameters

    #region EnemyUnits
    [HideInInspector]
    public float[,] indexesForEnemy;
    [HideInInspector]
    public float[,] speedIndexesForEnemy; //speed index is matter to balance independently
    [HideInInspector]
    public int[,] regularEnemyHPBase;
    [HideInInspector]
    public int[,,,] regularEnemyHPForAllLocations;
    [HideInInspector]
    public float[] regularEnemySpeed;
    [HideInInspector]
    public int[] regularEnemyEnergy;

    [HideInInspector]
    public int energyMultiplierForMiniBoss;
    [HideInInspector]
    public int energyMultiplierForBigBoss;
    [HideInInspector]
    public int HPMultiplierForMiniBoss;
    [HideInInspector]
    public int HPMultiplierForBigBoss;

    [HideInInspector]
    public int energyMultiplyerBaseFromAttackWave;
    [HideInInspector]
    public float wavePauseTimeBase;

    //indexes to change pararmeters of enemy units
    private void populateEnemyLevelIndexes()
    {
        indexesForEnemy = new float[7, 4];
        int rows = indexesForEnemy.GetUpperBound(0) + 1;
        int colums = indexesForEnemy.Length / rows;
        float multiplierBase = 1.5f;
        float multiplierForColumn = multiplierBase;
        float multiplierForRow = multiplierBase;
        float multiplierDiscount = 1.02f;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < colums; j++)
            {
                if (i == 0 && j == 0) indexesForEnemy[i, j] = 1; //first element is base and equals to 1
                else
                {
                    if (i == 0)
                    {
                        indexesForEnemy[i, j] = indexesForEnemy[i, j - 1] * multiplierForColumn;
                        multiplierForColumn /= multiplierDiscount;
                    }
                    else
                    {
                        indexesForEnemy[i, j] = indexesForEnemy[i - 1, j] * multiplierForColumn;
                    }
                }
            }
            if (i != 0)
            {
                multiplierForRow /= multiplierDiscount;
                multiplierForColumn = multiplierForRow;
            }
            else multiplierForColumn = multiplierBase;

        }
    }

    //speed index is matter to balance independently
    private void populateEnemyLevelSpeedIndexes()
    {
        speedIndexesForEnemy = new float[7, 4];
        int rows = speedIndexesForEnemy.GetUpperBound(0) + 1;
        int colums = speedIndexesForEnemy.Length / rows;
        float multiplierBase = 1.3f;
        float multiplierForColumn = multiplierBase;
        float multiplierForRow = multiplierBase;
        float multiplierDiscount = 1.02f;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < colums; j++)
            {
                if (i == 0 && j == 0) speedIndexesForEnemy[i, j] = 1; //first element is base and equals to 1
                else
                {
                    if (i == 0)
                    {
                        speedIndexesForEnemy[i, j] = speedIndexesForEnemy[i, j - 1] * multiplierForColumn;
                        multiplierForColumn /= multiplierDiscount;
                    }
                    else
                    {
                        speedIndexesForEnemy[i, j] = speedIndexesForEnemy[i - 1, j] * multiplierForColumn;
                    }
                }
            }
            if (i != 0)
            {
                multiplierForRow /= multiplierDiscount;
                multiplierForColumn = multiplierForRow;
            }
            else multiplierForColumn = multiplierBase;

        }
    }

    private void populateRegularEnemyHPBase()
    {
        int baseHP = baseHPOfRegularEnemy;
        int HPIncreaserForLevel = 40;
        int HPIncreaserForWave = 30;

        regularEnemyHPBase = new int[7, 4];
        int rows = regularEnemyHPBase.GetUpperBound(0) + 1;
        int colums = regularEnemyHPBase.Length / rows;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < colums; j++)
            {
                if (i == 0 && j == 0) regularEnemyHPBase[i, j] = baseHP;
                else
                {
                    if (j == 0)
                    {
                        regularEnemyHPBase[i, j] = (int)(regularEnemyHPBase[i - 1, j] + HPIncreaserForWave * indexesForEnemy[i, j]);
                    }
                    else
                    {
                        regularEnemyHPBase[i, j] = (int)(regularEnemyHPBase[i , j - 1] + HPIncreaserForLevel * indexesForEnemy[i, j]);
                    }
                }
            }
        }
    }

    // index explamation: 0-location, 1 - subLocation, 2-waveCount, 3 - enemyLevel 
    private void populateAllLocationsRegularEnemyHP()
    {
        regularEnemyHPForAllLocations = new int[4, 4, 7, 4];
        float baseMultiplierIndexForLication = 1.11f;
        float multiplierForLocation = 1;
        for (int i = 0; i < regularEnemyHPForAllLocations.GetLength(0); i++)
        {
            for (int j = 0; j < regularEnemyHPForAllLocations.GetLength(1); j++)
            {
                for (int x = 0; x < regularEnemyHPForAllLocations.GetLength(2); x++)
                {
                    for (int y = 0; y < regularEnemyHPForAllLocations.GetLength(3); y++)
                    {
                        if (i == 0 && j == 0) regularEnemyHPForAllLocations[i, j, x, y] = regularEnemyHPBase[x,y];
                        else {
                            if(j==0) regularEnemyHPForAllLocations[i, j, x, y] = (int)(regularEnemyHPBase[x, y] * multiplierForLocation); 
                            else regularEnemyHPForAllLocations[i, j, x, y] = (int)(regularEnemyHPForAllLocations[i, 0, x, y] * multiplierForLocation);
                        }
                    }
                }
                multiplierForLocation *= baseMultiplierIndexForLication;
            }
            if (i == 0) multiplierForLocation = baseMultiplierIndexForLication; 
            else multiplierForLocation = Mathf.Pow(baseMultiplierIndexForLication, i + 1);
        }
    }
    private void populateSpeedOfEnemyUnits() {
        regularEnemySpeed = new float[4]; 
        for (int i=0;i< regularEnemySpeed.Length;i++) {
            if (i == 0) regularEnemySpeed[i] = baseSpeedOfEnemy;
            else {
                regularEnemySpeed[i] = baseSpeedOfEnemy / speedIndexesForEnemy[0, i];
            }
        }
    }
    private void populateEnergyFromEnemyUnits() {
        regularEnemyEnergy = new int[7];
        for (int i = 0; i < regularEnemyEnergy.Length; i++)
        {
            if (i == 0) regularEnemyEnergy[i] = energyFromEnemyBase;
            else
            {
                regularEnemyEnergy[i] = regularEnemyEnergy[i-1] + energyFromRegularEnemyIncreaser;
            }
        }
    }


    #endregion EnemyUnits

    #region PlayerUnit

    [HideInInspector]
    public float[,] indexesForPlayerUnits;
    private void populatePlayerUnitsTypesArray()
    {
        //for (int i = 0; i < playerUnitTypesOnScene.Length; i++)
        //{
        //    playerUnitTypesOnScene[i] = i; //TO DO with more than 5 types (for now there only 5 types). Also necessary to limit unittypes on scene with ones chosen by player 

        //}
        if (GameParams.subLocationStatic == 0)
        {
            playerUnitTypesOnScene[0] = 0;
            playerUnitTypesOnScene[1] = 27;
            playerUnitTypesOnScene[2] = 4;
            playerUnitTypesOnScene[3] = 2;
            playerUnitTypesOnScene[4] = 15;
        }
        else if (GameParams.subLocationStatic == 1)
        {
            playerUnitTypesOnScene[0] = 27;
            playerUnitTypesOnScene[1] = 22;
            playerUnitTypesOnScene[2] = 5;
            playerUnitTypesOnScene[3] = 1;
            playerUnitTypesOnScene[4] = 10;
        }
        else {
            playerUnitTypesOnScene[0] = 5;
            playerUnitTypesOnScene[1] = 2;
            playerUnitTypesOnScene[2] = 0;
            playerUnitTypesOnScene[3] = 10;
            playerUnitTypesOnScene[4] = 15;

        }

    }

    private void populatePlayerUnitsTypesStartPowerUpLevels()
    {
        for (int i = 0; i < playerUnitTypesOnScenePowerUpLevel.Length; i++) playerUnitTypesOnScenePowerUpLevel[i] = 0;

    }

    //indexes to change pararmeters of player units
    private void populatePlayerUnitIndexes()
    {
        indexesForPlayerUnits = new float[5, 5];
        int rows = indexesForPlayerUnits.GetUpperBound(0) + 1;
        int colums = indexesForPlayerUnits.Length / rows;
        float multiplierBase = 1.24f;
        float multiplierForColumn = multiplierBase;
        float multiplierForRow = multiplierBase;
        float multiplierDiscount = 1.02f;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < colums; j++)
            {
                if (i == 0 && j == 0) indexesForPlayerUnits[i, j] = 1; //first element is base and equals to 1
                else
                {
                    if (i == 0)
                    {
                        indexesForPlayerUnits[i, j] = indexesForPlayerUnits[i, j - 1] * multiplierForColumn;
                        multiplierForColumn /= multiplierDiscount;
                    }
                    else
                    {
                        indexesForPlayerUnits[i, j] = indexesForPlayerUnits[i - 1, j] * multiplierForColumn;
                    }
                }
            }
            if (i != 0)
            {
                multiplierForRow /= multiplierDiscount;
                multiplierForColumn = multiplierForRow;
            }
            else multiplierForColumn = multiplierBase;

        }
    }
    public int getHarmOfUnit(int mergeLevel, int powerUpCount, int baseHarm) { 
        return (int) (baseHarm* indexesForPlayerUnits[mergeLevel, powerUpCount]);
    }
    public float getSpeedOfShotOfUnit(int mergeLevel, int powerUpCount, float baseSpeedOfShot)
    {
        return baseSpeedOfShot / indexesForPlayerUnits[mergeLevel, powerUpCount];
    }
    public float getAccuracyOfUnit(int mergeLevel, int powerUpCount, float baseAccuracy)
    {
        return baseAccuracy / indexesForPlayerUnits[mergeLevel, powerUpCount];
    }
    public int getSuperHitHarm(int mergeLevel, int powerUpCount, int baseSuperHitHarm)
    {
        return (int) (baseSuperHitHarm * indexesForPlayerUnits[mergeLevel, powerUpCount]);
    }
    public float getSuperHitTime(int mergeLevel, int powerUpCount, float baseSuperHitTime)
    {
        return baseSuperHitTime * indexesForPlayerUnits[mergeLevel, powerUpCount];
    }


    #endregion PlayerUnit

    private void Awake()
    {

        redColor = new Color(1, 0.2f, 0.2f, 1);
        greenColor = new Color(0.37f, 1, 0.33f, 1);

        leftEdgeofCastleTiles = -10;
        rightEdgeofCastleTiles = 10;
        cameraOfGame = Camera.main;
        //determine the sizes of view screen
        vertScreenSize = cameraOfGame.orthographicSize * 2;
        horisScreenSize = vertScreenSize * Screen.width / Screen.height;

        towerHPReduceAmount = 700;

        location = GameParams.locationStatic;
        subLocation = GameParams.subLocationStatic; 
        subLocationString = subLocation.ToString();
        locationString = location.ToString();

        playerUnitMaxLevel = 5;

        HPOfTile = 5;
        wavePauseTimeBase = 12;
        energyMultiplyerBaseFromAttackWave = 7;

        // TO DO in case of success
        if (subLocation == 0) energyOnStart = 100;
        else if (subLocation == 1) energyOnStart = 2000;
        else if (subLocation == 2) energyOnStart = 4000;

        if (GameParams.isTutor) energyOnStart = 5000;

        energyToNextUnitAddStep = 11;

        shotImpulse = 50;

        baseHPOfRegularEnemy = 17;

        baseSpeedOfEnemy = 0.03f;

        energyFromEnemyBase = 9;
        energyFromRegularEnemyIncreaser = 10;

        energyToPowerUpBase = 100;
        energyToPowerUpMax = 800;

        energyMultiplierForMiniBoss =15;
        energyMultiplierForBigBoss=35;
        HPMultiplierForMiniBoss=10;
        HPMultiplierForBigBoss=20;

        playerUnitTypesOnScene = new int[5];
        playerUnitTypesOnScenePowerUpLevel = new int[5];

        locationWaves = new Dictionary<Vector2, Dictionary<int, List<List<int>>>>
        {
            [new Vector2 (0,0)] = attackWavesForLocation00,//0 - location, 0- sub location
            [new Vector2(0,1)] = attackWavesForLocation01, //0 - location, 0- sub location
            [new Vector2(0, 2)] = attackWavesForLocation02 //0 - location, 0- sub location
        };
        enemyUnitsAll = new List<EnemyUnit>();

        castleTiles = new List<CastleTiles>();
        //allPlatformTiles = new List<PlatformTile>();

        fireTiles = new List<CastleFire>();

        castlePointsWithNoUnits = new List<Vector2>();
        castlePointsUp = new List<Vector2>();
        castlePointsDown = new List<Vector2>();

        if (location == 0 && subLocation == 0)
        {
            platformTilesUp = new Dictionary<Vector2, int>
        {
            {new Vector2(0, 4),0},
            {new Vector2(-4, 4),0},
            {new Vector2(4, 4),0},
            {new Vector2(-8, 4),0},
            {new Vector2(8, 4),0},

            {new Vector2(0, 8),0},
            {new Vector2(-4, 8),0},
            {new Vector2(4, 8),0},
            {new Vector2(-8, 8),0},
            {new Vector2(8, 8),0},
        };
            platformTilesDown = new Dictionary<Vector2, int>
        {
            {new Vector2(0, -4f),0},
            {new Vector2(-4, -4f),0},
            {new Vector2(4, -4f),0},
            {new Vector2(-8, -4f),0},
            {new Vector2(8, -4f),0},

            {new Vector2(0, -8),0},
            {new Vector2(-4, -8),0},
            {new Vector2(4, -8),0},
            {new Vector2(-8, -8),0},
            {new Vector2(8, -8),0},
        };

        }
        else if (location == 0 && subLocation == 1)
        {
            platformTilesUp = new Dictionary<Vector2, int>
        {
            {new Vector2(0, 4),0},
            {new Vector2(-4, 4),0},
            {new Vector2(4, 4),0},
            {new Vector2(-8, 4),0},
            {new Vector2(8, 4),0},

            {new Vector2(0, 8),0},

            {new Vector2(-4, 12),0},
            {new Vector2(4, 12),0},

            {new Vector2(-8, 8),0},
            {new Vector2(8, 8),0},
        };
            platformTilesDown = new Dictionary<Vector2, int>
        {
            {new Vector2(0, -4f),0},
            {new Vector2(-4, -4f),0},
            {new Vector2(4, -4f),0},
            {new Vector2(-8, -4f),0},
            {new Vector2(8, -4f),0},

            {new Vector2(0, -8),0},

            {new Vector2(-4, -12),0},
            {new Vector2(4, -12),0},

            {new Vector2(-8, -8),0},
            {new Vector2(8, -8),0},
        };
        }
        else if (location == 0 && subLocation == 2)
        {
            platformTilesUp = new Dictionary<Vector2, int>
        {
            {new Vector2(0, 4),0},
            {new Vector2(-4, 4),0},
            {new Vector2(4, 4),0},
            {new Vector2(-8, 4),0},
            {new Vector2(8, 4),0},

            {new Vector2(0, 12),0},

            {new Vector2(-8, 12),0},
            {new Vector2(8, 12),0},

            {new Vector2(-8, 8),0},
            {new Vector2(8, 8),0},
        };
            platformTilesDown = new Dictionary<Vector2, int>
        {
            {new Vector2(0, -4f),0},
            {new Vector2(-4, -4f),0},
            {new Vector2(4, -4f),0},
            {new Vector2(-8, -4f),0},
            {new Vector2(8, -4f),0},

            {new Vector2(0, -12),0},

            {new Vector2(-8, -12),0},
            {new Vector2(8, -12),0},

            {new Vector2(-8, -8),0},
            {new Vector2(8, -8),0},
        };
        }

        if (GameParams.isTutor)
        {
            platformTilesUp = new Dictionary<Vector2, int>
        {
            {new Vector2(0, 4),0},
            {new Vector2(-4, 4),0},
            {new Vector2(4, 4),0},
            {new Vector2(-8, 4),0},
            {new Vector2(8, 4),0}
        };
            platformTilesDown = new Dictionary<Vector2, int>
        {
            {new Vector2(0, -4f),0},
            {new Vector2(-4, -4f),0},
            {new Vector2(4, -4f),0},
            {new Vector2(-8, -4f),0},
            {new Vector2(8, -4f),0}
        };
        }

            enemyUnits = new Dictionary<int, List<EnemyUnit>>
        {
            [0] = new List<EnemyUnit>(),
            [1] = new List<EnemyUnit>()
        };
        playerUnits = new List<PlayerUnit>();


        populatePlayerUnitsTypesStartPowerUpLevels();
        populatePlayerUnitsTypesArray();
        populateEnemyLevelIndexes();
        populateEnemyLevelSpeedIndexes();
        populateRegularEnemyHPBase();
        populateAllLocationsRegularEnemyHP(); 
        populateSpeedOfEnemyUnits();
        populateEnergyFromEnemyUnits();

        populatePlayerUnitIndexes();

    }

}
