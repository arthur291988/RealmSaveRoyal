using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPuller : MonoBehaviour
{
    public static ObjectPuller current;

    //private int pullOfObjects2 = 2;
    //private int pullOfObjects3 = 3;
    private int pullOfObjects20 = 20;
    private bool willGrow;

    [SerializeField]
    private GameObject enemyUnits;
    [SerializeField]
    private GameObject playerUnits;
    [SerializeField]
    private GameObject enemySuriken;
    [SerializeField]
    private GameObject playerSuriken;

    [HideInInspector]
    public List<GameObject> enemyUnitsPull;
    [HideInInspector]
    public List<GameObject> playerUnitsPull;
    [HideInInspector]
    public List<GameObject> enemySurikenPull;
    [HideInInspector]
    public List<GameObject> playerSurikenPull;

    private void Awake()
    {
        willGrow = true;
        current = this;
    }

    private void OnEnable()
    {
        enemyUnitsPull = new List<GameObject>();
        playerUnitsPull = new List<GameObject>();
        playerSurikenPull = new List<GameObject>();
        enemySurikenPull = new List<GameObject>();

        
        for (int i = 0; i < pullOfObjects20; i++)
        {
            GameObject obj = (GameObject)Instantiate(playerUnits);
            obj.SetActive(false);
            playerUnitsPull.Add(obj);

            GameObject obj1 = (GameObject)Instantiate(enemyUnits);
            obj1.SetActive(false);
            enemyUnitsPull.Add(obj1);

            GameObject obj2 = (GameObject)Instantiate(enemySuriken);
            obj2.SetActive(false);
            enemySurikenPull.Add(obj2);

            GameObject obj3 = (GameObject)Instantiate(playerSuriken);
            obj3.SetActive(false);
            playerSurikenPull.Add(obj3);
        }
    }
    public List<GameObject> GetPlayerSurikenPullList()
    {
        return playerSurikenPull;
    }
    public List<GameObject> GetEnemySurikenPullList()
    {
        return enemySurikenPull;
    }
    public List<GameObject> GetEnemyUnitsPullList()
    {
        return enemyUnitsPull;
    }
    public List<GameObject> GetPlayerUnitsPullList()
    {
        return playerUnitsPull;
    }

    //universal method to set active proper game object from the list of GOs, it just needs to get correct List of game objects
    public GameObject GetGameObjectFromPull(List<GameObject> GOLists)
    {
        for (int i = 0; i < GOLists.Count; i++)
        {
            if (!GOLists[i].activeInHierarchy) return (GameObject)GOLists[i];
        }
        if (willGrow)
        {
            GameObject obj = (GameObject)Instantiate(GOLists[0]);
            obj.SetActive(false);
            GOLists.Add(obj);
            return obj;
        }
        return null;
    }

}
