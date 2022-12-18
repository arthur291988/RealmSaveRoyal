using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class CommonData : MonoBehaviour
{
    [HideInInspector]
    public static Camera cameraOfGame;
    [HideInInspector]
    public static float vertScreenSize;
    [HideInInspector]
    public static float horisScreenSize;

    public static Vector2 shotDirection;

    public static bool gameIsOn;

    public static List<EnemyUnit> enemyUnits;
    public static List<PlayerUnit> playerUnits;
    public static List<Vector2> platformPoints;
    public static List<Vector2> platformPointsWithNoUnits;

    public static int HPOfTile = 3;

    public static int energyOfEnemy1 = 10;

    public static int energy;

    public static int energyOnStart=100;
    public static int energyToNextUnitAddStep = 10;

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
