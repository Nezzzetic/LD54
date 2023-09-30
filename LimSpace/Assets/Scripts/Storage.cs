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

    private int _findNextFreePosition()
    {
        var a = CurrentHeadPosition + 1;
        while (a!= CurrentHeadPosition)
        {
            if (a == Space.Length) a = 0;
            if (Space[a] == 0) return a;
            a++;
        }
        return -1;
    }

    private void _addContentPiece(Content cont)
    {
        Space[CurrentHeadPosition] = cont.ID;
        cont.PartPlaced(CurrentHeadPosition, CurrentHeadPosition > 0 && Space[CurrentHeadPosition - 1] != cont.ID);
    }


    public int AddContentPiece(Content cont)
    {
        if (Space[CurrentHeadPosition] == 0) {
            _addContentPiece(cont); 
            return 0;
        }
        var a = _findNextFreePosition();
        if (a>=0)
        {
            CurrentHeadPosition = a;
            _addContentPiece(cont);
            return 0;
        }
        return -1;
    }

    public void RemoveContentPiece(Content cont)
    {
        for (int i = 0; i < Space.Length; i++)
        {
            if (Space[i] == cont.ID) Space[i] = 0;
        }
    }
}
