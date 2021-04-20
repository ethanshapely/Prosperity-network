using System;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ProsperityNetwork
{
    class ProsperitySimulation
    {
        int delay, totalCoop;
        double prosperity;

        Network network; ///Get values from UI passed in. For now they will be entered manually for debugging
        bool loop, run;

        public ProsperitySimulation(int noNodes, int benefitChosen, int costChosen, double selectionIntensityChosen, double roleConProbChosen, double roleNeighborConProbChosen, double roleMethodCopyProbChosen, double percentCooperators, int delay)
        {
            //Trace.WriteLine("No. Nodes: " + noNodes);
            //Trace.WriteLine("Percentage Cooperators(start): " + percentCooperators);
            loop = true;
            run = true;
            network = new Network(noNodes, benefitChosen, costChosen, selectionIntensityChosen, roleConProbChosen, roleNeighborConProbChosen, roleMethodCopyProbChosen, percentCooperators);
            this.delay = delay;
            totalCoop = network.TotalCooperators;
            prosperity = network.getProsperity();
        }

        public int TotalCooperators
        {
            get { return totalCoop; }
        }
        /*
         * get
         * {
         *      Dispatcher.Invoke({
         *          get simloop prosperity
         *      })
         * }
         */
        public double Prosperity
        {
            get { return prosperity; }
        }

        public void writeNewEntry(XmlTextWriter writer, string loopNum, double prosperity, int totalCoop)
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

        private string FileExistsCheck(string path, string fileName, int num)
        {
            if (File.Exists(Path.Combine(path, @"XML\" + fileName + "(" + num + ").xml")))
            {
                return FileExistsCheck(path, fileName, num + 1);
            }
            else
            {
                return Path.Combine(path, @"XML\" + fileName + "(" + num + ").xml");
            }
        }

        public void AsyncLoopStart(string simName)
        {
            Action SimLoop = () =>
            {
                string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), @"..\..\..\"));
                if (File.Exists(Path.Combine(path, @"XML\" + simName + ".xml")))
                {
                    path = FileExistsCheck(path, simName, 1);
                }
                else
                {
                    path = Path.Combine(path, @"XML\" + simName + ".xml");
                }
                Trace.WriteLine("Path:" + path);
                XmlTextWriter writer = new XmlTextWriter(path, System.Text.Encoding.UTF8);
                double prosperity;
                int totalCoop;
                int loopNum = 0;
                //writer.WriteStartDocument(true);
                writer.WriteStartElement("Sim", "");
                while (loop)
                {
                    //Indefinite loop to run the network
                    Thread.Sleep(1);
                    if (run)
                    {
                        loopNum += 1;
                        network.calcTotalPayoff();
                        if (loopNum % delay == 0)
                        {
                            prosperity = network.getProsperity();
                            totalCoop = network.TotalCooperators;
                            //Console.WriteLine(loopNum);
                            Trace.WriteLine("Loop number: "+ loopNum);
                            //Console.WriteLine(prosperity);
                            Trace.WriteLine("Prosperity: " + prosperity);
                            //Console.WriteLine(totalCoop);
                            Trace.WriteLine("TotalCoops: " + totalCoop);
                            writeNewEntry(writer, loopNum.ToString(), prosperity, totalCoop);

                            /*writer.WriteStartElement("Log", "");
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

                            writer.WriteEndElement();
                            writer.WriteEndElement();*/

                            //
                            // Call Diapatcher.Invoke to send data?
                            //
                        }
                        network.addNewNode(network.removeNode());
                    }
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();
            };
            Task.Factory.StartNew(SimLoop);
        }
    }
}