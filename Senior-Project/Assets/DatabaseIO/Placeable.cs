using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable
{
    private int id;
    private int tileItemID;
    private float xPos;
    private float yPos;
    private float health;
    private float heading;
    private int natural;

    public Placeable(int id, int tileItemID, float xPos, float yPos, float health, int natural) {
        this.id = id;
        this.tileItemID = tileItemID;
        this.xPos = xPos;
        this.yPos = yPos;
        this.health = health;
        this.natural = natural;
    }

    public int getID() {
        return id;
    }

    public int getTileItemID() {
        return tileItemID;
    }

    public float getXPos() {
        return xPos;
    }

    public float getYPos() {
        return yPos;
    }

    public float getHealth() {
        return health;
    }
    public int getNatural()
    {
        return natural;
    }

    public void setXPos(float xPos) {
        this.xPos = xPos;
    }

    public void setYPos(float yPos) {
        this.yPos = yPos;
    }

    public void setHealth(float health) {
        this.health = health;
    }
    public void setNatural(int natural)
    {
        this.natural = natural;
    }

    public override string ToString()
    {
        return xPos + ", " + yPos;
    }
}
