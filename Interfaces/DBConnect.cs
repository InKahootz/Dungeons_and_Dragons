using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Interfaces
{
    public class DBConnect
    {
        MySqlConnection connection = null;

        #region DBConnect(String hostname, String database, String username, String password[, int portNumber, int connectionTimeout])
        /// <summary>
        /// Connects to database
        /// </summary>
        /// <param name="hostname">Indicates where your server is hosted, ie. localhost.</param>
        /// <param name="database">Database name you wish to use.</param>
        /// <param name="username">Username with rights to access this database.</param>
        /// <param name="password">User's password.</param>
        /// <param name="portNumber">Port number of MySQL server.</param>
        /// <param name="connectionTimeout">Connection timeout in miliseconds.</param>
        /// <returns></returns>
        public DBConnect(String hostname, String database, String username, String password) {
            connection = new MySqlConnection("host=" + hostname + ";database=" + database + ";username=" + username + ";password=" + password + ";");
        }

        public DBConnect(String hostname, String database, String username, String password, int portNumber) {
            connection = new MySqlConnection("host=" + hostname + ";database=" + database + ";username=" + username + ";password=" + password + ";port=" + portNumber.ToString() + ";");
        }

        public DBConnect(String hostname, String database, String username, String password, int portNumber, int connectionTimeout) {
            connection = new MySqlConnection("host=" + hostname + ";database=" + database + ";username=" + username + ";password=" + password + ";port=" + portNumber.ToString() + ";Connection Timeout=" + connectionTimeout.ToString() + ";");
        }
        #endregion

        #region Open/Close Connection
        //This opens temporary connection
        private bool Open() {
            try {
                connection.Open();
                return true;
            } catch {
                return false;
            }
        }

        //This method closes the open connection
        private bool Close() {
            try {
                connection.Close();
                return true;
            } catch {
                return false;
            }
        }
        #endregion

        #region NonQuery Commands [Insert, Update, Delete]
        private void NonQuery(String query) {
            try {
                //Opens a connection, if succefull; run the query and then close the connection.
                if (this.Open()) {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                    this.Close();
                }
            } catch { }
            return;
        }

        //Insert values into the database.
        public void Insert(String table, String column, String value) {
            //Example: INSERT INTO names (name, age) VALUES('John Smith', '33')
            //Code: MySQLClient.Insert("names", "name, age", "'John Smith, '33'");
            String query = "INSERT INTO " + table + " (" + column + ") VALUES (" + value + ")";

            NonQuery(query);
        }

        //Update existing values in the database.
        public void Update(String table, String SET, String WHERE) {
            //Example: UPDATE names SET name='Joe', age='22' WHERE name='John Smith'
            //Code: MySQLClient.Update("names", "name='Joe', age='22'", "name='John Smith'");
            String query = "UPDATE " + table + " SET " + SET + " WHERE " + WHERE + "";

            NonQuery(query);
        }

        //Removes an entry from the database.
        public void Delete(String table, String WHERE) {
            //Example: DELETE FROM names WHERE name='John Smith'
            //Code: MySQLClient.Delete("names", "name='John Smith'");
            String query = "DELETE FROM " + table + " WHERE " + WHERE + "";

            NonQuery(query);
        }
        #endregion

        #region Count(String table[, String where])
        //This counts the numbers of entries in a table and returns it as an integear
        //Example: SELECT Count(*) FROM names
        //Code: int myInt = MySQLClient.Count("names");
        public int Count(String table) {
            String query = "SELECT Count(*) FROM " + table + "";
            return CountQuery(query);
        }
        public int Count(String table, String where) {
            String query = "SELECT Count(*) FROM " + table + " WHERE " + where;
            return CountQuery(query);
        }
        private int CountQuery(String query) {
            int Count = -1;
            if (this.Open() == true) {
                try {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    Count = int.Parse(cmd.ExecuteScalar() + "");
                    this.Close();
                } catch { this.Close(); }
                return Count;
            } else {
                return Count;
            }
        }
        #endregion

        #region Select(String table[, String where])
        /// <summary>
        /// <para>This methods selects from the database, it retrieves data from it.</para>
        /// <para>You must make a dictionary to use this since it both saves the column</para>
        /// <para>and the value. i.e. "age" and "33" so you can easily search for values.</para>
        /// <para>&#160;</para>
        /// <para>Example: SELECT * FROM names WHERE name='John Smith'</para>
        /// <para>This example would retrieve all data about the entry with the name "John Smith"</para>
        /// <para>&#160;</para>
        /// <para>Code = var myDictionary = Select("names", "name='John Smith'");</para>
        /// <para>This code creates a dictionary and fills it with info from the database.</para>
        /// </summary>
        /// <param name="table">Table name.</param>
        /// <param name="where">Condition for selective select.</param>
        /// <returns>Dictionary with key and value of rows.</returns>
        public List<Dictionary<String, String>> Select(String table) {
            String query = "SELECT * FROM " + table;
            return SelectQuery(query);
        }
        public List<Dictionary<String, String>> Select(String table, String where) {
            String query = "SELECT * FROM " + table + " WHERE " + where;
            return SelectQuery(query);
        }
        /// <summary>
        /// <para>This methods selects from the database, it retrieves data from it.</para>
        /// </summary>
        /// <param name="query">SQL SELECT query.</param>
        /// <returns>List of dictionaries with key and value of rows.</returns>
        public List<Dictionary<String, String>> SelectQuery(String query) {
            var results = new List<Dictionary<String, String>>();

            try {
                if (this.Open()) {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read()) {
                        var selectResult = new Dictionary<String, String>();
                        for (int i = 0; i < dataReader.FieldCount; i++) {
                            selectResult.Add(dataReader.GetName(i).ToString(), dataReader.GetValue(i).ToString());
                        }
                        results.Add(selectResult);
                    }
                    dataReader.Close();

                    this.Close();
                    return results;
                }

            } catch { }
            return results;
        }
        #endregion
    }
}
