using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public int currentPointIndex;
    public float speed = 10f;
    public List<Vector3> points;
    public bool reverse = false;
    public bool loop = false;
    public float acceptanceDistance = 3f;

    private NavMeshAgent agent;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GoToCurrentPoint();
    }

    // Update is called once per frame
    void Update()
    {
        var currentPoint = points[currentPointIndex];
        float curDistance = (transform.position - currentPoint).sqrMagnitude;
        if (curDistance < acceptanceDistance)
        {
            var change = reverse ? -1 : +1;
            currentPointIndex += change;
            if (currentPointIndex >= points.Count)
            {
                if (loop)
                {
                    currentPointIndex = 0;
                }
                else
                {
                    currentPointIndex = points.Count - 1;
                    return;
                }
            }

            if (currentPointIndex < 0)
            {
                if (loop)
                {
                    currentPointIndex = points.Count - 1;
                }
                else
                {
                    currentPointIndex = 0;
                    return;
                }
            }

            GoToCurrentPoint();
        }
    }

    public void GoToCurrentPoint()
    {
        var currentPoint = points[currentPointIndex];
        if (currentPoint != null)
        {
            agent.isStopped = false;
            agent.speed = speed;
            agent.avoidancePriority = 100;
            agent.SetDestination(currentPoint);
        }
    }

    public void Stop()
    {
         agent.isStopped = true;
    }

    public void Go()
    {
        agent.isStopped = false;
    }
}
