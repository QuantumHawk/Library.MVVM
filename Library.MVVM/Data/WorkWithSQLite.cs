using Library.MVVM.model;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Data;
using System.Data.SQLite;
using System;

namespace Library.MVVM.Data
{
    class WorkWithSQLite
    {
        string filename;
        SQLiteCommand cmd;
        SQLiteConnection conn;

        public WorkWithSQLite(string path)
        {
            filename = path;
            FileInfo fileInfo = new FileInfo(filename);
            if (File.Exists(filename) && fileInfo.Length != 0 && Directory.Exists(fileInfo.DirectoryName))
            {
                conn = new SQLiteConnection("Data Source=" + filename + "; Version=3;");
                try
                {
                    conn.Open();
                }
                catch (SQLiteException ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }

                cmd = new SQLiteCommand(conn);
            }
            else
            { System.Windows.MessageBox.Show("DataBase no found! Data will't download "); }
        }

        public void Finish()
        {
            if (conn != null)
                conn.Dispose();
        }
      
        public void Insert(string tableName, string objects)
        {
            cmd.CommandText = "INSERT INTO "+tableName+ " VALUES ( "+objects+" );";
            if (conn != null && conn.State == ConnectionState.Open)
            {
                try
                { cmd.ExecuteNonQuery(); }
                catch (SQLiteException ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            }
        }

        public void Update(string tableName, string objects, string condition)
        {
            cmd.CommandText = "UPDATE " + tableName + " SET  " + objects + " WHERE "+condition;
            if (conn != null && conn.State == ConnectionState.Open)
            {
                try
                { cmd.ExecuteNonQuery(); }
                catch (SQLiteException ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            }
        }

        public SQLiteDataReader Select(string tableName, string objects, string condition)
        {
            SQLiteDataReader selected = null;
            if (conn != null && conn.State == ConnectionState.Open)
            {
                string commandText ;
                if (condition.Length == 0)
                    commandText = "SELECT " + objects + " FROM " + tableName;
                else
                    commandText = "SELECT " + objects + " FROM " + tableName + " where "+condition;
                SQLiteCommand cmd = new SQLiteCommand(commandText, conn);
                try
                {
                    SQLiteDataReader r = cmd.ExecuteReader();
                    selected = r;
                }
                catch (SQLiteException ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            }
            return selected;
        }

        public void Remove(string tableName, string condition)
        {
            if (conn.State == ConnectionState.Open)
            {                    
                string sql_command = "Delete from " + tableName + " where " + condition;
                cmd.CommandText = sql_command;
                try
                { cmd.ExecuteNonQuery(); }
                catch (SQLiteException ex)
                {System.Windows.MessageBox.Show(ex.Message);}
            }            
        }
    }
}
