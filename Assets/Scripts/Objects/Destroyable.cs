using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    private HealthPoints HealthPoints;
    // Start is called before the first frame update
    void Start()
    {
        HealthPoints = gameObject.GetComponent<HealthPoints>();
    }

    // Update is called once per frame
    void Update()
    {
        if (HealthPoints.CurrentHP <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        //Debug.Log("(Destroyed) Object: " + gameObject.name);
    }
}
