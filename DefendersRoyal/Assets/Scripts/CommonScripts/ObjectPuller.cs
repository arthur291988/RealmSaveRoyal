
using System;
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
    private int pullOfObjects60 = 60;
    private bool willGrow;


    [SerializeField]
    private GameObject castleTile;
    [SerializeField]
    private GameObject platformTile;

    [SerializeField]
    private GameObject enemyUnits;
    [SerializeField]
    private GameObject miniBossUnits;
    [SerializeField]
    private GameObject bigBossUnits;

    [SerializeField]
    private GameObject playerUnit0;
    [SerializeField]
    private GameObject playerUnit1;
    [SerializeField]
    private GameObject playerUnit2;
    [SerializeField]
    private GameObject playerUnit3;
    [SerializeField]
    private GameObject playerUnit4;
    [SerializeField]
    private GameObject playerUnit5;
    [SerializeField]
    private GameObject playerUnit10;
    [SerializeField]
    private GameObject playerUnit15;
    [SerializeField]
    private GameObject playerUnit22;
    [SerializeField]
    private GameObject playerUnit27;


    //[SerializeField]
    //private GameObject playerShot0;
    //[SerializeField]
    //private GameObject playerShot1;
    //[SerializeField]
    //private GameObject playerShot2;
    //[SerializeField]
    //private GameObject playerShot4;
    //[SerializeField]
    //private GameObject playerShot5;

    [SerializeField]
    private List<GameObject> playerShotsList;

    //[SerializeField]
    //private GameObject peakSuperShot;
    [SerializeField]
    private List<GameObject> superHitEffects;

    [NonSerialized]
    public List<GameObject> castleTilePull;
    [NonSerialized]
    public List<GameObject> PlatformTilePull;

    [NonSerialized]
    public List<GameObject> enemyUnitsPull;
    [NonSerialized]
    public List<GameObject> miniBossUnitsPull;
    [NonSerialized]
    public List<GameObject> bigBossUnitsPull;

    [NonSerialized]
    public List<GameObject> playerUnit0Pull;
    [NonSerialized]
    public List<GameObject> playerUnit1Pull;
    [NonSerialized]
    public List<GameObject> playerUnit2Pull;
    [NonSerialized]
    public List<GameObject> playerUnit3Pull;
    [NonSerialized]
    public List<GameObject> playerUnit4Pull;
    [NonSerialized]
    public List<GameObject> playerUnit5Pull;
    [NonSerialized]
    public List<GameObject> playerUnit10Pull;
    [NonSerialized]
    public List<GameObject> playerUnit15Pull;
    [NonSerialized]
    public List<GameObject> playerUnit22Pull;
    [NonSerialized]
    public List<GameObject> playerUnit27Pull;

    [NonSerialized]
    private List<List<GameObject>> playerShotsPull;

    //[NonSerialized]
    //public List<GameObject> playerShot0Pull;
    //[NonSerialized]
    //public List<GameObject> playerShot1Pull;
    //[NonSerialized]
    //public List<GameObject> playerShot2Pull;
    //[NonSerialized]
    //public List<GameObject> playerShot4Pull;
    //[NonSerialized]
    //public List<GameObject> playerShot5Pull;

    //[NonSerialized]
    ////public List<GameObject> peakSuperShotPull;

    [NonSerialized]
    public List<List<GameObject>> superHitEffectsPull;

    private void Awake()
    {
        willGrow = true;
        current = this;
    }

    private void OnEnable()
    {
        castleTilePull = new List<GameObject>();
        PlatformTilePull = new List<GameObject>();

        enemyUnitsPull = new List<GameObject>();
        miniBossUnitsPull = new List<GameObject>();
        bigBossUnitsPull = new List<GameObject>();

        playerUnit0Pull = new List<GameObject>();
        playerUnit1Pull = new List<GameObject>();
        playerUnit2Pull = new List<GameObject>();
        playerUnit3Pull = new List<GameObject>();
        playerUnit4Pull = new List<GameObject>();
        playerUnit5Pull = new List<GameObject>();
        playerUnit10Pull = new List<GameObject>();
        playerUnit15Pull = new List<GameObject>();
        playerUnit22Pull = new List<GameObject>();
        playerUnit27Pull = new List<GameObject>();

        playerShotsPull = new List<List<GameObject>>();

        //playerShot0Pull = new List<GameObject>();
        //playerShot1Pull = new List<GameObject>();
        //playerShot2Pull = new List<GameObject>();
        //playerShot4Pull = new List<GameObject>();
        //playerShot5Pull = new List<GameObject>();

        //peakSuperShotPull = new List<GameObject>();

        superHitEffectsPull = new List<List<GameObject>>();

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
            GameObject obj = Instantiate(playerUnit0);
            obj.SetActive(false);
            playerUnit0Pull.Add(obj);

            GameObject obj1 = Instantiate(playerUnit1);
            obj1.SetActive(false);
            playerUnit1Pull.Add(obj1);

            GameObject obj2 = Instantiate(playerUnit2);
            obj2.SetActive(false);
            playerUnit2Pull.Add(obj2);

            GameObject obj9 = Instantiate(playerUnit3);
            obj9.SetActive(false);
            playerUnit3Pull.Add(obj9);

            GameObject obj3 = Instantiate(playerUnit4);
            obj3.SetActive(false);
            playerUnit4Pull.Add(obj3);

            GameObject obj4 = Instantiate(playerUnit5);
            obj4.SetActive(false);
            playerUnit5Pull.Add(obj4);

            GameObject obj5 = Instantiate(playerUnit10);
            obj5.SetActive(false);
            playerUnit10Pull.Add(obj5);

            GameObject obj6 = Instantiate(playerUnit15);
            obj6.SetActive(false);
            playerUnit15Pull.Add(obj6);

            GameObject obj7 = Instantiate(playerUnit22);
            obj7.SetActive(false);
            playerUnit22Pull.Add(obj7);

            GameObject obj8 = Instantiate(playerUnit27);
            obj8.SetActive(false);
            playerUnit27Pull.Add(obj8);
        }

        //for (int i = 0; i < pullOfObjects10; i++)
        //{
        //    GameObject obj = Instantiate(playerShot0);
        //    obj.SetActive(false);
        //    playerShot0Pull.Add(obj);

        //    GameObject obj2 = Instantiate(playerShot1);
        //    obj2.SetActive(false);
        //    playerShot1Pull.Add(obj2);

        //    GameObject obj1 = Instantiate(playerShot2);
        //    obj1.SetActive(false);
        //    playerShot2Pull.Add(obj1);

        //    GameObject obj3 = Instantiate(playerShot4);
        //    obj3.SetActive(false);
        //    playerShot4Pull.Add(obj3);
        //    GameObject obj4 = Instantiate(playerShot5);
        //    obj4.SetActive(false);
        //    playerShot5Pull.Add(obj4);
        //}

        //special loop for pulling the super effects
        for (int i = 0; i < playerShotsList.Count; i++)
        {
            playerShotsPull.Add(new List<GameObject>());
            for (int y = 0; y < pullOfObjects10; y++)
            {
                GameObject obj = Instantiate(playerShotsList[i]);
                obj.SetActive(false);
                playerShotsPull[i].Add(obj);
            }
        }

        //special loop for pulling the super effects
        for (int i = 0; i < superHitEffects.Count; i++)
        {
            superHitEffectsPull.Add(new List<GameObject>());
            for (int y = 0; y < pullOfObjects10; y++)
            {
                GameObject obj = Instantiate(superHitEffects[i]);
                obj.SetActive(false);
                superHitEffectsPull[i].Add(obj);
            }
        }

        for (int i = 0; i < pullOfObjects20; i++)
        {
            GameObject obj1 = Instantiate(enemyUnits);
            obj1.SetActive(false);
            enemyUnitsPull.Add(obj1);

            GameObject obj2 = Instantiate(platformTile);
            obj2.SetActive(false);
            PlatformTilePull.Add(obj2);

            
        }
        
        for (int i = 0; i < pullOfObjects30; i++)
        {
            GameObject obj1 = Instantiate(castleTile);
            obj1.SetActive(false);
            castleTilePull.Add(obj1);
        }

        //for (int i = 0; i < pullOfObjects60; i++)
        //{
        //    GameObject obj1 = Instantiate(peakSuperShot);
        //    obj1.SetActive(false);
        //    peakSuperShotPull.Add(obj1);
        //}
    }
    public List<GameObject> GetPlayerShotPullList(int shotType) //TO DO WITH OTHER TYPES OF player shots
    {
        if (shotType == 0) return playerShotsPull[shotType];
        else if (shotType == 1 || shotType == 4) return playerShotsPull[1];//same for fire elf and fire wizard
        else if (shotType == 2 || shotType == 5) return playerShotsPull[2];//2 shot type is frost ball is the same for frost elf and frost wizzard
        else if (shotType == 3) return playerShotsPull[7]; //7 is archer arrow shot
        else if (shotType == 10) return playerShotsPull[3]; //3 is enngineer gear shot
        else if (shotType == 15) return playerShotsPull[4]; //4 is peak woman peak shot
        else if (shotType == 22) return playerShotsPull[5]; //5 is desert warrior blade shot
        else if (shotType == 27) return playerShotsPull[6]; //6 is knife thrower knife shot
        return null;
    }

    //0-regularUnit, //1-miniBossUnit //2-bigBossUnit
    public List<GameObject> GetEnemyUnitsPullList(/*int enemyIndex*/) //TO DO WITH OTHER TYPES OF ENEMY UNITS
    {
        return enemyUnitsPull;
    }
    public List<GameObject> GetPlayerUnitsPullList(int unitType) //TO DO WITH OTHER TYPES OF player UNITS
    {
        if (unitType == 0) return playerUnit0Pull;
        else if (unitType == 1) return playerUnit1Pull;
        else if (unitType == 2) return playerUnit2Pull;
        else if (unitType == 3) return playerUnit3Pull;
        else if (unitType == 4) return playerUnit4Pull;
        else if (unitType == 5) return playerUnit5Pull;
        else if (unitType == 10) return playerUnit10Pull;
        else if (unitType == 15) return playerUnit15Pull;
        else if (unitType == 22) return playerUnit22Pull;
        else if (unitType == 27) return playerUnit27Pull;

        return null;
    }
    public List<GameObject> GetCastleTilePullList()
    {
        return castleTilePull;
    }
    public List<GameObject> GetPlatformTilePullList()
    {
        return PlatformTilePull;
    }
    public List<GameObject> GetSuperShot(int index)
    {
        return superHitEffectsPull[index];
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
