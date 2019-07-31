namespace PetaPoco.DBEntityGenerator.Outputs
{
    using System;
    using System.Text;

    public class ConsoleOutput : IOutput
    {
        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }

        public void Dispose()
        {
        }
    }
}
