using BlinkStickDotNet;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lombiq.Vsix.Orchard.Services.LogWatcher
{
    public sealed class BlinkStickManager : IBlinkStickManager
    {
        private readonly object _lock = new object();
        private BlinkStick _blinkStick = null;
        private bool _isInitialized = false;
        private Task _blinkTask = null;
        private CancellationTokenSource _cancellationTokenSource = null;


        public void TurnOn(string color) => RunForBlinkStickIfPresent(() => _blinkStick.SetColor(color));

        // Blink if you're not a lamp! https://youtu.be/_zCDvOsdL9Q?t=56
        public void Blink(string color)
        {
            // Since the built-in Blink() method blocks the thread we need to start a new one and do the blinking in a
            // safer way.
            _cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _cancellationTokenSource.Token;
            _blinkTask = Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    TurnOn(color);
                    if (!cancellationToken.IsCancellationRequested) await Task.Delay(500);
                    TurnOffWithoutBlinkCancellation();
                    if (!cancellationToken.IsCancellationRequested) await Task.Delay(500);
                }
            }, cancellationToken);
        }

        public void TurnOff()
        {
            _cancellationTokenSource?.Cancel();
            TurnOffWithoutBlinkCancellation();
        }

        public void Dispose()
        {
            _isInitialized = false;
            TurnOff();
            _blinkStick?.Dispose();
            _blinkStick = null;
        }


        public static bool IsValidColor(string color)
        {
            // Unfortunately, no way to check the color without using exceptions.
            try
            {
                RgbColor.FromString(color);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        private void TurnOffWithoutBlinkCancellation() => RunForBlinkStickIfPresent(() => _blinkStick.TurnOff());

        private void RunForBlinkStickIfPresent(Action proces)
        {
            EnsureInitialized();
            if (_blinkStick != null && _blinkStick.OpenDevice()) proces();
        }

        private void EnsureInitialized()
        {
            lock (_lock)
            {
                if (_isInitialized) return;

                _blinkStick = BlinkStick.FindFirst();
                _isInitialized = true;
            }
        }
    }
}
