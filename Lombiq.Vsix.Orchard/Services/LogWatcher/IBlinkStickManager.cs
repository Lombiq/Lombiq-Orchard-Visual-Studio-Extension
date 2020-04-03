namespace Lombiq.Vsix.Orchard.Services.LogWatcher
{
    public interface IBlinkStickManager
    {
        void Blink(string color);
        void Dispose();
        void TurnOff();
        void TurnOn(string color);
    }
}