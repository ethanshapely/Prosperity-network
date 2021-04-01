using System;
using System.Collections.Generic;

namespace ProsperityNetwork
{
    public class Node
    {
        protected int index, benefit, cost, payoff;
        protected double selectionIntensity;
        protected List<int> neighborIndexes;

        public int Index
        {
            get { return index; }
        }

        public int Benefit
        {
            get { return benefit; }
        }

        public int Cost
        {
            get { return cost; }
        }

        public int Payoff
        {
            get { return payoff; }
            set { payoff = value; }
        }

        public List<int> NeighborIndexes
        {
            get { return neighborIndexes; }
        }

        public Node(int indexChosen, int benefitChosen, int costChosen, double selectionIntensityChosen)
        {
            index = indexChosen;
            benefit = benefitChosen;
            cost = costChosen;
            selectionIntensity = selectionIntensityChosen;
            payoff = 0;
            neighborIndexes = new List<int>();
        }

        public double getEffectivePayoff()
        {
            return Math.Pow((1 + selectionIntensity), payoff);
        }


        public void addConnections(List<int> newConnections)
        {
            bool inNeighborIndexes;
            foreach(int item in newConnections)
            {
                inNeighborIndexes = false;
                for(int i = 0; i < neighborIndexes.Count; i++)
                {
                    if(neighborIndexes[i] == item)
                    {
                        inNeighborIndexes = true;
                    }
                }
                if(!inNeighborIndexes)
                {
                    neighborIndexes.Add(item);
                }
            }
        }

        public void removeConnection(int index)
        {
            neighborIndexes.Remove(index);
        }

    }
}
