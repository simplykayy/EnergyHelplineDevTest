namespace EnergyHelpline.QuotesTool.Common
{
    public interface IOutputService
    {
        void WriteMessage<T>(T message);
    }
}
