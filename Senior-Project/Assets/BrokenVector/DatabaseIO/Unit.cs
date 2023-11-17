using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit {
    private int id;
    private int type;
    private float xPos;
    private float yPos;
    private float xTarget;
    private float yTarget;
    private float health;
    private int pathMode;

    public Unit(int id, int type, float xPos, float yPos, float xTarget, float yTarget, float health, int pathMode) {
        this.id = id;
        this.type = type;
        this.xPos = xPos;
        this.yPos = yPos;
        this.xTarget = xTarget;
        this.yTarget = yTarget;
        this.health = health;
        this.pathMode = pathMode;

    }

    public int getID() {
        return id;
    }

    public int getType()
    {
        return type;
    }

    public float getXPos() {
        return xPos;
    }

    public float getYPos() {
        return yPos;
    }

    public float getXTarget() {
        return xTarget;
    }

    public float getYTarget() {
        return yTarget;
    }

    public float getHealth() {
        return health;
    }

    public int getPathMode() {
        return pathMode;
    }

    public void setXPos(float xPos) {
        this.xPos = xPos;
    }

    public void setYPos(float yPos) {
        this.yPos = yPos;
    }

    public void setXTarget(float xTarget) {
        this.xTarget = xTarget;
    }

    public void setYTarget(float yTarget) {
        this.yTarget = yTarget;
    }
}
