using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    [NonSerialized]
    public int _harm;
    [NonSerialized]
    public int _baseHarm;
    [NonSerialized]
    public float _attackSpeed;
    [NonSerialized]
    public int _superHitHarm;
    [NonSerialized]
    public float _superHitTime;
    [NonSerialized]
    public float _baseAttackSpeed;
    [NonSerialized]
    public float _accuracy;
    [NonSerialized]
    public float _baseAccuracy;
    [NonSerialized]
    public int _baseSuperHitHarm;
    [NonSerialized]
    public float _baseSuperHitTime;

    [NonSerialized]
    public int _unitMergeLevel;
    //[HideInInspector]
    //public int _unitPowerUpLevel;

    [NonSerialized]
    public int _unitType;

    [NonSerialized]
    public GameObject ObjectPulled;
    [NonSerialized]
    public List<GameObject> ObjectPulledList;

    [NonSerialized]
    public Transform _transform;
    [NonSerialized]
    public GameObject _gameObject;
    [NonSerialized]
    public Vector2 _unitStartPosition;
    [NonSerialized]
    public int unitSide; //0 up 1 down

    [NonSerialized]
    public float _shotImpulse;
    [NonSerialized]
    public float attackTimer;
    [NonSerialized]
    public Vector2 attackDirection;

    [NonSerialized]
    public float superHitsCount;
    [NonSerialized]
    public float superHitsCounter;

    [NonSerialized]
    public float simpleShotsCount;
    [NonSerialized]
    public float simpleShotsCounter;
    [NonSerialized]
    public int unitTalantLevel;

    [HideInInspector]
    public SpriteRenderer _unitSpriteRenderer;
    [HideInInspector]
    public Transform _spriteRendererTransform;
    //public SpriteAtlas _unitSpriteAtlas;

    public SpriteRenderer _rangeSprite;
    //public SpriteAtlas _rangeSpriteAtlas;

    //public ParticleSystem MergeOrPowerUpEffect;
    public ParticleSystem MergeAndSpawnEffect;
    public ParticleSystem PowerUpEffect;
    public ParticleSystem MegaHitEffect;

    [HideInInspector]
    public bool isMoved;

    [SerializeField]
    private Animator _animator;

    [NonSerialized]
    public bool isBlocked;
    private float blockedTime;
    [SerializeField]
    private ParticleSystem frozenEffect;

    public void Start()
    {
        isMoved = false;
        _shotImpulse = CommonData.instance.shotImpulse;
        _gameObject = gameObject;
    }

    public void setStartProperties() {
        if (_transform==null) _transform = transform;
        _transform.rotation = Quaternion.Euler(0, 0, 0);
        _transform.localScale = Vector3.one;
        attackTimer = UnityEngine.Random.Range(0.5f, 1);
    }

    public void setUnitFeatures(int harm, float speed, float accuracy, int superHitHarm, float superHItTime) {
        _harm = harm;
        _attackSpeed = speed;
        _accuracy = accuracy;
        _superHitHarm = superHitHarm;
        _superHitTime = superHItTime;
    }
    public virtual void updatePropertiesToLevel(bool powerUp)
    {
        //getting index of unit type and after that getting the power up level of that index
        int powerUpLevel = CommonData.instance.playerUnitTypesOnScenePowerUpLevel[Array.IndexOf(CommonData.instance.playerUnitTypesOnScene, _unitType)];
        setUnitFeatures(
            CommonData.instance.getHarmOfUnit(_unitMergeLevel, powerUpLevel, _baseHarm),
            CommonData.instance.getSpeedOfShotOfUnit(_unitMergeLevel, powerUpLevel, _baseAttackSpeed), 
            CommonData.instance.getAccuracyOfUnit(_unitMergeLevel, powerUpLevel, _baseAccuracy),
            CommonData.instance.getSuperHitHarm(_unitMergeLevel, powerUpLevel, _baseSuperHitHarm),
            CommonData.instance.getSuperHitTime(_unitMergeLevel, powerUpLevel, _baseSuperHitTime)
            );
        if (powerUp) PowerUpEffect.Play();
        else MergeAndSpawnEffect.Play();
    }
    public void setUnitMergeLevel(int level) => _unitMergeLevel = level;
    //public void setUnitPoweUpLevel(int level) => _unitPowerUpLevel = level;
    public void SetUnitType(int type) => _unitType = type;
    public void setUnitPosition() {
        _unitStartPosition = new Vector2(_transform.position.x, _transform.position.y);
        if (_unitStartPosition.y > 0) unitSide = 0;
        else unitSide = 1;
    }
    public void setSpriteOfUnit()
    {
        if (_unitSpriteRenderer == null) _unitSpriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRendererTransform == null) _spriteRendererTransform = _unitSpriteRenderer.transform;
        _unitSpriteRenderer.sprite = GameController.instance.playerSpriteAtlases.GetSprite(_unitType.ToString());
        _rangeSprite.sprite = GameController.instance.playerRangeSpriteAtlases.GetSprite(_unitMergeLevel.ToString());
    }

    public virtual void setUnitTalentLevel(int talentLvl) {
    }

    public void addToCommonData()
    {
        CommonData.instance.playerUnits.Add(this);
        //if (unitSide == 0) CommonData.instance.playerUnitsUp.Add(this);
        //else CommonData.instance.playerUnitsDown.Add(this);
    }
    public void removeFromCommonData()
    {
        CommonData.instance.playerUnits.Remove(this);
        //if (unitSide == 0) CommonData.instance.playerUnitsUp.Remove(this);
        //else CommonData.instance.playerUnitsDown.Remove(this);
    }
    public void disactivateUnit()
    {
        removeFromCommonData();
        isBlocked = false;
        blockedTime = 0;
        frozenEffect.Stop();
        simpleShotsCounter = 0;
        superHitsCounter = 0;
        _transform.rotation = Quaternion.Euler(0, 0, 0);
        _transform.localScale = Vector3.one;
        _gameObject.SetActive(false);
    }
    public virtual void attackSimple()
    {
        ObjectPulledList = ObjectPuller.current.GetPlayerShotPullList(_unitType);
        ObjectPulled = ObjectPuller.current.GetGameObjectFromPull(ObjectPulledList);
        ObjectPulled.transform.position = _transform.position;
        ObjectPulled.GetComponent<PlayerShot>()._harm = _harm;

        EnemyUnit unitToAttack = CommonData.instance.enemyUnits[unitSide].Count == 1 ? CommonData.instance.enemyUnits[unitSide][0] :
                CommonData.instance.enemyUnits[unitSide][UnityEngine.Random.Range(0, CommonData.instance.enemyUnits[unitSide].Count)];

        attackDirection = new Vector2(unitToAttack._transform.position.x, unitToAttack._transform.position.y);

        animationPlay();
        attackDirection -= _unitStartPosition;
        attackDirection = RotateAttackVector(attackDirection, UnityEngine.Random.Range(-_accuracy, _accuracy));
        ObjectPulled.SetActive(true);

        ObjectPulled.GetComponent<Rigidbody2D>().AddForce(attackDirection.normalized * _shotImpulse, ForceMode2D.Impulse);
    }

    public void animationFalse()
    {
        _animator.SetBool("Play", false);
    }

    public void animationPlay()
    {
        _animator.SetBool("Play", true);
        Invoke("animationFalse", 0.1f);
    }

    //Rotates the attack vector to add some randomness
    public Vector2 RotateAttackVector(Vector2 attackDirection, float delta)
    {
        return new Vector2(
            attackDirection.x * Mathf.Cos(delta) - attackDirection.y * Mathf.Sin(delta),
            attackDirection.x * Mathf.Sin(delta) + attackDirection.y * Mathf.Cos(delta)
        );
    }

    public virtual void superHit() {
        MegaHitEffect.Play();
    }

    public void blockTheUnit(float time) {
        frozenEffect.Play();
        blockedTime = time;
        isBlocked = true;
    }

    public virtual void Update()
    {
        if (attackTimer > 0 && GameController.instance.gameIsOn)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                if (CommonData.instance.enemyUnits[unitSide].Count > 0 && !isMoved && !isBlocked)
                {
                    attackSimple();
                }

                attackTimer = _attackSpeed;
            }

        }

        if (isBlocked) {
            blockedTime -= Time.deltaTime;
            if (blockedTime <= 0)
            {
                frozenEffect.Stop();
                isBlocked = false;
            }
        }
    }

}
