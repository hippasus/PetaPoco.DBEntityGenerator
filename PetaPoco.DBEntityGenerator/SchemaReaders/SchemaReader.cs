namespace PetaPoco.DBEntityGenerator.SchemaReaders
{
    using System.Data.Common;

    public abstract class SchemaReader
    {
        public abstract Tables ReadSchema(DbConnection connection, DbProviderFactory factory);

        public IOutput outer;

        public void WriteLine(string o)
        {
            outer.WriteLine(o);
        }

        protected string CleanUp(string text)
        {
            return Helpers.CleanUp(text);
        }
    }
}
