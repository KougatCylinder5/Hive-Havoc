using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;

public class DBInit : MonoBehaviour
{
    void Start()
    {
        DBAccess.startSave();
        FixMissing();
        DBAccess.commitSave();

        //TESTING ONLY. REMOVE LATER!

        DBAccess.startSave();
        Debug.Log("Was save created? " + DBAccess.addSave("test2", 0));
        DBAccess.commitSave();
        
        DBAccess.startSave();
        Debug.Log("Can the save be accessed? " + DBAccess.selectSave("test2"));
        DBAccess.commitSave();

        DBAccess.startSave();
        Debug.Log(DBAccess.addUnit(0,0,0,0,0,0));
        DBAccess.commitSave();

        //DBAccess.setUnit(1,1,1,1,1,1);

        DBAccess.startSave();
        List<Unit> units = DBAccess.getUnits();
        foreach(Unit unit in units){
            Debug.Log("Has Unit ID: " + unit.getID());
        }
        DBAccess.commitSave();

        //END TESTING.
    }

    private void FixMissing() {
        DBAccess.AddTableIfMissing("saves", "id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT NOT NULL UNIQUE, dif INTEGER, last_play TEXT, rank INTEGER, play_time INTEGER, day REAL"); //Table name followed by colume names and datatypes seperated by comma (,)
        DBAccess.AddTableIfMissing("placeables", "id INTEGER PRIMARY KEY AUTOINCREMENT, tile_item_id INTEGER, save_id INTEGER, x_pos REAL, y_pos REAL, health REAL, heading REAL, natural INTEGER");
        DBAccess.AddTableIfMissing("tile_items", "id INTEGER PRIMARY KEY AUTOINCREMENT, name VARCHAR(50) UNIQUE");
        DBAccess.AddTableIfMissing("unit", "id INTEGER PRIMARY KEY AUTOINCREMENT, save_id INTEGER, type INTEGER, x_pos REAL, y_pos REAL, target_x REAL, target_y REAL, health REAL");
        DBAccess.AddTableIfMissing("resources", "id INTEGER PRIMARY KEY AUTOINCREMENT, name VARCHAR(50)");
        DBAccess.AddTableIfMissing("inventory", "save_id INTEGER, resource_id INTEGER, amount INTEGER, PRIMARY KEY (save_id, resource_id)");
        DBAccess.AddTableIfMissing("tile_data", "id INTEGER PRIMARY KEY AUTOINCREMENT, save_id INTEGER, x_pos INTEGER, y_pos INTEGER, type INTEGER");
    }

}

