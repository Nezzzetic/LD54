using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage
{
    public int[] Space;
    public int CurrentHeadPosition;

    public void InitSpace(int Size)
    {
        Space = new int[Size];
        for (int i = 0; i < Size; i++)
        {
            Space[i] = 0;
        }
        CurrentHeadPosition = 0;
    }

    private int _placeHeadToNextFreePosition()
    {
        var a = CurrentHeadPosition+1;
        while (a!= CurrentHeadPosition)
        {
            if (Space[a] == 0) return a;
            a++;
            if (a == Space.Length) a = 0;
        }
        return -1;
    }

    public void AddContentPiece(Content cont)
    {
        Space[CurrentHeadPosition] = cont.ID;
        if (CurrentHeadPosition>0 && Space[CurrentHeadPosition-1]!=cont.ID) cont.Divisions++;
    }

    public void RemoveContentPiece(Content cont)
    {
        for (int i = 0; i < Space.Length; i++)
        {
            if (Space[i] == cont.ID) Space[i] = 0;
        }
    }
}
