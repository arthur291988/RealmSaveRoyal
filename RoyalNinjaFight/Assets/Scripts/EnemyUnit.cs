
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

    [SerializeField] 
    private Transform _lifeLine;
    private float LifelineMaxXPositionModule;
    private float HPtoTransforOfLifeLine;


    public void Start()
    {
        _transform = transform;
        _unitStartPosition = new Vector2(_transform.position.x, _transform.position.y);
    }

    private void OnEnable()
    {
        LifelineMaxXPositionModule = 3;
        _lifeLine.localPosition = new Vector2(0, 1.4f);
        HPtoTransforOfLifeLine = LifelineMaxXPositionModule / HP;
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
        if (GameController.instance.gameIsOn) _transform.position = Vector2.MoveTowards(_transform.position, _movePoint, _moveSpeed);
    }
}
