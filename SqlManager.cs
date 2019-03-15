using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlLibrary
{
    public class SqlManager
    {
        string CommandPrefix = "SqlManager: ";
        public string SqlConnectionString = "";

        public void CreateTable(string TableName, List<string> cs, string ConnectionString)
        {
            TableManager tableManager = new TableManager();
            string cmd = tableManager.GetCreateTableCommand(TableName, cs);
            ExecuteSqlCommand(cmd, ConnectionString);
            Console.WriteLine(CommandPrefix + "Table created");
        }
        public void CreateDatabase(string DatabaseName, string ConnectionString, string DatabaseFolder)
        {
            if (DatabaseFolder == "")
            {
                DatabaseFolder = AppDomain.CurrentDomain.BaseDirectory;
            }
            if (ConnectionString == "")
            {
                ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Integrated Security=true;";
            }
            string cmd = string.Format(@"
        CREATE DATABASE
            [{0}]
        ON PRIMARY (
           NAME=Database1_Data,
           FILENAME = '{1}\{0}_data.mdf'
        )
        LOG ON (
            NAME=Database1_log,
            FILENAME = '{1}\{0}_logs.ldf'
        )", DatabaseName, DatabaseFolder);
            ExecuteSqlCommand(cmd, ConnectionString);
            SqlConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + DatabaseFolder + @"\" + DatabaseName + "_data.mdf;Integrated Security=True";

        }

        public void DeleteTable(string TableName, string ConnectionString)
        {
            ExecuteSqlCommand("DROP TABLE [" + TableName + "]", ConnectionString);
            Console.WriteLine(CommandPrefix + "Table deleted");
        }
        public void DeleteDatabase(string DatabaseName, string ConnectionString)
        {
            if (DatabaseName == "")
            {
                SqlConnection sqlConnection = new SqlConnection(ConnectionString);
                try
                {
                    sqlConnection.Open();
                    DatabaseName = sqlConnection.Database;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
            
            ExecuteSqlCommand("USE master;" +
            "ALTER DATABASE[" + DatabaseName + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;" +
            "DROP DATABASE[" + DatabaseName + "];", ConnectionString);
        }

        public void Insert(List<Tuple<dynamic, dynamic>> ColoumnName_ValueToInsert, string TableName, string ConnectionString)
        {
            string cmd = @"INSERT INTO [" + TableName + "]\n (";
            string cmd_coloumns = "";
            string cmd_values = " VALUES(";
            foreach (Tuple<dynamic, dynamic> item in ColoumnName_ValueToInsert)
            {
                cmd_coloumns += "[" + item.Item1.ToString() + "],";
                cmd_values += "'" + item.Item2 + "',";
            }
            cmd_coloumns = cmd_coloumns.Remove(cmd_coloumns.Count() - 1);
            cmd_coloumns = cmd_coloumns += ")";
            cmd_values = cmd_values.Remove(cmd_values.Count() - 1);
            cmd_values = cmd_values += ")";
            cmd += cmd_coloumns + cmd_values + ";";
            ExecuteSqlCommand(cmd, ConnectionString);
            Console.WriteLine(cmd);
            Console.WriteLine(CommandPrefix + "Inserted");
        }
        public void Update(List<Tuple<dynamic, dynamic>> ColoumnName_ValueToInsert, List<Tuple<dynamic, dynamic>> ColoumnName_ValueToMatch, string TableName, string ConnectionString)
        {
            string cmd = "Update [" + TableName + "] SET\n";

            foreach (var item in ColoumnName_ValueToInsert)
            {
                cmd += "[" + item.Item1 + "]='" + item.Item2 + "',";
            }
            cmd = cmd.Remove(cmd.Count() - 1);
            cmd = cmd += " \nWHERE\n";
            int count = 0;
            foreach (var item in ColoumnName_ValueToMatch)
            {
                if (count == 0)
                {
                    cmd += "[" + item.Item1 + "]='" + item.Item2 + "',";
                }
                else
                {
                    cmd += " AND [" + item.Item1 + "]='" + item.Item2 + "',";
                }
                count++;
            }
            cmd = cmd.Remove(cmd.Count() - 1);
            cmd = cmd += ";";
            ExecuteSqlCommand(cmd, ConnectionString);
            Console.WriteLine(cmd);
            Console.WriteLine(CommandPrefix + "Updated");
        }
        public void Delete(List<Tuple<dynamic, dynamic>> ColoumnName_ValueToMatch, string TableName, string ConnectionString)
        {
            string cmd = "DELETE FROM [" + TableName + "] WHERE\n";
            int count = 0;
            foreach (var item in ColoumnName_ValueToMatch)
            {
                if (count == 0)
                {
                    cmd += "[" + item.Item1 + "]='" + item.Item2 + "'";
                }
                else
                {
                    cmd += " AND [" + item.Item1 + "]='" + item.Item2 + "'";
                }
                count++;
            }
            //cmd = cmd.Remove(cmd.Count() - 1);
            cmd += ";";
            ExecuteSqlCommand(cmd, ConnectionString);
            Console.WriteLine(cmd);
            Console.WriteLine(CommandPrefix + "Deleted");
        }

        public void Backup(string ConnectionString)
        {
            BackupManager backupManager = new BackupManager();
            backupManager.BackupDatabase(ConnectionString);
            Console.WriteLine(CommandPrefix + "Backup created");
        }
        public void Restore(string ConnectionString)
        {
            BackupManager backupManager = new BackupManager();
            backupManager.RestoreDatabase(ConnectionString);
            Console.WriteLine(CommandPrefix + "Backup restored");
        }
        
        public DataTable BindGridView(string TableName, string ConnectionString)
        {
            DataTable dt = new DataTable();
            string cmd_string = "SELECT * FROM [" + TableName + "]";
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand(cmd_string, sqlConnection);
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                dt.Dispose();
                sqlConnection.Close();
            }
            Console.WriteLine(CommandPrefix + "Bind gridview successfull");
            return dt;
        }
        public void ExecuteSqlCommand(string SqlCommand, string ConnectionString)
        {
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand(SqlCommand, sqlConnection);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Command executed succesfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                }
            }
        }



    }
}
