﻿using System;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace eNumismat
{
    class FileBackup
    {
        //=====================================================================================================================================================================
        public void RunBackup()
        {
            if (Directory.Exists(Globals.AppDataPath + @"\DBBackUps\"))
            {
                ExcecuteBackup();
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(Globals.AppDataPath + @"\DBBackUps\");
                    ExcecuteBackup();
                }
                catch(Exception ex)
                { }
            }
        }

        //=====================================================================================================================================================================
        private void ExcecuteBackup()
        {
            string SourceFile = Path.Combine(Globals.FileBrowserInitDir, Globals.DBFile);
            string DestFile = Path.Combine(Globals.AppDataPath, @"DBBackUps\" + DateTime.Now.ToString("yyyy_MM_dd-HHmmss") + ".encBack");

            using (var source = new SQLiteConnection("Data Source=" + SourceFile))
            {
                using (var destination = new SQLiteConnection("Data Source=" + DestFile))
                {
                    try
                    {
                        source.Open();
                        CompactDatabase(source);
                        destination.Open();

                        source.BackupDatabase(destination, "main", "main", -1, null, 0);

                        MessageBox.Show("Backup erfolgreich erstellt!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        //=====================================================================================================================================================================
        public void CompactDatabase(SQLiteConnection Database = null)
        {
            if (Database == null)
            {
                string SourceFile = Path.Combine(Globals.FileBrowserInitDir, Globals.DBFile);
                Database = new SQLiteConnection("DataSource=" + SourceFile);
            }

            using (SQLiteCommand cmd = Database.CreateCommand())
            {
                cmd.CommandText = "vacuum";
                cmd.ExecuteNonQuery();
            }
        }
    }
} 