using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class DragController : MonoBehaviour
{
    private Transform unitToDrag;
    private Vector2 touchStartPosition;
    private bool unitIsDrugged;
    private Unit druggedUnitScript;

    public Transform getUnitUnderTouch(Vector2 pos)
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, 0));
        for (int i = 0; i < CommonData.playerUnits.Count; i++) {
            if (worldPosition.x < (CommonData.playerUnits[i]._unitStartPosition.x + CommonData.playerUnits[i]._unitSpriteRenderer.bounds.size.x / 2) &&
                worldPosition.x > (CommonData.playerUnits[i]._unitStartPosition.x - CommonData.playerUnits[i]._unitSpriteRenderer.bounds.size.x / 2) &&
                worldPosition.y < (CommonData.playerUnits[i]._unitStartPosition.y + CommonData.playerUnits[i]._unitSpriteRenderer.bounds.size.y / 2) &&
                worldPosition.y > (CommonData.playerUnits[i]._unitStartPosition.y - CommonData.playerUnits[i]._unitSpriteRenderer.bounds.size.y / 2))
            {
                druggedUnitScript = CommonData.playerUnits[i];
                return unitToDrag = CommonData.playerUnits[i]._transform;
            }
        }
        return null;
    }


    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1) {
            Touch _touch = Input.GetTouch(0);

            if (_touch.phase == TouchPhase.Began)
            {
                if (unitToDrag==null && getUnitUnderTouch(_touch.position) != null)
                {
                    Vector2 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(_touch.position.x, _touch.position.y, 0));
                    unitToDrag.position = worldPosition; 
                    unitIsDrugged = true;
                }
            }
            if (unitIsDrugged)
            {
                if (_touch.phase == TouchPhase.Moved)
                {
                    Vector2 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(_touch.position.x, _touch.position.y, 0));
                    unitToDrag.position = worldPosition;
                }
                else if (_touch.phase == TouchPhase.Ended)
                {
                    unitIsDrugged = false;
                }
            }
        }
    }
    private void FixedUpdate()
    {
        if (unitToDrag!=null && !unitIsDrugged && druggedUnitScript._unitStartPosition != (Vector2)unitToDrag.position) {
            unitToDrag.position = Vector2.Lerp((Vector2)unitToDrag.position, druggedUnitScript._unitStartPosition, 0.2f);
            if ((druggedUnitScript._unitStartPosition - (Vector2)unitToDrag.position).sqrMagnitude < 0.1f) {
                unitToDrag.position = druggedUnitScript._unitStartPosition;
                unitToDrag = null;
                druggedUnitScript = null;
            }
        }
    }
}
