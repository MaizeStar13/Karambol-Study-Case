
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Line")]
    [Tooltip("Masukan Line Gameobject yang ada pada spheres kesini, Line Renderer 'Use World Space' = true")]
    [SerializeField] private LineRenderer _linerender;

    [Header("RigidBody")]
    [Tooltip("RigidBody Spheres masuk sini")]
    private Rigidbody _RigidB;

    [Header("Velocity")]
    [Tooltip("Movement Bola")]
    [SerializeField] private float _stopvelocity = 0.5f;
    [SerializeField] private float _shotpower = 100f;

    private bool _isidle;
    private bool _isaiming;

    [Header("MoveLimit")]
    [Tooltip("Banyak batas gerak")]
    [SerializeField] private int _maxmoves = 5;
    private int _movecount = 0;

    [Header("RespawnSystem")]
    [Tooltip("")]
    [SerializeField] private GameObject _falldetector;
    private Vector3 _respawnPoint;

    [Header("GameManager")]
    [Tooltip("Masukan Gameobject GameManager")]
    [SerializeField] private GameManager _gamemanager;

    [Header("Enemy Manager")]
    [Tooltip("Referensi ke Enemy Manager untuk status win/lose")]
    [SerializeField] private EnemyManager _enemymanager;

    private void Awake()
    {
        _RigidB = GetComponent<Rigidbody>();

        _isaiming = false;
        _linerender.enabled = false;

        _respawnPoint = transform.position;
    }

    public void Start()
    {
        _enemymanager = FindObjectOfType<EnemyManager>();
    }

    private void FixedUpdate()
    {
        if (_RigidB.velocity.magnitude < _stopvelocity)
        {
            Stop();
        }

        ProcessAim();

    }

    private void Stop()
    {
        _RigidB.velocity = Vector3.zero;
        _RigidB.angularVelocity = Vector3.zero;
        _isidle = true;
    }

    private void OnMouseDown()
    {
        if (_isidle)
        {
            _isaiming = true;
        }
    }

    private void ProcessAim()
    {
        if (!_isidle || !_isaiming)
        {
            return;
        }

        Vector3? worldPoint = CastMouseClickRaycast();
        if (!worldPoint.HasValue)
        {
            return;
        }

        DrawLine(worldPoint.Value);

        if (Input.GetMouseButtonUp(0))
        {
            Shoot(worldPoint.Value);
        }
    }

    private void Shoot(Vector3 worldPoint)
    {
        _isaiming = false;
        _linerender.enabled = false;

        //konstruktor
        Vector3 horizontalWorldPoint = new Vector3(worldPoint.x, transform.position.y, worldPoint.z);

        Vector3 direction = (horizontalWorldPoint - transform.position).normalized;
        float strength = Vector3.Distance(transform.position, horizontalWorldPoint);

        //-direction untuk flip 
        _RigidB.AddForce(direction * strength * _shotpower);

        Condition();
    }

    private void DrawLine(Vector3 worldPoint)
    {
        Vector3[] positions =
        {
            transform.position, worldPoint
        };
        _linerender.SetPositions(positions);
        _linerender.enabled = true;
    }

    private Vector3? CastMouseClickRaycast()
    //penggunaan vector3? akan mengembalikan hasil NULL jika objek tidak terdeteksi
    {
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        if (Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit, float.PositiveInfinity))
        {
            return hit.point;
        }
        else
        {
            return null;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("OOB"))
        {
            transform.position = _respawnPoint;
        }
    }
    private void Condition()
    {

        _movecount++;


        if ((_movecount >= _maxmoves) && (_enemymanager != null) && !_enemymanager._winorlose  ) 
        {
            if (_gamemanager)
            {
                _gamemanager.Losing();
            }
        }
    }
}
