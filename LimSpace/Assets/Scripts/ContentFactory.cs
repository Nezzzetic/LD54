using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentFactory
{
    private static ContentFactory instance;
    public static ContentFactory Instance { get {
            if (instance != null) instance = new ContentFactory();
            return instance; 
        } }

    public int Count = 1;
    public static Content CreateContent(int size) { 
        var a = new Content();
        a.ID = Instance.Count++;
        a.Coords = new List<int>();
        a.PartsRemains = size;
        a.Divisions = 0;
        return a;
    }
}
