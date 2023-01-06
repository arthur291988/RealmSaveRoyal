﻿
using System.Collections.Generic;
using UnityEngine;

public class ObjectPuller : MonoBehaviour
{
    public static ObjectPuller current;


    private int pullOfObjects2 = 2;
    private int pullOfObjects6 = 6;
    private int pullOfObjects10 = 10;
    private int pullOfObjects15 = 15;
    private int pullOfObjects20 = 20;
    private int pullOfObjects30 = 30;
    private bool willGrow;


    [SerializeField]
    private GameObject castleTile;

    [SerializeField]
    private GameObject enemyUnits;
    [SerializeField]
    private GameObject miniBossUnits;
    [SerializeField]
    private GameObject bigBossUnits;

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
    public List<GameObject> castleTilePull;

    [HideInInspector]
    public List<GameObject> enemyUnitsPull;
    [HideInInspector]
    public List<GameObject> miniBossUnitsPull;
    [HideInInspector]
    public List<GameObject> bigBossUnitsPull;

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
        castleTilePull = new List<GameObject>();

        enemyUnitsPull = new List<GameObject>();
        miniBossUnitsPull = new List<GameObject>();
        bigBossUnitsPull = new List<GameObject>();

        playerUnit1Pull = new List<GameObject>();
        playerUnit2Pull = new List<GameObject>();
        playerUnit3Pull = new List<GameObject>();

        playerShot1Pull = new List<GameObject>();
        playerShot2Pull = new List<GameObject>();
        playerShot3Pull = new List<GameObject>();

        for (int i = 0; i < pullOfObjects2; i++)
        {
            GameObject obj1 = Instantiate(miniBossUnits);
            obj1.SetActive(false);
            miniBossUnitsPull.Add(obj1);

            GameObject obj2 = Instantiate(bigBossUnits);
            obj2.SetActive(false);
            bigBossUnitsPull.Add(obj2);
        }

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
        
        for (int i = 0; i < pullOfObjects30; i++)
        {
            GameObject obj1 = Instantiate(castleTile);
            obj1.SetActive(false);
            castleTilePull.Add(obj1);
        }
    }
    public List<GameObject> GetPlayerShotPullList(int shotType)
    {
        if (shotType == 0) return playerShot1Pull;
        else if (shotType == 1) return playerShot2Pull;
        else return playerShot3Pull;
    }

    //0-regularUnit, //1-miniBossUnit //2-bigBossUnit
    public List<GameObject> GetEnemyUnitsPullList(int enemyIndex)
    {
        if (enemyIndex == 0) return enemyUnitsPull;
        else if (enemyIndex == 1) return miniBossUnitsPull;
        else return bigBossUnitsPull; 
    }
    public List<GameObject> GetPlayerUnitsPullList(int unitType)
    {
        if (unitType == 0) return playerUnit1Pull;
        else if (unitType == 1) return playerUnit2Pull;
        else return playerUnit3Pull;
    }
    public List<GameObject> GetCastleTilePullList()
    {
        return castleTilePull;
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
