using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace ProsperityNetwork
{
    class BaseNetwork
    {
        protected Node[] nodeList;
        protected int benefit, cost, roleModelIndex, totalPayoff, maxPayoff, totalCooperators;
        protected double selectionIntensity, roleConProb, roleNeighborConProb, roleMethodCopyProb;

        public int TotalPayoff
        {
            get { return totalPayoff; }
        }

        public int TotalCooperators
        {
            get { return totalCooperators; }
        }

        public BaseNetwork(int noNodes, int benefitChosen, int costChosen, double selectionIntensityChosen, double roleConProbChosen, double roleNeighborConProbChosen, double roleMethodCopyProbChosen, double percentCooperators)
        {
            nodeList = new Node[noNodes];
            benefit = benefitChosen;
            cost = costChosen;
            selectionIntensity = selectionIntensityChosen;
            roleConProb = roleConProbChosen;
            roleNeighborConProb = roleNeighborConProbChosen;
            roleMethodCopyProb = roleMethodCopyProbChosen;
            roleModelIndex = -1;
            totalPayoff = 0;
            maxPayoff = (noNodes * (noNodes - 1) * (benefit - cost));
            totalCooperators = (int)(percentCooperators * noNodes);
        }

        protected virtual bool IsCooperator(Node node)
        {
            return node is Cooperator;
        }

        public double GetProsperity()
        {
            return (((double)totalPayoff / maxPayoff) * 100);
        }

        private int CalcNodePayoff(Node node)
        {
            List<int> nodeConnections = node.NeighborIndexes;
            int payoff = 0;
            foreach (int index in nodeConnections)
            {
                if (IsCooperator(nodeList[index]))
                {
                    payoff += nodeList[index].Benefit;
                }
            }
            payoff -= (node.Cost * nodeConnections.Count);
            nodeList[node.Index].Payoff = payoff;
            return payoff;
        }

        public void CalcTotalPayoff()
        {
            int totalPayoffcalc = 0;
            int rmIndex = 0;
            if(nodeList[0] == null)
            {
                Trace.WriteLine("Start Node is null");
            }
            foreach (Node node in nodeList)
            {
                if(node == null)
                {
                    Trace.WriteLine("Node read as null");
                }
                totalPayoffcalc += CalcNodePayoff(node);
                if (nodeList[rmIndex].GetEffectivePayoff() < node.GetEffectivePayoff())
                {
                    rmIndex = node.Index;
                }
            }
            totalPayoff = totalPayoffcalc;
            roleModelIndex = rmIndex;
            Console.WriteLine("Total Payoff: " + totalPayoffcalc);
        }

        public int RemoveNode()
        {
            Random rng = new Random();
            int removedIndex = rng.Next(0, nodeList.Length);
            Node removedNode = nodeList[removedIndex];
            if (IsCooperator(removedNode))
            {
                totalCooperators -= 1;
            }
            List<int> neighborIndexes = removedNode.NeighborIndexes;
            for (int i = 0; i < neighborIndexes.Count; i++)
            {
                nodeList[neighborIndexes[i]].RemoveConnection(removedIndex);
            }
            nodeList[removedIndex] = null;
            Console.WriteLine("Node removed");
            return removedIndex;
        }

        public virtual void AddNewNode(int index)
        {
            Random rng = new Random();
            Node newNode;
            if (index == roleModelIndex)
            {
                roleModelIndex = -1;
                foreach (Node node in nodeList)
                {
                    if (node != null)
                    {
                        if (roleModelIndex == -1)
                        {
                            roleModelIndex = node.Index;
                        }
                        else if (nodeList[roleModelIndex].GetEffectivePayoff() < node.GetEffectivePayoff())
                        {
                            roleModelIndex = node.Index;
                        }
                    }
                }
            }
            List<int> roleModelNeighbors = nodeList[roleModelIndex].NeighborIndexes;
            List<int> newNodeCons = new List<int>();
            int roleCopyNum = rng.Next(0, 100);
            int roleConNum = rng.Next(0, 100);
            int roleNeighborConNum;
            if (IsCooperator(nodeList[roleModelIndex]))
            {
                if (roleCopyNum < (roleMethodCopyProb * 100))
                {
                    newNode = new Cooperator(index, benefit, cost, selectionIntensity);
                    totalCooperators += 1;
                }
                else
                {
                    newNode = new Defector(index, selectionIntensity);
                }
            }
            else
            {
                if (roleCopyNum < (roleMethodCopyProb * 100))
                {
                    newNode = new Defector(index, selectionIntensity);
                }
                else
                {
                    newNode = new Cooperator(index, benefit, cost, selectionIntensity);
                    totalCooperators += 1;
                }
            }
            if (roleConNum < (roleConProb * 100))
            {
                newNodeCons.Add(roleModelIndex);
            }
            for (int i = 0; i < roleModelNeighbors.Count; i++)
            {
                roleNeighborConNum = rng.Next(0, 100);
                if (roleNeighborConNum < (roleNeighborConProb * 100))
                {
                    newNodeCons.Add(roleModelNeighbors[i]);
                }
            }
            newNode.AddConnections(newNodeCons);
            nodeList[index] = newNode;
            Console.WriteLine("New node added");
        }
    }
}
