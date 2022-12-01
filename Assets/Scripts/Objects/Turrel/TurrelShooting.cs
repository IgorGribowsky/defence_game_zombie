using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class TurrelShooting : MonoBehaviour
{
    public GameObject Gun;
    public float attackDistance = 240f;
    public bool canRotate = true;
    public GameObject rotationPart;
    public float rotationCalibration = 0;

    private MachineGunAttributes MachineGunAttributes;
    private float spreading;
    private int ammoNumber;
    private float timeCd;
    private float timeReloading;

    private bool reloading;
    private bool cd;
    // Start is called before the first frame update
    void Start()
    {
        SetupGun();
    }

    private void SetupGun()
    {
        MachineGunAttributes = Gun.GetComponent<MachineGunAttributes>();

        reloading = false;
        cd = false;

        timeCd = MachineGunAttributes.rate;
        timeReloading = MachineGunAttributes.reload;
        ammoNumber = MachineGunAttributes.magazineVolume;
        spreading = MachineGunAttributes.minimalSpreading;
    }

    // Update is called once per frame
    void Update()
    {
        var gameObjects = GameObject.FindGameObjectsWithTag("WithTag")
            .Where(o => o.GetComponent<Tags>().HeroTarget)
            .Where(o => !StaticMethods.HasWallsBetween(gameObject, o))
            .ToArray();

        if (gameObjects.Length > 0)
        {
            var nearestEnemy = StaticMethods.GetNearestObject(gameObject, gameObjects, out var distance, true);

            if (distance <= attackDistance)
            {
                if (canRotate)
                {
                    Rotate(nearestEnemy.transform.position);
                }

                if (!cd && !reloading)
                {
                    Vector3 vec = nearestEnemy.transform.position - gameObject.transform.position;
                    vec.y = 0;

                    float mod = (float)Math.Sqrt(vec.x * vec.x + vec.z * vec.z);
                    vec = vec / mod;

                    float spreadingRad = (float)(spreading * Math.PI / 180);
                    double randomSpreadingRad = UnityEngine.Random.Range(-spreadingRad / 2, spreadingRad / 2);

                    if (!MachineGunAttributes.isSpreaded) randomSpreadingRad = 0;

                    vec.x = vec.x * (float)Math.Cos(randomSpreadingRad) - vec.z * (float)Math.Sin(randomSpreadingRad);
                    vec.z = vec.x * (float)Math.Sin(randomSpreadingRad) + vec.z * (float)Math.Cos(randomSpreadingRad);

                    GameObject newBullet = Instantiate(MachineGunAttributes.bullet, transform.position, transform.rotation);
                    newBullet.GetComponent<BulletBehaviour>().Vector = vec;
                    newBullet.GetComponent<BulletBehaviour>().Damage = MachineGunAttributes.bulletDamage;
                    newBullet.GetComponent<BulletBehaviour>().Speed = MachineGunAttributes.bulletSpeed;
                    newBullet.GetComponent<BulletBehaviour>().FlightTime = MachineGunAttributes.bulletFlightTime;
                    newBullet.GetComponent<BulletBehaviour>().Shooter = gameObject;
                    cd = true;

                    ammoNumber -= 1;
                    if (ammoNumber <= 0)
                        reloading = true;
                    if (MachineGunAttributes.isSpreaded)
                    {
                        spreading += (MachineGunAttributes.maximalSpreading - MachineGunAttributes.minimalSpreading) * MachineGunAttributes.rate / MachineGunAttributes.timeToMaximalSpreading;
                        if (spreading > MachineGunAttributes.maximalSpreading) spreading = MachineGunAttributes.maximalSpreading;
                    }
                }
            }
        }

        

        ReduceSpreading();
        ReduceCd(); 
        ReduceReloading();
    }

    private void Rotate(Vector3 positionToRotate)
    {
        var transform = gameObject.transform;
        var relVecNorm = (positionToRotate - transform.position).normalized;

        var yNewRotation = (float)(Math.Acos(relVecNorm.x / relVecNorm.magnitude) * 180 / Math.PI);

        if (positionToRotate.z > transform.position.z)
        {
            yNewRotation = -yNewRotation;
        }

        var rotateValue = yNewRotation - rotationPart.transform.rotation.eulerAngles.y + rotationCalibration;

        rotationPart.transform.Rotate(new Vector3(0, rotateValue, 0));
    }

    private void ReduceReloading()
    {
        if (reloading) timeReloading -= Time.deltaTime;
        if (timeReloading <= 0)
        {
            reloading = false;
            timeReloading = MachineGunAttributes.reload;
            ammoNumber = MachineGunAttributes.magazineVolume;
        }
    }

    private void ReduceCd()
    {
        if (cd) timeCd -= Time.deltaTime;
        if (timeCd <= 0)
        {
            cd = false;
            timeCd = MachineGunAttributes.rate;
        }
    }

    private void ReduceSpreading()
    {
        spreading -= MachineGunAttributes.maximalSpreading / MachineGunAttributes.relaxTime * Time.deltaTime;
        if (spreading < MachineGunAttributes.minimalSpreading) spreading = MachineGunAttributes.minimalSpreading;
    }
}
