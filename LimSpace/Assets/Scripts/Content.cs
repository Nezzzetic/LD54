using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Content
{
    public int ID;
    public List<int> Coords = new List<int>();
    public int Divisions;
    public int PartsRemains;
    public bool Placed;
    public bool Watched;
    public int Type;
    public Action<Content> OnPlaced = delegate { };

    public void PartPlaced(int x, bool separate = false)
    {
        Coords.Add(x);
        if (separate) Divisions++;
        PartsRemains--;
        if (PartsRemains == 0)
        {
            Placed = true;
            OnPlaced(this);
        }
    }

}
