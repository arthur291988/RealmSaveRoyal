using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.PlayerSettings;


public class DragController : MonoBehaviour
{
    private Transform unitToDragTransorm;
    private PlayerUnit unitToMergeWith;
    private PlayerUnit draggedUnitScript;
    private Vector2 touchStartPosition;
    private bool unitIsDragged;

    private Transform getUnitUnderTouch(Vector2 pos)
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, 0));
        for (int i = 0; i < CommonData.instance.playerUnits.Count; i++) {
            if (worldPosition.x < (CommonData.instance.playerUnits[i]._unitStartPosition.x + CommonData.instance.playerUnits[i]._unitSpriteRenderer.bounds.size.x / 2) &&
                worldPosition.x > (CommonData.instance.playerUnits[i]._unitStartPosition.x - CommonData.instance.playerUnits[i]._unitSpriteRenderer.bounds.size.x / 2) &&
                worldPosition.y < (CommonData.instance.playerUnits[i]._unitStartPosition.y + CommonData.instance.playerUnits[i]._unitSpriteRenderer.bounds.size.y / 2) &&
                worldPosition.y > (CommonData.instance.playerUnits[i]._unitStartPosition.y - CommonData.instance.playerUnits[i]._unitSpriteRenderer.bounds.size.y / 2))
            {
                draggedUnitScript = CommonData.instance.playerUnits[i];
                return unitToDragTransorm = CommonData.instance.playerUnits[i]._transform;
            }
        }
        return null;
    }

    private PlayerUnit getUnitToMergeWith(Vector2 pos) {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, 0));
        if (draggedUnitScript != null) {
            for (int i = 0; i < CommonData.instance.playerUnits.Count; i++)
            {
                if (CommonData.instance.playerUnits[i] != draggedUnitScript) {
                    if (worldPosition.x < (CommonData.instance.playerUnits[i]._unitStartPosition.x + CommonData.instance.playerUnits[i]._unitSpriteRenderer.bounds.size.x / 2) &&
                    worldPosition.x > (CommonData.instance.playerUnits[i]._unitStartPosition.x - CommonData.instance.playerUnits[i]._unitSpriteRenderer.bounds.size.x / 2) &&
                    worldPosition.y < (CommonData.instance.playerUnits[i]._unitStartPosition.y + CommonData.instance.playerUnits[i]._unitSpriteRenderer.bounds.size.y / 2) &&
                    worldPosition.y > (CommonData.instance.playerUnits[i]._unitStartPosition.y - CommonData.instance.playerUnits[i]._unitSpriteRenderer.bounds.size.y / 2))
                    {
                        return unitToMergeWith = CommonData.instance.playerUnits[i];
                    }
                }
            }
        }
        return null;
    }

    private void mergeUnits(PlayerUnit unitToUpgrade, PlayerUnit unitToDelete) {
        unitToDelete.disactivateUnit();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1) {
            Touch _touch = Input.GetTouch(0);

            if (_touch.phase == TouchPhase.Began)
            {
                if (unitToDragTransorm == null && getUnitUnderTouch(_touch.position) != null)
                {
                    Vector2 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(_touch.position.x, _touch.position.y, 0));
                    unitToDragTransorm.position = worldPosition;
                    unitIsDragged = true;
                }
            }
            if (unitIsDragged)
            {
                if (_touch.phase == TouchPhase.Moved)
                {
                    Vector2 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(_touch.position.x, _touch.position.y, 0));
                    unitToDragTransorm.position = worldPosition;
                }
                else if (_touch.phase == TouchPhase.Ended)
                {
                    if (getUnitToMergeWith(_touch.position)!=null) {
                        mergeUnits(unitToMergeWith, draggedUnitScript);
                    }
                    else unitIsDragged = false;
                }
            }
        }
    }
    private void FixedUpdate()
    {
        if (unitToDragTransorm != null && !unitIsDragged && draggedUnitScript._unitStartPosition != (Vector2)unitToDragTransorm.position) {
            unitToDragTransorm.position = Vector2.Lerp((Vector2)unitToDragTransorm.position, draggedUnitScript._unitStartPosition, 0.2f);
            if ((draggedUnitScript._unitStartPosition - (Vector2)unitToDragTransorm.position).sqrMagnitude < 0.1f) {
                unitToDragTransorm.position = draggedUnitScript._unitStartPosition;
                unitToDragTransorm = null;
                draggedUnitScript = null;
            }
        }
    }
}
