using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviourTreeArchitecture
{
    public interface IStrategy
    {
        Node.Status Process();
        void Reset() { }
    }

    public class ActionStrategy : IStrategy
    {
        readonly Action doSomething;

        public ActionStrategy(Action doSomething) 
        {
            this.doSomething = doSomething;
        }

        public Node.Status Process() 
        {
            doSomething();
            return Node.Status.SUCCESS;
        }
    }

    public class Condition : IStrategy
    {
        readonly Func<bool> predicate;

        public Condition(Func<bool> predicate)
        {
            this.predicate = predicate;
        }

        public Node.Status Process() => predicate() ? Node.Status.SUCCESS : Node.Status.FAILURE;
    }

    public class PatrolStrategy : IStrategy
    {
        readonly Transform entity;
        readonly NavMeshAgent agent;
        readonly List<Transform> patrolPoints;
        readonly float patrolSpeed;
        int currentIndex;
        bool isPathCalculated;

        public PatrolStrategy(Transform entity, NavMeshAgent agent, List<Transform> patrolPoints, float patrolSpeed = 2f)
        {
            this.entity = entity;
            this.agent = agent;
            this.patrolPoints = patrolPoints;
            this.patrolSpeed = patrolSpeed;
        }

        public Node.Status Process()
        {
            if (currentIndex == patrolPoints.Count) return Node.Status.SUCCESS;

            var target = patrolPoints[currentIndex];
            agent.SetDestination(target.position);
            entity.LookAt(target.position);

            if(isPathCalculated && agent.remainingDistance < 0.1f)
            {
                currentIndex++;
                isPathCalculated = false;
            }

            if (agent.pathPending) 
            {
                isPathCalculated = true;
            }

            return Node.Status.RUNNING;
        }

        public void Reset() => currentIndex = 0;
    }
}

