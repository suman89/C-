// (c) Copyright KTC 2012 by HOMAY
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.ComponentModel;
using DataAccessHandler.Properties;

namespace DataAccessHandler
{
    public enum SqlCommand
    {
        Insert=1,
        Update=2,
        Select=3,
        Delete=4
    }

    public static class DataAccessAdapter
    {
        static string _dataSource;
        public static string DataSource
        {
            get { return _dataSource; }
            set { _dataSource = value; }
        }

        static string _Password = "";

        public static string Password
        {
            get { return DataAccessAdapter._Password; }
            set { DataAccessAdapter._Password = value; }
        }

        static string _provider = "Microsoft.Jet.OLEDB.4.0";
        public static string Provider
        {
            get { return DataAccessAdapter._provider; }
            set { DataAccessAdapter._provider = value; }
        }

        static OleDbConnectionStringBuilder connectionBuilder;
        //_prifix and _posfix to be used when the entity name is not exactlly table name in database
        //for example entity name is Domain and table name of this entity is the tblDomain so _prefix is tbl
        static string _perfix = Settings.Default.Perfix;

        public static string Perfix
        {
            get { return DataAccessAdapter._perfix; }
            set { DataAccessAdapter._perfix = value; }
        }
        static string _suffix = Settings.Default.Suffix;

        public static string Suffix
        {
            get { return DataAccessAdapter._suffix; }
            set { DataAccessAdapter._suffix = value; }
        }
        static OleDbConnection Oledbconnection = null;

        #region Private Methods
        private static string UpdateCommand<T>(T entity) where T:IEntity<T>
        {
            string Update = "UPDATE " + _perfix + entity.EntityName + _suffix + " SET ";
            Dictionary<string, object> infos = GetInfo(entity);
            foreach (var item in infos)
            {
                if (item.Value != null && item.Value != "" && !entity.PrimaryKey.Exists(p => p.Name == item.Key))
                    Update += item.Key + "=" + Formatting(item.Value.ToString()) + ",";
            }
            Update = Update.Remove(Update.Length - 1, 1) + " ";
            Update += "WHERE " + entity.PrimaryKey.First().Name + "=" + Formatting(entity.PrimaryKey.First().Value.ToString());
            for (int i = 1; i < entity.PrimaryKey.Count; i++)
                Update += " AND " + entity.PrimaryKey[i].Name + "=" + Formatting(entity.PrimaryKey[i].Value.ToString());
            return Update;
        }
        private static string UpdateCommand<T>(T entity, string where)where T:IEntity<T>
        {
            string Update = "UPDATE " + _perfix + ((IEntity<T>)entity).EntityName + _suffix + " SET ";
            Dictionary<string, object> infos = GetInfo(entity);
            foreach (var item in infos)
            {
                if (item.Value != null && item.Value != "" && !entity.PrimaryKey.Exists(p => p.Name == item.Key))
                        Update += item.Key + "=" + Formatting(item.Value.ToString()) + ",";
            }
            Update = Update.Remove(Update.Length - 1, 1) + " ";
            Update += where;
            return Update;
        }

        private static string InsertCommand<T>(T entity) where T:IEntity<T>
        {
            string Insert = "INSERT INTO " + _perfix + ((IEntity<T>)entity).EntityName + _suffix;
            string columns = "(";
            string values = "VALUES(";
            Dictionary<string, object> infos = GetInfo(entity);
            //Read Column Names
            foreach (var item in infos)
            {
                if (item.Value != null && item.Value != "" && !entity.PrimaryKey.Exists(p => p.Name == item.Key))
                {
                    columns += item.Key + ",";
                        values += Formatting(item.Value.ToString()) + ",";
                }
            }
            columns = columns.Remove(columns.Length - 1, 1)+") ";
            values = values.Remove(values.Length - 1, 1) +") ";
            Insert += columns + values;
            return Insert;
        }

        private static string selectCommand<T>(T entity)where T:IEntity<T>
        {
            string select = "SELECT * FROM " + _perfix + ((IEntity<T>)entity).EntityName + _suffix;
            return select;
        }
        private static string selectCommand<T>(T entity, string where) where T : IEntity<T>
        {
            string select = "SELECT * FROM " + _perfix + ((IEntity<T>)entity).EntityName + _suffix + " " + where;
            return select;
        }

