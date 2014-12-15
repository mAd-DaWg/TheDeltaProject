/* Originally coded and documented by Joshua Gatley-Dewing */

using System;

namespace TheDeltaProject.Brain.NeuralNetwork
{
    class Neuron
    {
        private NeuralSynapse[] m_input;//used for the connections between each neuron
        double m_output, m_delta, m_lastdelta;
        int m_ID;//the neurons Identification number, this makes it easier to save each neurons state to a database
        NeuralFactor m_bias;//the neurons bias

		//constructor. requires (id for the neuron, the bias of the neuron)
        public Neuron(int ID, double bias)
        {
            m_ID = ID;//set the id
            m_bias = new NeuralFactor(bias);//set the bias
            m_delta = 0;//initialize the error variable
        }

		//provides access to the neurons output
        public double Output
        {
            get { return m_output; }
            set { m_output = value; }
        }

		//returns the neurons ID
        public int ID
        {
            get { return m_ID; }
        }

		//provides access to the neurons synapse input, which is populated at the initialiazation of the neural network
        public NeuralSynapse[] Input
        {
            get { return m_input; }
            set { m_input = value; }
        }

		//provides access to the neurons biasing
        public NeuralFactor Bias
        {
            get { return m_bias; }
            set { m_bias = value; }
        }

		//provides access to the neurons current error on the expected output 
        public double Delta
        {
            get { return m_delta; }
            set
            {
                m_lastdelta = m_delta;//update error history
                m_delta = value;
            }
        }

		//provides access to the last error on the output that the neuron had(i.e how wrong it was on its last calculation)
        public double LastDelta
        {
            get { return m_lastdelta; }
            set { m_lastdelta = value; }
        }

		//sets all unapplied weight changes of the neuron to zero
        public void InitializeLearning()
        {
            for (int i = 0; i < m_input.Length; i++)//loop for each input synapse
            {
                m_input[i].synapseWeight.ResetWeightChange();//set synapses unapplied weight changes to zero
            }

            m_bias.ResetWeightChange();//set the neurons unapplied weight changes of the bias to zero
        }

		//applies the batch of cumlutive weight changes to the input synapses and bias
		//requires the rate at which to adjust the weighting(learningRate)
        public void ApplyLearning(ref double learningRate)
        {
            for (int i = 0; i < m_input.Length; i++)//loop for each input synapse
            {
                m_input[i].synapseWeight.ApplyWeightChange(ref learningRate);//apply the weight adjustments to the synapse
            }
            m_bias.ApplyWeightChange(ref learningRate);//apply the weight adjustments to the bias
        }

		//calculates the neurons output based on the synapses weighting and the neurons biasing
        public void Pulse(bool sigmoidActiv)
        {
            lock (this)
            {
                m_output = 0;//reset the output

                for (int i = 0; i < m_input.Length; i++)//loop for each synapse
                {
                    m_output += m_input[i].Mother.Output * m_input[i].synapseWeight.Weight;//add up(all the input synapses in total) the sum of the input signal to the Neuron via the synapse and the synapses weighting
                }

                m_output += m_bias.Weight;//apply the neurons biasing to the output signal

                //check the atvivation function to use
                if (sigmoidActiv == true)
                {
                    m_output = Mathematics.Sigmoid(m_output);//squash the output signal between 0 and 1
                }
                else
                {
                    m_output = Mathematics.TanH(m_output);
                }
            }
        }

    }
}