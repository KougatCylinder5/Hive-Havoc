using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.UIElements.Experimental;
using UnityEngine.Rendering.HighDefinition;
using static UnityEngine.GraphicsBuffer;

public class DBAccess
{
    private static int saveID = 0;
    private static int diff = -1;
    private static string dbConnectionString = "URI=file:" + Application.persistentDataPath + "\\storage.db"; //e
    private static bool transactionActive = false;
    private const string noTransactionError = "Transaction has not been started!";
    private static SqliteConnection sqliteDB = new SqliteConnection(dbConnectionString);

    protected static string getConnectionString() {
        return dbConnectionString;
    }

    public static void startTransaction() {
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

    public static void commitTransaction() {
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

    public static void rollbackTransaction() {
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
            sqliteCommand.CommandText = "SELECT id, dif FROM saves WHERE name LIKE '" + savename + "';";
            IDataReader saves = sqliteCommand.ExecuteReader();

            try {
                while(saves.Read()) {
                    saveID = saves.GetInt32(0);
                }
            } catch {}

            if(saveID != 0) {
                diff = saves.GetInt32(1);
                return true;
            }

            return false;
        }
    }

    public static List<Save> getSaves() {
        if (!transactionActive) {
            Debug.LogError(noTransactionError);
            return new List<Save>();
        } else {
            List<Save> saves = new List<Save>();

            var sqliteCommand = sqliteDB.CreateCommand();

            sqliteCommand.CommandText = "SELECT name, dif, rank, last_play, play_time, thumbnail, level_name FROM saves;";
            IDataReader asave = sqliteCommand.ExecuteReader();

  
                while (asave.Read()) {
                    saves.Add(new Save(asave.GetString(0), asave.GetInt32(1), asave.GetInt32(2), asave.GetString(3), asave.GetInt32(4), asave.GetString(5), asave.GetString(6)));
                    Debug.Log("save: " + asave.GetString(0));
                }

            return saves;
        }
    }

    public static void exitSave() {
        saveID = 0;
    }

    public static bool addSave(string savename, int diff) {
        if(!transactionActive) {
            Debug.LogError(noTransactionError);
            return false;
        } else {
            try
            {
                var sqliteCommand = sqliteDB.CreateCommand();
                sqliteCommand.CommandText = "INSERT INTO saves ('name', 'dif', 'rank', last_play, play_time, thumbnail, level_name) VALUES ('" + savename + "', '" + diff + "', '0', 'Never', '0', '', 'Home Base');";
                sqliteCommand.ExecuteNonQuery();
                return true;
            }
            catch { }

            return true;
        }
    }

    public static void deleteSave(string savename) {
        if (!transactionActive) {
            Debug.LogError(noTransactionError);
        } else {
            var sqliteCommand = sqliteDB.CreateCommand();

            sqliteCommand.CommandText = "DELETE FROM saves WHERE name LIKE '" + savename + "';";
            try {
                sqliteCommand.ExecuteNonQuery();

            } catch {
                Debug.LogWarning("Can't delete save: " + savename);
                
            }
        }
    }

    public static List<Unit> getUnits() {
        if(!transactionActive) {
            Debug.LogError(noTransactionError);
            return new List<Unit>();
        } else {
            List<Unit> units = new List<Unit>();

            var sqliteCommand = sqliteDB.CreateCommand(); 
            
            sqliteCommand.CommandText = "SELECT id, type, x_pos, y_pos, target_x, target_y, health, path_mode FROM unit WHERE save_id IS " + saveID + ";";
            IDataReader aunit = sqliteCommand.ExecuteReader();

            try {
                while(aunit.Read()) {
                    units.Add(new Unit(aunit.GetInt32(0), aunit.GetInt32(1), aunit.GetFloat(2), aunit.GetFloat(3), aunit.GetFloat(4), aunit.GetFloat(5), aunit.GetFloat(6), aunit.GetInt32(7)));
                }
            } catch {}

            return units;
        }
    }

    public static int addUnit(int type, float xPos, float yPos, float xTarget, float yTarget, float health, int pathMode) {
        if(!transactionActive) {
            Debug.LogError(noTransactionError);
            return 0;
        } else {
            int rowid = 0;
            var sqliteCommand = sqliteDB.CreateCommand();
            sqliteCommand.CommandText = "INSERT INTO unit ('x_pos', 'y_pos', 'target_x', 'target_y', 'health', 'type', 'path_mode', 'save_id') VALUES ('" + xPos + "', '" + yPos + "', '" + xTarget + "', '" + yTarget + "', '" + health + "', '" + type + "', '" + pathMode + "','" + saveID + "');";
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

    public static void setUnit(int id, float xPos, float yPos, float xTarget, float yTarget, float health, int pathMode) {
        if(!transactionActive) {
            Debug.LogError(noTransactionError);
        } else {
            var sqliteCommand = sqliteDB.CreateCommand();
            sqliteCommand.CommandText = " UPDATE unit SET x_pos=" + xPos + ", y_pos=" + yPos + ", target_x=" + xTarget + ", target_y=" + yTarget + ", health=" + health + ", path_mode=" + pathMode + " WHERE id IS " + id + ";";
            sqliteCommand.ExecuteNonQuery();
        }
    }

    public static void deleteUnit(int id) {
        if (!transactionActive) {
            Debug.LogError(noTransactionError);
        } else {
            var sqliteCommand = sqliteDB.CreateCommand();

            sqliteCommand.CommandText = "DELETE FROM unit WHERE save_id LIKE '" + saveID + "' AND id IS " + id + ";";
            try {
                sqliteCommand.ExecuteNonQuery();

            } catch {
                Debug.LogWarning("Can't delete Unit: " + id);

            }
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

    public static int getTileItem(string itemName) {
        if(!transactionActive) {
            Debug.LogError(noTransactionError);
            return 0;
        } else {

            var sqliteCommand = sqliteDB.CreateCommand(); 
            int itemid = 0;
            sqliteCommand.CommandText = "SELECT id, FROM tile_items WHERE name IS '" + itemName + "';";
            IDataReader titem = sqliteCommand.ExecuteReader();

            try {
                while(titem.Read()) {
                    itemid = titem.GetInt32(0);
                }
            } catch {}

            return itemid;
        }
    }

    public static bool addResource(string resourcesName) {
        if (!transactionActive) {
            Debug.LogError(noTransactionError);
            return false;
        }
        else
        {
            bool passed = false;

            var sqliteCommand = sqliteDB.CreateCommand();

            sqliteCommand.CommandText = "INSERT INTO resources ('name') VALUES ('" + resourcesName + "');";
            try {
                sqliteCommand.ExecuteNonQuery();
                passed = true;

            } catch {
                passed = false;
            }

            return passed;
        }
    }

    public static int getResource(string resourcesName) {
        if (!transactionActive) {
            Debug.LogError(noTransactionError);
            return 0;
        } else {

            var sqliteCommand = sqliteDB.CreateCommand();
            int itemid = 0;
            sqliteCommand.CommandText = "SELECT id, FROM resources WHERE name IS '" + resourcesName + "';";
            IDataReader titem = sqliteCommand.ExecuteReader();

            try {
                while (titem.Read()) {
                    itemid = titem.GetInt32(0);
                }
            } catch { }

            return itemid;
        }
    }

    public static List<Placeable> getPlaceables() {
        if(!transactionActive) {
            Debug.LogError(noTransactionError);
            return new List<Placeable>();
        } else {
            List<Placeable> placeables = new List<Placeable>();

            var sqliteCommand = sqliteDB.CreateCommand(); 
            
            sqliteCommand.CommandText = "SELECT id, tile_item_id, x_pos, y_pos, health, heading, natural FROM placeables WHERE save_id IS " + saveID + ";";
            IDataReader aplaceable = sqliteCommand.ExecuteReader();

            try {
                while(aplaceable.Read()) {
                    placeables.Add(new Placeable(aplaceable.GetInt32(0), aplaceable.GetInt32(1), aplaceable.GetFloat(2), aplaceable.GetFloat(3), aplaceable.GetFloat(4), aplaceable.GetFloat(5), aplaceable.GetInt32(6)));
                }
            } catch {}

            return placeables;
        }
    }

    public static List<Placeable> getNaturalPlaceables()
    {
        if (!transactionActive) {
            Debug.LogError(noTransactionError);
            return new List<Placeable>();
        } else {
                    List<Placeable> placeables = new List<Placeable>();

            var sqliteCommand = sqliteDB.CreateCommand();

            sqliteCommand.CommandText = "SELECT id, tile_item_id, x_pos, y_pos, health, heading, natural FROM placeables WHERE save_id IS " + saveID + " AND natural IS 1;";
            IDataReader aplaceable = sqliteCommand.ExecuteReader();

            try {
                while (aplaceable.Read()) {
                    placeables.Add(new Placeable(aplaceable.GetInt32(0), aplaceable.GetInt32(1), aplaceable.GetFloat(2), aplaceable.GetFloat(3), aplaceable.GetFloat(4), aplaceable.GetFloat(5), aplaceable.GetInt32(6)));
                }
            }
            catch { }

            return placeables;
        }
    }

    public static List<Placeable> getUnnaturalPlaceables() {
        if (!transactionActive) {
            Debug.LogError(noTransactionError);
            return new List<Placeable>();
        } else {
            List<Placeable> placeables = new List<Placeable>();

            var sqliteCommand = sqliteDB.CreateCommand();

            sqliteCommand.CommandText = "SELECT id, tile_item_id, x_pos, y_pos, health, heading, natural FROM placeables WHERE save_id IS " + saveID + " AND natural IS 0;";
            IDataReader aplaceable = sqliteCommand.ExecuteReader();

            try {
                while (aplaceable.Read())
                {
                    placeables.Add(new Placeable(aplaceable.GetInt32(0), aplaceable.GetInt32(1), aplaceable.GetFloat(2), aplaceable.GetFloat(3), aplaceable.GetFloat(4), aplaceable.GetFloat(5), aplaceable.GetInt32(6)));
                }
            }
            catch { }

            return placeables;
        }
    }

    public static int addPlaceable(int tileItemID, float xPos, float yPos, float xTarget, float yTarget, float health, int natural) {
        if(!transactionActive) {
            Debug.LogError(noTransactionError);
            return 0;
        } else {
            int rowid = 0;
            var sqliteCommand = sqliteDB.CreateCommand();
            sqliteCommand.CommandText = "INSERT INTO placeables ('x_pos', 'y_pos', 'health', 'heading', 'save_id', 'tile_item_id', 'natural') VALUES ('" + xPos + "', '" + yPos + "', '" + xTarget + "', '" + yTarget + "', '" + health + "', '" + saveID + "', '" + tileItemID + "', '" + natural + "');";
            sqliteCommand.ExecuteNonQuery();

            sqliteCommand.CommandText = "SELECT last_insert_rowid() FROM placeables;";
            IDataReader lastRow = sqliteCommand.ExecuteReader();

            try {
                while(lastRow.Read()) {
                    rowid = lastRow.GetInt32(0);
                }
            } catch {}

            return rowid;
        }
    }

    public static void setPlaceable(int id, float xPos, float yPos, float health, float heading) {
        if(!transactionActive) {
            Debug.LogError(noTransactionError);
        } else {
            var sqliteCommand = sqliteDB.CreateCommand();
            sqliteCommand.CommandText = " UPDATE placeable SET x_pos=" + xPos + ", y_pos=" + yPos + ", health=" + health + ", heading=" + heading + " WHERE id IS " + id + ";";
            sqliteCommand.ExecuteNonQuery();
        }
    }

    public static void deletePlaceable(int id) {
        if (!transactionActive) {
            Debug.LogError(noTransactionError);
        } else {
            var sqliteCommand = sqliteDB.CreateCommand();

            sqliteCommand.CommandText = "DELETE FROM placeable WHERE save_id LIKE '" + saveID + "' AND id IS " + id + ";";
            try {
                sqliteCommand.ExecuteNonQuery();

            } catch {
                Debug.LogWarning("Can't delete Unit: " + id);

            }
        }
    }

    public static int addTile(int xPos, int yPos, int type) {
        if (!transactionActive) {
            Debug.LogError(noTransactionError);
            return 0;
        } else {
            int rowid = 0;
            var sqliteCommand = sqliteDB.CreateCommand();
            sqliteCommand.CommandText = "INSERT INTO tile_data ('x_pos', 'y_pos', 'type','save_id') VALUES ('" + xPos + "', '" + yPos + "', '" + type + "', '" + saveID + "');";
            sqliteCommand.ExecuteNonQuery();

            sqliteCommand.CommandText = "SELECT last_insert_rowid() FROM tile_data;";
            IDataReader lastRow = sqliteCommand.ExecuteReader();

            try {
                while (lastRow.Read()) {
                    rowid = lastRow.GetInt32(0);
                }
            } catch { }

            return rowid;
        }
    }

    public static int getTile(int xPos, int yPos) {
        if (!transactionActive) {
            Debug.LogError(noTransactionError);
            return 0;
        } else {

            var sqliteCommand = sqliteDB.CreateCommand();
            int tileTypeID = 0;
            sqliteCommand.CommandText = "SELECT type FROM tile_data WHERE x_pos IS '" + xPos + "' AND y_pos IS '" + yPos +"';";
            IDataReader tileType = sqliteCommand.ExecuteReader();

            try {
                while (tileType.Read()) {
                    tileTypeID = tileType.GetInt32(0);
                }
            }
            catch { }

            return tileTypeID;
        }
    }

    public static void deleteTile(int id) {
        if (!transactionActive) {
            Debug.LogError(noTransactionError);
        } else {
            var sqliteCommand = sqliteDB.CreateCommand();

            sqliteCommand.CommandText = "DELETE FROM tile_data WHERE save_id LIKE '" + saveID + "' AND id IS " + id + ";";
            try {
                sqliteCommand.ExecuteNonQuery();

            } catch {
                Debug.LogWarning("Can't delete Unit: " + id);

            }
        }
    }

    public static void addItemToInventory(int id, int value) {
        if (!transactionActive) {
            Debug.LogError(noTransactionError);
        } else {
            var sqliteCommand = sqliteDB.CreateCommand();
            try {
                sqliteCommand.CommandText = "INSERT INTO tile_data ('resource_id', 'amount', save_id) VALUES ('" + id + "', '" + value + "', '" + saveID + "');";
            } catch { }
            sqliteCommand.ExecuteNonQuery();
        }
    }

    public static void updateInventory(int id, int value, int max) {
        if (!transactionActive) {
            Debug.LogError(noTransactionError);
        } else {
            var sqliteCommand = sqliteDB.CreateCommand();
            sqliteCommand.CommandText = " UPDATE placeable SET amount=" + value + "max=" + max + " WHERE resource_id IS " + id + " AND save_id IS " + saveID + ";";
            sqliteCommand.ExecuteNonQuery();
        }
    }

    public static int amountInInventory(int id) {
        if (!transactionActive) {
            Debug.LogError(noTransactionError);
            return 0;
        } else {

            var sqliteCommand = sqliteDB.CreateCommand();
            int itemAmount = 0;
            sqliteCommand.CommandText = "SELECT amount, FROM resources WHERE name IS '" + id + "' AND save_id IS '" + saveID + "';";
            IDataReader titem = sqliteCommand.ExecuteReader();

            try {
                while (titem.Read()) {
                    itemAmount = titem.GetInt32(0);
                }
            }
            catch { }

            return itemAmount;
        }
    }



}
