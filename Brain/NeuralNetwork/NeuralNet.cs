/* Originally coded and documented by Joshua Gatley-Dewing */

using System;


namespace TheDeltaProject.Brain.NeuralNetwork
{
    class NeuralNet
    {
		//prevents cross variable reference issues amoung objects
        private double m_learningRate;//learning rate applied to the neural network
        private NeuralLayer m_inputLayer;//input neural layer object
        private NeuralLayer m_outputLayer;//output neural layer object
        private NeuralLayer[] m_hiddenLayers;//hidden neural layer array(contains neural layer objects)

        //default constructor
		public NeuralNet()
        {
            m_learningRate = 0.5;//default learning rate
        }
		
		//constructor overload
        public NeuralNet(double learningRate)
        {
            m_learningRate = learningRate;
        }
		
		//squashes values between 0 and 1
        private static double SigmoidDerivative(double value)
        {
            return value * (1 - value);
        }
		
		//return method for input/'perception' layer(generally used to obtain individual neuron/synapse data)
        public NeuralLayer PerceptionLayer
        {
            get { return m_inputLayer; }
        }
		
		//return method for the 'HiddenLayer' array(generally used to obtain individual neuron/synapse data)
        public NeuralLayer[] HiddenLayer
        {
            get { return m_hiddenLayers; }
        }
		
		//return method for output layer(generally used to obtain individual neuron/synapse data)
        public NeuralLayer OutputLayer
        {
            get { return m_outputLayer; }
        }
		
		//rate of error correction in the neural network. values too low are ineffiecient,
		//values too high will overshoot correlation(will never settle, thus never past a certain point)
        public double LearningRate
        {
            get { return m_learningRate; }
            set { m_learningRate = value; }
        }
		
		//prevents cross contamination. Specifically references This neuralnet and not any other that could be running.
		//randomSeed refers to the number used by a Random object to generate 
        public void Initialize(int randomSeed, int inputNeuronCount, int hiddenLayerCount, int hiddenNeuronCount, int outputNeuronCount)
        {
            Initialize(this, randomSeed, inputNeuronCount, hiddenLayerCount, hiddenNeuronCount, outputNeuronCount);
        }
		
