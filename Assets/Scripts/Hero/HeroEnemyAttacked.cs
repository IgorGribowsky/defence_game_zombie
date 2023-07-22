using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEditor.PlayerSettings;
using System.Linq;
using System.Reflection.Emit;

public class HeroEnemyAttacked : MonoBehaviour
{
    private GameObject[] enemies;

    public GameObject[] Enemies { 
        get { return enemies; }
    }

    // Start is called before the first frame update
    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag(UnityTags.WithTag.ToString())
        .Where(o => o.GetComponent<Tags>().EnemyTarget)
        .ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        var nearestEnemies = enemies
            .Where(o => StaticMethods.GetDistance(o, gameObject, true) <= o.GetComponent<EnemyAI>().HeroNoticeDistance)
            .Where(o => !StaticMethods.HasWallsBetween(gameObject, o))
            .ToArray();

        var lostAimEnemies = enemies.Except(nearestEnemies).Where(o => o.GetComponent<EnemyAI>().FindHeroBehaviour.enabled);

        foreach (var enemy in lostAimEnemies)
        { 
            
        }
        //foreach(var enemy in gameObjects)
        //{
        //    var player = StaticMethods.GetNearestObject(gameObject, gameObjects, out var distance, true);
        //    heroCurrentMissingTime = HeroMissingTime;

        //    if (distance <= HeroNoticeDistance)
        //    {
        //        PatrolBehaviour.enabled = false;
        //        FindHeroBehaviour.enabled = true;

        //        FindHeroBehaviour.player = player;
        //        AttackBehaviour.Attack(player, distance);
        //    }
        //    else
        //    {
        //        RejectPersecution();
        //    }
        //}
        //else if (FindHeroBehaviour.enabled)
        //{
        //    heroCurrentMissingTime -= Time.deltaTime;
        //    if (heroCurrentMissingTime < 0)
        //    {
        //        RejectPersecution();
        //    }
        //}
    }
}
