using BehaviourTreeArchitecture;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class TwoHandedAI : MonoBehaviour
{
    [SerializeField] private List<Transform> patrolPoints;
    [SerializeField] private GameObject treasure;
    [SerializeField] private int priority;
    [SerializeField] private GameObject treasure2;
    [SerializeField] private int priority2;

    private NavMeshAgent agent;
    private BehaviourTree tree;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        tree = new BehaviourTree("2-Handed Boss");
        //tree.AddChild(new Leaf("Patrol", new PatrolStrategy(transform, agent, patrolPoints)));

        Sequence goToTreasure = new Sequence("GoToTreasure", priority);
        goToTreasure.AddChild(new Leaf("IsTreasurePresent", new Condition(() => treasure.activeSelf)));
        goToTreasure.AddChild(new Leaf("MoveToTreasure", new ActionStrategy(() => agent.SetDestination(treasure.transform.position))));

        Sequence goToTreasure2 = new Sequence("GoToTreasure2", priority2);
        goToTreasure2.AddChild(new Leaf("IsTreasure2Present", new Condition(() => treasure2.activeSelf)));
        goToTreasure2.AddChild(new Leaf("MoveToTreasure2", new ActionStrategy(() => agent.SetDestination(treasure2.transform.position))));

        PrioritySelector goTotreasures = new PrioritySelector("GoToTreasures", 4);
        goTotreasures.AddChild(goToTreasure2);
        goTotreasures.AddChild(goToTreasure);

        tree.AddChild(goTotreasures);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tree.Process();
    }
}
