using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed;
    public GameObject Player;
    public float verOffset;
    public float horOffset;
    public float h;
    public float ux; //угол по x
    public float uy; //угол по y
    public float uz; //угол по z
    public KeyCode leftRotateKey;
    public KeyCode rightRotateKey;

    public KeyCode scopeKey;
    public GameObject scopeCanvas;

    private bool scopeMode;
    private float maxScopeRange;
    private float scopeShow;

    void Start()
    {
        scopeCanvas.SetActive(false);
        scopeMode = false;
    }
    void Update()   
    {
        if (IsGunWithScope())
        {
            if (Input.GetKeyDown(scopeKey))
            {
                scopeMode = !scopeMode;
                if (scopeMode)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    scopeCanvas.SetActive(true);
                    SetupScope();
                }
                else
                {
                    Cursor.lockState = CursorLockMode.None;
                    scopeCanvas.SetActive(false);
                }
            }
        }
        else
        {
            scopeMode = false;
            Cursor.lockState = CursorLockMode.None;
            scopeCanvas.SetActive(false);
        }
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (scopeMode)
        {
            ScopeMovementV2();
        }
        else
        {
            Vector3 position = Player.transform.position;

            position.x += horOffset;
            position.z += -verOffset;
            position.y += h;

            transform.position = position;
        }

        transform.eulerAngles = new Vector3(ux, uy, uz);
    }

    private bool IsGunWithScope()
    {
        var shooting = Player.GetComponent<HeroShooting>();
        if (shooting is null) return false;
        if (shooting.Gun.Length == 0) return false;

        var gun = shooting.Gun[shooting.CurrentGunNum];

        var attributes = gun.GetComponent<MachineGunAttributes>();
        if (attributes != null && attributes.hasScope)
            return true;

        return false;
    }

    private void SetupScope()
    {
        var shooting = Player.GetComponent<HeroShooting>();
        var gun = shooting.Gun[shooting.CurrentGunNum];

        var attributes = gun.GetComponent<MachineGunAttributes>();

        maxScopeRange = attributes.scopeRange;
        scopeShow = attributes.scopeShow;

        Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {


            transform.position = hit.point + GetScopeOffset();
        }
    }

    private Vector3 GetScopeOffset()
    {
        var offsetVector = new Vector3();
        offsetVector.x = horOffset;
        offsetVector.z = -verOffset * scopeShow;
        offsetVector.y = h * scopeShow;

        return offsetVector;
    }

    private void ScopeMovementV1()
    {
        var delta = new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y"));

        var lookPosition = transform.position - GetScopeOffset() + delta;
        lookPosition.y = 0;

        var playerPosition = Player.transform.position;
        playerPosition.y = 0;

        var rangeVector = lookPosition - playerPosition;
        var range = Vector3.Magnitude(rangeVector);
        if (range <= maxScopeRange)
        {
            transform.position += delta;
        }
        else
        {
            var fitRangeVector = rangeVector / range * maxScopeRange;
            lookPosition = playerPosition + fitRangeVector;

            lookPosition += GetScopeOffset();

            transform.position = lookPosition;
        }
    }

    private void ScopeMovementV2()
    {
        var delta = new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y"));

        var heroMovement = Player.GetComponent<HeroMovement>();
        var speed = heroMovement.Speed;
        var moveVector = heroMovement.MoveVector;
        var deltaMovement = moveVector * speed * Time.deltaTime;

        var lookPosition = transform.position - GetScopeOffset() + delta;
        lookPosition.y = 0;

        var playerPosition = Player.transform.position;
        playerPosition.y = 0;

        var rangeVector = lookPosition - playerPosition;
        var range = Vector3.Magnitude(rangeVector);

        Vector3 relVector;
        if (range <= maxScopeRange)
        {
            transform.position += delta + deltaMovement;
        }
        else
        {
            var fitRangeVector = rangeVector / range * maxScopeRange;
            lookPosition = playerPosition + fitRangeVector;

            lookPosition += GetScopeOffset();

            transform.position = lookPosition;
        }
    }
}
