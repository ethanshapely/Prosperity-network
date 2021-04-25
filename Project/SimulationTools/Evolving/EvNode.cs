using System;
using System.Collections.Generic;
using System.Text;

namespace ProsperityNetwork.Evolving
{
    public class EvNode : Node
    {
        double roleConProb, roleNeighborConProb;

        public double RoleConProb
        {
            get { return roleConProb; }
        }

        public double RoleNeighborConProb
        {
            get { return roleNeighborConProb; }
        }

        public EvNode(int indexChosen, int benefitChosen, int costChosen, double selectionIntensityChosen, double roleConProbChosen, double roleNeighborConProbChosen) : base(indexChosen, benefitChosen, costChosen, selectionIntensityChosen)
        {
            roleConProb = roleConProbChosen;
            roleNeighborConProb = roleNeighborConProbChosen;
        }
    }
}
