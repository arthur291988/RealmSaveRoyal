using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerUnit : MonoBehaviour
{
    [HideInInspector]
    public int _harm;
    [HideInInspector]
    public int _unitLevel;
    [HideInInspector]
    public int _unitType;

    [HideInInspector]
    public GameObject ObjectPulled;
    [HideInInspector]
    public List<GameObject> ObjectPulledList;

    [HideInInspector]
    public Transform _transform;
    [HideInInspector]
    public GameObject _gameObject;
    [HideInInspector]
    public Vector2 _unitStartPosition;

    [HideInInspector]
    public float _shotImpulse;
    [HideInInspector]
    public float _attackSpeed;
    [HideInInspector]
    public float attackTimer;
    [HideInInspector]
    public Vector2 attackDirection;

    [HideInInspector]
    public SpriteRenderer _unitSpriteRenderer;

    public SpriteAtlas _unitSpriteAtlas;

    public void Start()
    {
        _shotImpulse = CommonData.instance.shotImpulse;
        _gameObject = gameObject;
    }


    public void SetAttackSpeed(float speed) => _attackSpeed = speed;
    public void SetAttackHarm(int harm) => _harm = harm;
    public void SetUnitType(int type) => _unitType = type;
    public virtual void updatePropertiesToLevel() {}
    public void setUnitLevel(int level) => _unitLevel = level;
    public void setUnitPosition() {
        _unitStartPosition = new Vector2(_transform.position.x, _transform.position.y);
    }
    public void setSpriteOfUnit()
    {
        _unitSpriteRenderer = GetComponent<SpriteRenderer>();
        _unitSpriteRenderer.sprite = _unitSpriteAtlas.GetSprite(_unitType.ToString() + _unitLevel.ToString());
    }

    //public void addToCommonData()
    //{
    //    CommonData.instance.playerUnits.Add(this);
    //}
    public void removeFromCommonData()
    {
        CommonData.instance.playerUnits.Remove(this);
    }
    public void disactivateUnit()
    {
        removeFromCommonData();
        _gameObject.SetActive(false);
    }
    public virtual void attackSimple()
    {
       
    }

    //Rotates the attack vector to att some randomness
    public Vector2 RotateAttackVector(Vector2 attackDirection, float delta)
    {
        return new Vector2(
            attackDirection.x * Mathf.Cos(delta) - attackDirection.y * Mathf.Sin(delta),
            attackDirection.x * Mathf.Sin(delta) + attackDirection.y * Mathf.Cos(delta)
        );
    }

    private void Update()
    {
        if (attackTimer > 0 && CommonData.instance.enemyUnits.Count>0 /*&& CommonData.gameIsOn*/)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                attackSimple();
                attackTimer = _attackSpeed;
            }

        }
    }

}
