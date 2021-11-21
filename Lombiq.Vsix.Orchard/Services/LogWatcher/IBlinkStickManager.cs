using System;

namespace Lombiq.Vsix.Orchard.Services.LogWatcher
{
    /// <summary>
    /// An interface for BlinkStick LED operations.
    /// </summary>
    public interface IBlinkStickManager : IDisposable
    {
        /// <summary>
        /// Makes the led blink.
        /// </summary>
        /// <param name="color">The color of the led light.</param>
        void Blink(string color);

        /// <summary>
        /// Turn of the led.
        /// </summary>
        void TurnOff();

        /// <summary>
        /// Turn on the led.
        /// </summary>
        /// <param name="color">The color of the led light.</param>
        void TurnOn(string color);
    }
}
