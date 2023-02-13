
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    [NonSerialized]
    public int _enemyLevel; //each level has its own sprite
    [NonSerialized]
    public int _enemySide;
    [NonSerialized]
    public int _energyOnDestroy;
    [NonSerialized]
    public float _moveSpeed;

    public SpriteRenderer _spriteRendererOfLifeLine;
    [NonSerialized]
    public MaterialPropertyBlock matBlockOfLifeLineSprite;

    [NonSerialized]
    public int HP;
    [NonSerialized]
    public float maxHP;
    //[NonSerialized]
    //public int TowerHP;

    [NonSerialized]
    public Transform _transform;
    [NonSerialized]
    public GameObject _gameObject;
    //[HideInInspector]
    //public Vector2 _unitStartPosition;
    [NonSerialized]
    public Vector2 _movePoint;
    [NonSerialized]
    public SpriteRenderer _unitSpriteRenderer;

    private bool _includedToShotPull;

    //[SerializeField]
    //private Transform _lifeLine;
    //private float LifelineMaxXPositionModule;
    //private float HPtoTransforOfLifeLine;

    private int lastingEffectIndex;

    [NonSerialized]
    public bool underLastingFireInjure;
    private float lastingFireEffectTimer;
    private float lastingFireEffectInjureTimer;
    private float lastingFireEffectInjureTime;
    private int lastingFireEffectInjure;

    [NonSerialized]
    public bool underLastingFrost;
    private float lastingFrostEffectTimer;

    private bool underLastingPeakBlood;
    private float lastingPeakBloodEffectTimer;
    private float moveSpeedBuffer;

    [SerializeField]
    private List<ParticleSystem> effectsOnEnemy;

    private bool isPushedBack;
    private Vector2 pushBackPoint;


    //private void OnEnable()
    //{

    //    if (matBlockOfLifeLineSprite==null) matBlockOfLifeLineSprite = new MaterialPropertyBlock();

    //    lastingPeakBloodEffectTimer = 0;
    //    lastingFrostEffectTimer = 0;
    //    lastingFireEffectTimer = 0;

    //    isPushedBack = false;
    //    underLastingFireInjure = false;
    //    underLastingFrost = false;

    //    lastingFireEffectInjureTime = 0.25f; //all injures from lasting effects are applied per 0.25 of second

    //    //LifelineMaxXPositionModule = 3;
    //    //_lifeLine.localPosition = new Vector2(0, 1.4f);
    //    //HPtoTransforOfLifeLine = LifelineMaxXPositionModule / HP;
    //    _includedToShotPull = false;
    //    if (_transform==null) _transform = transform;


    //    UpdateParams();
    //    //Debug.Log(_enemyLevel.ToString()+"0"+ CommonData.instance.location.ToString());
    //}

    public virtual void startSettings()
    {
        if (matBlockOfLifeLineSprite == null) matBlockOfLifeLineSprite = new MaterialPropertyBlock();

        lastingPeakBloodEffectTimer = 0;
        lastingFrostEffectTimer = 0;
        lastingFireEffectTimer = 0;

        isPushedBack = false;
        underLastingFireInjure = false;
        underLastingFrost = false;

        lastingFireEffectInjureTime = 0.25f; //all injures from lasting effects are applied per 0.25 of second

        _includedToShotPull = false;
        if (_gameObject == null) _gameObject = gameObject;
        if (_transform == null) _transform = transform;

        UpdateParams();
    }

    public void addToCommonDataOnEnableaAndSetParameters()
    {
        Invoke("setMoveToPoint", 1f);
        CommonData.instance.enemyUnitsAll.Add(this);
    }

    public virtual void setEnemyLevel(int level, int attackWaveCount)
    {
        _enemyLevel = level;
        //if (_enemyLevel < 10) setEnemyFeatures(attackWaveCount);
        //else if (_enemyLevel < 100) setMiniBossFeatures(attackWaveCount);
        //else setBigBossFeatures(attackWaveCount);

        //setEnemyFeatures(attackWaveCount);

    }

    public void setEnemySide (int side) => _enemySide = side; //0 - up; 1 - down;

    //private void setEnemyFeatures(int waveNumber)
    //{
    //    if (_unitSpriteRenderer==null) _unitSpriteRenderer = GetComponent<SpriteRenderer>();
    //    _unitSpriteRenderer.sprite = CommonData.instance.enemyAtlas.GetSprite(_enemyLevel.ToString() + CommonData.instance.location.ToString()); 
        
    //    _energyOnDestroy = CommonData.instance.regularEnemyEnergy[waveNumber];
    //    _moveSpeed = CommonData.instance.regularEnemySpeed[_enemyLevel];
    //    HP = CommonData.instance.regularEnemyHPForAllLocations[CommonData.instance.location, CommonData.instance.subLocation, waveNumber, _enemyLevel];
    //    maxHP = HP;
    //}

    //private void setMiniBossFeatures(int waveNumber) {
    //    if (_unitSpriteRenderer == null) _unitSpriteRenderer = GetComponent<SpriteRenderer>();
    //    //1010 - 10 is mini boss, 1 wave count, location 0 mini boss: 1011 - 10 is mini boss, 1 wave count, location 1 mini boss 
    //    _unitSpriteRenderer.sprite = CommonData.instance.enemyAtlas.GetSprite(_enemyLevel.ToString() + waveNumber.ToString() + CommonData.instance.location.ToString());

    //    int intHolder = 0;
    //    for (int i = 0; i < 4; i++) {
    //        intHolder += CommonData.instance.regularEnemyHPForAllLocations[CommonData.instance.location, CommonData.instance.subLocation, waveNumber, i] * CommonData.instance.HPMultiplierForMiniBoss;
    //    }
    //    HP = intHolder / 4;
    //    maxHP = HP;

    //    _energyOnDestroy = CommonData.instance.regularEnemyEnergy[waveNumber]* CommonData.instance.energyMultiplierForMiniBoss;

    //    float floatHolder = 0;
    //    for (int i = 0; i < 4; i++)
    //    {
    //        floatHolder += CommonData.instance.regularEnemySpeed[i];
    //    }
    //    _moveSpeed = floatHolder / 4;
    //}

    //private void setBigBossFeatures(int waveNumber) {
    //    if (_unitSpriteRenderer == null) _unitSpriteRenderer = GetComponent<SpriteRenderer>();


    //    //10000 - 100 is location boss, 0 wave count (always zero), location 0 mini boss: 10001 - 100 is location boss, 0 wave count (always zero), location 1 mini boss 
    //    _unitSpriteRenderer.sprite = CommonData.instance.enemyAtlas.GetSprite(_enemyLevel.ToString() + "0" + CommonData.instance.location.ToString()); 

    //    int intHolder = 0;
    //    for (int i = 0; i < 4; i++)
    //    {
    //        intHolder += 
    //            CommonData.instance.regularEnemyHPForAllLocations[CommonData.instance.location, CommonData.instance.subLocation, waveNumber, i] * CommonData.instance.HPMultiplierForBigBoss;
    //    }
    //    HP = intHolder / 4;
    //    maxHP = HP;

    //    _energyOnDestroy = CommonData.instance.regularEnemyEnergy[waveNumber] * CommonData.instance.energyMultiplierForBigBoss;

    //    float floatHolder = 0;
    //    for (int i = 0; i < 4; i++)
    //    {
    //        floatHolder += CommonData.instance.regularEnemySpeed[i];
    //    }
    //    _moveSpeed = floatHolder / 4;
    //}


    private void setMoveToPoint() =>
        _movePoint = new Vector2(_transform.position.x, 0);/* CommonData.instance.platformPoints.Count > 0 ? CommonData.instance.platformPoints[Random.Range(0, CommonData.instance.platformPoints.Count)] : Vector2.zero;*/

    public void reduceHP(int harm)
    {
        HP -= harm;
        //if (HP > 0 && harm == 1000) pushEnemyBack(); //harm equality is checket to make shure the this is tower hit

        UpdateParams();
        //_lifeLine.localPosition = new Vector2(HP * HPtoTransforOfLifeLine-LifelineMaxXPositionModule, 1.4f);  
        if (HP <= 0) disactivateUnit();
    }

    //tower reduces
    public void reduceHPFromTower()
    {
        HP -= CommonData.instance.towerHPReduceAmount;
        if (HP > 0)
        {
            UpdateParams();
            pushEnemyBack();
            //_lifeLine.localPosition = new Vector2(HP * HPtoTransforOfLifeLine - LifelineMaxXPositionModule, 1.4f);
        }
        if (HP <= 0) disactivateUnit();
    }

    private void UpdateParams()
    {
        _spriteRendererOfLifeLine.GetPropertyBlock(matBlockOfLifeLineSprite);
        matBlockOfLifeLineSprite.SetFloat("_Fill", HP / maxHP);
        _spriteRendererOfLifeLine.SetPropertyBlock(matBlockOfLifeLineSprite);
    }

    public void pushEnemyBack() {
        pushBackPoint = _enemySide > 0 ? new Vector2(_transform.position.x, _transform.position.y - 6) : new Vector2(_transform.position.x, _transform.position.y + 6);
        isPushedBack = true;
    }

    public void removeFromCommonData()
    {
        CommonData.instance.enemyUnitsAll.Remove(this);
        CommonData.instance.enemyUnits[_enemySide].Remove(this);
    }

    public void disactivateUnit()
    {
        GameController.instance.incrementEnergy(_energyOnDestroy);
        //GameController.instance.updateEneryText();
        removeFromCommonData();
        _gameObject.SetActive(false);
        underLastingFireInjure = false;
        underLastingFrost=false;
        underLastingPeakBlood=false;
        foreach (ParticleSystem lastingEffect in effectsOnEnemy) lastingEffect.Stop(); // stop all playing effects
        isPushedBack = false;
    }

    public void lastingInjure(float time, int injurePerFourthOfSecont, int effectIndex)
    {
        if (effectIndex == 0 && !underLastingFireInjure)
        {
            lastingEffectIndex = effectIndex;
            lastingFireEffectInjureTimer = lastingFireEffectInjureTime;
            lastingFireEffectTimer = time;
            lastingFireEffectInjure = injurePerFourthOfSecont;
            effectsOnEnemy[lastingEffectIndex].Play();
            underLastingFireInjure = true;
        }
        else if (effectIndex == 1 && !underLastingFrost) {
            lastingEffectIndex = effectIndex;
            lastingFrostEffectTimer = time;
            effectsOnEnemy[lastingEffectIndex].Play();
            underLastingFrost = true;
        }
        else if (effectIndex == 2 && !underLastingPeakBlood)
        {
            lastingEffectIndex = effectIndex;
            lastingPeakBloodEffectTimer = time;
            effectsOnEnemy[lastingEffectIndex].Play();
            underLastingPeakBlood = true;
            moveSpeedBuffer = _moveSpeed;
            _moveSpeed *= 0.5f;
        }
    }

    public virtual void Update()
    {
        if (!_includedToShotPull) {
            if (_enemySide == 0)
            {
                if (_transform.position.y < GameController.instance.topShotLine)
                {
                    CommonData.instance.enemyUnits[_enemySide].Add(this);
                    _includedToShotPull = true;
                }
            }
            else if (_transform.position.y > GameController.instance.bottomShotLine)
            {
                CommonData.instance.enemyUnits[_enemySide].Add(this);
                _includedToShotPull = true;
            }
        }

        //counting the time of lasting effect and reducing HP per fixed time if applicable by effect
        if (underLastingFireInjure) {
            lastingFireEffectInjureTimer -= Time.deltaTime;
            if (lastingFireEffectInjureTimer <= 0)
            {
                lastingFireEffectInjureTimer = lastingFireEffectInjureTime;
                reduceHP(lastingFireEffectInjure);
            }
            lastingFireEffectTimer -= Time.deltaTime;
            if (lastingFireEffectTimer <= 0) {
                underLastingFireInjure = false;
                effectsOnEnemy[0].Stop();
            }
        }
        if (underLastingFrost) {
            lastingFrostEffectTimer -= Time.deltaTime;
            if (lastingFrostEffectTimer <= 0)
            {
                underLastingFrost = false;
                effectsOnEnemy[1].Stop();
            }
        }
        if (underLastingPeakBlood)
        {
            lastingPeakBloodEffectTimer -= Time.deltaTime;
            if (lastingPeakBloodEffectTimer <= 0)
            {
                _moveSpeed = moveSpeedBuffer;
                underLastingPeakBlood = false;
                effectsOnEnemy[2].Stop();
            }
        }
    }

    private void FixedUpdate()
    {
        //regular enemy walk towards player castle
        if (GameController.instance.gameIsOn && !underLastingFrost && !isPushedBack) _transform.position = Vector2.MoveTowards(_transform.position, _movePoint, _moveSpeed);

        //pushing enemy back effect
        if (GameController.instance.gameIsOn && !underLastingFrost && isPushedBack) {
            if ((Vector2)_transform.position != pushBackPoint)
            {
                _transform.position = Vector2.Lerp((Vector2)_transform.position, pushBackPoint, 0.2f);
                if ((pushBackPoint - (Vector2)_transform.position).sqrMagnitude < 0.1f)
                {
                    _transform.position = pushBackPoint; 
                    isPushedBack = false;
                }
            }
        }
    }
}
