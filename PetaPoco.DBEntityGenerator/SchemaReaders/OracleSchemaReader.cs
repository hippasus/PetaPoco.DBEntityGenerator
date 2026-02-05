namespace PetaPoco.DBEntityGenerator.SchemaReaders
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;

    public class OracleSchemaReader : SchemaReader
    {
        // SchemaReader.ReadSchema
        public override Tables ReadSchema(DbConnection connection, DbProviderFactory factory)
        {
            var result = new Tables();

            _connection = connection;
            _factory = factory;

            var cmd = _factory.CreateCommand();
            cmd.Connection = connection;
            cmd.CommandText = TABLE_SQL;
            cmd.GetType().GetProperty("BindByName").SetValue(cmd, true, null);
            cmd.GetType().GetProperty("InitialLONGFetchSize").SetValue(cmd, -1, null);

            //pull the tables in a reader
            using (cmd)
            {

                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        Table tbl = new Table();
                        tbl.Name = rdr["TABLE_NAME"].ToString();
                        tbl.Schema = rdr["TABLE_SCHEMA"].ToString();
                        tbl.IsView = string.Compare(rdr["TABLE_TYPE"].ToString(), "View", true) == 0;
                        tbl.CleanName = CleanUp(tbl.Name);
                        tbl.ClassName = Inflector.MakeSingular(tbl.CleanName);
                        result.Add(tbl);
                    }
                }
            }

            foreach (var tbl in result)
            {
                tbl.Columns = LoadColumns(tbl);

                // Mark the primary key
                string PrimaryKey = GetPK(tbl.Name);
                var pkColumn = tbl.Columns.SingleOrDefault(x => x.Name.ToLower().Trim() == PrimaryKey.ToLower().Trim());
                if (pkColumn != null)
                    pkColumn.IsPK = true;
            }


            return result;
        }

        DbConnection _connection;
        DbProviderFactory _factory;


        List<Column> LoadColumns(Table tbl)
        {

            using (var cmd = _factory.CreateCommand())
            {
                cmd.Connection = _connection;
                cmd.CommandText = COLUMN_SQL;
                cmd.GetType().GetProperty("BindByName").SetValue(cmd, true, null);
                cmd.GetType().GetProperty("InitialLONGFetchSize").SetValue(cmd, -1, null);

                var p = cmd.CreateParameter();
                p.ParameterName = ":tableName";
                p.Value = tbl.Name;
                cmd.Parameters.Add(p);

                var result = new List<Column>();
                using (IDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        Column col = new Column();
                        col.Name = rdr["ColumnName"].ToString();
                        col.PropertyName = CleanUp(col.Name);
                        col.PropertyType = GetPropertyType(rdr["DataType"].ToString(), 
                            (rdr["DataScale"] == DBNull.Value ? null : rdr["DataScale"].ToString()),
                            (rdr["DataPrecision"] == DBNull.Value ? null : rdr["DataPrecision"].ToString()));
                        col.IsNullable = "YES".Equals(rdr["isnullable"].ToString()) || "Y".Equals(rdr["isnullable"].ToString());
                        col.IsAutoIncrement = true;
                        result.Add(col);
                    }
                }

                return result;
            }
        }

        string GetPK(string table)
        {

            string sql = @"select column_name from USER_CONSTRAINTS uc
  inner join USER_CONS_COLUMNS ucc on uc.constraint_name = ucc.constraint_name
where uc.constraint_type = 'P'
and uc.table_name = :tableName
and ucc.position = 1";

            using (var cmd = _factory.CreateCommand())
            {
                cmd.Connection = _connection;
                cmd.CommandText = sql;
                cmd.GetType().GetProperty("BindByName").SetValue(cmd, true, null);
                cmd.GetType().GetProperty("InitialLONGFetchSize").SetValue(cmd, -1, null);

                var p = cmd.CreateParameter();
                p.ParameterName = ":tableName";
                p.Value = table;
                cmd.Parameters.Add(p);

                var result = cmd.ExecuteScalar();

                if (result != null)
                    return result.ToString();
            }

            return "";
        }

        string GetPropertyType(string sqlType, string dataScale, string dataPrecision)
        {
            string sysType = "string";
            sqlType = sqlType.ToLower();
            switch (sqlType)
            {
                case "bigint":
                    sysType = "long";
                    break;
                case "smallint":
                    sysType = "short";
                    break;
                case "int":
                case "integer":
                    sysType = "int";
                    break;
                case "uniqueidentifier":
                    sysType = "Guid";
                    break;
                case "smalldatetime":
                case "datetime":
                case "date":
                case "timestamp":
                case "timestamp with time zone":
                case "timestamp with local time zone":
                    sysType = "DateTime";
                    break;
                case "float":
                    sysType = "double";
                    break;
                case "real":
                case "numeric":
                case "smallmoney":
                case "decimal":
                case "money":
                case "number":
                    sysType = "decimal";
                    break;
                case "tinyint":
                    sysType = "byte";
                    break;
                case "bit":
                    sysType = "bool";
                    break;
                case "image":
                case "binary":
                case "varbinary":
                case "raw":
                case "long raw":
                case "blob":
                case "bfile":
                    sysType = "byte[]";
                    break;
                case "clob":
                case "nclob":
                case "char":
                case "nchar":
                case "varchar":
                case "varchar2":
                case "nvarchar2":
                    sysType = "string";
                    break;
            }

            if (sqlType == "number" && dataScale == "0")
            {
                if (String.IsNullOrEmpty(dataPrecision) || int.Parse(dataPrecision) > 10)
                   return "long";
                if (int.Parse(dataPrecision) <= 10)
                   return "int";
            }

            return sysType;
        }



        const string TABLE_SQL = @"select TABLE_NAME, 'Table' TABLE_TYPE, USER TABLE_SCHEMA
from USER_TABLES
where TABLE_NAME not like 'BIN$%'
union all
select VIEW_NAME, 'View', USER
from USER_VIEWS";


        const string COLUMN_SQL = @"select table_name TableName, 
 column_name ColumnName, 
 data_type DataType, 
 data_scale DataScale,
 data_precision DataPrecision,
 nullable IsNullable
 from USER_TAB_COLS utc 
 where table_name = :tableName
 and virtual_column = 'NO'
 order by column_id";
    }
}
