

using UnityEngine;


public class AimController : MonoBehaviour
{
    //[SerializeField]
    //private Joystick joystick;
    private Transform _transform;
    [HideInInspector]
    private float _vertScreenSize;
    [HideInInspector]
    private float _horisScreenSize;

    // X Y radius
    public Vector2 Velocity = new Vector2(1, 0);

    // rotational direction
    public bool Clockwise = true;

    public float RotateSpeed;
    public float RotateRadiusX;
    public float RotateRadiusY;

    private Vector2 _centre;
    private float _angle;

    private float widthOfAimSprite;

    private void Awake()
    {
        widthOfAimSprite = GetComponent<SpriteRenderer>().bounds.size.x/2;
        _centre = Vector2.zero;
        RotateSpeed = 0.7f;
        _transform = transform;
    }

    private void Start()
    {
        _vertScreenSize = CommonData.instance.vertScreenSize / 2;
        _horisScreenSize = CommonData.instance.horisScreenSize / 2;
        RotateRadiusX = _horisScreenSize- widthOfAimSprite;
        RotateRadiusY = RotateRadiusX;
    }


    private void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch _touch = Input.GetTouch(0);//

            //if (_touch.phase == TouchPhase.Began)
            //{
            //    _transform.position = _touch.position;
            //}
            if (_touch.phase == TouchPhase.Moved)
            {
                Vector2 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(_touch.position.x, _touch.position.y, 0));
                _transform.position = worldPosition;
            }
        }


        CommonData.instance.shotDirection = _transform.position;
    }

    private void FixedUpdate()
    {
        if (Input.touchCount == 1)
        {
            _transform.position = new Vector2(Mathf.Clamp(_transform.position.x, -_horisScreenSize, _horisScreenSize),
                Mathf.Clamp(_transform.position.y, -_vertScreenSize, _vertScreenSize));
        }
        else if(Input.touchCount == 0)
        {
            //_centre += Velocity * Time.deltaTime;
            _angle += (Clockwise ? RotateSpeed : -RotateSpeed) * Time.deltaTime;
            var x = Mathf.Sin(_angle) * RotateRadiusX;
            var y = Mathf.Cos(_angle) * RotateRadiusY;
            _transform.position = _centre + new Vector2(x, y);
        }
    }

}
