/* Originally coded and documented by Joshua Gatley-Dewing */

using System;

namespace TheDeltaProject.Brain.NeuralNetwork
{
    class NeuralSynapse
    {
        public Neuron Mother;//the neuron that the synapse eminates from
        public NeuralFactor synapseWeight;//the weighting system of the synapse

		//constructor. requires (input neuron, the synapses weight)
        public NeuralSynapse(Neuron mother, double weight)
        {
            Mother = mother;//quantum id(all of the characteristics of the neuron) of mother neuron
            synapseWeight = new NeuralFactor(weight);//set the weight
        }
    }
}