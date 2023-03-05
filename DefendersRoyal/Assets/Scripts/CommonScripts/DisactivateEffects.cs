
using UnityEngine;

public class DisactivateEffects : MonoBehaviour
{
    //disactivates any effect prefab after it was pulled from Object Puller


    private void OnEnable()
    {
        Invoke("setFalseGameObj", 1);
    }

    private void setFalseGameObj()
    {
        gameObject.SetActive(false);
    }
}
