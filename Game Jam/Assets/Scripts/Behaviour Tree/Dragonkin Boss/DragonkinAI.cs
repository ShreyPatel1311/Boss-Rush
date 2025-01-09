using UnityEngine;
using BehaviourTreeArchitecture;
using System.Collections.Generic;
using UnityEngine.AI;

public class DragonkinAI : MonoBehaviour
{
    //[SerializeField] private List<Transform> patrolPoints;
    //[SerializeField] private GameObject treasure;
    //[SerializeField] private int priority;
    //[SerializeField] private GameObject treasure2;
    //[SerializeField] private int priority2;

    [SerializeField] private GameObject playerRef;
    [SerializeField] private DragonkinConfigurationSO config;
 
    [Header("Phase 1")]
    [SerializeField] private List<AnimationClip> phase1LightAnimations;
    [SerializeField] private List<AnimationClip> phase1ComboAnimations;
    [SerializeField] private List<AnimationClip> phase1LongRangeAnimations;

    [Header("Phase 2")]
    [SerializeField] private List<AnimationClip> phase2LightAnimations;
    [SerializeField] private List<AnimationClip> phase2ComboAnimations;

    private NavMeshAgent agent;
    private BehaviourTree tree;
    private Vector3 destPoint;

    void Awake()
    {
        agent = GetComponentInChildren<NavMeshAgent>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        tree = CreateTreeArchitecture();
        tree.Process();
    }

    //Action Strategy
    private void LightAttackSeq()
    {

    }

    //Action strategy
    private void HeavyAttackSeq()
    {

    }

    private BehaviourTree CreateTreeArchitecture()
    {
        BehaviourTree tree = new BehaviourTree("Dragonkin");
        //tree.AddChild(new Leaf("Patrol", new PatrolStrategy(transform, agent, patrolPoints)));

        //Sequence goToTreasure = new Sequence("GoToTreasure", priority);
        //goToTreasure.AddChild(new Leaf("IsTreasurePresent", new Condition(() => treasure.activeSelf)));
        //goToTreasure.AddChild(new Leaf("MoveToTreasure", new ActionStrategy(() => agent.SetDestination(treasure.transform.position))));

        //Sequence goToTreasure2 = new Sequence("GoToTreasure2", priority2);
        //goToTreasure2.AddChild(new Leaf("IsTreasure2Present", new Condition(() => treasure2.activeSelf)));
        //goToTreasure2.AddChild(new Leaf("MoveToTreasure2", new ActionStrategy(() => agent.SetDestination(treasure2.transform.position))));

        //PrioritySelector goTotreasures = new PrioritySelector("GoToTreasures", 4);
        //goTotreasures.AddChild(goToTreasure2);
        //goTotreasures.AddChild(goToTreasure);

        //tree.AddChild(goTotreasures);

        Selector phaseChange = new Selector("Phase Change", 10);

        Selector phase1 = new Selector("Attack Selection", 8);
        Sequence lightAttack = new Sequence("Light attacks", 6);
        lightAttack.AddChild(new Leaf("Light Attacks", new Condition(() => (Vector3.Distance(transform.position, playerRef.transform.position) <= config.lightAttackDistance))));




        return tree;
    }
}
