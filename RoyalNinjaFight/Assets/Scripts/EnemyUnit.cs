
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    private int _enemyLevel; //each level has its own sprite
    private int _enemySide;
    private int _energyOnDestroy;
    private float _moveSpeed;

    [HideInInspector]
    public int HP;

    [HideInInspector]
    public Transform _transform;
    //[HideInInspector]
    //public Vector2 _unitStartPosition;
    [HideInInspector]
    public Vector2 _movePoint;
    private SpriteRenderer _unitSpriteRenderer;

    private bool _includedToShotPull;

    [SerializeField]
    private Transform _lifeLine;
    private float LifelineMaxXPositionModule;
    private float HPtoTransforOfLifeLine;

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

    [SerializeField]
    private List<ParticleSystem> effectsOnEnemy;


    private void OnEnable()
    {
        underLastingFireInjure = false;
        underLastingFrost = false;

        lastingFireEffectInjureTime = 0.25f; //all injures from lasting effects are applied per 0.25 of second


        LifelineMaxXPositionModule = 3;
        _lifeLine.localPosition = new Vector2(0, 1.4f);
        HPtoTransforOfLifeLine = LifelineMaxXPositionModule / HP;
        _includedToShotPull = false;
        if (_transform==null) _transform = transform;
        Invoke("setMoveToPoint", 1f);
    }

    public void setEnemyLevel(int level, int attackWaveCount)
    {
        _enemyLevel = level;
        if (_enemyLevel < 10) setEnemyFeatures(attackWaveCount);
        else if (_enemyLevel < 100) setMiniBossFeatures(attackWaveCount);
        else setBigBossFeatures(attackWaveCount);
    }

    public void setEnemySide (int side) => _enemySide = side;

    private void setEnemyFeatures(int waveNumber)
    {
        if (_unitSpriteRenderer==null) _unitSpriteRenderer = GetComponent<SpriteRenderer>();
        _unitSpriteRenderer.sprite = CommonData.instance.enemyAtlas.GetSprite(_enemyLevel.ToString() + CommonData.instance.location.ToString()); 
        
        _energyOnDestroy = CommonData.instance.regularEnemyEnergy[waveNumber];
        _moveSpeed = CommonData.instance.regularEnemySpeed[_enemyLevel];
        HP = CommonData.instance.regularEnemyHPForAllLocations[CommonData.instance.location, CommonData.instance.subLocation, waveNumber, _enemyLevel];
    }

    private void setMiniBossFeatures(int waveNumber) {
        if (_unitSpriteRenderer == null) _unitSpriteRenderer = GetComponent<SpriteRenderer>();
        _unitSpriteRenderer.sprite = CommonData.instance.enemyAtlas.GetSprite(_enemyLevel.ToString() + CommonData.instance.location.ToString());

        int intHolder = 0;
        for (int i = 0; i < 4; i++) {
            intHolder += CommonData.instance.regularEnemyHPForAllLocations[CommonData.instance.location, CommonData.instance.subLocation, waveNumber, i] * CommonData.instance.HPMultiplierForMiniBoss;
        }
        HP = intHolder / 4;

        _energyOnDestroy = CommonData.instance.regularEnemyEnergy[waveNumber]* CommonData.instance.energyMultiplierForMiniBoss;

        float floatHolder = 0;
        for (int i = 0; i < 4; i++)
        {
            floatHolder += CommonData.instance.regularEnemySpeed[i];
        }
        _moveSpeed = floatHolder / 4;
    }

    private void setBigBossFeatures(int waveNumber) {
        if (_unitSpriteRenderer == null) _unitSpriteRenderer = GetComponent<SpriteRenderer>();
        _unitSpriteRenderer.sprite = CommonData.instance.enemyAtlas.GetSprite(_enemyLevel.ToString() + CommonData.instance.location.ToString());

        int intHolder = 0;
        for (int i = 0; i < 4; i++)
        {
            intHolder += 
                CommonData.instance.regularEnemyHPForAllLocations[CommonData.instance.location, CommonData.instance.subLocation, waveNumber, i] * CommonData.instance.HPMultiplierForBigBoss;
        }
        HP = intHolder / 4;

        _energyOnDestroy = CommonData.instance.regularEnemyEnergy[waveNumber] * CommonData.instance.energyMultiplierForBigBoss;

        float floatHolder = 0;
        for (int i = 0; i < 4; i++)
        {
            floatHolder += CommonData.instance.regularEnemySpeed[i];
        }
        _moveSpeed = floatHolder / 4;
    }


    private void setMoveToPoint() =>
        _movePoint = new Vector2(_transform.position.x, 0);/* CommonData.instance.platformPoints.Count > 0 ? CommonData.instance.platformPoints[Random.Range(0, CommonData.instance.platformPoints.Count)] : Vector2.zero;*/

    public void reduceHP(int harm)
    {
        HP -= harm;
        _lifeLine.localPosition = new Vector2(HP * HPtoTransforOfLifeLine-LifelineMaxXPositionModule, 1.4f);  
        if (HP <= 0) disactivateUnit();
    }



    public void removeFromCommonData()
    {
        CommonData.instance.enemyUnits[_enemySide].Remove(this);
    }

    public void disactivateUnit()
    {
        GameController.instance.incrementEnergy(_energyOnDestroy);
        GameController.instance.updateEneryText();
        removeFromCommonData();
        gameObject.SetActive(false);
        underLastingFireInjure = false;
        underLastingFrost=false;
        effectsOnEnemy[lastingEffectIndex].Stop();
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
    }

    private void Update()
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

        //counting the time of lasting effect and reducing HP per fixed time
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
                effectsOnEnemy[lastingEffectIndex].Stop();
            }
        }
        if (underLastingFrost) {
            lastingFrostEffectTimer -= Time.deltaTime;
            if (lastingFrostEffectTimer <= 0)
            {
                underLastingFrost = false;
                effectsOnEnemy[lastingEffectIndex].Stop();
            }
        }
    }

    private void FixedUpdate()
    {
        if (GameController.instance.gameIsOn && !underLastingFrost) _transform.position = Vector2.MoveTowards(_transform.position, _movePoint, _moveSpeed);
    }
}
