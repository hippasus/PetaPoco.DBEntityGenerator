namespace PetaPoco.DBEntityGenerator.Outputs
{
    using System.Text;

    public class TextOutput : IOutput
    {
        private readonly StringBuilder _builder = new StringBuilder();

        public void WriteLine(string text)
        {
            _builder.AppendLine(text);
        }

        public override string ToString()
        {
            return _builder.ToString();
        }

        public void Dispose()
        {
        }
    }
}
