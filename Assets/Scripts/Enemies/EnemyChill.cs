using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChill : MonoBehaviour
{
    public float Speed = 4f;
    public float Rate = 3f;
    public float StepDistance = 20f;

    public bool EndlessChilling = false;

    public float ChillingPhaseDuration = 30f;
    public bool RandomChillingPhaseDuration = true;
    public float MaxChillingPhaseDuration = 40f;
    public float MinChillingPhaseDuration = 10f;

    private NavMeshAgent agent;
    private float cdTime;
    private bool cd;
    private float chillingPhaseDuration;
    public float currentChillingTime;

    // Start is called before the first frame update
    public void Start()
    {
        cd = false;
        agent = GetComponent<NavMeshAgent>();
        cdTime = Rate;
        if (!EndlessChilling)
        {
            StartChillingTime();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!cd)
        {
            cd = true;
            var randomAngle = UnityEngine.Random.Range(0f, 6.28319f); //generates random angle in radians
            var randomVector = new Vector3(Mathf.Cos(randomAngle),0, Mathf.Sin(randomAngle));
            var promotedVector = randomVector * StepDistance;
            var resultVector = gameObject.transform.position + promotedVector;
            agent.isStopped = false;
            agent.speed = Speed;
            agent.avoidancePriority = 100;
            agent.SetDestination(resultVector);
        }

        if (cd)
        {
            cdTime -= Time.deltaTime;
        }
        if (cdTime <= 0)
        {
            cd = false;
            cdTime = Rate;
        }

        if (!EndlessChilling)
        {
            currentChillingTime -= Time.deltaTime;
            if (currentChillingTime <= 0)
            {
                agent.isStopped = true;
                OnChillingEnded(new ChillingEndedEventArgs { ChillingTime = chillingPhaseDuration });
            }
        }
    }

    public void StartChillingTime()
    {
        chillingPhaseDuration = RandomChillingPhaseDuration ? UnityEngine.Random.Range(MinChillingPhaseDuration, MaxChillingPhaseDuration) : ChillingPhaseDuration;
        currentChillingTime = chillingPhaseDuration;
    }

    public delegate void ChillingEndedHandler(object sender, ChillingEndedEventArgs e);

    public event ChillingEndedHandler ChillingEnded;

    protected void OnChillingEnded(ChillingEndedEventArgs e)
    {
        ChillingEnded?.Invoke(this, e);
    }

    public class ChillingEndedEventArgs
    {
        public float ChillingTime { get; set; }
    }
}
