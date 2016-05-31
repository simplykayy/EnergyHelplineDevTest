using EnergyHelpline.QuotesTool.Common;
using Newtonsoft.Json;
using System;

namespace EnergyHelpline.QuotesTool.InputOutputService
{
    public class ConsoleOutputService : IOutputService
    {
        public virtual void WriteMessage<T>(T message)
        {
            if(message.GetType() == typeof(string))
            {
                Console.WriteLine(message);
                return;
            }

            Console.WriteLine(JsonConvert.SerializeObject(message));
        }
    }
}
