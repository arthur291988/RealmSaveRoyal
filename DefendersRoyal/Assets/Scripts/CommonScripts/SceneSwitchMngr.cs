
using UnityEngine;


public class SceneSwitchMngr : MonoBehaviour
{
    public static void LoadMenuScene()
    {
        Loader.Load(0);
    }
    public static void LoadBattleScene()
    {
        Loader.Load(1);
    }
}
