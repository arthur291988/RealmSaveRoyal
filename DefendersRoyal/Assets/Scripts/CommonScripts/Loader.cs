using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    private static Action onLoaderCallback;

    private class LoadingMonoBihave : MonoBehaviour { }

    public static void Load(int index)
    {
        //Set the loader callback action to load the target scene
        onLoaderCallback = () =>
        {
            GameObject loadingGO = new GameObject("Loading Game Object");
            loadingGO.AddComponent<LoadingMonoBihave>().StartCoroutine(LoadSceneAsync(index));
        };
        //load the loading scene whitch is always interstitial to a target scene (with special animation)
        SceneManager.LoadScene(2);
    }

    private static IEnumerator switchSceneCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

    }

    private static IEnumerator LoadSceneAsync(int index)
    {
        yield return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(index);
        while (!asyncOperation.isDone)
        {
            yield return null;
        }

    }

    public static void LoaderCallback()
    {

        //triggered after the first update which lets the screen to refresh (not be frozen and make player think that game has crushed)
        // execute the loader callback action which will meanwhile load the target scene
        if (onLoaderCallback != null)
        {
            onLoaderCallback();
            onLoaderCallback = null;
        }
    }

}
