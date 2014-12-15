using System;

namespace TheDeltaProject.Brain.NeuralNetwork
{
    class Mathematics
    {
        //squash value to a value between 0 and 1
        public static double Sigmoid(double value)
        {
            return 1 / (1 + Math.Exp(-value));
        }

        //derivative of the Sigmoid function
        public static double SigmoidDerivative(double value)
        {
            return value * (1 - value);
        }

        //hyperbolic tangent
        public static double TanH(double value)
        {
            return Math.Tanh(value);
        }

        //derivative of the TanH function
        public static double TanHDerivative(double value)
        {
            return (1 - System.Math.Pow(TanH(value), 2));
        }
    }
}
