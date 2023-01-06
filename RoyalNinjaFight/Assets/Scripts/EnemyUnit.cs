
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
    [HideInInspector]
    public Vector2 _unitStartPosition;
    [HideInInspector]
    public Vector2 _movePoint;
    private SpriteRenderer _unitSpriteRenderer;

    private bool _includedToShotPull;


    public void Start()
    {
        _transform = transform;
        _unitStartPosition = new Vector2(_transform.position.x, _transform.position.y);
    }

    private void OnEnable()
    {
        _includedToShotPull = false;
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
        _movePoint =/* CommonData.instance.platformPoints.Count > 0 ? CommonData.instance.platformPoints[Random.Range(0, CommonData.instance.platformPoints.Count)] : */Vector2.zero;

    public void reduceHP(int harm)
    {
        HP -= harm;
        if (HP < 1) disactivateUnit();
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.TryGetComponent<PlayerShot>(out PlayerShot shot))
    //    {
    //        reduceHP();
    //    }
    //}
    

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
    }

    private void FixedUpdate()
    {
        _transform.position = Vector2.MoveTowards(_transform.position, _movePoint, _moveSpeed);
    }
}
