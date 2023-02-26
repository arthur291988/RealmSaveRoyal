using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoaderCallback : MonoBehaviour
{
    public Text loadingTxt;

    private bool isFirstUpdateUfterStartTheScene = true;


    // Update is called once per frame
    void Update()
    {
        //so this script sends the sygnal to Loader class and allow the universal loading scene refresh itself and show the animation
        if (isFirstUpdateUfterStartTheScene)
        {
            isFirstUpdateUfterStartTheScene = false;
            Time.timeScale = 1;
            Loader.LoaderCallback();
        }
    }
}