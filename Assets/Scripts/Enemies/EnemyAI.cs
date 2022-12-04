using System.Linq;
using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float HeroNoticeDistance;
    public float HeroMissingTime;
    public float ChillProbability;

    private float heroCurrentMissingTime;

    private EnemyFollowHero FindHeroBehaviour;
    private EnemyMovement PatrolBehaviour;
    private EnemyChill ChillingBehaviour;

    private bool wasChilling;

    private NavMeshAgent NavMeshAgent;
    // Start is called before the first frame update
    void Start()
    {
        FindHeroBehaviour = GetComponent<EnemyFollowHero>();
        PatrolBehaviour = GetComponent<EnemyMovement>();
        ChillingBehaviour = GetComponent<EnemyChill>();
        NavMeshAgent = GetComponent<NavMeshAgent>();

        PatrolBehaviour.enabled = true;
        FindHeroBehaviour.enabled = false;
        ChillingBehaviour.enabled = false;
        PatrolBehaviour.Start();

        wasChilling = false;

        ChillingBehaviour.ChillingEnded += OnChillingEndedHandler;
        PatrolBehaviour.CameToPoint += OnCameToPointHandler;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(UnityTags.WithTag.ToString())
            .Where(o => o.GetComponent<Tags>().EnemyTarget)
            .Where(o => !StaticMethods.HasWallsBetween(gameObject, o))
            .ToArray();
        if (gameObjects.Length > 0)
        {
            var player = StaticMethods.GetNearestObject(gameObject, gameObjects, out var distance, true);
            heroCurrentMissingTime = HeroMissingTime;

            if (distance <= HeroNoticeDistance)
            {
                PatrolBehaviour.enabled = false;
                FindHeroBehaviour.enabled = true;

                FindHeroBehaviour.player = player;
            }
            else
            {
                RejectPersecution();
            }
        }
        else if (FindHeroBehaviour.player != null)
        {
            heroCurrentMissingTime -= Time.deltaTime;
            if (heroCurrentMissingTime <= 0)
            {
                RejectPersecution();
            }
        }
        else if (FindHeroBehaviour.player == null)
        {
            RejectPersecution();
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
