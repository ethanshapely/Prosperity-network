using System;
using System.Collections;
using System.IO;
using System.Diagnostics;
//using System.Xml;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ProsperityNetwork.Evolving;

namespace ProsperityNetwork
{
    class ProsperitySimulation
    {
        private int delay, totalCoop, loopNum;
        private double prosperity, avrgRoleConProb, avrgRoleNeighborConProb, avrgCoopTotal;

        private BaseNetwork network;
        private bool loop, run;
        private readonly  bool isEvolving;

        public int TotalCooperators
        {
            get { return totalCoop; }
        }
        
        public double Prosperity
        {
            get { return prosperity; }
        }

        public double AverageCooperators
        {
            get { return avrgCoopTotal; }
        }

        public double AverageRoleConProb
        {
            get { return avrgRoleConProb; }
        }

        public double AverageRoleNeighborConProb
        {
            get { return avrgRoleNeighborConProb; }
        }

        public ProsperitySimulation(int noNodes, int benefitChosen, int costChosen, double selectionIntensityChosen, double roleConProbChosen, double roleNeighborConProbChosen, double roleMethodCopyProbChosen, double percentCooperators, int delay, bool evolveCheck, double mutationExtremeRMC, double mutationExtremeRMNC)
        {
            loop = true;
            run = true;
            isEvolving = evolveCheck;
            if (isEvolving)
            {
                network = new EvNetwork(noNodes, benefitChosen, costChosen, selectionIntensityChosen, roleConProbChosen, roleNeighborConProbChosen, roleMethodCopyProbChosen, percentCooperators, mutationExtremeRMC, mutationExtremeRMNC);
            }
            else
            {
                network = new Network(noNodes, benefitChosen, costChosen, selectionIntensityChosen, roleConProbChosen, roleNeighborConProbChosen, roleMethodCopyProbChosen, percentCooperators);
            }
            this.delay = delay;
            totalCoop = network.TotalCooperators;
            prosperity = network.GetProsperity();
            loopNum = 0;
            avrgCoopTotal = 0;
        }

        public void WriteNewTXTEntry(StreamWriter writer)
        {
            if (isEvolving)
            {
                writer.WriteLine(loopNum + ", " + prosperity + ", " + totalCoop + ", " + avrgCoopTotal + ", " + avrgRoleConProb + ", " + avrgRoleNeighborConProb);
            }
            else
            {
                writer.WriteLine(loopNum + ", " + prosperity + ", " + totalCoop + ", " + avrgCoopTotal);
            }
        }

        //No longer implemented due to an error with XML file formatting while using the XMLTextWriter to write the header,
        //in other cases the file would write but in an invalid format

        /*public void WriteNewXMLEntry(XmlTextWriter writer, string loopNum)
        {
            writer.WriteStartElement("Log", "");
            writer.WriteStartElement("Log_num", "");
            writer.WriteValue(loopNum);
            writer.WriteEndElement();
            writer.WriteStartElement("Data", "");
            //Get Values and record them in tags
            writer.WriteStartElement("Prosperity", "");
            writer.WriteValue(prosperity);
            writer.WriteEndElement();
            writer.WriteStartElement("Total_coops", "");
            writer.WriteValue(totalCoop);
            writer.WriteEndElement();
            //Add evolving probabilities here when implemented
            if (isEvolving)
            {
                writer.WriteStartElement("Average RoleConProb", "");
                writer.WriteValue(avrgRoleConProb);
                writer.WriteEndElement();
                writer.WriteStartElement("Average RoleNeighborConProb", "");
                writer.WriteValue(avrgRoleNeighborConProb);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndElement();
        }*/

