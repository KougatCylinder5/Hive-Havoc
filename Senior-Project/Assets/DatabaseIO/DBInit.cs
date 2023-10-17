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

       
    }

    private void FixMissing() {
        DBAccess.AddTableIfMissing("saves", "id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT NOT NULL UNIQUE, last_play TEXT, rank INTEGER, play_time INTEGER, day REAL"); //Table name followed by colume names and datatypes seperated by comma (,)
        DBAccess.AddTableIfMissing("placeables", "id INTEGER PRIMARY KEY AUTOINCREMENT, tile_item_id INTEGER, save_id INTEGER, x_pos REAL, y_pos REAL, health REAL, heading REAL, natural INTEGER");
        DBAccess.AddTableIfMissing("tile_items", "id INTEGER PRIMARY KEY AUTOINCREMENT, name VARCHAR(50) UNIQUE");
        DBAccess.AddTableIfMissing("unit", "id INTEGER PRIMARY KEY AUTOINCREMENT, save_id INTEGER, type INTEGER, x_pos REAL, y_pos REAL, target_x REAL, target_y REAL, health REAL");
        DBAccess.AddTableIfMissing("resources", "id INTEGER PRIMARY KEY AUTOINCREMENT, name VARCHAR(50)");
        DBAccess.AddTableIfMissing("inventory", "save_id INTEGER, resource_id INTEGER, amount INTEGER, PRIMARY KEY (save_id, resource_id)");
        DBAccess.AddTableIfMissing("tile_data", "id INTEGER PRIMARY KEY AUTOINCREMENT, save_id INTEGER, x_pos INTEGER, y_pos INTEGER, type INTEGER");
    }

}

