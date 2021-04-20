using System;
using System.Collections.Generic;

namespace ProsperityNetwork
{
    class Network
    {
        private Node[] nodeList;
        private int benefit, cost, roleModelIndex, totalPayoff, maxPayoff, totalCooperators;
        private double selectionIntensity, roleConProb, roleNeighborConProb, roleMethodCopyProb;

        public int TotalPayoff
        {
            get { return totalPayoff; }
            set { totalPayoff = value; }
        }

        public int TotalCooperators
        {
            get { return totalCooperators; }
        }

        public Network(int noNodes, int benefitChosen, int costChosen, double selectionIntensityChosen, double roleConProbChosen, double roleNeighborConProbChosen, double roleMethodCopyProbChosen, double percentCooperators)
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
            maxPayoff = (noNodes * (noNodes-1) * (benefit - cost));

            //Populate the Network
            totalCooperators = (int)(percentCooperators * noNodes);
            for(int i = 0; i < noNodes; i++)
            {
                if(i < totalCooperators)
                {
                    nodeList[i] = new Cooperator(i, benefit, cost, selectionIntensity);
                }
                else
                {
                    nodeList[i] = new Defector(i, selectionIntensity);
                }
            }

            //Create random relations
            Random rng = new Random();
            for(int i = 0; i < noNodes; i++)
            {
                List<int> nodeConnections = new List<int>();
                for(int j = 0; j < noNodes; j++)
                {
                    if(i != j | !(nodeList[i].NeighborIndexes.Contains(j)))
                    {
                        if(rng.Next(0, 2) > 0)
                        {
                            nodeConnections.Add(nodeList[j].Index);
                        }
                    }
                }
                nodeList[i].addConnections(nodeConnections);
            }

        }

        public double getProsperity()
        {
            return (((double)totalPayoff/maxPayoff) * 100);
        }

        private int calcNodePayoff(Node node)
        {
            List<int> nodeConnections = node.NeighborIndexes;
            int payoff = 0;
            foreach(int index in nodeConnections)
            {
                if (nodeList[index] is Cooperator)
                {
                    payoff += nodeList[index].Benefit;
                }
            }
            payoff -= (node.Cost * nodeConnections.Count);
            nodeList[node.Index].Payoff = payoff;
            return payoff;
        }

        public void calcTotalPayoff()
        {
            int totalPayoffcalc = 0;
            int rmIndex = 0;
            foreach(Node node in nodeList)
            {
                totalPayoffcalc += calcNodePayoff(node);
                if(nodeList[rmIndex].getEffectivePayoff() < node.getEffectivePayoff())
                {
                    rmIndex = node.Index;
                }
            }
            totalPayoff = totalPayoffcalc;
            roleModelIndex = rmIndex;
            Console.WriteLine("Total Payoff: "+totalPayoffcalc);
        }

        public int removeNode()
        {
            Random rng = new Random();
            int removedIndex = rng.Next(0, nodeList.Length);
            Node removedNode = nodeList[removedIndex];
            if(removedNode is Cooperator)
            {
                totalCooperators -= 1;
            }
            List<int> neighborIndexes = removedNode.NeighborIndexes;
            for(int i = 0; i < neighborIndexes.Count; i++)
            {
                nodeList[neighborIndexes[i]].removeConnection(removedIndex);
            }
            nodeList[removedIndex] = null;
            Console.WriteLine("Node removed");
            return removedIndex;
        }

        public void addNewNode(int index)
        {
            Random rng = new Random();
            Node newNode;
            if(index == roleModelIndex)
            {
                roleModelIndex = -1;
                foreach (Node node in nodeList)
                {
                    if (node != null)
                    {
                        if(roleModelIndex == -1)
                        {
                            roleModelIndex = node.Index;
                        }
                        else if (nodeList[roleModelIndex].getEffectivePayoff() < node.getEffectivePayoff())
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
            if(nodeList[roleModelIndex] is Cooperator)
            {
                if(roleCopyNum < (roleMethodCopyProb * 100))
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
                if(roleCopyNum < (roleMethodCopyProb * 100))
                {
                    newNode = new Defector(index, selectionIntensity);
                }
                else
                {
                    newNode = new Cooperator(index, benefit, cost, selectionIntensity);
                    totalCooperators += 1;
                }
            }
            if(roleConNum < (roleConProb * 100))
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
            newNode.addConnections(newNodeCons);
            nodeList[index] = newNode;
            Console.WriteLine("New node added");
        }

        /*public int getTotalCooperators(){
            int totalCoop = 0;
            foreach(Node node in nodeList){
                if(node is Cooperator){
                    totalCoop += 1;
                }
            }
            Console.WriteLine("Total number of Cooperators: "+totalCoop);
            return totalCoop;
        }*/

    }
}
