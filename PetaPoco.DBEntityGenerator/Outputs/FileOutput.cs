namespace PetaPoco.DBEntityGenerator.Outputs
{
    using System.IO;

    public class FileOutput : IOutput
    {
        private StreamWriter _streamWriter;

        public FileOutput(string fileName)
        {
            _streamWriter = new StreamWriter(fileName, false);
            _streamWriter.AutoFlush = true;
        }

        public void WriteLine(string text)
        {
            _streamWriter.WriteLine(text);
        }

        public void Dispose()
        {
            if (_streamWriter != null)
            {
                _streamWriter.Dispose();
                _streamWriter = null;
            }
        }
    }
}
