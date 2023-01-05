using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPuller : MonoBehaviour
{
    public static ObjectPuller current;


    private int pullOfObjects6 = 6;
    private int pullOfObjects10 = 10;
    private int pullOfObjects15 = 15;
    private int pullOfObjects20 = 20;
    private bool willGrow;

    [SerializeField]
    private GameObject enemyUnits;

    [SerializeField]
    private GameObject playerUnit1;
    [SerializeField]
    private GameObject playerUnit2;
    [SerializeField]
    private GameObject playerUnit3;

    [SerializeField]
    private GameObject playerShot1;
    [SerializeField]
    private GameObject playerShot2;
    [SerializeField]
    private GameObject playerShot3;

    [HideInInspector]
    public List<GameObject> enemyUnitsPull;

    [HideInInspector]
    public List<GameObject> playerUnit1Pull;
    [HideInInspector]
    public List<GameObject> playerUnit2Pull;
    [HideInInspector]
    public List<GameObject> playerUnit3Pull;

    [HideInInspector]
    public List<GameObject> playerShot1Pull;
    [HideInInspector]
    public List<GameObject> playerShot2Pull;
    [HideInInspector]
    public List<GameObject> playerShot3Pull;

    private void Awake()
    {
        willGrow = true;
        current = this;
    }

    private void OnEnable()
    {
        enemyUnitsPull = new List<GameObject>();

        playerUnit1Pull = new List<GameObject>();
        playerUnit2Pull = new List<GameObject>();
        playerUnit3Pull = new List<GameObject>();

        playerShot1Pull = new List<GameObject>();
        playerShot2Pull = new List<GameObject>();
        playerShot3Pull = new List<GameObject>();

        for (int i = 0; i < pullOfObjects6; i++)
        {
            GameObject obj = Instantiate(playerUnit1);
            obj.SetActive(false);
            playerUnit1Pull.Add(obj);

            GameObject obj1 = Instantiate(playerUnit2);
            obj1.SetActive(false);
            playerUnit2Pull.Add(obj1);

            GameObject obj2 = Instantiate(playerUnit3);
            obj2.SetActive(false);
            playerUnit3Pull.Add(obj2);

        }

        for (int i = 0; i < pullOfObjects10; i++)
        {
            GameObject obj = Instantiate(playerShot1);
            obj.SetActive(false);
            playerShot1Pull.Add(obj);

            GameObject obj1 = Instantiate(playerShot2);
            obj1.SetActive(false);
            playerShot2Pull.Add(obj1);

            GameObject obj2 = Instantiate(playerShot3);
            obj2.SetActive(false);
            playerShot3Pull.Add(obj2);

        }

        for (int i = 0; i < pullOfObjects20; i++)
        {
            GameObject obj1 = Instantiate(enemyUnits);
            obj1.SetActive(false);
            enemyUnitsPull.Add(obj1);

        }
    }
    public List<GameObject> GetPlayerShotPullList(int shotType)
    {
        if (shotType == 0) return playerShot1Pull;
        else if (shotType == 1) return playerShot2Pull;
        else return playerShot3Pull;
    }

    public List<GameObject> GetEnemyUnitsPullList()
    {
        return enemyUnitsPull;
    }
    public List<GameObject> GetPlayerUnitsPullList(int unitType)
    {
        if (unitType == 1) return playerUnit1Pull;
        else if (unitType == 2) return playerUnit2Pull;
        else return playerUnit3Pull;
    }

    //universal method to set active proper game object from the list of GOs, it just needs to get correct List of game objects
    public GameObject GetGameObjectFromPull(List<GameObject> GOLists)
    {
        for (int i = 0; i < GOLists.Count; i++)
        {
            if (!GOLists[i].activeInHierarchy) return GOLists[i];
        }
        if (willGrow)
        {
            GameObject obj = Instantiate(GOLists[0]);
            obj.SetActive(false);
            GOLists.Add(obj);
            return obj;
        }
        return null;
    }

}
