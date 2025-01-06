using System.Collections.Generic;
using System.Linq;

namespace BehaviourTreeArchitecture
{
    public class BehaviourTree : Node
    {
        public BehaviourTree(string name) : base(name) { }

        public override Status Process()
        {
            while(currentChild < children.Count)
            {
                var status = children[currentChild].Process();
                if(status != Status.SUCCESS)
                {
                    return status;
                }
                currentChild++;
            }
            return Status.SUCCESS;
        }
    }

    public class Leaf : Node
    {
        readonly IStrategy strategy;

        public Leaf(string name, IStrategy strategy, int priority = 0) : base(name, priority)
        {
            this.strategy = strategy;
        }

        public override Status Process()
        {
            return strategy.Process();
        }

        public override void Reset()
        {
            strategy.Reset();
        }
    }

    public class Node
    {
        public enum Status
        {
            SUCCESS,
            FAILURE,
            RUNNING
        }

        public readonly string name;
        public readonly int priority;

        public readonly List<Node> children = new();
        protected int currentChild;

        public Node(string name = "Node", int priority = 0)
        {
            this.priority = priority;
            this.name = name;
        }

        public void AddChild(Node child) => children.Add(child);

        public virtual Status Process() => children[currentChild].Process();

        public virtual void Reset()
        {
            currentChild = 0;
            foreach(var child in children)
            {
                child.Reset();
            }
        }
    }

    public class Inverter : Node
    {
        public Inverter(string name) : base(name) { }

        public override Status Process()
        {
            switch (children[currentChild].Process())
            {
                case Status.RUNNING:
                    return Status.RUNNING;
                case Status.SUCCESS:
                    return Status.FAILURE;
                default:
                    return Status.SUCCESS;
            }
        }
    }

    public class UntilFail : Node
    {
        public UntilFail(string name) : base(name) { }

        public override Status Process() 
        {
            if (children[0].Process() == Status.FAILURE)
            {
                Reset();
                return Status.FAILURE;
            }

            return Status.RUNNING;
        }
    }

    public class UntilSuccess : Node
    {
        public UntilSuccess(string name) : base(name) { }

        public override Status Process()
        {
            if (children[0].Process() == Status.SUCCESS)
            {
                Reset();
                return Status.SUCCESS;
            }

            return Status.RUNNING;
        }
    }

    public class Repeat : Node
    {
        int x = 0;
        int count = 0;
        public Repeat(string name, int noOfTimes) : base(name) 
        {
            x = noOfTimes;
        }

        public override Status Process() 
        {
            if (children[0].Process() != Status.SUCCESS && count <= x)
            {
                count++;
                return Status.RUNNING;
            }
            else if(children[0].Process() == Status.SUCCESS)
            {
                Reset();
                return Status.SUCCESS;
            }
            return Status.FAILURE;

        }

    }

    public class Selector : Node
    {
        public Selector(string name, int priority) : base(name, priority) { }

        public override Status Process()
        {
            if (currentChild < children.Count)
            {
                switch (children[currentChild].Process())
                {
                    case Status.RUNNING:
                        return Status.RUNNING;
                    case Status.SUCCESS:
                        Reset();
                        return Status.SUCCESS;
                    default:
                        currentChild++;
                        return Status.RUNNING;
                }
            }
            Reset();
            return Status.FAILURE;
        }
    }

    public class Sequence : Node
    {
        public Sequence(string name, int priority) : base(name, priority) { }

        public override Status Process()
        {
            if (currentChild < children.Count)
            {
                switch (children[currentChild].Process())
                {
                    case Status.RUNNING:
                        return Status.RUNNING;
                    case Status.FAILURE: 
                        Reset();
                        return Status.FAILURE;
                    default:
                        currentChild++;
                        return currentChild == children.Count ? Status.SUCCESS : Status.RUNNING;
                }
            }

            Reset();
            return Status.SUCCESS;
        }
    }

    public class PrioritySelector : Selector
    {
        List<Node> sortedChildren;
        List<Node> SortedChildren => sortedChildren ?? SortChildren();

        protected virtual List<Node> SortChildren() => children.OrderByDescending(child => child.priority).ToList();

        public PrioritySelector(string name, int priority) : base(name, priority) { }

        public override void Reset()
        {
            base.Reset();
            sortedChildren = null;
        }

        public override Status Process()
        {
            foreach(var child in SortedChildren)
            {
                switch (child.Process())
                {
                    case Status.RUNNING:
                        return Status.RUNNING;
                    case Status.SUCCESS:
                        return Status.SUCCESS;
                    default:
                        continue;
                }
            }

            return Status.FAILURE;
        }
    }
}

