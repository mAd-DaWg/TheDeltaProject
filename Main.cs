/*
 * Originally written by mAd_DaWg 4 November 2013 
 * 
 * The Delta Project
 * 
 * Use of MainClass: Currently it is being used to allow easier interaction with the program.
 * 					 At the moment the main interaction with the program is via a terminal(console)
 * 					 window, however the terminal will be phased out as the project progresses towards
 *                   its maturity.
 * 
 */

using System;

namespace TheDeltaProject
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			printIntro ();


			//deals with arguments passed to the program at startup i.e.: thedeltaproject <command>
			//                                                            thedeltaproject -e "Awesome Sauce"
			//***************************************************************************************
			int argsLength = args.Length;//get the length of the argument array, this array contains all arguments passed to it at startup
			for (int i = 0; argsLength > i; i++) //loop through the entire args array so long as there actualy ARE arguments passed to it
			{

				if (args [i].Equals ("-h") || args [i].Equals ("-H") || args [i].Equals ("-help") || args [i].Equals ("-Help")) //redundancy check for the help command
				{
					help ();
				}
				else if (args [i].Equals ("-e")) //check if the ith index of the args array contains a -e which represents the echo command
				{
					echo (args, i);
				}
			}
			//***************************************************************************************


			//if no arguments have been passed at startup, use the commandLineMode function to
			//allow user interaction with the program via the terminal.
			if (args.Length == 0)
            {
				commandLineMode();
            }
		}//end of Main()

		//This function deals with user interaction with the program.
		static void commandLineMode ()
		{
			Console.WriteLine ("Please enter a command:");//prompt the user for their first input

			string command = "";
			bool quit = false;

			while (quit == false) //loop untill a quite command is issued
			{
				Console.Write("prompt> "); // Prompt the user
				command = Console.ReadLine ();//read input from the terminal

				if(command.Equals ("help"))
				{
					help ();
				}

				else if(command.ToLower().Equals ("neuralnettest"))
				{
					Tests.NeuralNetTest test = new Tests.NeuralNetTest();
				}
				else if(command.ToLower().Equals ("quit"))
				{
					quit = true;
					Console.WriteLine ("Goodbye!");
				}
				else if(command.Length > 3)
				{
					if(command.Substring(0, 4).Equals ("echo"))
					{
						string echoString = "";
						if(command.Length > 5)
						{
							echoString = command.Substring(5);//pass only the string to echo to the echo function
						}
						echo (echoString);
					}
					else
					{
						printCommandNotRecognised(command);
					}
				}
				else//if the command is not listed above
				{
					printCommandNotRecognised(command);
				}
			}
		}//end of commandLineMode()


		//Simple function to echo a string. Used when -e is passed to the program at startup.
		private static void echo(string[] args, int i)
        {
            try//will fail if the -e was the last item in the args array
            {
                if(!args[i+1].Equals("-*"))//makes sure not to echo any arguments to the program if no string was placed after the -e
				{
					string echo = args[i + 1];//get the string to echo, i.e. the index after
                	Console.WriteLine(echo);//spit the string out to the console
				}
				else//if the next command is not a string, echo something anyway
				{
					printStringEmpty ();
				}
            }
            catch//if no string was found directly after the -e argument, echo something anyway
            {
				printStringEmpty ();
            }
        }


		//Simple function to echo a string. Used when echo command is passed via the terminal
		private static void echo (string echoString)
		{
			if (echoString.Length > 0) 
			{
				Console.WriteLine (echoString);
			} 
			else 
			{
				printStringEmpty ();
			}
		}

		//prints out the avaliable commands and their usage
        private static void help()
        {
            Console.WriteLine();
			Console.WriteLine("thedeltaproject -e <string>     Echos the string. Note, strings must be enclosed");
			Console.WriteLine("                                in \"'s");
			Console.WriteLine("alternate use: echo <string>    To be issued while running the program terminal.");
			Console.WriteLine();
			Console.WriteLine("NeuralNetTest                   Enters a testing ground to test the Neural ");
			Console.WriteLine("                                Networks. To be issued while running the ");
			Console.WriteLine("                                program terminal.");
            Console.WriteLine();
            Console.WriteLine("quit                            exits The Delta Project");

        }

		public static void printIntro ()
		{
			//basic intro
			Console.WriteLine ("Welcome to the Delta Project!");
			Console.WriteLine ("+++++++++++++++++++++++++++++++++++++++++++++++");
			Console.WriteLine ("We are still in the early phases of development");
			Console.WriteLine ("Type help for a list of commands");
			Console.WriteLine ("+++++++++++++++++++++++++++++++++++++++++++++++");
		}

		private static void printStringEmpty ()
		{
			Console.WriteLine("The System trembles as the echo of \"nothing\" shakes its very binary...");
		}

		private static void printCommandNotRecognised (string command)
		{
			Console.WriteLine ("Command \"" + command + "\" is not recognised, type help for a list of commands");
		}

	}
}
