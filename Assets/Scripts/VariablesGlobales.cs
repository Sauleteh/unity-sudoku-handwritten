using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariablesGlobales : MonoBehaviour
{
    public int dibujable = 0;
    public float[] coords = { 0, 0 };

    public void setDibujable(int n) { this.dibujable = n; }
    public int getDibujable() { return this.dibujable; }

    public void setCoords(float x, float y)
    {
        this.coords[0] = x;
        this.coords[1] = y;
    }
    public float[] getCoords() { return this.coords; }
}
