using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;


public class DBAccess : MonoBehaviour
{
    private static int saveID = 0;
    private static string dbConnectionString = "URI=file:" + Application.persistentDataPath + "\\storage.db";
    private static bool transactionActive = false;
    private const string noTransactionError = "Transaction has not been started!";
    private static SqliteConnection sqliteDB = new SqliteConnection(dbConnectionString);

    protected static string getConnectionString() {
        return dbConnectionString;
    }

    public static void startSave() {
        if (!transactionActive) {
            sqliteDB.Open();
            var sqliteCommand = sqliteDB.CreateCommand();

            sqliteCommand.CommandText = "BEGIN TRANSACTION;";
            sqliteCommand.ExecuteNonQuery();

            transactionActive = true;

            Debug.Log("Transaction started at: " + System.DateTime.Now);
        } else {
            Debug.LogWarning("Transaction alreaded started!");
        }
    }

    public static void commitSave() {
        if(transactionActive) {
            var sqliteCommand = sqliteDB.CreateCommand();

            sqliteCommand.CommandText = "COMMIT;";
            sqliteCommand.ExecuteNonQuery();

            sqliteDB.Close();
            transactionActive = false;
            
            Debug.Log("Transaction commited at: " + System.DateTime.Now);
        } else {
            Debug.LogError("No transaction to commit!");
        }
    }

    public static void rollbackSave() {
        if(transactionActive) {
            var sqliteCommand = sqliteDB.CreateCommand();

            sqliteCommand.CommandText = "ROLLBACK;";
            sqliteCommand.ExecuteNonQuery();

            sqliteDB.Close();
            transactionActive = false;

            Debug.Log("Transaction rollback at: " + System.DateTime.Now);
        } else {
            Debug.LogError("No transaction to rollback!");
        }
    }

    public static void AddTableIfMissing(string name, string columns) {
        if(!transactionActive) {
            Debug.LogError(noTransactionError);
        } else {

            var sqliteCommand = sqliteDB.CreateCommand();

            sqliteCommand.CommandText = "CREATE TABLE IF NOT EXISTS " + name + " (" + columns + ");";
            sqliteCommand.ExecuteNonQuery();
        }
    }

    //Return true if signin is found.
    public static bool selectSave(string savename) {
        if(!transactionActive) {
            Debug.LogError(noTransactionError);
            return false;
        } else {
            var sqliteCommand = sqliteDB.CreateCommand();
            sqliteCommand.CommandText = "SELECT id FROM saves WHERE name LIKE '" + savename + "';";
            IDataReader saves = sqliteCommand.ExecuteReader();

            try {
                while(saves.Read()) {
                    saveID = saves.GetInt32(0);
                }
            } catch {}

            if(saveID != 0) {
                return true;
            }

            return false;
        }
    }

    public static void exitSave() {
        saveID = 0;
    }

    public static bool addSave(string savename) {
        if(!transactionActive) {
            Debug.LogError(noTransactionError);
            return false;
        } else {
            bool passed = false;

            var sqliteCommand = sqliteDB.CreateCommand();

            sqliteCommand.CommandText = "INSERT INTO saves ('name', 'last_play', 'rank', 'play_time') VALUES ('" + savename + "', + '" + System.DateTime.Now + "', 0, 0);";
            try {
                sqliteCommand.ExecuteNonQuery();
                passed = true;

            } catch {
                passed = false;
            }

            return passed;
        }
    }

    public static List<Unit> getUnits() {
        if(!transactionActive) {
            Debug.LogError(noTransactionError);
            return new List<Unit>();
        } else {
            List<Unit> units = new List<Unit>();

            var sqliteCommand = sqliteDB.CreateCommand(); 
            
            sqliteCommand.CommandText = "SELECT id, x_pos, y_pos, target_x, target_y, health FROM unit WHERE save_id IS " + saveID + ";";
            IDataReader aunit = sqliteCommand.ExecuteReader();

            try {
                while(aunit.Read()) {
                    units.Add(new Unit(aunit.GetInt32(0), aunit.GetFloat(1), aunit.GetFloat(2), aunit.GetFloat(3), aunit.GetFloat(4), aunit.GetFloat(5)));
                }
            } catch {}

            return units;
        }
    }

    public static int addUnit(float xPos, float yPos, float xTarget, float yTarget, float health) {
        if(!transactionActive) {
            Debug.LogError(noTransactionError);
            return 0;
        } else {
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
    }

    public static void setUnit(int id, float xPos, float yPos, float xTarget, float yTarget, float health) {
        if(!transactionActive) {
            Debug.LogError(noTransactionError);
        } else {
            var sqliteCommand = sqliteDB.CreateCommand();
            sqliteCommand.CommandText = " UPDATE unit SET x_pos=" + xPos + ", y_pos=" + yPos + ", target_x=" + xTarget + ", target_y=" + yTarget + ", health=" + health + " WHERE id IS " + id + ";";
            sqliteCommand.ExecuteNonQuery();
        }
    }

    public static bool addTileItem(string itemName) {
        if(!transactionActive) {
            Debug.LogError(noTransactionError);
            return false;
        } else {
            bool passed = false;

            var sqliteCommand = sqliteDB.CreateCommand();

            sqliteCommand.CommandText = "INSERT INTO tile_items ('name') VALUES ('" + itemName + "');";
            try {
                sqliteCommand.ExecuteNonQuery();
                passed = true;

            } catch {
                passed = false;
            }

            return passed;
        }
    }
    
}