        /*
        Example of XML file structure:
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

        public void ChangeRun()
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

        public void StopLoop()
        {
            loop = false;
        }

        private string FileExistsCheck(string path, string fileName, string extension, int num)
        {
            if (num == 0)
            {
                if (File.Exists(Path.Combine(path, fileName + extension)))
                {
                    return FileExistsCheck(path, fileName, extension, num + 1);
                }
                else
                {
                    return Path.Combine(path, fileName + extension);
                }
            }
            else
            {
                if (File.Exists(Path.Combine(path, fileName + "(" + num + ")" + extension)))
                {
                    return FileExistsCheck(path, fileName, extension, num + 1);
                }
                else
                {
                    return Path.Combine(path, fileName + "(" + num + ")" + extension);
                }
            }
        }

        /*public void AsyncLoopStart(string simName)
        {
            Action SimLoop = () =>
            {
                string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), @"..\..\..\"));

                path = Path.Combine(path, @"TXT\");
                string extension = ".txt";

                ///path = Path.Combine(path, @"XML\");
                ///string extension = ".xml";

                if (File.Exists(Path.Combine(path, simName + extension)))
                {
                    path = FileExistsCheck(path, simName, extension, 1);
                }
                else
                {
                    path = Path.Combine(path, simName + extension);
                }
                Trace.WriteLine("Path:" + path);
                StreamWriter writer = new StreamWriter(path);
                if (isEvolving)
                {
                    writer.WriteLine("Loop number | Prosperity | Total Cooperators | Average RoleConProb | Average RoleNeighborConProb");
                }
                else
                {
                    writer.WriteLine("Loop number | Prosperity | Total Cooperators");
                }
                //XmlTextWriter writer = new XmlTextWriter(path, System.Text.Encoding.UTF8);
                int loopNum = 0;
                //writer.WriteStartDocument(true);
                //writer.WriteStartElement("Sim", "");
                while (loop)
                {
                    //Indefinite loop to run the network
                    Thread.Sleep(1);
                    if (run)
                    {
                        loopNum += 1;
                        network.CalcTotalPayoff();
                        if (loopNum % delay == 0)
                        {
                            prosperity = network.GetProsperity();
                            totalCoop = network.TotalCooperators;
                            //Console.WriteLine(loopNum);
                            Trace.WriteLine("Loop number: "+ loopNum);
                            //Console.WriteLine(prosperity);
                            Trace.WriteLine("Prosperity: " + prosperity);
                            //Console.WriteLine(totalCoop);
                            Trace.WriteLine("TotalCoops: " + totalCoop);
                            if (isEvolving)
                            {
                                avrgRoleConProb = ((EvNetwork)network).CalcAvrgRoleConProb();
                                Trace.WriteLine("Average RoleConProb: " + avrgRoleConProb);
                                avrgRoleNeighborConProb = ((EvNetwork)network).CalcAvrgRoleNeighborConProb();
                                Trace.WriteLine("Average RoleNeighborConProb: " + avrgRoleNeighborConProb);
                                writer.WriteLine(loopNum + ", " + prosperity + ", " + totalCoop + ", " + avrgRoleConProb + ", " + avrgRoleNeighborConProb);
                            }
                            else
                            {
                                writer.WriteLine(loopNum + ", " + prosperity + ", " + totalCoop);
                            }

                            //WriteNewXMLEntry(writer, loopNum.ToString(), prosperity, totalCoop);

                            //Add evolving probabilities here when implemented

                            //
                            // Call Diapatcher.Invoke to send data?
                            //
                        }
                        network.AddNewNode(network.RemoveNode());
                    }
                }
                ///writer.WriteEndElement();
                ///writer.WriteEndDocument();
                ///writer.Close();
                writer.Close();
            };
            Task.Factory.StartNew(SimLoop);
        }*/

        public Action AsyncLoopStart(string simName)
        {
            return () =>
            {
                string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), @"..\..\..\"));

                path = Path.Combine(path, @"TXT\");
                string extension = ".txt";

                ///path = Path.Combine(path, @"XML\");
                ///string extension = ".xml";

                if (File.Exists(Path.Combine(path, simName + extension)))
                {
                    path = FileExistsCheck(path, simName, extension, 1);
                }
                else
                {
                    path = Path.Combine(path, simName + extension);
                }
                Trace.WriteLine("Path:" + path);
                StreamWriter writer = new StreamWriter(path);
                if (isEvolving)
                {
                    writer.WriteLine("Loop number | Prosperity | Total Cooperators | Average No. Cooperators | Average RoleConProb | Average RoleNeighborConProb");
                }
                else
                {
                    writer.WriteLine("Loop number | Prosperity | Total Cooperators | Average No. Cooperators");
                }
                //XmlTextWriter writer = new XmlTextWriter(path, System.Text.Encoding.UTF8);
                //writer.WriteStartDocument(true);
                //writer.WriteStartElement("Sim", "");
                while (loop)
                {
                    //Indefinite loop to run the network
                    Thread.Sleep(1);
                    if (run)
                    {
                        loopNum += 1;
                        network.CalcTotalPayoff();
                        avrgCoopTotal = (network.TotalCooperators + (avrgCoopTotal * (loopNum - 1))) / loopNum;
                        if (loopNum % delay == 0)
                        {
                            prosperity = network.GetProsperity();
                            totalCoop = network.TotalCooperators;
                            //Console.WriteLine(loopNum);
                            Trace.WriteLine("Loop number: " + loopNum);
                            //Console.WriteLine(prosperity);
                            Trace.WriteLine("Prosperity: " + prosperity);
                            //Console.WriteLine(totalCoop);
                            Trace.WriteLine("TotalCoops: " + totalCoop);
                            //Console.WriteLine(avrgCoopTotal);
                            Trace.WriteLine("AvrgCoops: " + avrgCoopTotal);
                            if (isEvolving)
                            {
                                avrgRoleConProb = ((EvNetwork)network).CalcAvrgRoleConProb();
                                Trace.WriteLine("Average RoleConProb: " + avrgRoleConProb);
                                avrgRoleNeighborConProb = ((EvNetwork)network).CalcAvrgRoleNeighborConProb();
                                Trace.WriteLine("Average RoleNeighborConProb: " + avrgRoleNeighborConProb);
                                //writer.WriteLine(loopNum + ", " + prosperity + ", " + totalCoop + ", " + avrgCoopTotal + ", " + avrgRoleConProb + ", " + avrgRoleNeighborConProb);
                            }
                            WriteNewTXTEntry(writer);
                            //WriteNewXMLEntry(writer, loopNum.ToString(), prosperity, totalCoop);
                        }
                        network.AddNewNode(network.RemoveNode());
                    }
                }
                //writer.WriteEndElement();
                //writer.WriteEndDocument();
                //writer.Close();
                writer.Close();
            };
        }
    }
}