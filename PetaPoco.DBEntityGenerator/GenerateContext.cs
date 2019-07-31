namespace PetaPoco.DBEntityGenerator
{
    using System;

    public class GenerateContext
    {
        private Func<string, string> _escapeSqlIdentifier = sqlIdentifier => $"[{sqlIdentifier}]";

        public Func<string, string> EscapeSqlIdentifier
        {
            get { return _escapeSqlIdentifier; }
            set
            {
                _escapeSqlIdentifier = value;
            }
        }

        public GenerateCommand Command { get; set; }
    }
}
