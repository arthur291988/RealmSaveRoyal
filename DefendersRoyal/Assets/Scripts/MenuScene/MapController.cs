using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;

public class MapController : MonoBehaviour
{
    [SerializeField]
    private SpriteAtlas mapAtlas;
    private SpriteRenderer _spriteRenderer;

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
    }

    private string getComingSoonText()
    {
        if (GameParams.isEnglishStatic) return "Update soon...";
        else return "Обновление скоро...";
    }
    private string getMapName()
    {
        if (GameParams.isEnglishStatic) return "Villages on the border of the Kingdom of Damar";
        else return "Деревни на границе королевства Дамар";
    }

}
