using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;


public class DBAccess : MonoBehaviour
{
    private static int userId = 0;
    private static string dbConnectionString = "URI=file:" + Application.persistentDataPath + "\\storage.db";
    public static void Start()
    {
        userId = 0;
    }

    public static string getConnectionString() {
        return dbConnectionString;
    }

    //Return true if signin is found.
    public static bool signIn(string username) {
        var sqliteDB = new SqliteConnection(dbConnectionString);
        sqliteDB.Open();
        var sqliteCommand = sqliteDB.CreateCommand(); 
        
        sqliteCommand.CommandText = "SELECT id FROM users WHERE username LIKE '" + username + "';";
        Debug.Log("SELECT id FROM 'users' WHERE 'username' LIKE '" + username + "';");
        IDataReader users = sqliteCommand.ExecuteReader();

        try {
            while(users.Read()) {
                Debug.Log(users.GetValue(0));
                //int.TryParse(users["id"], out userId);
            }
        } catch {}

        sqliteDB.Close();

        if(userId != 0) {
            return true;
        }

        return false;
    }

    public static void signOut() {
        userId = 0;
    }

    public static void addUser(string username) {
        var sqliteDB = new SqliteConnection(dbConnectionString);
        sqliteDB.Open();
        var sqliteCommand = sqliteDB.CreateCommand();

        sqliteCommand.CommandText = "INSERT INTO users ('username') VALUES ('" + username + "');";
        sqliteCommand.ExecuteNonQuery();

        sqliteDB.Close();
    }

    /*private static List<Object> query(string queryString) {
        List<var> output;
        var sqliteDB = new SqliteConnection(dbConnectionString);
        sqliteDB.Open();
        var sqliteCommand = sqliteDB.CreateCommand(); 
        
        sqliteCommand.CommandText = queryString;
        output = sqliteCommand.ExecuteReader();

        while(users.Read()) {
            output.add(users);
        }

        sqliteDB.Close();

        return output;
    }*/
}
