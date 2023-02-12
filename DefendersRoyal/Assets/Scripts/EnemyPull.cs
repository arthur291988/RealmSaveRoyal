﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPull : MonoBehaviour
{
    public static EnemyPull current;
    private bool willGrow;

    private int pullOfObjects6 = 6;

    public Dictionary<Vector2, GameObject> miniBosses;


    [SerializeField]
    private List<GameObject> Location00MiniBoss;
    [SerializeField]
    private List<GameObject> Location01MiniBoss;
    [SerializeField]
    private List<GameObject> Location02MiniBoss;

    private List<List<GameObject>> location0MiniBosses;
    private List<List<GameObject>> location1MiniBosses;

    private List<List<List<GameObject>>> AllLocationMiniBosses;

    [NonSerialized]
    public List<GameObject> LocationMiniBossesPull;


    [SerializeField]
    private GameObject FinalBoss00;
    [SerializeField]
    private GameObject FinalBoss01;

    [NonSerialized]
    public Dictionary<Vector2, GameObject> allFinalBosses;

    [SerializeField]
    private GameObject enemyShot;

    [NonSerialized]
    private List <GameObject> enemyShotPull;
    private void Awake()
    {
        current = this;
    }

    private void OnEnable()
    {
        enemyShotPull = new List<GameObject>();

        allFinalBosses = new Dictionary<Vector2, GameObject>()
        {
            [new Vector2 (0,0)] = FinalBoss00,
            [new Vector2(0, 1)] = FinalBoss01,
        };

        location0MiniBosses = new List<List<GameObject>>()
        {
            Location00MiniBoss,
            Location01MiniBoss,
            Location02MiniBoss
        };
        AllLocationMiniBosses = new List<List<List<GameObject>>>()
        {
            location0MiniBosses
        };

        LocationMiniBossesPull = new List<GameObject>();

        for (int i = 0; i < AllLocationMiniBosses[CommonData.instance.location][CommonData.instance.subLocation].Count; i++)
        {
            //all bosses are instantiated twise for up and down sides
            GameObject obj = Instantiate(AllLocationMiniBosses[CommonData.instance.location][CommonData.instance.subLocation][i]);
            obj.SetActive(false);
            LocationMiniBossesPull.Add(obj); 
        }
        for (int i = 0; i < pullOfObjects6; i++)
        {
            GameObject obj1 = Instantiate(enemyShot);
            obj1.SetActive(false);
            enemyShotPull.Add(obj1);
        }

    }

    public List<GameObject> GetEnemyShotPullPullList()
    {
        return enemyShotPull;
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
