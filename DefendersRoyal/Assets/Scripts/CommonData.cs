
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CommonData : MonoBehaviour
{

    public static CommonData instance { get; private set; }

    public CommonData() {
        instance = this;
    }

    public SpriteAtlas enemyAtlas;
    public SpriteAtlas playerSpriteAtlases;
    public SpriteAtlas playerRangeSpriteAtlases;
    public SpriteAtlas castleTileSpriteAtlases;

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
    public Dictionary<int, List<EnemyUnit>> enemyUnits;

    [NonSerialized]
    public List<PlayerUnit> playerUnits;

    [NonSerialized]
    public List<CastleTiles> castleTiles;

    //value shows if tile is empty 0-empty 1 - not Empty 
    [NonSerialized]
    public Dictionary<Vector2, int> platformTilesUp;

    //value shows if tile is empty 0-empty 1 - not Empty 
    [NonSerialized]
    public Dictionary<Vector2, int> platformTilesDown;

    [NonSerialized]
    public List<Vector2> castlePointsUp;
    [NonSerialized]
    public List<Vector2> castlePointsDown;

    [NonSerialized]
    public List<Vector2> castlePointsWithNoUnits;

    [NonSerialized]
    public List<Vector2> insideCastlePointsForEnemyUnits;

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
    public float leftEdgeofCastleTiles;
    [NonSerialized]
    public float rightEdgeofCastleTiles;

    [NonSerialized]
    public int towerHPReduceAmount;

    [NonSerialized]
    public Color redColor;
    [NonSerialized]
    public Color greenColor;

    [NonSerialized]
    public bool isEnglish = true;
    
    public string getLoseText()
    {
        if (isEnglish) return "Defeat...";
        else return "Поражение...";
    }
    public string getWinText()
    {
        if (isEnglish) return "Victory!";
        else return "Победа!";
    }

    #region levelParameters
    //index explanation: 0 - attack side counts (2 means attack will go from up and down simultaniously); 1-2-3-4 first-second-third-forth level enemies count 
    //keys are the wave counts
    [HideInInspector]
    public Dictionary<int, List<List<int>>> attackWavesForLocation00 = new Dictionary<int, List<List<int>>>
    {
        [0] = new List<List<int>> {
            new List<int> {1, 20, 10, 0, 0},
            new List<int> { 2, 25, 11, 0, 0 },
            new List<int> { 1, 20, 22, 0, 0 },
        },
        [1] = new List<List<int>>
        {
            new List<int>() { 2, 18, 15, 5, 0 },
            new List<int>() { 1, 20, 20, 10, 0 },
            new List<int>() { 2, 10, 20, 4, 0 },
            new List<int>() { 1, 35, 20, 13, 0 },
        },
        [2] = new List<List<int>>
        {
            new List<int>() { 2, 30, 25, 5, 0 },
            new List<int>() { 1, 40, 15, 8, 0 },
            new List<int>() { 1, 30, 25, 10, 0 },
            new List<int>() { 2, 30, 30, 7, 0 },
            new List<int>() { 2, 30, 40, 8, 0 },
        }
    };

    [HideInInspector]
    public Dictionary<Vector2, Dictionary<int, List<List<int>>>> locationWaves; //Vector2 key determined by location and sub location, for example 0 - location and 1 is sub locatio new Vector2 (0,1);

    #endregion levelParameters

    #region EnemyUnits
    [HideInInspector]
    public float[,] indexesForEnemy ;
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
                regularEnemySpeed[i] = baseSpeedOfEnemy / indexesForEnemy[0, i];
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
        playerUnitTypesOnScene[0] = 0;
        playerUnitTypesOnScene[1] = 27;
        playerUnitTypesOnScene[2] = 15;
        playerUnitTypesOnScene[3] = 22;
        playerUnitTypesOnScene[4] = 10;

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

        towerHPReduceAmount = 1000;

        subLocation = 0;
        location = 0;
        playerUnitMaxLevel = 5;

        HPOfTile = 5;

        energyOnStart = 100;

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
            [new Vector2 (0,0)] = attackWavesForLocation00 //0 - location, 0- sub location
        };
        enemyUnitsAll = new List<EnemyUnit>();

        castleTiles = new List<CastleTiles>();

        castlePointsWithNoUnits = new List<Vector2>();
        castlePointsUp = new List<Vector2>();
        castlePointsDown = new List<Vector2>();

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

            //{new Vector2(0, 10.5f),0},
            //{new Vector2(-3.5f, 10.5f),0},
            //{new Vector2(3.5f, 10.5f),0},
            //{new Vector2(-7, 10.5f),0},
            //{new Vector2(7, 10.5f),0},
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

            //{new Vector2(0, -10.5f),0},
            //{new Vector2(-3.5f, -10.5f),0},
            //{new Vector2(3.5f, -10.5f),0},
            //{new Vector2(-7, -10.5f),0},
            //{new Vector2(7, -10.5f),0},
        };
        insideCastlePointsForEnemyUnits = new List<Vector2>{
            new Vector2(0, 0),
            new Vector2(-3.5f, 0),
            new Vector2(3.5f, 0),
            new Vector2(-7, 0),
            new Vector2(7, 0)
        }; 

        enemyUnits = new Dictionary<int, List<EnemyUnit>>
        {
            [0] = new List<EnemyUnit>(),
            [1] = new List<EnemyUnit>()
        };
        playerUnits = new List<PlayerUnit>();

        
        populatePlayerUnitsTypesStartPowerUpLevels();
        populatePlayerUnitsTypesArray();
        populateEnemyLevelIndexes();
        populateRegularEnemyHPBase();
        populateAllLocationsRegularEnemyHP(); 
        populateSpeedOfEnemyUnits();
        populateEnergyFromEnemyUnits();

        populatePlayerUnitIndexes();

    }

}
