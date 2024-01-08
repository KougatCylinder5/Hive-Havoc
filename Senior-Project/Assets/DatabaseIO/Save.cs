using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save
{ // Holds information about a save for later.
    private string saveName;
    private int dif;
    private int rank;
    private string lastPlay;
    private float playTime;
    private string thumbnail;
    private string levelName;

    public Save(string saveName, int dif, int rank, string lastPlay, float playTime , string thumbnail, string levelName) {
        this.saveName = saveName;
        this.dif = dif;
        this.rank = rank;
        this.lastPlay = lastPlay;
        this.playTime = playTime;
        this.thumbnail = thumbnail;
        this.levelName = levelName;
    }

    public string getSaveName() {
        return saveName;
    }

    public int getDif() {
        return dif;
    }

    public int getRank() {
        return rank;
    }

    public string getLastPlay() {
        return lastPlay;
    }

    public float getPlayTime() {
        return playTime;
    }

    public string getThumbnail() {
        return thumbnail;
    }

    public string getLevelName() {
        return levelName;
    }

    public void increaseRank() {
        rank++;
    } 
}