        private static string deleteCommand<T>(T entity) where T : IEntity<T>
        {
            string delete = "DELETE FROM " + _perfix + entity.EntityName + _suffix + " WHERE " + entity.PrimaryKey.First().Name + "=" + Formatting(entity.PrimaryKey.First().Value.ToString());
            for (int i = 1; i < entity.PrimaryKey.Count; i++)
                delete += " AND " + entity.PrimaryKey[i].Name + "=" + Formatting(entity.PrimaryKey[i].Value.ToString());
            return delete;
        }
        private static string deleteCommand<T>(T entity, string where) where T : IEntity<T>
        {
            string delete = "DELETE FROM " + _perfix + entity.EntityName + _suffix + " " + where;
            return delete;
        }

        private static string Formatting(string item)
        {
            bool r1 = false;
            Int32 r2 = 0;
            if (bool.TryParse(item, out r1) || Int32.TryParse(item, out r2))
                return item ;
            else
                return "'" + item.Replace(">", "(>)").Replace("<", "(<)").Replace(".", "(.)").Replace("*", "(*)").Replace(":", "(:)").Replace("^", "(^)").Replace("+", "(+)").Replace("\\", "(\\)").Replace("/", "(/)").Replace("=", "(=)").Replace("&", "(&)") + "'";
        }
        #endregion

        #region Public Methods

