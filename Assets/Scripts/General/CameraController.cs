using System;
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

    private HeroShooting heroShooting;
    private Destroyable playerDestroyable;
    private HeroMovement heroMovement;

    void Start()
    {
        scopeCanvas.SetActive(false);
        scopeMode = false;
        heroShooting = Player.GetComponent<HeroShooting>();
        playerDestroyable = Player.GetComponent<Destroyable>();
        heroMovement = Player.GetComponent<HeroMovement>();

        heroShooting.GunChanged += OnGunChangedHandler;
        playerDestroyable.Destroyed += OnPlayerDestroyedHandler;
    }
    void Update()   
    {
        if (Input.GetKeyDown(scopeKey) && IsGunWithScope())
        {
            if (!scopeMode)
            {
                SetupScope();
            }
            else
            {
                ScopeOut();
            }
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
            if (Player != null)
            {
                Vector3 position = Player.transform.position;

                position.x += horOffset;
                position.z += -verOffset;
                position.y += h;

                transform.position = position;
            }
        }

        transform.eulerAngles = new Vector3(ux, uy, uz);
    }

    protected void OnGunChangedHandler(object sender, HeroShooting.GunChangedEventArgs e)
    {
        ScopeOut();
    }

    protected void OnPlayerDestroyedHandler(object sender, Destroyable.DestroyedEventArgs e)
    {
        ScopeOut();
    }

    private bool IsGunWithScope()
    {
        if (heroShooting is null) return false;
        if (heroShooting.Gun.Length == 0) return false;

        var gun = heroShooting.Gun[heroShooting.CurrentGunNum];

        var attributes = gun.GetComponent<MachineGunAttributes>();
        if (attributes != null && attributes.hasScope)
            return true;

        return false;
    }

    private void ScopeOut()
    {
        scopeMode = false;
        Cursor.lockState = CursorLockMode.None;
        scopeCanvas.SetActive(false);
    }

    private void SetupScope()
    {
        scopeMode = true;
        Cursor.lockState = CursorLockMode.Locked;
        scopeCanvas.SetActive(true);

        var gun = heroShooting.Gun[heroShooting.CurrentGunNum];

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

    private void ScopeMovementV2()
    {
        var delta = new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y"));

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
