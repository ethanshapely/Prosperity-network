using System;
using System.Collections.Generic;
using System.Text;

namespace ProsperityNetwork.Evolving
{
    class EvCooperator : EvNode
    {
        public EvCooperator(int indexChosen, int benefitChosen, int costChosen, double selectionIntensityChosen, double roleConProbChosen, double roleNeighborConProbChosen) : base(indexChosen, benefitChosen, costChosen, selectionIntensityChosen, roleConProbChosen, roleNeighborConProbChosen) { }
    }
}
