using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollowHero : MonoBehaviour
{
    public float visibleDistance = 50f;
    public float speed = 10f;

    private GameObject player;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("WithTag").Where(o => o.GetComponent<Tags>().EnemyTarget).ToArray();
        if (gameObjects.Length > 0)
        {
            float distance = Mathf.Infinity;

            //Поиск ближайшего противника
            foreach (GameObject o in gameObjects)
            {
                float curDistance = (o.transform.position - transform.position).sqrMagnitude;
                if (curDistance < distance)
                {
                    player = o;
                    distance = curDistance;
                }
            }
            if (distance <= visibleDistance)
            {
                agent.isStopped = false;
                agent.speed = speed;
                agent.avoidancePriority = (int)Math.Ceiling(Vector3.Distance(player.transform.position, gameObject.transform.position) * 10);
                agent.SetDestination(player.transform.position);
            }
            else
            {
                agent.isStopped = true;
            }
        }

    }
}
