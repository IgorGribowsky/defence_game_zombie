using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBarScript : MonoBehaviour
{
    public GameObject unit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var percent = 0f;
        if (unit != null)
        {
            var healthPoints = unit.GetComponent<HealthPoints>();

            percent = healthPoints.CurrentHP / healthPoints.MaximumHP;
        }

        var currentHpBarScale = gameObject.transform.GetChild(1).gameObject.transform.localScale;

        var newCurrentHpBarScale = new Vector3(percent, currentHpBarScale.y, currentHpBarScale.z);

        gameObject.transform.GetChild(1).gameObject.transform.localScale = newCurrentHpBarScale;
    }
}
