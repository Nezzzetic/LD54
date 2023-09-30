using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentFactory
{
    private static ContentFactory instance;
    public static ContentFactory Instance { get {
            if (instance == null)
            {
                instance = new ContentFactory();
                instance.Count = 1;
            }

            return instance; 
        } }

    public int Count;
    public static Content CreateContent(int size, int type) { 
        var a = new Content();
        a.ID = Instance.Count++;
        a.Coords = new List<int>();
        a.PartsRemains = size;
        a.Divisions = 0;
        a.Watched= false;
        a.Type = type;
        return a;
    }
}
