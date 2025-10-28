using System;

namespace Dithering
{
    /// <summary>
    /// Struct representing the luminescence weights for red, green, and blue color channels with validation and ability to compute luminescence value.
    /// <remarks>
    /// The default weights correspond to the standard luminance calculation used in many color spaces, where green has the highest contribution. 
    /// </remarks>
    /// </summary>
    public readonly struct LuminescenceWeights
    {
        public LuminescenceWeights() : this(0.299f, 0.587f, 0.114f) { }

        public LuminescenceWeights(float rWeight, float gWeight, float bWeight) { 
            RWeight = rWeight;
            GWeight = gWeight;
            BWeight = bWeight;
            if (!IsValid())
            {
                throw new ArgumentException("Invalid luminescence weights configuration.");
            }   
        }
        public float RWeight { get; init; }
        public float GWeight { get; init; }
        public float BWeight { get; init; }

        /// <summary>
        /// Determines whether the current weight configuration is valid based on predefined constraints.
        /// </summary>
        /// <remarks>A valid configuration requires that RWeight, GWeight, and BWeight are each greater
        /// than or equal to zero, and that their combined sum is within 0.01 of 1. This method is typically used to
        /// verify that the weights can be used reliably in calculations that assume normalization.</remarks>
        /// <returns>true if all weight values are non-negative and their sum is approximately equal to 1; otherwise, false.</returns>
        public readonly bool IsValid() => RWeight >= 0 && GWeight >= 0 && BWeight >= 0 && Math.Abs(RWeight + GWeight + BWeight - 1) < 0.01f;

       
    }
}
