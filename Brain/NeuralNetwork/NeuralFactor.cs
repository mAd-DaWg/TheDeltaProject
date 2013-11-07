/* Originally coded and documented by Joshua Gatley-Dewing */

using System;

namespace TheDeltaProject.Brain.NeuralNetwork
{
    class NeuralFactor//weighting system
    {
        private double m_weight, m_lastDelta, m_delta;

		//constructor. requires the initial weight to be used
        public NeuralFactor(double weight)
        {
            m_weight = weight;//set weight
            m_lastDelta = m_delta = 0;//instantiate m_lastDelta and m_delta
        }

		//provides access to the weight value
        public double Weight
        {
            get { return m_weight; }
            set { m_weight = value; }
        }

		//provides access to the cumulative weight change to be applied
        public double Delta
        {
            get { return m_delta; }
            set { m_delta = value; }
        }

		//returns the last cumulative weight change that was applied
        public double LastDelta
        {
            get { return m_lastDelta; }
            //set { m_lastDelta = value; }
        }

		//applies the weight changes
        public void ApplyWeightChange(ref double learningRate)
        {
            m_lastDelta = m_delta;//update the last weight change
            m_weight += m_delta * learningRate;//change the weight according to the learning rate
        }

		//set all weight changes to be made to zero and clear weight history
        public void ResetWeightChange()
        {
            m_lastDelta = m_delta = 0;
        }
    }
}
