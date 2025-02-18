// file:	Math\Functions\Logistic.cs
//
// summary:	Implements the logistic class
namespace numl.Math.Functions
{
    /// <summary>A logistic.</summary>
    public class Logistic : Function
    {
        /// <summary>Computes the given x coordinate.</summary>
        /// <param name="x">The Vector to process.</param>
        /// <returns>A Vector.</returns>
        public override double Compute(double x)
        {
            return 1d / (1d + this.exp(-x));
        }
        /// <summary>Derivatives the given x coordinate.</summary>
        /// <param name="x">The Vector to process.</param>
        /// <returns>A Vector.</returns>
        public override double Derivative(double x)
        {
            var c = this.Compute(x);
            return c * (1d - c);
        }
    }
}
