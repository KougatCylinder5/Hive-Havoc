using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;


public class DBAccess : MonoBehaviour
{
    private static int saveID = 0;
    private static string dbConnectionString = "URI=file:" + Application.persistentDataPath + "\\storage.db";

    protected static string getConnectionString() {
        return dbConnectionString;
    }

    public static void AddTableIfMissing(string name, string columes) {
        //Scorce https://www.youtube.com/watch?v=8bpYHCKdZno

        var sqliteDB = new SqliteConnection(dbConnectionString);
        sqliteDB.Open();
        var sqliteCommand = sqliteDB.CreateCommand();

        sqliteCommand.CommandText = "CREATE TABLE IF NOT EXISTS " + name + " (" + columes + ");";
        sqliteCommand.ExecuteNonQuery();

        sqliteDB.Close();
        
    }

    //Return true if signin is found.
    public static bool selectSave(string savename) {
        var sqliteDB = new SqliteConnection(dbConnectionString);
        sqliteDB.Open();
        var sqliteCommand = sqliteDB.CreateCommand(); 
        
        sqliteCommand.CommandText = "SELECT id FROM saves WHERE name LIKE '" + savename + "';";
        IDataReader saves = sqliteCommand.ExecuteReader();

        try {
            while(saves.Read()) {
                saveID = saves.GetInt32(0);
            }
        } catch {}

        sqliteDB.Close();

        if(saveID != 0) {
            return true;
        }

        return false;
    }

    public static void exitSave() {
        saveID = 0;
    }

    public static bool addSave(string savename) {
        bool passed = false;
        var sqliteDB = new SqliteConnection(dbConnectionString);
        sqliteDB.Open();
        var sqliteCommand = sqliteDB.CreateCommand();

        sqliteCommand.CommandText = "INSERT INTO saves ('name', 'last_play', 'rank', 'play_time') VALUES ('" + savename + "', + '" + System.DateTime.Now + "', 0, 0);";
        try {
            sqliteCommand.ExecuteNonQuery();
            passed = true;

        } catch {
            passed = false;
        }

        sqliteDB.Close();
        return passed;
    }

    public static List<Unit> getUnits() {
        List<Unit> units = new List<Unit>();
        var sqliteDB = new SqliteConnection(dbConnectionString);
        sqliteDB.Open();
        var sqliteCommand = sqliteDB.CreateCommand(); 
        
        sqliteCommand.CommandText = "SELECT id, x_pos, y_pos, target_x, target_y, health FROM unit WHERE save_id IS " + saveID + ";";
        IDataReader aunit = sqliteCommand.ExecuteReader();

        try {
            while(aunit.Read()) {
                units.Add(new Unit(aunit.GetInt32(0), aunit.GetFloat(1), aunit.GetFloat(2), aunit.GetFloat(3), aunit.GetFloat(4), aunit.GetFloat(5)));
            }
        } catch {}

        sqliteDB.Close();

        return units;
    }

    public static int addUnit(float xPos, float yPos, float xTarget, float yTarget, float health) {
        int rowid = 0;
        var sqliteDB = new SqliteConnection(dbConnectionString);
        sqliteDB.Open();
        var sqliteCommand = sqliteDB.CreateCommand();
        sqliteCommand.CommandText = "INSERT INTO unit ('x_pos', 'y_pos', 'target_x', 'target_y', 'health', save_id) VALUES ('" + xPos + "', '" + yPos + "', '" + xTarget + "', '" + yTarget + "', '" + health + "', '" + saveID + "');";
        sqliteCommand.ExecuteNonQuery();

        sqliteCommand.CommandText = "SELECT last_insert_rowid() FROM unit;";
        IDataReader lastRow = sqliteCommand.ExecuteReader();

        try {
            while(lastRow.Read()) {
                rowid = lastRow.GetInt32(0);
            }
        } catch {}

        return rowid;
    }

    public static void setUnit(int id, float xPos, float yPos, float xTarget, float yTarget, float health) {
        var sqliteDB = new SqliteConnection(dbConnectionString);
        sqliteDB.Open();
        var sqliteCommand = sqliteDB.CreateCommand();
        sqliteCommand.CommandText = " UPDATE unit SET x_pos=" + xPos + ", y_pos=" + yPos + ", target_x=" + xTarget + ", target_y=" + yTarget + ", health=" + health + " WHERE id IS " + id + ";";
        sqliteCommand.ExecuteNonQuery();
    }

    
}
