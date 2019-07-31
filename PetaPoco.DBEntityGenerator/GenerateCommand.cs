namespace PetaPoco.DBEntityGenerator
{
    using System;
    using System.Collections.Generic;

    public class GenerateCommand
    {
        public string ProviderName { get; set; }

        public string ConnectionString { get; set; }

        public string SchemaName { get; set; }

        public string ClassPrefix { get; set; }
        public string ClassSuffix { get; set; }

        public bool IncludeViews { get; set; }

        public string[] ExcludePrefix { get; set; }

        public string Namespace { get; set; }

        public bool ExplicitColumns { get; set; }
        public bool TrackModifiedColumns { get; set; }

        public AutoValueDictionary<string, GenerateTableCommand> Tables { get; set; }

        public GenerateCommand()
        {
            Tables = new AutoValueDictionary<string, GenerateTableCommand>(StringComparer.InvariantCultureIgnoreCase);

            ExplicitColumns = true;
            TrackModifiedColumns = true;
        }
    }

    public class GenerateTableCommand
    {
        public bool Ignore { get; set; }

        public string ClassName { get; set; }

        public AutoValueDictionary<string, GenerateColumnCommand> Columns { get; set; }

        public GenerateTableCommand()
        {
            Columns = new AutoValueDictionary<string, GenerateColumnCommand>(StringComparer.InvariantCultureIgnoreCase);
        }
    }

    public class GenerateColumnCommand
    {
        public bool Ignore { get; set; }
        public bool ForceToUtc { get; set; }

        public string PropertyName { get; set; }

        public string PropertyType { get; set; }

        public string InsertTemplate { get; set; }

        public string UpdateTemplate { get; set; }
    }
}
