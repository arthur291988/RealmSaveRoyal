using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerUnit : MonoBehaviour
{


    [HideInInspector]
    public int _harm;
    [HideInInspector]
    public int _baseHarm;
    [HideInInspector]
    public float _attackSpeed;
    [HideInInspector]
    public float _baseAttackSpeed;
    [HideInInspector]
    public float _accuracy;
    [HideInInspector]
    public float _baseAccuracy;

    [HideInInspector]
    public int _unitMergeLevel;
    [HideInInspector]
    public int _unitPowerUpLevel;

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
    public int unitSide; //0 up 1 down

    [HideInInspector]
    public float _shotImpulse;
    [HideInInspector]
    public float attackTimer;
    [HideInInspector]
    public Vector2 attackDirection;

    [HideInInspector]
    public SpriteRenderer _unitSpriteRenderer;
    //public SpriteAtlas _unitSpriteAtlas;

    public SpriteRenderer _rangeSprite;
    //public SpriteAtlas _rangeSpriteAtlas;

    public ParticleSystem MergeOrPowerUpEffect;

    [HideInInspector]
    public bool isMoved;

    private void OnEnable()
    {
    }

    public void Start()
    {
        isMoved = false;
        _shotImpulse = CommonData.instance.shotImpulse;
        _gameObject = gameObject;

    }

    public void setStartProperties() {
        _transform = transform;
        attackTimer = Random.Range(0.5f, 1);
    }

    public void setUnitFeatures(int harm, float speed, float accuracy) {
        _harm = harm;
        _attackSpeed = speed;
        _accuracy = accuracy;
    }
    public virtual void updatePropertiesToLevel()
    {
        setSpriteOfUnit();
        setUnitFeatures(CommonData.instance.getHarmOfUnit(_unitMergeLevel, _unitPowerUpLevel, _baseHarm), 
            CommonData.instance.getSpeedOfShotOfUnit(_unitMergeLevel, _unitPowerUpLevel, _baseAttackSpeed), 
            CommonData.instance.getAccuracyOfUnit(_unitMergeLevel, _unitPowerUpLevel, _baseAccuracy));
        MergeOrPowerUpEffect.Play();
    }
    public void setUnitMergeLevel(int level) => _unitMergeLevel = level;
    public void setUnitPoweUpLevel(int level) => _unitPowerUpLevel = level;
    public void SetUnitType(int type) => _unitType = type;
    public void setUnitPosition() {
        _unitStartPosition = new Vector2(_transform.position.x, _transform.position.y);
        if (_unitStartPosition.y > 0) unitSide = 0;
        else unitSide = 1;
    }
    public void setSpriteOfUnit()
    {
        if (_unitSpriteRenderer == null) _unitSpriteRenderer = GetComponent<SpriteRenderer>();
        _unitSpriteRenderer.sprite = CommonData.instance.playerSpriteAtlases.GetSprite(_unitType.ToString());
        _rangeSprite.sprite = CommonData.instance.playerRangeSpriteAtlases.GetSprite(_unitMergeLevel.ToString());
    }

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
        ObjectPulledList = ObjectPuller.current.GetPlayerShotPullList(_unitType);
        ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
        ObjectPulled.transform.position = _transform.position;
        ObjectPulled.GetComponent<PlayerShot>()._harm = _harm;
        ObjectPulled.SetActive(true);

        EnemyUnit unitToAttack = CommonData.instance.enemyUnits[unitSide].Count == 1 ? CommonData.instance.enemyUnits[unitSide][0] :
                CommonData.instance.enemyUnits[unitSide][Random.Range(0, CommonData.instance.enemyUnits[unitSide].Count)];

        attackDirection = new Vector2(unitToAttack._transform.position.x, unitToAttack._transform.position.y);
        attackDirection -= _unitStartPosition;
        attackDirection = RotateAttackVector(attackDirection, Random.Range(-_accuracy, _accuracy));

        ObjectPulled.GetComponent<Rigidbody2D>().AddForce(attackDirection.normalized * _shotImpulse, ForceMode2D.Impulse);
    }

    //Rotates the attack vector to add some randomness
    public Vector2 RotateAttackVector(Vector2 attackDirection, float delta)
    {
        return new Vector2(
            attackDirection.x * Mathf.Cos(delta) - attackDirection.y * Mathf.Sin(delta),
            attackDirection.x * Mathf.Sin(delta) + attackDirection.y * Mathf.Cos(delta)
        );
    }

    private void Update()
    {
        if (attackTimer > 0 && GameController.instance.gameIsOn)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                if (CommonData.instance.enemyUnits[unitSide].Count > 0 && !isMoved)
                {
                    attackSimple();
                }

                attackTimer = _attackSpeed;
            }

        }
    }

}
