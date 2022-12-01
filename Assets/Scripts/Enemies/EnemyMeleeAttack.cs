using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    public float damage = 16;
    public float rate = 0.6f;
    public float attackDistance = 0.5f;
    public float attackDuration = 0.1f;

    private float currentAttackDuration;
    private float timeCd;
    private bool cd;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        timeCd = rate;
        currentAttackDuration = attackDuration;
        cd = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeCd <= 0)
        {
            cd = false;
            timeCd = rate;
        }

        if (cd)
        {
            timeCd -= Time.deltaTime;
            return;
        }

        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("WithTag")
            .Where(o => o.GetComponent<Tags>().EnemyTarget)
            .ToArray();

        if (gameObjects.Length <= 0)
        {
            return;
        }

        player = StaticMethods.GetNearestObject(gameObject, gameObjects, out var distance, true);

        if (distance <= attackDistance)
        {
            currentAttackDuration -= Time.deltaTime;
            if (currentAttackDuration <= 0)
            {
                var playerHP = player.GetComponent<HealthPoints>();
                playerHP.GetDamage(damage, gameObject);
                cd = true;
                currentAttackDuration = attackDuration;
            }
        }
        else
        {
            currentAttackDuration = attackDuration;
        }
    }
}
