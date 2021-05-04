# Prosperity-network
A WPF application made to model and record the data of a dynamic network of nodes that can 
be used to model real world situations such as work place environments and cell networks.

Required libraries outside of the CS.NET framework:
- Livecharts
- Livecharts.Geared

# Simulation Tools Notes:
A collection of C# class files designed and first created to imitate the proposed network model

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