		//instantiate the neural network as an object(also provides access to each neural layer, and in turn each neuron and synapse)
        private static void Initialize (NeuralNet net, int randomSeed, int inputNeuronCount, int hiddenLayerCount, int hiddenNeuronCount, int outputNeuronCount)
		{
			if (hiddenLayerCount > 0) {//neural network will not function with no hidden neural layers.
				int i, j, k;//a more efficient way of using counters.
				Random rand = new Random (randomSeed);//obj rand is used to assign a random bias to each neuron(gets passed through the neurons respective neural layer).
	
				//initialize
				net.m_inputLayer = new NeuralLayer (inputNeuronCount);//make an input neural layer object, load it with spaces for neurons
				net.m_outputLayer = new NeuralLayer (outputNeuronCount);//make an output neural layer object, load it with spaces for neurons
				net.m_hiddenLayers = new NeuralLayer[hiddenLayerCount];//instantiate the array of layers to be contained in the hidden layer region of the neural net
				for (k = 0; k < hiddenLayerCount; k++) {
					net.m_hiddenLayers [k] = new NeuralLayer (hiddenNeuronCount);//make a single neural layer object and load it with spaces for neurons. loops for the number of hidden layer required.
				}
	
				//populate
				//Create(number of neurons in layer, bias of neurons)
				net.m_inputLayer.Create (inputNeuronCount, 0);//populate neural layer with actual neurons. input layer does not get a bias(doesnt interact with any layers before it)
				net.m_outputLayer.Create (outputNeuronCount, rand.NextDouble ());//populate neural layer with actual neurons and assign a randomly generated bias to all neurons
				for (k = 0; k < hiddenLayerCount; k++) {
					net.m_hiddenLayers [k].Create (hiddenNeuronCount, rand.NextDouble ());//populate each neural layer with actual neurons and assign a randomly generated bias to all neurons in the layer
				}
	
				// wire-up input layer to hidden layer
				int synapseCount = 0;//variable used to determine length needed for the neural synapse array
				for (j = 0; j < net.m_inputLayer.Count; j++) {//loop for each neuron in layer
					synapseCount++;//count number of synapses needed to join input neural layer to first hidden neural layer
				}
	            
				NeuralSynapse[] synapses = new NeuralSynapse[synapseCount];//instantiate a neural synapse array(used to make it possible for dynamically generating synapses)
	
				int synapseTracker = 0;//variable used to count synapses between a two layers
				for (j = 0; j < net.m_inputLayer.Count; j++) {//loop for each neuron in layer
					synapses [synapseTracker] = new NeuralSynapse (net.m_inputLayer [j], rand.NextDouble ());//populate the synapse array with synapses, each containing a relative (neuron id, and individual synapse weight)   
					synapseTracker++;//keep track of the next synapse index number to be populated. cannot be replaced by j, as will result in code attempting to store value to an index that is not within the bounds of the array
				}
				for (i = 0; i < net.m_hiddenLayers[0].Count; i++) {//loop for each neuron in layer 0 of hidden layers array
					net.m_hiddenLayers [0] [i].Input = synapses;//connect the synapses from the input/'perception' layers neurons to the first hidden layers neurons(needed for back propogation and network pulsing)
				}
	
				// wire-up all hidden layers if more than 1 exists
				if (hiddenLayerCount > 1) {
					synapseCount = 0;//reset variable used to determine length needed for the neural synapse array
					for (i = 0; i < net.m_hiddenLayers[hiddenLayerCount - 1].Count; i++) {
						synapseCount++;//count number of synapses needed to connect one hidden neural layer to another
					}
	
					for (k = hiddenLayerCount-1; k > 0; k--) {//loop for each hidden layer. hiddenLayerCount - 1 used due to arrays starting at index 0. connect each hidden layer, from the last layer through to the first
						synapses = new NeuralSynapse[synapseCount];//clear and re-instantiate the synapse array(note: this array is only used to quickly, dynamically and efficiently aid population of the synapse connections between each of the neural layers, after which the array will be destroyed)
						synapseTracker = 0;//reset synapse tracker to 0. more efficiant than making a new storage space in memory.
						for (j = 0; j < net.m_hiddenLayers[k - 1].Count; j++) {//loop for each neuron in the layer just before layer k(the layer before layer k contains the input neurons for layer k)
							synapses [synapseTracker] = new NeuralSynapse (net.m_hiddenLayers [k - 1] [j], rand.NextDouble ());//populate the synapse array with synapses, each containing a relative (neuron id, and individual synapse weight)
							synapseTracker++;//keep track of the next synapse index number to be populated. cannot be replaced by j, as will result in code attempting to store value to an index that is not within the bounds of the array
						}
						for (i = 0; i < net.m_hiddenLayers[k].Count; i++) {//loop for each neuron in layer
							net.m_hiddenLayers [k] [i].Input = synapses;//connect the synapses from the layer before k to layer k(needed for back propogation and network pulsing)
						}
					}
				}
	
				// wire-up output layer to last hidden layer(if only 1 hidden neural layer is used in neural network, this will be the layer that the input layer was connected to)
				synapseCount = 0;//reset variable used to determine length needed for the neural synapse array
				for (i = 0; i < net.m_hiddenLayers[hiddenLayerCount - 1].Count; i++) {//loop for each neuron in last hidden neural layer
					synapseCount++;//count number of synapses needed to join last hidden neural layer to output neural layer
				}
				synapses = new NeuralSynapse[synapseCount];//clear and re-instantiate the synapse array(note: this array is only used to quickly, dynamically and efficiently aid population of the synapse connections between each of the neural layers, after which the array will be destroyed)
				synapseTracker = 0;//reset synapse tracker to 0. more efficiant than making a new storage space in memory.
				for (j = 0; j < net.m_hiddenLayers[hiddenLayerCount - 1].Count; j++) {//loop for each neuron in last hidden neural layer
					synapses [synapseTracker] = new NeuralSynapse (net.m_hiddenLayers [hiddenLayerCount - 1] [j], rand.NextDouble ());//populate the synapse array with synapses, each containing a relative (neuron id, and individual synapse weight)
					synapseTracker++;//keep track of the next synapse index number to be populated. cannot be replaced by j, as will result in code attempting to store value to an index that is not within the bounds of the array
				}
				for (i = 0; i < net.m_outputLayer.Count; i++) {//loop for each neuron in output neural layer
					net.m_outputLayer [i].Input = synapses;//connect the synapses from the last hidden layer' neurons to the output neural layer' neurons(needed for back propogation and network pulsing)
				}
			} 
			else 
			{
				throw new ArgumentException("hiddenLayerCount must be greater than zero");//throw an error if hiddenLayerCount is below zero
			}
        }


