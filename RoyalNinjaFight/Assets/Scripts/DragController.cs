
using System.Collections.Generic;
using UnityEngine;


public class DragController : MonoBehaviour
{
    private Transform unitToDragTransorm;
    private PlayerUnit unitToMergeWith;
    private PlayerUnit draggedUnitScript;
    private CastleTiles draggedFromTile;
    private CastleTiles toPutOnTile;
    private Vector2 touchStartPosition;
    private bool unitIsDragged;
    [HideInInspector]
    public GameObject ObjectPulled;
    [HideInInspector]
    public List<GameObject> ObjectPulledList;

    private Transform getUnitUnderTouch(Vector2 pos)
    {
        Vector2 worldPosition = CommonData.instance.cameraOfGame.ScreenToWorldPoint(new Vector3(pos.x, pos.y, 0));
        CastleTiles tile;

        //castle tiles used to identify unit that is staying on it
        for (int i = 0; i < CommonData.instance.castleTiles.Count; i++)
        {
            tile = CommonData.instance.castleTiles[i];
            if (worldPosition.x < (tile._position.x + tile._spriteRenderer.bounds.size.x / 2) &&
                worldPosition.x > (tile._position.x - tile._spriteRenderer.bounds.size.x / 2) &&
                worldPosition.y < (tile._position.y + tile._spriteRenderer.bounds.size.y / 2) &&
                worldPosition.y > (tile._position.y - tile._spriteRenderer.bounds.size.y / 2))
            {
                if (tile._playerUnit != null)
                {
                    draggedFromTile = tile;
                    draggedUnitScript = tile._playerUnit;
                    return unitToDragTransorm = draggedUnitScript._transform;
                }
                else return null;
            }
        }

        //for (int i = 0; i < CommonData.instance.playerUnits.Count; i++) {
        //    unit = CommonData.instance.playerUnits[i];
        //    if (worldPosition.x < (unit._unitStartPosition.x + unit._unitSpriteRenderer.bounds.size.x / 2) &&
        //        worldPosition.x > (unit._unitStartPosition.x - unit._unitSpriteRenderer.bounds.size.x / 2) &&
        //        worldPosition.y < (unit._unitStartPosition.y + unit._unitSpriteRenderer.bounds.size.y / 2) &&
        //        worldPosition.y > (unit._unitStartPosition.y - unit._unitSpriteRenderer.bounds.size.y / 2))
        //    {
        //        draggedUnitScript = unit;
        //        return unitToDragTransorm = unit._transform;
        //    }
        //}
        return null;
    }

    private PlayerUnit getUnitToMergeWith(Vector2 pos) {
        Vector2 worldPosition = CommonData.instance.cameraOfGame.ScreenToWorldPoint(new Vector3(pos.x, pos.y, 0));
        if (draggedUnitScript != null) {
            CastleTiles tile; 

            for (int i = 0; i < CommonData.instance.castleTiles.Count; i++)
            {
                if (CommonData.instance.castleTiles[i] != draggedFromTile)
                {
                    tile = CommonData.instance.castleTiles[i];
                    if (worldPosition.x < (tile._position.x + tile._spriteRenderer.bounds.size.x / 2) &&
                    worldPosition.x > (tile._position.x - tile._spriteRenderer.bounds.size.x / 2) &&
                    worldPosition.y < (tile._position.y + tile._spriteRenderer.bounds.size.y / 2) &&
                    worldPosition.y > (tile._position.y - tile._spriteRenderer.bounds.size.y / 2))
                    {
                        if (tile._playerUnit != null)
                        {
                            toPutOnTile = tile;
                            return unitToMergeWith = tile._playerUnit;
                        }
                        else return null;
                    }
                }
            }

            //for (int i = 0; i < CommonData.instance.playerUnits.Count; i++)
            //{
            //    if (CommonData.instance.playerUnits[i] != draggedUnitScript) {
            //        unit = CommonData.instance.playerUnits[i];
            //        if (worldPosition.x < (unit._unitStartPosition.x + unit._unitSpriteRenderer.bounds.size.x / 2) &&
            //        worldPosition.x > (unit._unitStartPosition.x - unit._unitSpriteRenderer.bounds.size.x / 2) &&
            //        worldPosition.y < (unit._unitStartPosition.y + unit._unitSpriteRenderer.bounds.size.y / 2) &&
            //        worldPosition.y > (unit._unitStartPosition.y - unit._unitSpriteRenderer.bounds.size.y / 2))
            //        {
            //            return unitToMergeWith = unit;
            //        }
            //    }
            //}
        }
        return null;
    }
    private CastleTiles getTileToPutOn(Vector2 pos)
    {
        Vector2 worldPosition = CommonData.instance.cameraOfGame.ScreenToWorldPoint(new Vector3(pos.x, pos.y, 0));
        if (draggedUnitScript != null)
        {
            CastleTiles tile;

            for (int i = 0; i < CommonData.instance.castleTiles.Count; i++)
            {
                if (CommonData.instance.castleTiles[i] != draggedFromTile)
                {
                    tile = CommonData.instance.castleTiles[i];
                    if (worldPosition.x < (tile._position.x + tile._spriteRenderer.bounds.size.x / 2) &&
                    worldPosition.x > (tile._position.x - tile._spriteRenderer.bounds.size.x / 2) &&
                    worldPosition.y < (tile._position.y + tile._spriteRenderer.bounds.size.y / 2) &&
                    worldPosition.y > (tile._position.y - tile._spriteRenderer.bounds.size.y / 2))
                    {
                        if (tile._playerUnit == null) return toPutOnTile = tile;
                        else return null;
                    }
                }
            }
        }
        return null;
    }


