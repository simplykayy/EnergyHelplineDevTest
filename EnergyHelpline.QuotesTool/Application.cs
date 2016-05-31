using EnergyHelpline.QuotesTool.Common;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Configuration;

namespace EnergyHelpline.QuotesTool
{
    /// <summary>
    /// Application bootstrapper. Responsible for setting up the 
    /// IoC container before the quote tool gets invoked.
    /// </summary>
    class Application
    {
        static void Main(string[] args)
        {
            //IoC Configuration Check
            if(ConfigurationManager.GetSection("unity") != null)
            {
                IUnityContainer container = new UnityContainer();
                container.LoadConfiguration();

                //Start processing quotes..
                IQuotesRunner quotesRunner = container.Resolve<IQuotesRunner>();
                quotesRunner.Run();

                Console.Read();
            }  
        }

    }
}
