using EnergyHelpline.QuotesTool.Common.Models;

namespace EnergyHelpline.QuotesTool.Common
{
    /// <summary>
    /// Type that specifies how the tool can obtain input from a user.
    /// Currently, this tool supports a console input, but it can ensuring other target
    /// input implement this interface.
    /// </summary>
    public interface IInputService
    {
        string ReadCommand();
        QuoteParameter ReadParameters();
    }
}
