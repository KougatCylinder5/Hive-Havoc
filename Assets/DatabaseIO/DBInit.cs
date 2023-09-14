using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;

public class DBInit : MonoBehaviour
{
    void Start()
    {
        FixMissing();
        try { DBAccess.addUser("test"); } catch {}
        
        Debug.Log(DBAccess.signIn("test"));
    }

    private void FixMissing() {
        AddTableIfMissing("users", "id INTEGER PRIMARY KEY AUTOINCREMENT, username varchar(50) NOT NULL UNIQUE"); //Table name followed by colume names and datatypes seperated by comma (,)
    }

    

    private void AddTableIfMissing(string name, string columes) {
        //Scorce https://www.youtube.com/watch?v=8bpYHCKdZno

        var sqliteDB = new SqliteConnection(DBAccess.getConnectionString());
        sqliteDB.Open();
        var sqliteCommand = sqliteDB.CreateCommand();

        sqliteCommand.CommandText = "CREATE TABLE IF NOT EXISTS " + name + " (" + columes + ");";
        sqliteCommand.ExecuteNonQuery();

        sqliteDB.Close();
        
    }

}
