using BlinkStickDotNet;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Lombiq.Vsix.Orchard.Services.LogWatcher
{
    [SuppressMessage(
        "Critical Code Smell",
        "S4487:Unread \"private\" fields should be removed",
        Justification = "The _backgroundTask variable is required to keep the the Task alive.")]
    public sealed class BlinkStickManager : IBlinkStickManager
    {
        private readonly object _lock = new object();
        private BlinkStick _blinkStick;
        private bool _isInitialized;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _backgroundTask;

        public void TurnOn(string color)
        {
            // It's necessary to start a background thread to keep the light on otherwise it would just go out for some
            // reason (possibly due to VS stopping the thread's execution due to it being in an AsyncPackage).
            // Doing ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync() in the Task won't help (maybe a bit but
            // the light still goes out after a few seconds).
            _cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _cancellationTokenSource.Token;
            _backgroundTask = Task.Run(
                () =>
                {
                    while (!_cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        TurnOnWithoutCancellation(color);
                        // For some reason Thread.Sleep() is better, with Task.Delay() the light will sometimes flicker.
                        // Possibly because the thread is dispatched to work on something else.
                        // This 10ms refresh causes no measurable CPU load.
                        Thread.Sleep(10);
                    }
                },
                cancellationToken);
        }

        // Blink if you're not a lamp! https://youtu.be/_zCDvOsdL9Q?t=56
        public void Blink(string color)
        {
            // Since the built-in Blink() method blocks the thread we need to start a new one and do the blinking in a
            // safer way.
            _cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _cancellationTokenSource.Token;
            _backgroundTask = Task.Run(
                () =>
                    {
                        while (!cancellationToken.IsCancellationRequested)
                        {
                            TurnOnWithoutCancellation(color);
                            // See the notes on why using Thread.Sleep() instead of Task.Delay() above.
                            if (!cancellationToken.IsCancellationRequested) Thread.Sleep(500);
                            TurnOffWithoutCancellation();
                            if (!cancellationToken.IsCancellationRequested) Thread.Sleep(500);
                        }
                    },
                cancellationToken);
        }

        public void TurnOff()
        {
            _cancellationTokenSource?.Cancel();
            TurnOffWithoutCancellation();
        }

        public void Dispose()
        {
            _isInitialized = false;
            TurnOff();
            _blinkStick?.Dispose();
            _blinkStick = null;
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "No other way to check the color.")]
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

        private void TurnOnWithoutCancellation(string color) => RunForBlinkStickIfPresent(() => _blinkStick.SetColor(color));

        private void TurnOffWithoutCancellation() => RunForBlinkStickIfPresent(() => _blinkStick.TurnOff());

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
