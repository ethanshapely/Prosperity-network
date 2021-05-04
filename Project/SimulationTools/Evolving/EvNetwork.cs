using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace ProsperityNetwork.Evolving
{
    class EvNetwork : BaseNetwork
    {
        //private new EvNode[] nodeList;
        private double mutationExtremeRMC, mutationExtremeRMNC;

        public EvNetwork(int noNodes, int benefitChosen, int costChosen, double selectionIntensityChosen, double startRoleConProbChosen, double startRoleNeighborConProbChosen, double roleMethodCopyProbChosen, double percentCooperators, double mutationExtremeRMCChosen, double mutationExtremeRMNCChosen)
            : base(noNodes, benefitChosen, costChosen, selectionIntensityChosen, startRoleConProbChosen, startRoleNeighborConProbChosen, roleMethodCopyProbChosen, percentCooperators)
        {
            nodeList = new EvNode[noNodes];
            benefit = benefitChosen;
            cost = costChosen;
            selectionIntensity = selectionIntensityChosen;
            //startRoleConProb = startRoleConProbChosen;
            //startRoleNeighborConProb = startRoleNeighborConProbChosen;
            roleMethodCopyProb = roleMethodCopyProbChosen;
            roleModelIndex = -1;
            totalPayoff = 0;
            maxPayoff = (noNodes * (noNodes - 1) * (benefit - cost));
            mutationExtremeRMC = mutationExtremeRMCChosen;
            mutationExtremeRMNC = mutationExtremeRMNCChosen;

            //Populate the Network
            totalCooperators = (int)(percentCooperators * noNodes);
            for (int i = 0; i < noNodes; i++)
            {
                if (i < totalCooperators)
                {
                    nodeList[i] = new EvCooperator(i, benefit, cost, selectionIntensity, startRoleConProbChosen, startRoleNeighborConProbChosen);
                }
                else
                {
                    nodeList[i] = new EvDefector(i, selectionIntensity, startRoleConProbChosen, startRoleNeighborConProbChosen);
                }
                Trace.WriteLine("node Added");
            }

            //Create random relations
            Random rng = new Random();
            for (int i = 0; i < noNodes; i++)
            {
                List<int> nodeConnections = new List<int>();
                for (int j = 0; j < noNodes; j++)
                {
                    if (i != j | !(nodeList[i].NeighborIndexes.Contains(j)))
                    {
                        if (rng.Next(0, 2) > 0)
                        {
                            nodeConnections.Add(nodeList[j].Index);
                        }
                    }
                }
                nodeList[i].AddConnections(nodeConnections);
            }

            if (nodeList[0] == null)
            {
                Trace.WriteLine("Node added as null");
            }

        }

        protected override bool IsCooperator(Node node)
        {
            return node is EvCooperator;
        }

        private double ProbChange(double prob, double mutationExtreme)
        {
            double returnProb;
            Random rng = new Random();
            if(rng.Next(0, 2) > 0)
            {
                returnProb = prob + (prob * mutationExtreme);
                if(returnProb > 1)
                {
                    returnProb = 1;
                }
            }
            else
            {
                returnProb =  prob - (prob * mutationExtreme);
            }
            return returnProb;
        }

        public override void AddNewNode(int index)
        {
            Random rng = new Random();
            EvNode newNode;
            if (index == roleModelIndex)
            {
                roleModelIndex = -1;
                foreach (EvNode node in nodeList)
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
            EvNode roleModel = (EvNode)nodeList[roleModelIndex];
            List<int> newNodeCons = new List<int>();
            int roleCopyNum = rng.Next(0, 101);
            int roleConNum = rng.Next(0, 101);
            int roleNeighborConNum;
            double newRoleConProb = ProbChange(roleModel.RoleConProb, mutationExtremeRMC);
            double newRoleNeighborConProb = ProbChange(roleModel.RoleNeighborConProb, mutationExtremeRMNC);
            if (roleConNum <= (roleModel.RoleConProb * 100))
            {
                newNodeCons.Add(roleModelIndex);
            }
            for (int i = 0; i < roleModel.NeighborIndexes.Count; i++)
            {
                roleNeighborConNum = rng.Next(0, 101);
                if (roleNeighborConNum <= (roleModel.RoleNeighborConProb * 100))
                {
                    newNodeCons.Add(roleModel.NeighborIndexes[i]);
                }
            }
            if (IsCooperator(roleModel))
            {
                if (roleCopyNum <= (roleMethodCopyProb * 100))
                {
                    newNode = new EvCooperator(index, benefit, cost, selectionIntensity, newRoleConProb, newRoleNeighborConProb);
                    totalCooperators += 1;
                }
                else
                {
                    newNode = new EvDefector(index, selectionIntensity, newRoleConProb, newRoleNeighborConProb);
                }
            }
            else
            {
                if (roleCopyNum <= (roleMethodCopyProb * 100))
                {
                    newNode = new EvDefector(index, selectionIntensity, newRoleConProb, newRoleNeighborConProb);
                }
                else
                {
                    newNode = new EvCooperator(index, benefit, cost, selectionIntensity, newRoleConProb, newRoleNeighborConProb);
                    totalCooperators += 1;
                }
            }
            newNode.AddConnections(newNodeCons);
            nodeList[index] = newNode;
            //Trace.WriteLine("New node added");
        }

        public double CalcAvrgRoleConProb()
        {
            double totalProbs = 0;
            for(int i = 0; i < nodeList.Length; i++)
            {
                totalProbs += ((EvNode)nodeList[i]).RoleConProb;
            }
            return totalProbs / nodeList.Length;
        }

        public double CalcAvrgRoleNeighborConProb()
        {
            double totalProbs = 0;
            for (int i = 0; i < nodeList.Length; i++)
            {
                totalProbs += ((EvNode)nodeList[i]).RoleNeighborConProb;
            }
            return totalProbs / nodeList.Length;
        }
    }
}
