using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class CommonData : MonoBehaviour
{
    public static CommonData instance { get; private set;}

    public CommonData() {
        instance = this;
    }

    [HideInInspector]
    public Camera cameraOfGame;
    [HideInInspector]
    public float vertScreenSize;
    [HideInInspector]
    public float horisScreenSize;

    public Vector2 shotDirection;

    public bool gameIsOn;

    public List<EnemyUnit> enemyUnits;
    public List<PlayerUnit> playerUnits;
    public List<Vector2> platformPoints;
    public List<Vector2> platformPointsWithNoUnits;

    public int playerUnitMaxLevel = 4;

    public int HPOfTile = 3;

    public int HPOfEnemy1 = 12;

    public int energyFromEnemy1 = 10;

    public int energy;

    public int energyOnStart=100;
    public int energyToNextUnitAddStep = 10;

    public int[] playerUnitTypesOnScene;

    public int harmOfUnit1 = 8;
    public int harmOfUnit2 = 14;
    public int harmOfUnit3 = 20;

    public float attackSpeed1 = 1.1f;
    public float attackSpeed2 = 1.7f;
    public float attackSpeed3 = 2.5f;
 
    public float attackSpeedIncreaseStep1 = 0.1f;
    public float attackSpeedIncreaseStep2 = 0.08f;
    public float attackSpeedIncreaseStep3 = 0.15f;

    public int attackHarmIncreaseStep1 = 2;
    public int attackHarmIncreaseStep2 = 3;
    public int attackHarmIncreaseStep3 = 5;

    public float shotImpulse = 60;



    public float speedOfEnemy1 = 0.008f;


    private void Awake()
    {
        playerUnitTypesOnScene = new int[3];
        platformPointsWithNoUnits = new List<Vector2>();
        platformPoints = new List<Vector2>();
        enemyUnits = new List<EnemyUnit>();
        playerUnits = new List<PlayerUnit>();

        cameraOfGame = Camera.main;
        //determine the sizes of view screen
        vertScreenSize = cameraOfGame.orthographicSize * 2;
        horisScreenSize = vertScreenSize * Screen.width / Screen.height;
    }

}