    private void mergeUnits()
    {
        int unitTypeIndex = Random.Range(0, CommonData.instance.playerUnitTypesOnScene.Length);
        ObjectPulledList = ObjectPuller.current.GetPlayerUnitsPullList(CommonData.instance.playerUnitTypesOnScene[unitTypeIndex]);
        ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
        ObjectPulled.transform.position = unitToMergeWith._unitStartPosition;
        PlayerUnit unit = ObjectPulled.GetComponent<PlayerUnit>();
        unit.setUnitMergeLevel(unitToMergeWith._unitMergeLevel + 1);
        unit.SetUnitType(CommonData.instance.playerUnitTypesOnScene[unitTypeIndex]);
        unit.updatePropertiesToLevel();
        CommonData.instance.playerUnits.Add(unit);

        ObjectPulled.SetActive(true);
        unit.setUnitPosition();
        toPutOnTile._playerUnit = unit;
        draggedFromTile._playerUnit = null;

        CommonData.instance.castlePointsWithNoUnits.Add(draggedFromTile._position);
        GameController.instance.updateUnitsAndCastleTileAddButtonsUI();

        unitToMergeWith.disactivateUnit();
        draggedUnitScript.disactivateUnit();
        draggedFromTile = null;
        toPutOnTile = null;
    }

    private void putUnitOnNewPosition() {
        draggedUnitScript._transform.position = toPutOnTile._position;
        draggedUnitScript.setUnitPosition();

        CommonData.instance.castlePointsWithNoUnits.Add(draggedFromTile._position);
        CommonData.instance.castlePointsWithNoUnits.Remove(toPutOnTile._position);

        toPutOnTile._playerUnit = draggedUnitScript;
        draggedFromTile._playerUnit = null;
    }

    private bool checkIfCanMerge() {
        if (unitToMergeWith._unitMergeLevel == draggedUnitScript._unitMergeLevel && unitToMergeWith._unitType == draggedUnitScript._unitType
            && unitToMergeWith._unitMergeLevel < CommonData.instance.playerUnitMaxLevel) return true;
        else return false;
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
                    Vector2 worldPosition = CommonData.instance.cameraOfGame.ScreenToWorldPoint(new Vector3(_touch.position.x, _touch.position.y, 0));
                    unitToDragTransorm.position = worldPosition;
                    unitIsDragged = true;
                }
            }
            if (unitIsDragged)
            {
                if (_touch.phase == TouchPhase.Moved)
                {
                    Vector2 worldPosition = CommonData.instance.cameraOfGame.ScreenToWorldPoint(new Vector3(_touch.position.x, _touch.position.y, 0));
                    unitToDragTransorm.position = worldPosition;
                }
                else if (_touch.phase == TouchPhase.Ended)
                {
                    if (getUnitToMergeWith(_touch.position) != null && checkIfCanMerge())
                    {
                        unitToDragTransorm = null;
                        unitIsDragged = false;
                        mergeUnits();
                    }
                    else if (getTileToPutOn(_touch.position))
                    {
                        unitToDragTransorm = null;
                        unitIsDragged = false;
                        putUnitOnNewPosition();

                    }
                    else
                    {
                        unitIsDragged = false;
                        unitToMergeWith = null;
                        //draggedUnitScript = null;
                    }
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
                draggedFromTile = null;
                toPutOnTile = null;
            }
        }
    }
}
