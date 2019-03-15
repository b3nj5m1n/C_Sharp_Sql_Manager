using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlLibrary
{
    public class BackupManager
    {

        public void BackupDatabase(string ConnectionString)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "*.bak | *.bak";
            saveFileDialog.FileName = "Backup_" + DateTime.Today;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Backup(saveFileDialog.FileName, ConnectionString);
            }
        }
        public void RestoreDatabase(string ConnectionString)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "*.bak | *.bak";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Restore(openFileDialog.FileName, ConnectionString);
            }
        }

        public void Backup(string BackupFileName, string ConnectionString)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            string cmd = "";
            try
            {
                con.Open();
                cmd = @"BACKUP DATABASE [" + con.Database + "] TO DISK = '" + BackupFileName + "'";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }
            SqlManager sqlManager = new SqlManager();
            sqlManager.ExecuteSqlCommand(cmd, ConnectionString);
        }
        public void Restore(string BackupFileName, string ConnectionString)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            string cmd1 = "";
            string cmd2 = "";
            try
            {
                con.Open();
                cmd1 = "ALTER DATABASE [" + con.Database + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE ";
                cmd2 = "USE MASTER RESTORE DATABASE [" + con.Database + "] FROM DISK='" + BackupFileName + "' WITH REPLACE";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }
            SqlManager sqlManager = new SqlManager();
            sqlManager.ExecuteSqlCommand(cmd1, ConnectionString);
            sqlManager.ExecuteSqlCommand(cmd2, ConnectionString);
        }


    }
}
