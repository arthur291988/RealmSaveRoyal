
using UnityEngine;

public class PlayerShot : MonoBehaviour
{
    [HideInInspector]
    public Transform _shotTransform;
    [HideInInspector]
    public Rigidbody2D _shotRigidBody2D;
    //public const float BALL_ROTATION_SPEED = 1800;
    [HideInInspector]
    public GameObject _gameObject;
    [HideInInspector]
    public TrailRenderer _trailRenderer;
    [HideInInspector]
    public int _harm;



    private void Awake()
    {
        _trailRenderer = gameObject.GetComponent<TrailRenderer>();
    }

    private void Start()
    {
        _gameObject = gameObject;
        _shotTransform = transform;
        _shotRigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _trailRenderer.Clear();
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<EnemyUnit>(out EnemyUnit enemyUnit))
        {
            enemyUnit.reduceHP(_harm);
            _gameObject.SetActive(false);
        }
        _trailRenderer.Clear();
    }


    // Update is called once per frame
    void Update()
    {
        if (_shotTransform.position.y > CommonData.instance.vertScreenSize / 2 + 1 || _shotTransform.position.y < -CommonData.instance.vertScreenSize / 2 - 1)
        {
            gameObject.SetActive(false);
        }
    }
    //private void FixedUpdate()
    //{
    //    //_shotRigidBody2D.MoveRotation(_shotRigidBody2D.rotation - BALL_ROTATION_SPEED * Time.fixedDeltaTime);
    //}
}
