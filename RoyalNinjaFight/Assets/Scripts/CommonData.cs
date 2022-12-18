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

    public int HPOfTile = 3;

    public int energyOfEnemy1 = 10;

    public int energy;

    public int energyOnStart=100;
    public int energyToNextUnitAddStep = 10;

    public float speedOfEnemy1 = 0.008f;


    private void Awake()
    {
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
