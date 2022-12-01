using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class HPBarScript : MonoBehaviour
{
    public GameObject target;
    public bool IsUnit;

    private bool isBarActive = false;
    private HealthPoints healthPoints;

    private GameObject maxHpBar
    {
        get { return gameObject.transform.GetChild(0).gameObject; }
    }

    private GameObject currentHpBar
    {
        get { return gameObject.transform.GetChild(1).gameObject; }
    }

    public void SetFlagIsDamaged(bool flag)
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        if (target != null)
        {
            healthPoints = target.GetComponent<HealthPoints>();
        }

        if (IsUnit)
        {
            isBarActive = false;
            maxHpBar.SetActive(false);
            currentHpBar.SetActive(false);
            healthPoints.Damaged += OnDamagedHandler;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsUnit || isBarActive)
        {
            var percent = healthPoints.CurrentHP / healthPoints.MaximumHP;

            var currentHpBarScale = currentHpBar.transform.localScale;

            var newCurrentHpBarScale = new Vector3(percent, currentHpBarScale.y, currentHpBarScale.z);

            currentHpBar.gameObject.transform.localScale = newCurrentHpBarScale;

            if (IsUnit)
            {
                transform.LookAt(transform.position + Camera.main.transform.forward);
            }
        }

    }
    private void OnDamagedHandler(object sender, DamagedEventArgs e)
    {
        if (!isBarActive)
        {
            isBarActive = true;
            maxHpBar.SetActive(true);
            currentHpBar.SetActive(true);
        }
    }
}
