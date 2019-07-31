using CommandLine;

namespace PetaPoco.DBEntityGenerator
{
    public class ProgramOptions
    {
        [Option("config", HelpText = "Config file path")]
        public string ConfigFile { get; set; }

        [Option('p', "providerName", HelpText = "Database provider name, supported arguments are Npgsql, SqlServer, MySql and Oracle")]
        public string ProviderName { get; set; }

        [Option('c', "connectionString", HelpText = "Connection string to the database")]
        public string ConnectionString { get; set; }

        [Option("namespace", Default = "Entities", HelpText = "Namespace of generated classes")]
        public string Namespace { get; set; }

        [Option("explicitColumns", Default = true, HelpText = "Explicit columns")]
        public bool ExplicitColumns { get; set; }

        [Option("trackModifiedColumns", Default = true, HelpText = "Track modified columns")]
        public bool TrackModifiedColumns { get; set; }

        [Option('o', "output", Default = "console", HelpText = "Output, valid options are console, file")]
        public string Output { get; set; }

        [Option("outputFile", Default = "Database.cs", HelpText = "Output file name")]
        public string OutputFile { get; set; }
    }
}
