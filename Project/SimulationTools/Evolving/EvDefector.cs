using System;
using System.Collections.Generic;
using System.Text;

namespace ProsperityNetwork.Evolving
{
    class EvDefector : EvNode
    {
        public EvDefector(int indexChosen, double selectionIntensityChosen, double roleConProbChosen, double roleNeighborConProbChosen) : base(indexChosen, 0, 0, selectionIntensityChosen, roleConProbChosen, roleNeighborConProbChosen) { }
    }
}
