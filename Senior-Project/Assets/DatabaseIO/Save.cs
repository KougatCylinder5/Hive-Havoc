using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save : MonoBehaviour
{
    private string saveName;
    private int dif;
    private int rank;
    private string lastPlay;
    private int playTime;
    private string thumbnail;

    public Save(string saveName, int dif, int rank, string lastPlay, int playTime , string thumbnail) {
        this.saveName = saveName;
        this.dif = dif;
        this.rank = rank;
        this.lastPlay = lastPlay;
        this.playTime = playTime;
        this.thumbnail = thumbnail;
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

    public int getPlayTime() {
        return playTime;
    }

    public string getThumbnail() {
        return thumbnail;
    }

    public void increaseRank() {
        rank++;
    } 
}
