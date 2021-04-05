namespace PetaPoco.DBEntityGenerator
{
    using System.Collections.Generic;
    using System.Linq;

    public class Table
    {
        public List<Column> Columns;
        public string Name;
        public string Schema;
        public bool IsView;
        public string CleanName;
        public string ClassName;
        public string SequenceName;
        public bool Ignore;

        public List<Column> PrimaryKeys
        {
            get
            {
                return this.Columns.Where(x => x.IsPK).ToList();
            }
        }

        public Column GetColumn(string columnName)
        {
            return Columns.Single(x => string.Compare(x.Name, columnName, true) == 0);
        }

        public Column this[string columnName]
        {
            get
            {
                return GetColumn(columnName);
            }
        }
    }

    public class Column
    {
        public string Name;
        public string PropertyName;
        public string PropertyType;
        public bool IsPK;
        public bool IsNullable;
        public bool IsAutoIncrement;
        public bool Ignore;
        public bool ForceToUtc;
        public string InsertTemplate;
        public string UpdateTemplate;
    }

    public class Tables : List<Table>
    {
        public Tables()
        {
        }

        public Table GetTable(string tableName)
        {
            return this.Single(x => string.Compare(x.Name, tableName, true) == 0);
        }

        public Table this[string tableName]
        {
            get
            {
                return GetTable(tableName);
            }
        }

    }
}
