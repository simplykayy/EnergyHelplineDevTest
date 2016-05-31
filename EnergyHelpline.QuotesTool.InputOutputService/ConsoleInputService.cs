using EnergyHelpline.QuotesTool.Common;
using EnergyHelpline.QuotesTool.Common.Models;
using System;

namespace EnergyHelpline.QuotesTool.InputOutputService
{
    /// <summary>
    /// Type of input service specific to the console.
    /// </summary>
    public class ConsoleInputService : IInputService
    {
        private IOutputService _outputService;

        public ConsoleInputService(IOutputService outputService)
        {
            _outputService = outputService;
        }

        public virtual string ReadCommand()
        {
            return Console.ReadLine();
        }

        public virtual QuoteParameter ReadParameters()
        {
            _outputService.WriteMessage("Enter gas usage: ");
            double gasUsage = ParseConsoleInput<double>(ReadCommand(), QuoteParametersEnum.GasUsage);
            _outputService.WriteMessage(string.Empty);

            _outputService.WriteMessage("Enter electricity usage: ");
            double electricityUsage = ParseConsoleInput<double>(ReadCommand(), QuoteParametersEnum.ElectricityUsage);
            _outputService.WriteMessage(string.Empty);

            _outputService.WriteMessage("Enter username: ");
            string username = ParseConsoleInput<string>(ReadCommand(), QuoteParametersEnum.Username);
            _outputService.WriteMessage(string.Empty);

            _outputService.WriteMessage("Enter email address: ");
            string email = ParseConsoleInput<string>(ReadCommand(), QuoteParametersEnum.EmailAddress);
            _outputService.WriteMessage(string.Empty);

            return new QuoteParameter
            {
                GasUsage = gasUsage,
                ElectricityUsage = electricityUsage,
                TimeOfQuote = DateTime.Now,
                QuoteUser = new User { Username = username, EmailAddress = email }
            };
        }

        private U ParseConsoleInput<U>(string input, QuoteParametersEnum parameter)
        {
            dynamic parsedInput = default(U);

            switch (parameter)
            {
                case QuoteParametersEnum.GasUsage:
                case QuoteParametersEnum.ElectricityUsage:
                    double value;
                    while (!double.TryParse(input, out value))
                    {
                        _outputService.WriteMessage(string.Format("Incorrect value: {0} for {1}", input, parameter));
                        _outputService.WriteMessage(string.Empty);
                        input = ReadCommand();
                    }

                    parsedInput = value;
                    break;

                case QuoteParametersEnum.EmailAddress:
                    while(!IsValidEmail(input))
                    {
                        _outputService.WriteMessage(string.Format("Invalid/malformed email provided: {0}", input));
                        _outputService.WriteMessage(string.Empty);
                        input = ReadCommand();
                    }

                    parsedInput = input;
                    break;

                default:
                    while(string.IsNullOrWhiteSpace(input))
                    {
                        _outputService.WriteMessage(string.Format("Incorrect value: {0} for {1}", input, parameter));
                        _outputService.WriteMessage(string.Empty);
                        input = ReadCommand();
                    }

                    parsedInput = input;
                    break;
            }

            return parsedInput;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
