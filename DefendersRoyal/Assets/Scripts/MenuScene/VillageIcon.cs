using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageIcon : MonoBehaviour
{
    [SerializeField]
    int location;
    [SerializeField]
    int subLocation;
    [SerializeField]
    private bool isTutor;
    //GameObject _gameObject;
    // Start is called before the first frame update
    void Start()
    {
        //_gameObject = gameObject;
        //location = (int)char.GetNumericValue(_gameObject.name[0]);
        //subLocation = (int)char.GetNumericValue(_gameObject.name[1]);
    }
    void OnMouseDown()
    {
        GameParams.locationStatic = location;
        GameParams.subLocationStatic = subLocation;
        GameParams.isTutor = isTutor;
        SceneSwitchMngr.LoadBattleScene();
    }

    public void tutorButtonPush()
    {
        GameParams.locationStatic = location;
        GameParams.subLocationStatic = subLocation;
        GameParams.isTutor = isTutor;
        SceneSwitchMngr.LoadBattleScene();
    }

}
