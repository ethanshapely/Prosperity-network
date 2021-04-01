using System;
using System.Collections;

namespace ProsperityNetwork
{
    public class Defector : Node
    {
        public Defector(int indexChosen, double selectionIntensityChosen) : base(indexChosen, 0, 0, selectionIntensityChosen) { }
    }
}
