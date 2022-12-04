using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HeroShooting : MonoBehaviour
{
    public KeyCode shootKey;
    public KeyCode previousGun;
    public KeyCode nextGun;

    public GameObject[] Gun;

    public int CurrentGunNum;

    private MachineGunAttributes MachineGunAttributes;
    private HeroMovement heroMovement;
    private float spreading;
    private int ammoNumber;
    private float timeCd;
    private bool cd;
    private float timeReloading;
    private bool reloading;
    // Start is called before the first frame update
    void Start()
    {
        heroMovement = gameObject.GetComponent<HeroMovement>();
        CurrentGunNum = 0;
        SetupGun(CurrentGunNum);
    }

    private void SetupGun(int num)
    {
        MachineGunAttributes = Gun[num].GetComponent<MachineGunAttributes>();

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
        if (Input.GetKeyDown(previousGun))
        {
            CurrentGunNum -= 1;
            if (CurrentGunNum < 0) CurrentGunNum = Gun.Length - 1;
            SetupGun(CurrentGunNum);

            OnGunChanged(new GunChangedEventArgs 
            { 
                Gun = Gun[CurrentGunNum],
                GunNumber = CurrentGunNum,
                HasChangedOnNextGun = false
            });
        }

        if (Input.GetKeyDown(nextGun))
        {
            CurrentGunNum += 1;
            if (CurrentGunNum > Gun.Length - 1) CurrentGunNum = 0;
            SetupGun(CurrentGunNum);

            OnGunChanged(new GunChangedEventArgs
            {
                Gun = Gun[CurrentGunNum],
                GunNumber = CurrentGunNum,
                HasChangedOnNextGun = true
            });
        }

        if ((Input.GetKeyDown(shootKey) && !MachineGunAttributes.isAuto) || (Input.GetKey(shootKey) && MachineGunAttributes.isAuto))
        {
            if (!cd && !reloading && IsHeroNotRun())
            {
                Vector3 vec = GetComponent<HeroMovement>().LookDirection;
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
                    spreading += (MachineGunAttributes.maximalSpreading- MachineGunAttributes.minimalSpreading) * MachineGunAttributes.rate / MachineGunAttributes.timeToMaximalSpreading;
                    if (spreading > MachineGunAttributes.maximalSpreading) spreading = MachineGunAttributes.maximalSpreading;
                }
            }
        }

        spreading -= MachineGunAttributes.maximalSpreading / MachineGunAttributes.relaxTime * Time.deltaTime;
        if (spreading < MachineGunAttributes.minimalSpreading) spreading = MachineGunAttributes.minimalSpreading;

        if (cd) timeCd -= Time.deltaTime;
        if (timeCd <= 0)
        {
            cd = false;
            timeCd = MachineGunAttributes.rate;
        }
        if (reloading) timeReloading -= Time.deltaTime;
        if (timeReloading <= 0)
        {
            reloading = false;
            timeReloading = MachineGunAttributes.reload;
            ammoNumber = MachineGunAttributes.magazineVolume;
        }
    }

    private bool IsHeroNotRun()
    {
        return heroMovement == null || !heroMovement.RunIndicator;
    }

    public delegate void GunChangedHandler(object sender, GunChangedEventArgs e);

    public event GunChangedHandler GunChanged;

    protected void OnGunChanged(GunChangedEventArgs e)
    {
        GunChanged?.Invoke(this, e);
    }

    public class GunChangedEventArgs
    {
        public GameObject Gun { get; set; }

        public int GunNumber { get; set; }

        public bool HasChangedOnNextGun { get; set; }
    }
}