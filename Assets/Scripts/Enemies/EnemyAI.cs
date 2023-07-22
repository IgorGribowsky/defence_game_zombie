using System.Linq;
using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float HeroNoticeDistance;
    public float HeroMissingTime;
    public float ChillProbability;

    public EnemyFollowHero FindHeroBehaviour;
    public EnemyMovement PatrolBehaviour;
    public EnemyChill ChillingBehaviour;
    public EnemyMeleeAttack AttackBehaviour;

    private float heroCurrentMissingTime;

    private bool wasChilling;
    public GameObject[] gos;
    // Start is called before the first frame update
    void Start()
    {
        FindHeroBehaviour = GetComponent<EnemyFollowHero>();
        PatrolBehaviour = GetComponent<EnemyMovement>();
        ChillingBehaviour = GetComponent<EnemyChill>();
        AttackBehaviour = GetComponent<EnemyMeleeAttack>();

        PatrolBehaviour.enabled = true;
        FindHeroBehaviour.enabled = false;
        ChillingBehaviour.enabled = false;
        PatrolBehaviour.Start();

        wasChilling = false;

        ChillingBehaviour.ChillingEnded += OnChillingEndedHandler;
        PatrolBehaviour.CameToPoint += OnCameToPointHandler;

        gos = GameObject.FindGameObjectsWithTag(UnityTags.WithTag.ToString())
        .Where(o => o.GetComponent<Tags>().EnemyTarget)
        .ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        if (FindHeroBehaviour.enabled && heroCurrentMissingTime < 0)
        {
            RejectPersecution();
        }
        //GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(UnityTags.WithTag.ToString())
        //    .Where(o => o.GetComponent<Tags>().EnemyTarget)
        //    .Where(o => !StaticMethods.HasWallsBetween(gameObject, o))
        //    .ToArray();
        var gameObjects = gos.Where(o => !StaticMethods.HasWallsBetween(gameObject, o)).ToArray();
        //GameObject[] gameObjects = new GameObject[0];
        if (gameObjects.Count() > 0)
        {
            var player = StaticMethods.GetNearestObject(gameObject, gameObjects, out var distance, true);
            heroCurrentMissingTime = HeroMissingTime;

            if (distance <= HeroNoticeDistance)
            {
                PatrolBehaviour.enabled = false;
                FindHeroBehaviour.enabled = true;

                FindHeroBehaviour.player = player;
                AttackBehaviour.Attack(player, distance);
            }
            else
            {
                RejectPersecution();
            }
        }
        else if (FindHeroBehaviour.enabled)
        {
            heroCurrentMissingTime -= Time.deltaTime;

        }
    }

    public void RejectPersecution()
    {
        FindHeroBehaviour.player = null;
        FindHeroBehaviour.enabled = false;

        if (wasChilling)
        {
            ChillingBehaviour.enabled = true;
        }
        else
        {
            PatrolBehaviour.enabled = true;
            PatrolBehaviour.Start();
        }
    }

    protected void OnChillingEndedHandler(object sender, EnemyChill.ChillingEndedEventArgs e)
    {
        wasChilling = false;

        PatrolBehaviour.enabled = true;
        PatrolBehaviour.Start();
    }

    protected void OnCameToPointHandler(object sender, EnemyMovement.CameToPointEventArgs e)
    {
        var randomValue = UnityEngine.Random.Range(0, 1f);
        if (randomValue < ChillProbability)
        {
            wasChilling = true;
            PatrolBehaviour.enabled = false;
            ChillingBehaviour.enabled = true;
            ChillingBehaviour.StartChillingTime();
        }
    }
}
