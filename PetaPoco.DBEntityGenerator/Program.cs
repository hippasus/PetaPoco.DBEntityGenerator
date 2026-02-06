namespace PetaPoco.DBEntityGenerator
{
    using CommandLine;
    using MySql.Data.MySqlClient;
    using Newtonsoft.Json;
    using Npgsql;
    using Oracle.ManagedDataAccess.Client;
    using PetaPoco.DBEntityGenerator.Outputs;
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using Microsoft.Data.SqlClient;
    using System.IO;

    class Program
    {
        static void Main(string[] args)
        {
#if NETCOREAPP
            DbProviderFactories.RegisterFactory("Npgsql", NpgsqlFactory.Instance);
            DbProviderFactories.RegisterFactory("SqlServer", SqlClientFactory.Instance);
            DbProviderFactories.RegisterFactory("MySql", MySqlClientFactory.Instance);
            DbProviderFactories.RegisterFactory("Oracle", OracleClientFactory.Instance);
#endif

            Parser.Default.ParseArguments<ProgramOptions>(args)
                .WithParsed<ProgramOptions>(opts => RunWithOptions(opts));
        }

        private static void RunWithOptions(ProgramOptions opts)
        {
            GenerateCommand generateCommand = null;
            if (!string.IsNullOrWhiteSpace(opts.ConfigFile))
            {
                var configFileContent = string.Empty;
                using (var fs = new FileStream(opts.ConfigFile, FileMode.Open))
                {
                    using (var sr = new StreamReader(fs))
                    {
                        configFileContent = sr.ReadToEnd();
                    }
                }

                generateCommand = JsonConvert.DeserializeObject<GenerateCommand>(configFileContent);
            }
            else
            {
                generateCommand = new GenerateCommand
                {
                    ConnectionString = opts.ConnectionString,
                    ProviderName = opts.ProviderName,
                    ExplicitColumns = opts.ExplicitColumns,
                    TrackModifiedColumns = opts.TrackModifiedColumns,
                    Nullable = opts.Nullable,
                    Namespace = opts.Namespace
                };
            }

            IOutput output = null;

            if (string.Equals("file", opts.Output, StringComparison.InvariantCultureIgnoreCase))
            {
                output = new FileOutput(opts.OutputFile);
            }
            else
            {
                output = new ConsoleOutput();
            }

            using (output)
            {
                var generator = new Generator(output);
                generator.Generate(generateCommand);
            }
        }
    }
}
