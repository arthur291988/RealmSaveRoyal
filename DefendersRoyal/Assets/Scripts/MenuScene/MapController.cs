using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    [SerializeField]
    private SpriteAtlas mapAtlas;
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private Dropdown languageDropdown; //0 - English, 1-Russian; 2-Spanish

    [SerializeField]
    private List<GameObject> subLocationPointsOnMap;

    [SerializeField]
    private TextMeshProUGUI mapNameText;
    [SerializeField]
    private TextMeshProUGUI comingSoonText;
    [SerializeField]
    private GameObject comingSoonTextObject;


    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = mapAtlas.GetSprite(GameParams.achievedLocationStatic.ToString()+ GameParams.achievedSubLocationStatic.ToString());
        languageDropdown.value = GameParams.language;
        mapNameText.text = getMapName();

        for (int i = 0; i < GameParams.achievedSubLocationStatic + 1; i++) {
            subLocationPointsOnMap[i].SetActive(true);
        }
        if (GameParams.achievedSubLocationStatic < 3)
        {
            subLocationPointsOnMap[GameParams.achievedSubLocationStatic].transform.GetChild(0).gameObject.SetActive(true);
            subLocationPointsOnMap[GameParams.achievedSubLocationStatic].transform.GetChild(1).gameObject.SetActive(true);
        }
        else {
            comingSoonText.text = getComingSoonText();
            comingSoonTextObject.SetActive(true);
        }
        GameParams.isTutor = false;
    }

    public void updateMenuText()
    {
        GameParams.language = languageDropdown.value;
        comingSoonText.text = getComingSoonText();
        mapNameText.text = getMapName();
        SaveAndLoad.instance.savePlayerPrefs();
    }

    private string getComingSoonText()
    {
        if (GameParams.language==0) return "Update soon...";
        else if(GameParams.language == 1) return "Обновление скоро...";
        else return "Actualizar pronto...";
    }
    private string getMapName()
    {
        if (GameParams.language == 0) return "Villages on the border of the Kingdom of Damar";
        else if (GameParams.language == 1) return "Деревни на границе королевства Дамар";
        else return "Pueblos en la frontera del Reino de Damar";
    }

}
