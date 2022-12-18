using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [HideInInspector]
    public Transform _knifeTransform;
    [HideInInspector]
    public Rigidbody2D _knifeRigidBody2D;
    public const float BALL_ROTATION_SPEED = 1800;
    public GameObject _gameObject;
    public TrailRenderer _trailRenderer;

    private void Awake()
    {
        _trailRenderer = gameObject.GetComponent<TrailRenderer>();
    }

    private void Start()
    {
        _gameObject = gameObject;
        _knifeTransform = transform;
        _knifeRigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _trailRenderer.Clear();
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        _trailRenderer.Clear();
    }
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        _trailRenderer.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if (_knifeTransform.position.y > CommonData.instance.vertScreenSize / 2 + 1 || _knifeTransform.position.y < -CommonData.instance.vertScreenSize / 2-1)
        {
            gameObject.SetActive(false);
        }
    }
    private void FixedUpdate()
    {
        _knifeRigidBody2D.MoveRotation(_knifeRigidBody2D.rotation - BALL_ROTATION_SPEED * Time.fixedDeltaTime);
    }
}
