using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletBehaviour : MonoBehaviour
{
    public float Speed { get; set; }
    public float FlightTime { get; set; }
    public float Damage { get; set; }
    public Vector3 Vector { get; set; }
    public GameObject Shooter { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(transform.position + new Vector3(Vector.x, 0, Vector.z));
        transform.Rotate(new Vector3(0, 90, 0));
    }

    // Update is called once per frame
    void Update()
    {
        var dPosition = Vector * Time.deltaTime * Speed;
        transform.position += dPosition;

        RaycastHit hit;
        if (Physics.Linecast(transform.position, transform.position - dPosition, out hit))
        {
            OnTriggerEnter(hit.collider);
        }

        FlightTime -= Time.deltaTime;
        if (FlightTime <= 0) Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.Equals(Shooter))
        {
            var tags = other.gameObject.GetComponent<Tags>();

            if (tags == null)
            {
                return;
            }

            if (tags.Wall)
            {
                Destroy(gameObject);
            }

            if (tags.DestroyableObject)
            {
                var healthPoints = other.GetComponent<HealthPoints>();
                healthPoints.GetDamage(Damage);

                Destroy(gameObject);
            }
        }
    }
}
