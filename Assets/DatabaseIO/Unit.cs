using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit {
    private int id;
    private float xPos;
    private float yPos;
    private float xTarget;
    private float yTarget;
    private float health;

    public Unit(int id, float xPos, float yPos, float xTarget, float yTarget, float health) {
        this.id = id;
        this.xPos = xPos;
        this.yPos = yPos;
        this.xTarget = xTarget;
        this.yTarget = yTarget;
        this.health = health;
    }

    public int getID() {
        return id;
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