        public static bool Initialize(string datasource, string password)
        {
            try
            {
                DataSource = datasource;
                _Password = password;
                connectionBuilder = new OleDbConnectionStringBuilder();
                connectionBuilder.Provider = Provider;
                connectionBuilder.DataSource = DataSource;
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static Dictionary<string, object> GetInfo<T>(T entity) where T : IEntity<T>
        {
            try
            {
                Dictionary<string, object> values = new Dictionary<string, object>();
                foreach (var item in ((IEntity<T>)entity).GetType().GetProperties())
                {
                    if (item.MemberType == System.Reflection.MemberTypes.Property)
                    {
                        if (item.CanRead && item.PropertyType.IsSerializable && item.PropertyType.IsPublic)
                        {
                            if (item.Name == "PrimaryKey" || item.Name == "EntityName")
                                continue;
                            else if (item.PropertyType.IsEnum)
                                values.Add(item.Name, item.GetValue(entity, null).GetHashCode());
                            else
                                values.Add(item.Name, item.GetValue(entity, null));
                        }

                    }
                }
                //IsSqlInjection(values);
                return values;
            }
            catch (Exception)
            {

                throw;
            }
        }

        internal static void IsSqlInjection(Dictionary<string,object> source)
        {
            foreach (var item in source)
            {
                if (item.Value != null)
                    if (item.Value.ToString().Contains('@') || item.Value.ToString().Contains('=') || item.Value.ToString().Contains("'"))
                        throw new Exception("It is not secure using");
            }
        }

        public static OleDbConnectionStringBuilder ConnectionBuilder
        {
            get { return DataAccessAdapter.connectionBuilder; }
            set { DataAccessAdapter.connectionBuilder = value; }
        }

        public static OleDbDataReader ExecuteReader(string CommandText)
        {
            try
            {
                if (Oledbconnection == null)
                {
                    connectionBuilder.Add("Jet OLEDB:Database Password", _Password);
                    Oledbconnection = new OleDbConnection();
                    Oledbconnection.ConnectionString = connectionBuilder.ConnectionString;
                }
                if (Oledbconnection.State == System.Data.ConnectionState.Open)
                    Oledbconnection.Close();
                Oledbconnection.Open();
                OleDbDataReader myReader = null;
                OleDbCommand myCommand = new OleDbCommand(CommandText, Oledbconnection);
                myReader = myCommand.ExecuteReader();
                return myReader;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static bool ExecuteReader(string CommandText,out List<Dictionary<string,object>> entityList)
        {
            try
            {
                entityList = new List<Dictionary<string, object>>();
                if (Oledbconnection == null)
                {
                    connectionBuilder.Add("Jet OLEDB:Database Password", _Password);
                    Oledbconnection = new OleDbConnection();
                    Oledbconnection.ConnectionString = connectionBuilder.ConnectionString;
                }
                if (Oledbconnection.State == System.Data.ConnectionState.Open)
                    Oledbconnection.Close();
                Oledbconnection.Open();
                OleDbDataReader myReader = null;
                OleDbCommand myCommand = new OleDbCommand(CommandText, Oledbconnection);
                myReader = myCommand.ExecuteReader();
                if (myReader.HasRows)
                {
                    int i = 0;
                    while (myReader.Read())
                    {
                        Dictionary<string, object> dictionary = new Dictionary<string, object>();
                        for (int j = 0; j < myReader.FieldCount - 1; j++)
                            dictionary.Add(myReader.GetSchemaTable().Rows[j][0].ToString(), myReader[j]);
                        entityList.Add(dictionary);
                        i++;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                Oledbconnection.Close();
            }
        }

        public static object ExecuteScaler(string CommandText)
        {
            try
            {
                OleDbCommand Com = Oledbconnection.CreateCommand();
                if (Oledbconnection.State == ConnectionState.Open)
                    Oledbconnection.Close();
                Oledbconnection.Open();
                return Com.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                Oledbconnection.Close();
            }
        }

        public static bool ExecuteNoneQuery(string CommandText)
        {
            try
            {
                if (Oledbconnection == null)
                {
                    connectionBuilder.Add("Jet OLEDB:Database Password", _Password);
                    Oledbconnection = new OleDbConnection();
                    Oledbconnection.ConnectionString = connectionBuilder.ConnectionString;
                }
                OleDbCommand Com = Oledbconnection.CreateCommand();
                if (Oledbconnection.State == ConnectionState.Open)
                    Oledbconnection.Close();
                Oledbconnection.Open();
                Com.CommandText = CommandText;
                Com.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Oledbconnection.Close();
            }
        }

        public static string GenerateCommand<T>(SqlCommand sqlCommand,T entity,string where = "")where T:IEntity<T>
        {
            string commandText = string.Empty;
            switch (sqlCommand)
            {
                case SqlCommand.Insert:
                    commandText = InsertCommand(entity);
                    break;
                case SqlCommand.Update:
                    if(where == "")
                        commandText = UpdateCommand(entity);
                    else if(where != "")
                        commandText = UpdateCommand(entity, where);
                    break;
                case SqlCommand.Select:
                    if (where == "")
                        commandText = selectCommand(entity);
                    else if (where != "")
                        commandText = selectCommand(entity, where);
                    break;
                case SqlCommand.Delete:
                    if (where == "")
                        commandText = deleteCommand(entity);
                    else if (where != "")
                        commandText = deleteCommand(entity, where);
                    break;
                default:
                    break;
            }
            return commandText;
        }

        public static bool DoEntity<T>(T entity, SqlCommand sqlCommand, string where = "") where T:IEntity<T>
        {
            bool result = false; ;
            try
            {
                switch (sqlCommand)
                {
                    case SqlCommand.Insert:
                        result = ExecuteNoneQuery(GenerateCommand(SqlCommand.Insert, entity, where));
                        break;
                    case SqlCommand.Update:
                        result = ExecuteNoneQuery(GenerateCommand(SqlCommand.Update, entity, where));
                        break;
                    case SqlCommand.Select:
                        result = ExecuteNoneQuery(GenerateCommand(SqlCommand.Select, entity, where));
                        break;
                    case SqlCommand.Delete:
                        result = ExecuteNoneQuery(GenerateCommand(SqlCommand.Delete, entity, where));
                        break;
                    default:
                        break;
                }
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            try
            {
                PropertyDescriptorCollection props =
                    TypeDescriptor.GetProperties(typeof(T));
                DataTable table = new DataTable();
                for (int i = 0; i < props.Count; i++)
                {
                    PropertyDescriptor prop = props[i];
                    table.Columns.Add(prop.Name, prop.PropertyType);
                }
                object[] values = new object[props.Count];
                foreach (T item in data)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = props[i].GetValue(item);
                    }
                    table.Rows.Add(values);
                }
                return table;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
