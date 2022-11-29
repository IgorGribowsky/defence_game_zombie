using UnityEngine;

public class MachineGunAttributes : MonoBehaviour
{
    public GameObject bullet;

    public bool isAuto = false;

    public float bulletDamage;
    public float bulletSpeed;
    public float bulletFlightTime;
    public float rate = 0.5f;
    public float reload = 1.5f;
    public int magazineVolume = 12;

    public bool isSpreaded = false;
    public float maximalSpreading = 30f;
    public float minimalSpreading = 0f;
    public float relaxTime = 4f;
    public float timeToMaximalSpreading = 1.5f;

    public bool hasScope = false;
    public float scopeRange = 10f;
    public float scopeShow = 1;
}
