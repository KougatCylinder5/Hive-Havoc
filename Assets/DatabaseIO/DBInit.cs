using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;

public class DBInit : MonoBehaviour
{
    void Start()
    {
        FixMissing();
        Debug.Log("Was save created? " + DBAccess.addSave("test2"));
        
        Debug.Log("Can the save be accessed? " + DBAccess.selectSave("test2"));

        Debug.Log(DBAccess.addUnit(0,0,0,0,0));

        //DBAccess.setUnit(1,1,1,1,1,1);

        List<Unit> units = DBAccess.getUnits();
        foreach(Unit unit in units){
            Debug.Log("Has Unit ID: " + unit.getID());
        }
    }

    private void FixMissing() {
        DBAccess.AddTableIfMissing("saves", "id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT NOT NULL UNIQUE, last_play TEXT, rank INTEGER, play_time INTEGER, day REAL"); //Table name followed by colume names and datatypes seperated by comma (,)
        DBAccess.AddTableIfMissing("placeables", "id INTEGER PRIMARY KEY AUTOINCREMENT, tile_item_id INTEGER, save_id INTEGER, x_pos REAL, y_pos REAL, unit_id INTEGER health REAL, heading REAL");
        DBAccess.AddTableIfMissing("tile_items", "id INTEGER PRIMARY KEY AUTOINCREMENT, name VARCHAR(50)");
        DBAccess.AddTableIfMissing("unit", "id INTEGER PRIMARY KEY AUTOINCREMENT, save_id INTEGER, x_pos REAL, y_pos REAL, target_x REAL, target_y REAL, health REAL");
        DBAccess.AddTableIfMissing("resources", "id INTEGER PRIMARY KEY AUTOINCREMENT, name VARCHAR(50)");
        DBAccess.AddTableIfMissing("invertory", "save_id INTEGER, resource_id INTEGER, amount INTEGER, PRIMARY KEY (save_id, resource_id)");
    }

}

