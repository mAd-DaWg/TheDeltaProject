/*
 * Originally written by mAd_DaWg 4 November 2013 
 * 
 * The Delta Project
*/


using System;
using System.Text;
using TheDeltaProject.Brain.NeuralNetwork;

namespace TheDeltaProject.Tests
{
	class NeuralNetTest
	{
		public NeuralNetTest ()
		{
			Console.Clear ();
			Console.WriteLine ("This is the neural net testing ground.");
			Console.WriteLine ("+++++++++++++++++++++++++++++++++++++++++++++++++++++");
			Console.WriteLine ("Currently there is only 1 test and only 1 neural net.");
			Console.WriteLine ("More neural networks and tests for each network will");
			Console.WriteLine ("be added.");
			Console.WriteLine ("+++++++++++++++++++++++++++++++++++++++++++++++++++++");
			Console.WriteLine ();
			Console.WriteLine ("Choose a test to be done:");
			Console.WriteLine ("Type in only the test Number.");
			Console.WriteLine ("--------------------------------------------------------");
			Console.WriteLine ("1  XOR - Trains the network to a basic logic function");
			Console.WriteLine ("         Used to test network convergance and accuracy.");
            //Console.WriteLine ();
            //Console.WriteLine ("2  NOR - Trains the network to a basic logic function");
            //Console.WriteLine ("         Used to test network convergance and accuracy.");
            //Console.WriteLine("         Currently broken, use ctrl+c to quit");
            //Console.WriteLine ();
            //Console.WriteLine ("3  XNOR - Trains the network to a basic logic function");
            //Console.WriteLine ("         Used to test network convergance and accuracy.");
            //Console.WriteLine("         Currently broken, use ctrl+c to quit");
			Console.WriteLine ("--------------------------------------------------------");

			while(true)
			{
				int testChoice = int.Parse(Console.ReadLine ().Trim());

				if (testChoice == 1) 
				{
					runXORtest ();
					break;
				}
				else 
				{
					Console.WriteLine("unknown option");
				}
			}
		}


