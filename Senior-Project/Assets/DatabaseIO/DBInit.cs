using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;

public class DBInit : MonoBehaviour
{
    void Start()
    {
        DBAccess.startTransaction();
        FixMissing();
        DBAccess.commitTransaction();
    }

    private void FixMissing() {
        DBAccess.AddTableIfMissing("saves", "id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT NOT NULL UNIQUE, dif INTEGER, rank INTEGER, last_play TEXT, play_time INTEGER, thumbnail TEXT, level_name TEXT"); //Table name followed by colume names and datatypes seperated by comma (,)
        DBAccess.AddTableIfMissing("placeables", "id INTEGER PRIMARY KEY AUTOINCREMENT, tile_item_id INTEGER, save_id INTEGER REFERENCES saves(id) ON DELETE CASCADE, x_pos REAL, y_pos REAL, health REAL, natural INTEGER");
        //DBAccess.AddTableIfMissing("tile_items", "id INTEGER PRIMARY KEY AUTOINCREMENT, name VARCHAR(50) UNIQUE");
        DBAccess.AddTableIfMissing("unit", "id INTEGER PRIMARY KEY AUTOINCREMENT, save_id INTEGER REFERENCES saves(id) ON DELETE CASCADE, type INTEGER, x_pos REAL, y_pos REAL, target_x REAL, target_y REAL, health REAL, path_mode INTEGER");
        DBAccess.AddTableIfMissing("resources", "id INTEGER PRIMARY KEY AUTOINCREMENT, name VARCHAR(50)");
        DBAccess.AddTableIfMissing("inventory", "save_id INTEGER REFERENCES saves(id) ON DELETE CASCADE, resource_id INTEGER, amount INTEGER, max INTEGER, PRIMARY KEY (save_id, resource_id)");
        DBAccess.AddTableIfMissing("tile_data", "id INTEGER PRIMARY KEY AUTOINCREMENT, save_id INTEGER REFERENCES saves(id) ON DELETE CASCADE, x_pos INTEGER, y_pos INTEGER, type INTEGER");
        DBAccess.AddTableIfMissing("played_levels", "save_id INTEGER REFERENCES saves(id) ON DELETE CASCADE, level_id INTEGER, PRIMARY KEY (save_id, level_id)");
        //DBAccess.AddTableIfMissing("unlocked_tech", "save_id INTEGER REFERENCES saves(id) ON DELETE CASCADE, tech_id INTEGER, PRIMARY KEY (save_id, tech_id)");
        DBAccess.AddTableIfMissing("level_data", "save_id INTEGER REFERENCES saves(id) ON DELETE CASCADE, key TEXT, value TEXT, PRIMARY KEY (save_id, key)");
    }

}

