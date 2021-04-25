using System;
using System.Collections.Generic;

namespace ProsperityNetwork
{
    class Network : BaseNetwork
    {
        public Network(int noNodes, int benefitChosen, int costChosen, double selectionIntensityChosen, double roleConProbChosen, double roleNeighborConProbChosen, double roleMethodCopyProbChosen, double percentCooperators)
            : base(noNodes, benefitChosen, costChosen, selectionIntensityChosen, roleConProbChosen, roleNeighborConProbChosen, roleMethodCopyProbChosen, percentCooperators)
        {
            //Populate the Network
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
                nodeList[i].AddConnections(nodeConnections);
            }

        }
    }
}
