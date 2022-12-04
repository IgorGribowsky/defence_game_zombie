using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollowHero : MonoBehaviour
{
    //public float visibleDistance = 50f;
    public float speed = 10f;

    public GameObject player;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(UnityTags.WithTag.ToString())
        //    .Where(o => o.GetComponent<Tags>().EnemyTarget)
        //    .Where(o => !StaticMethods.HasWallsBetween(gameObject, o))
        //    .ToArray();
        //if (gameObjects.Length > 0)
        //{
        //    player = StaticMethods.GetNearestObject(gameObject, gameObjects, out var distance, true);

        //    if (distance <= visibleDistance)
        //    {
        agent.isStopped = false;
                agent.speed = speed;
                agent.avoidancePriority = (int)Math.Ceiling(Vector3.Distance(player.transform.position, gameObject.transform.position) * 10);
                agent.SetDestination(player.transform.position);
        //    }
        //    else
        //    {
        //        agent.isStopped = true;
        //        player = null;
        //    }
        //}
        //else
        //{
        //    player = null;
        //}
    }
}
