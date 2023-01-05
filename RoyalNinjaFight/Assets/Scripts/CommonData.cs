
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.U2D;

public class CommonData : MonoBehaviour
{

    public static CommonData instance { get; private set; }

    public CommonData() {
        instance = this;
    }

    public SpriteAtlas enemyAtlas;

    [HideInInspector]
    public Camera cameraOfGame;
    [HideInInspector]
    public float vertScreenSize;
    [HideInInspector]
    public float horisScreenSize;

    [HideInInspector]
    public Vector2 shotDirection;

    [HideInInspector]
    public bool gameIsOn;

    [HideInInspector]
    public Dictionary<int, List<EnemyUnit>> enemyUnits;

    [HideInInspector]
    public List<PlayerUnit> playerUnits;

    [HideInInspector]
    public List<CastleTiles>  castleTiles;

    [HideInInspector]
    public List<Vector2> platformPoints;

    [HideInInspector]
    public List<Vector2> platformPointsWithNoUnits;

    [HideInInspector]
    public int[] playerUnitTypesOnScene;

    [HideInInspector]
    public int playerUnitMaxLevel;

    [HideInInspector]
    public int HPOfTile;

    [HideInInspector]
    public int energy;

    [HideInInspector]
    public int energyOnStart;
    [HideInInspector]
    public int energyToNextUnitAddStep;

    [HideInInspector]
    public float shotImpulse;

    [HideInInspector]
    public int baseHPOfRegularEnemy;

    [HideInInspector]
    public float baseSpeedOfEnemy;

    [HideInInspector]
    public int energyFromEnemyBase;
    [HideInInspector]
    public int energyFromRegularEnemyIncreaser;

    [HideInInspector]
    public int subLocation;
    [HideInInspector]
    public int location;


    //index explanation: 0 - attack side counts (2 means attack will go from up and down simultaniously); 1-2-3 first-second-third level enemies count 
    [HideInInspector]
    public Dictionary<int, List<int>> attackWavesForLevel1 = new Dictionary<int, List<int>>
    {
        [0] = new List<int>() { 1, 20, 10, 0 },
        [1] = new List<int>() { 2, 25, 11, 0 },
        [2] = new List<int>() { 2, 9, 18, 0 },
        [3] = new List<int>() { 2, 18, 15, 0 },
        [4] = new List<int>() { 1, 11, 20, 0 },
        [5] = new List<int>() { 2, 20, 20, 0 },
        [6] = new List<int>() { 2, 35, 20, 0 },
        [7] = new List<int>() { 2, 30, 25, 0 },
    };

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
    public float[,] indexesForPlayerUnits;


    //indexes to change pararmeters of enemy units
    private void populateEnemyLevelIndexes()
    {
        indexesForEnemy = new float[7, 4];
        int rows = indexesForEnemy.GetUpperBound(0) + 1;
        int colums = indexesForEnemy.Length / rows;
        float multiplierBase = 1.15f;
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

    private void populateBaseregularEnemyHPBase()
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

    private void populateSpeedOfEnemyUNits() {
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

    //indexes to change pararmeters of player units
    private void populatePlayerUnitIndexes()
    {
        indexesForPlayerUnits = new float[5, 5];
        int rows = indexesForPlayerUnits.GetUpperBound(0) + 1;
        int colums = indexesForPlayerUnits.Length / rows;
        float multiplierBase = 1.3f;
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


    private void Awake()
    {
        subLocation = 0;
        location = 0;
        playerUnitMaxLevel = 5;

        HPOfTile = 3;

        energyOnStart = 100;

        energyToNextUnitAddStep = 10;

        shotImpulse = 60;

        baseHPOfRegularEnemy = 15;

        baseSpeedOfEnemy = 0.02f;

        energyFromEnemyBase = 10;
        energyFromRegularEnemyIncreaser = 10;

        playerUnitTypesOnScene = new int[3];
        platformPointsWithNoUnits = new List<Vector2>();
        platformPoints = new List<Vector2>();
        castleTiles = new List<CastleTiles>();
        enemyUnits = new Dictionary<int, List<EnemyUnit>>
        {
            [0] = new List<EnemyUnit>(),
            [1] = new List<EnemyUnit>()
        };
        playerUnits = new List<PlayerUnit>();

        cameraOfGame = Camera.main;
        //determine the sizes of view screen
        vertScreenSize = cameraOfGame.orthographicSize * 2;
        horisScreenSize = vertScreenSize * Screen.width / Screen.height;



        populateEnemyLevelIndexes();
        populateBaseregularEnemyHPBase();
        populateAllLocationsRegularEnemyHP(); 
        populateSpeedOfEnemyUNits();
        populateEnergyFromEnemyUnits();

        populatePlayerUnitIndexes();

    }

}