		//sets all unapplied weight changes of all neurons of each neural layer to zero
		//m_lastDelta and m_delta in the NeuralFactor objects pertaining to each neuron:
		//1. m_bias directly of neuron
		//2. synapseWeight of m_input(a NeuralSynapse object array)
        public void InitializeLearning()
        {
            lock (this)
            {
                if (m_hiddenLayers.Length > 1)//only do itterations if more than one hidden neural network exists in the network(more efficient)
                {
                    for (int i = 0; i < m_hiddenLayers.Length; i++)//loop for each hidden neural layer
                    {
                        m_hiddenLayers[i].InitializeLearning();//trigger the InitializeLearning of the neural layer(which in turn triggers the function in each neuron of the layer)
                    }
                }
                else//if only one hidden neural layer exists in the network
                {
                    m_hiddenLayers[0].InitializeLearning();//trigger the InitializeLearning of the neural layer(which in turn triggers the function in each neuron of the layer)
                }
                m_outputLayer.InitializeLearning();//trigger the InitializeLearning of the neural layer(which in turn triggers the function in each neuron of the layer)
            }
        }


		//prevents cross contamination. Specifically references This neuralnet and not any other that could be running.
		//requires an array of double precision decimals to be used as the input of the neural network
		//array length must be the same as the number of neurons in the input neural layer(no smaller or greater)
        public void PreparePerceptionLayerForPulse(double[] input)
        {
            PreparePerceptionLayerForPulse(this, input);
        }

		//sets the inputs of the neural network array. Use when utilizing the neural network after it has been trained/restored
		//input-array values should be in order: first index of the array is assigned to the first neuron of the layer(left-most side)
		//										 last index of array is assigned to the last neuron of the layer(right-most side)
        public static void PreparePerceptionLayerForPulse(NeuralNet net, double[] input)
        {
            int i;

            if (input.Length != net.m_inputLayer.Count)//check that the input array length matches the number of input neurons in the input/perception layer 
            {
                throw new ArgumentException(string.Format("Expecting {0} inputs for this net", net.m_inputLayer.Count));//throw an error saying what length the input array should be
            }

            // set input data
            for (i = 0; i < net.m_inputLayer.Count; i++)//loop for each neuron in the layer
            {
                net.m_inputLayer[i].Output = input[i];//set the output of the neuron to the matching index of the input array. the output of the neuron is set so as to prevent the input layer from interacting with the input data
            }

        }

		//pass the inputs through the network doing all the various caluclations to determine the final output
        public void Pulse()
        {
            lock (this)
            {
                if (m_hiddenLayers.Length > 1)//only use if more than 1 hidden neural layer(more efficient)
                {
                    for (int i = 0; i < m_hiddenLayers.Length; i++)//loop for each layer
                    {
                        m_hiddenLayers[i].Pulse();//pass the pulse command to the neural layer(wich in turn passes command to each neuron)
                    }
                }
                else//use if only 1 hidden nueral layer
                {
                    m_hiddenLayers[0].Pulse();//pass the pulse command to the neural layer(wich in turn passes command to each neuron)
                }
                m_outputLayer.Pulse();//pass the pulse command to the neural layer(wich in turn passes command to each neuron)
            }
        }


