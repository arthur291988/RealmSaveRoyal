using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPull : MonoBehaviour
{
    public static EnemyPull current;


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


    private void Awake()
    {
        current = this;
    }

    private void OnEnable()
    {
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


    }

}
