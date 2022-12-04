using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public int CurrentPointIndex = 0;
    public float Speed = 10f;
    public List<Vector3> Points;
    public bool Reverse = false;
    public bool Loop = false;
    public float acceptanceDistance = 3f;

    private NavMeshAgent agent;


    // Start is called before the first frame update
    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GoToCurrentPoint();
    }

    // Update is called once per frame
    void Update()
    {
        var currentPoint = Points[CurrentPointIndex];
        float curDistance = (transform.position - currentPoint).sqrMagnitude;
        if (curDistance < acceptanceDistance)
        {
            var tempCurrentPointIndex = CurrentPointIndex;

            var change = Reverse ? -1 : +1;
            CurrentPointIndex += change;
            if (CurrentPointIndex >= Points.Count)
            {
                if (Loop)
                {
                    CurrentPointIndex = 0;
                }
                else
                {
                    CurrentPointIndex = Points.Count - 1;
                    return;
                }
            }

            if (CurrentPointIndex < 0)
            {
                if (Loop)
                {
                    CurrentPointIndex = Points.Count - 1;
                }
                else
                {
                    CurrentPointIndex = 0;
                    return;
                }
            }
            GoToCurrentPoint();

            OnCameToPoint(new CameToPointEventArgs()
            {
                PointNumber = CurrentPointIndex,
                PointPosition = currentPoint,
            });
        }
    }

    public void GoToCurrentPoint()
    {
        var currentPoint = Points[CurrentPointIndex];
        if (currentPoint != null)
        {
            agent.isStopped = false;
            agent.speed = Speed;
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

    #region Came To Point Event

    public delegate void CameToPointHandler(object sender, CameToPointEventArgs e);

    public event CameToPointHandler CameToPoint;

    protected void OnCameToPoint(CameToPointEventArgs e)
    {
        CameToPoint?.Invoke(this, e);
    }

    public class CameToPointEventArgs
    {
        public int PointNumber { get; set; }

        public Vector3 PointPosition { get; set; }
    }

    #endregion
}