		//Calculates how much the error of the calculated output differs from the desired output(used for back prop)
		//desiredResults array must have the same length as the number of ouptut layer neurons
        private static void CalculateErrors(NeuralNet net, double[] desiredResults)
        {
            #region Declarations

            int i, j, k;
            double actualResult, error;

            #endregion

            #region Execution

            // Calcualte output error values 
            for (i = 0; i < net.m_outputLayer.Count; i++)//loop for each neuron in the output layer
            {
                actualResult = net.m_outputLayer[i].Output;//the neurons actual output

                net.m_outputLayer[i].Error = (desiredResults[i] - actualResult) * SigmoidDerivative(actualResult); //sigmoidDerivative = actualResult * (1 - actualResult)
            }

            // calculate last hidden layer error values
            for (i = 0; i < net.m_hiddenLayers[net.m_hiddenLayers.Length-1].Count; i++)//loop for each neuron
            {
                actualResult = net.m_hiddenLayers[net.m_hiddenLayers.Length - 1][i].Output;//the neurons actual output

                error = 0;//reset the error value
                for (j = 0; j < net.m_outputLayer.Count; j++)//loop for each neuron in the output layer
                {
                    error += (net.m_outputLayer[j].Error * net.m_outputLayer[j].Input[net.m_hiddenLayers[net.m_hiddenLayers.Length - 1][i].ID].synapseWeight.Weight); //calculate the error of hidden layer neuron according to the output layer neurons synapse weight, ouptut-layer neurons error and simgmoid derivative of the neurons output
                }
                error = error * SigmoidDerivative(actualResult);//sigmoidDerivative = actualResult * (1 - actualResult)

                net.m_hiddenLayers[net.m_hiddenLayers.Length - 1][i].Error = error;//update the last hidden layer neurons error

            }

			// if more than 1 hidden layer, calculate all hidden layer error values.
            if (net.m_hiddenLayers.Length > 1)//Synapses hookups are coded in a way that will complete all network hookups for any amount hidden layers used, making this code dispensible if only 1 hidden layer is used.
            {
                for (k = net.m_hiddenLayers.Length-1; k > 0; k--)//loop from the last hidden layer to the first hidden layer
                {
                    for (i = 0; i < net.m_hiddenLayers[k-1].Count; i++)//loop for each neuron
                    {
                        actualResult = net.m_hiddenLayers[k - 1][i].Output;//the neurons actual output

                        error = 0;//clear the error value
                        for (j = 0; j < net.m_hiddenLayers[k].Count; j++)//loop for each neuron
                        {
                            error += (net.m_hiddenLayers[k][j].Error * net.m_hiddenLayers[k][j].Input[net.m_hiddenLayers[k - 1][i].ID].synapseWeight.Weight); // calculate the error
                        }
                        error = error * SigmoidDerivative(actualResult);//sigmoidDerivative = actualResult * (1 - actualResult)

                        net.m_hiddenLayers[k - 1][i].Error = error;//update the neurons error

                    }
                }
            }

            #endregion
        }

		//correct the neuron bias' and synapse weightings of each progressive neural layer so as to minimize the output error.
        public void ApplyLearning()
        {
            lock (this)
            {
                if (m_hiddenLayers.Length > 1)//only run if more than 1 hidden neural layer
                {
                    for (int i = 0; i < m_hiddenLayers.Length; i++)//loop for each hidden neural layer
                    {
                        m_hiddenLayers[i].ApplyLearning(m_learningRate);//pass ApplyLearning command to each hidden layer
                    }
                }
                else//run if only 1 hidden neural layer
                {
                    m_hiddenLayers[0].ApplyLearning(m_learningRate);//pass ApplyLearning command to the hidden neural layer
                }
                m_outputLayer.ApplyLearning(m_learningRate);//pass ApplyLearning command to the output neural layer
            }
        }

