using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace SqlLibrary
{
    public class TableManager
    {
        public String GetCreateTableCommand(string TableName, List<String> Colomns)
        {
            string cmd = @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='" + TableName + "' AND xtype='U')\n" +
                            "CREATE TABLE [dbo].[" + TableName + "](\n";
            foreach (string coloumn in Colomns)
            {
                cmd += coloumn + ",\n";
            }
            cmd += ")";

            Console.WriteLine(cmd);



            return cmd;
        }








    }
}