		//this function runs the XOR test on the Neural Network
		private void runXORtest ()
		{
			Console.Clear ();
			Console.WriteLine ("This is the XOR test");
			Console.WriteLine ("------------------------------------------");
			Console.WriteLine ("First we need to train the network.");

			//instantiate variables needed for the test
			//define the difference between high and low in binary
			//this is done as the networks output is squashed between 0 and 1
			double high = .99;
			double low = .01;
			double mid = .5;

			double ll, lh, hl, hh;//binary input combinations
			int count = 0; //keeps track of how many training sessions where needed to train the network to get an accurate result
			int iterations = 5;//how many time to iterate through the data for each training session. More iterations give more acurate outputs from the Neural network;
			double[][] input, output;
			StringBuilder bld = new StringBuilder ();

			//array of inputs for training.
			input = new double[4][];
			input [0] = new double[] { high, high };
			input [1] = new double[] { low, high };
			input [2] = new double[] { high, low };
			input [3] = new double[] { low, low };

			//array of expected outputs. These outputs match the array of inputs.
			output = new double[4][];
			output [0] = new double[] { low };
			output [1] = new double[] { high };
			output [2] = new double[] { high };
			output [3] = new double[] { low };

			NeuralNet net = new NeuralNet ();//create the NeuralNet object, The Neural Network still needs to be initialised.
			// initialize the Neural Network with 
			//   2 perception neurons(number of inputs to the network)
			//   1 hidden layer (the number hidden layers to use[minimum of 1 is needed],
			//                   Currently using more than one layer will cause a convergance error
			//                   and training will loop infinitly in this example. This is a fault 
			//                   in the Neural Network itself.)
			//   2 hidden layer neurons (the number of neurons in the hidden layer/each hidden layer
			//   1 output neuron (the number of outputs from the network)
			net.Initialize (1, 2, 1, 2, 1);

			Console.WriteLine ();
			Console.WriteLine ("Okay, all variables needed for the test have been accounted for!");
			Console.WriteLine ("Now we will train the network to act like an XOR logic function...");

			double[] inputData = {0, 0};

			do {
				count++;//increas the count of training sessions done by 1

				net.LearningRate = 3;//set the rate that the neural network learns. By default the network has a learning rate of 0.5
				net.Train (input, output, iterations);//do a training session!

				//get the results of training to see if more training is needed! Used by the while statement.
				//show the actual value for the output for a binary input of 0 0
				inputData [0] = low;
				inputData [1] = low;

				ll = net.CalculateOutput(inputData)[0];

				//show the actual value for the output for a binary input of 1 0
				inputData [0] = high;
				inputData [1] = low;

                hl = net.CalculateOutput(inputData)[0];

				//show the actual value for the output for a binary input of 0 1
				inputData [0] = low;
				inputData [1] = high;

                lh = net.CalculateOutput(inputData)[0];

				//show the actual value for the output for a binary input of 1 1
				inputData [0] = high;
				inputData [1] = high;

                hh = net.CalculateOutput(inputData)[0];
			}
            // really train this thing well...
            while (hh > (mid + low)/2 || lh < (mid + high)/2 || hl < (mid + low) /2 || ll > (mid + high)/2);

			//show the actual value for the output for a binary input of 0 0
			inputData [0] = low;
			inputData [1] = low;

            ll = net.CalculateOutput(inputData)[0];

			//show the actual value for the output for a binary input of 1 0
			inputData [0] = high;
			inputData [1] = low;

            hl = net.CalculateOutput(inputData)[0];

			//show the actual value for the output for a binary input of 0 1
			inputData [0] = low;
			inputData [1] = high;

            lh = net.CalculateOutput(inputData)[0];

			//show the actual value for the output for a binary input of 1 1
			inputData [0] = high;
			inputData [1] = high;

            hh = net.CalculateOutput(inputData)[0];

			Console.WriteLine ();
			Console.WriteLine ("Training is Complete!");
			Console.WriteLine ("Here are the results of the networks learning");
			Console.WriteLine ("==================================");

			//print out training results
			bld.Remove (0, bld.Length);
			bld.Append ((count * iterations).ToString ()).Append (" iterations required for training\n");

			bld.Append ("hh: ").Append (hh.ToString ()).Append (" < .5\n");
			bld.Append ("ll: ").Append (ll.ToString ()).Append (" < .5\n");

			bld.Append ("hl: ").Append (hl.ToString ()).Append (" > .5\n");
			bld.Append ("lh: ").Append (lh.ToString ()).Append (" > .5\n");

			Console.WriteLine (bld.ToString ());
			Console.WriteLine ("==================================");

			Console.WriteLine ("Press any key to continue...");
			Console.ReadKey ();

			Console.Clear ();

			Console.WriteLine ("Now for the fun part!");
			Console.WriteLine ("-----------------------------------------------");
			Console.WriteLine ("To really get a feel for what the neural net");
			Console.WriteLine ("does, put in the binary for yourself(either ");
			Console.WriteLine ("a 1 or a 0 only.) You will be asked for two ");
			Console.WriteLine ("values, one at a time. The Neural Network will");
			Console.WriteLine ("then calculate the answer. I suggest you lookup");
			Console.WriteLine ("an \"XOR Truth Table\" to compare the answers");
			Console.WriteLine ("-----------------------------------------------");

			bool quit = false;
			double[] inputDat = {0, 0};
			double[] outputDat = {0};//used for values pertaining to network io
			string ans = "";

			while (quit == false) 
			{
				Console.WriteLine();
				//get input for first input neuron aka input A
				while(true)//loop untill they give a valid input
				{
					Console.WriteLine();
					Console.Write("Please enter a value for input A: ");
					ans = Console.ReadLine();//get value from console
					if(ans.Equals("0") || ans.ToLower().Equals("false") || ans.ToLower().Equals("f"))
					{
						inputDat[0] = low;
						break;//end the loop
					}
					else if(ans.Equals("1") || ans.ToLower().Equals("true")  || ans.ToLower().Equals("t"))
					{
						inputDat[0] = high;
						break;//end the loop
					}
					else
					{
						Console.WriteLine(ans + " is not a valid input! Type in only a 0 or 1");
					}
				}

				//get input for second input neuron aka input B
				while(true)//loop untill they give a valid input
				{
					Console.WriteLine();
					Console.Write("Please enter a value for input B: ");
					ans = Console.ReadLine();//get value from console
					if(ans.Equals("0") || ans.ToLower().Equals("false") || ans.ToLower().Equals("f"))
					{
						inputDat[1] = low;
						break;//end the loop
					}
					else if(ans.Equals("1") || ans.ToLower().Equals("true") || ans.ToLower().Equals("t"))
					{
						inputDat[1] = high;
						break;//end the loop
					}
					else
					{
						Console.WriteLine(ans + " is not a valid input! Type in only a 0 or 1");
					}
				}

                outputDat = net.CalculateOutput(inputDat);//calculate the output value for the users input

                printBinaryResult("XOR", inputDat, outputDat[0]);

				Console.Write ("Do you want to try another input combination [Y or n]: ");
				ans = Console.ReadLine().ToLower();//get value from console
				if(ans.Equals("n") || ans.Equals("no"))
				{
					quit = true;
					Console.Clear();
					//for asthetic sakes
                    MainClass.printIntro();
				}
			}
		}

        //print the results of a binary calculation
        private void printBinaryResult(string function, double[] input, double output)
        {
            Console.WriteLine();
            Console.WriteLine(function + " result:");
            Console.WriteLine("A  B  |  Q");
            Console.WriteLine("----------");
            Console.WriteLine(ToBinaryString(input[0]) + "  " + ToBinaryString(input[1]) + "  |  " + ToBinaryString(output));
            Console.WriteLine("----------");
            Console.WriteLine();
        }

		//converts a double into a binary value returned as a string
		string ToBinaryString (double input)
		{
			if (input > 0.5) 
			{
				return "1";
			} 
			else
			{
				return "0";
			}
		}
	}
}