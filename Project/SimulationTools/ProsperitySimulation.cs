using System;
using System.Collections;
using System.IO;
using System.Xml;

namespace ProsperityNetwork
{
    class ProsperitySimulation
    {
        int noNodes;
        int benefitChosen;
        int costChosen;
        double selectionIntensityChosen;
        double roleConProbChosen;
        double roleNeighborConProbChosen;
        double roleMethodCopyProbChosen;
        double percentCooperators;
        Network network; ///Get values from UI passed in. For now they will be entered manually for debugging
        bool loop = true;
        bool run = true;

        public ProsperitySimulation()
        {
            int noNodes = 100;
            int benefitChosen = 9;
            int costChosen = 1;
            double selectionIntensityChosen = 1.5;
            double roleConProbChosen = 0.5;
            double roleNeighborConProbChosen = 0.5;
            double roleMethodCopyProbChosen = 0.5;
            double percentCooperators = 0.75;
            network = new Network(noNodes, benefitChosen, costChosen, selectionIntensityChosen, roleConProbChosen, roleNeighborConProbChosen, roleMethodCopyProbChosen, percentCooperators);
        }

        public void writeNewEntry(XmlTextWriter writer, string loopNum, double prosperity, int totalCoop)
        {
            writer.WriteStartElement("Log");
            writer.WriteStartElement("Log_num");
            writer.WriteValue(loopNum);
            writer.WriteEndElement();
            writer.WriteStartElement("Data");
            //Get Values and record them in tags
            writer.WriteStartElement("Prosperity");
            writer.WriteValue(prosperity);
            writer.WriteEndElement();
            writer.WriteStartElement("Total_coops");
            writer.WriteValue(totalCoop);
            writer.WriteEndElement();
            //Add evolving probabilities here when implemented

            writer.WriteEndElement();
            writer.WriteEndElement();
        }
        /*
        Example of XML file:
        <log>
            <Log_num>
                1
            </Log_num>
            <Data>
                <Prosperity>
                    68
                </Prosperity>
                <Total_coops>
                    53
                </Total_coops>
            </Data>
        </log>
        <log>
            <Log_num>
                2
            </Log_num>
            ...
        */

        public void changeRun()
        {
            if (run)
            {
                run = false;
            }
            else
            {
                run = true;
            }
        }

        public void AsyncLoopStart()
        {
            XmlTextWriter writer = new XmlTextWriter("network.xml", System.Text.Encoding.UTF8);
            int loopNum = 0;
            double prosperity;
            int totalCoop;
            while(loop)
            {
                //Indefinite loop to run the network
                if (run)
                {
                    loopNum += 1;
                    prosperity = network.getProsperity();
                    totalCoop = network.TotalCooperators;
                    network.addNewNode(network.removeNode());
                    Console.WriteLine(prosperity);
                    Console.WriteLine(totalCoop);
                    writeNewEntry(writer, loopNum.ToString(), prosperity, totalCoop);
                }
            }
            writer.Close();
        }
    }
}