		//calculates the weight changes that need to be made(for the synapses and biases) according to the precalculated[precalculated with CalculateErrors()] errors
        public static void CalculateAndAppendTransformation(NeuralNet net)
        {
            int i, j, k;
            Neuron outputNode, inputNode, hiddenNode;
            
            // adjust output layer weight change
            for (j = 0; j < net.m_outputLayer.Count; j++)//loop for each neuron in the output neural layer
            {
                outputNode = net.m_outputLayer[j];//place holder neuron

                for (i = 0; i < net.m_hiddenLayers[net.m_hiddenLayers.Length - 1].Count; i++)//loop for each neuron in the last hidden neural layer
                {
                    hiddenNode = net.m_hiddenLayers[net.m_hiddenLayers.Length - 1][i];//place holder neuron
					outputNode.Input[hiddenNode.ID].synapseWeight.H_Vector += outputNode.Error * hiddenNode.Output;//calculate synapse weight adjustment to be made
                }

                outputNode.Bias.H_Vector += outputNode.Error * outputNode.Bias.Weight;//calculate bias weight adjustment to be made
            }

            //if more than 1 hidden layer, adjust all hidden layers weight changes. works from the last hidden neural layer to the first
            if (net.m_hiddenLayers.Length > 1)
            {
                for (k = net.m_hiddenLayers.Length - 1; k > 0; k--)//loop for each hidden layer
                {
                    for (j = 0; j < net.m_hiddenLayers[k].Count; j++)//loop for each neuron
                    {
                        outputNode = net.m_hiddenLayers[k][j];//place holder neuron

                        for (i = 0; i < net.m_hiddenLayers[k - 1].Count; i++)//loop for each neuron
                        {
                            hiddenNode = net.m_hiddenLayers[k - 1][i];//place holder neuron
                            outputNode.Input[hiddenNode.ID].synapseWeight.H_Vector += outputNode.Error * hiddenNode.Output;//calculate synapse weight adjustment to be made
                        }

                        outputNode.Bias.H_Vector += outputNode.Error * outputNode.Bias.Weight;//calculate bias weight adjustment to be made
                    }
                }
            }

            // adjust first hidden layer weight change
            for (j = 0; j < net.m_hiddenLayers[0].Count; j++)//loop for each neuron in the first neural layer
            {
                hiddenNode = net.m_hiddenLayers[0][j];//place holder neuron

                for (i = 0; i < net.m_inputLayer.Count; i++)//loop for each neuron
                {
                    inputNode = net.m_inputLayer[i];//place holder neuron
                    hiddenNode.Input[inputNode.ID].synapseWeight.H_Vector += hiddenNode.Error * inputNode.Output;//calculate synapse weight adjustment to be made
                }

                hiddenNode.Bias.H_Vector += hiddenNode.Error * hiddenNode.Bias.Weight;//calculate bias weight adjustment to be made
            }
        }


		//used to train the network on one itteration of a single io dataset. does not apply actual weight changes
        public static void BackPropogation_TrainingSession(NeuralNet net, double[] input, double[] desiredResult)
        {
            PreparePerceptionLayerForPulse(net, input);//set the networks input
            net.Pulse();//calculate the networks output
            CalculateErrors(net, desiredResult);//determine how wrong the outputs are
            CalculateAndAppendTransformation(net);//determine how much the weightings need to be adjusted
        }


		//used to train the network on the specified number of iterations of multiple io datasets
        public void Train(double[][] inputs, double[][] expected, int iterations)
        {
            int i, j;

            lock (this)
            {

                for (i = 0; i < iterations; i++)//loop for the amount of specifies iterations
                {

                    InitializeLearning(); // set all weight changes to zero

                    for (j = 0; j < inputs.Length; j++)//loop for each input dataset
					{
                        BackPropogation_TrainingSession(this, inputs[j], expected[j]);//do the network training to aquire cumlutive weight changes to be made
					}

                    ApplyLearning(); // apply batch of cumlutive weight changes
                }

            }
        }
    }
}


