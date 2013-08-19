/* Originally coded and documented by Joshua Gatley-Dewing */

using System;

namespace TheDeltaProject.Brain.NeuralNetwork
{
    class NeuralLayer
    {
        private Neuron[] m_neurons;//array of the neurons in the neural layer

        //constructor. requires the amount of neurons to be had in the layer
		public NeuralLayer(int neuronCount)
        {
            m_neurons = new Neuron[neuronCount];
        }

		//populate the neuron array with neuron objects. requires (neuron ID, and the neurons bias)
        public void Create(int neuronCount, double bias)
        {
            for (int i = 0; i < neuronCount; i++)//loop for each neuron to be added
            {
                m_neurons[i] = new Neuron(i, bias);//add the nuron object to the neuron array
            }
        }

		//gives direct access to the neurons in the neuron array. Returns a Neuron object. usage: <NeuralLayer object>[index/id of the neuron in the array]
        public Neuron this[int index]
        {
            get { return m_neurons[index]; }
            set { m_neurons[index] = value; }
        }

		//returns the number of neurons in the layer
        public int Count
        {
            get { return m_neurons.Length; }
        }

		//passes on the pulse command to each neuron from the NeuralNet object. triggers each neuron to do the caluclations nessecary to determine an output
        public void Pulse()
        {
            for (int i = 0; i < m_neurons.Length; i++)//loop for each neuron in the array
            {
                m_neurons[i].Pulse();//send the pulse command
            }
        }

		//apply batch of cumlutive weight changes. requires the rate at which to correct the errors(learningRate)
        public void ApplyLearning(double learningRate)
        {
            for (int i = 0; i < m_neurons.Length; i++)//loop for each neuron in the layer
            {
                m_neurons[i].ApplyLearning(ref learningRate);//pass the ApplyLearning command to each neuron
            }
        }

		//sets all unapplied weight changes of each neuron in the layer to zero
		//m_lastDelta and m_delta in the NeuralFactor objects pertaining to each neuron:
		//1. m_bias directly of neuron
		//2. synapseWeight of m_input(a NeuralSynapse object array)
        public void InitializeLearning()
        {
            for (int i = 0; i < m_neurons.Length; i++)//loop for each neuron
            {
                m_neurons[i].InitializeLearning();//pass the InitializeLearning command to the neuron
            }
        }
    }
}

