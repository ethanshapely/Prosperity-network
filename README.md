# Prosperity-network
A WPF application made to model and record the data of a dynamic network of nodes that can 
be used to model real world situations such as work place environments and cell networks.

Required libraries outside of the CS.NET framework:
- Livecharts
- Livecharts.Geared

When running this application from cloning this repository:
Load the 'Project' folder as a WPF application project in Visual Studio and build a 
release build to run/test (you will also have the option to load a debug build as well)

# Simulation Tools Notes:
A collection of C# class files designed and first created to imitate the proposed network model

-------------------------------------

'ProsperityNetwork' Namespace:

- 'BaseNetwork' - A base class used to model a network of nodes 
	represented as a list of 'Node' class objects
- 'Node' - A Base class used to hold the statistics for a node within the network
- 'Cooperator' and 'Defector' - Classes that inherit from 'Node'
	and are primarily used if someone wants to model different behaviors
	between Cooperators and Defectors
- 'Network' - A network class that inherits from 'BaseNetwork' and populates 
	the initial list of with 'Cooperators' and 'Defectors' using the calculated
	starting total of 'Cooperators'
- 'ProsperitySimulation' - The Main run file that is called from outside of the 
	'ProsperityNetwork' namespace by modeling either a 'Network' or 'EvNetwork'

'ProsperityNetwork.Evolving' Namespace:

- 'EvNetwork' - A network class that inherits from 'BaseNetwork' but modifies 
	its methods and list of 'Nodes' to operate off of classes from the
	'ProsperityNetwork.Evolving' namespace instead to record how 
	previously constant and static probabilities have an effect on the network

- 'EvNode', 'EvCooperator', and 'EvDefector' - These classes inherit from one another
	similar to the classes found in the earlier ProsperityNetwork namespace but with
	'EvNode' inheriting from 'Node' but with the addition of tracking independent connection
	probabilities for if they are the role model of the 'EvNetwork'

-------------------------------------

# Example MatLab code
Example code for how Matlab could be used to parse data produced from a network instance

-------------------------------------
%Evolving Network: 

% Replace file name with data file produced
fileID = fopen('EvTest.txt','r');

%Change dataFormat to: '%d, %f, %d, %f' if parsing data 
% from a non-evolving networks recorded data
dataFormat = '%d, %f, %d, %f, %f, %f';

A = textscan(fileID, dataFormat,'HeaderLines', 1,'Delimiter',' | ');
fclose(fileID);
%{
Cell array layout:
A = 1x5
{Loop number}  {Prosperity}  {Total Cooperators} {Average Number of Cooperators up to that point}  {Average probability of connecting to the role model}   {Average probability of connecting to a neighbor of the role model}
%}
plot(cell2mat(A(1,1)), cell2mat(A(1,2)));

-------------------------------------
