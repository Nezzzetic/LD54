using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    public BitView[] SpaceView;
    public int CurrentHeadPosition;
    public Transform SpaceTransform;
    public BitView BitView;
    public int Xsize;

    public void InitSpace(int Size)
    {
        SpaceView = new BitView[Size];
        var x = 0;
        var y = 0;
        for (int i = 0; i < Size; i++)
        {
            
            SpaceView[i] = Instantiate(BitView, SpaceTransform);
            SpaceView[i].transform.position += Vector3.right * 1.071f * x + Vector3.left * 7.5f+ Vector3.up * 1.2f * y + Vector3.down * 4.2f;
            SpaceView[i].ID = 0;
            x++;
            if (x == Xsize)
            {
                x = 0;
                y++;
            }
        }
        CurrentHeadPosition = 0;

    }

    private int _findNextFreePosition()
    {
        var t = 0;
        var a = CurrentHeadPosition + 1;
        while (a!= CurrentHeadPosition && t<1000)
        {
            if (a == SpaceView.Length) a = 0;
            if (SpaceView[a].ID == 0) return a;
            a++;
            t++;
        }
        return -1;
    }

    private void _addContentPiece(Content cont)
    {
        SpaceView[CurrentHeadPosition].ID = cont.ID;
        SpaceView[CurrentHeadPosition].content = cont;
        cont.PartPlaced(CurrentHeadPosition, CurrentHeadPosition == 0 || SpaceView[CurrentHeadPosition - 1].ID != cont.ID);
        if (CurrentHeadPosition == SpaceView.Length - 1 || SpaceView[CurrentHeadPosition + 1].ID != 0) SpaceView[CurrentHeadPosition].Right.SetActive(true);
        if (CurrentHeadPosition == 0 || SpaceView[CurrentHeadPosition - 1].ID != cont.ID) SpaceView[CurrentHeadPosition].Left.SetActive(true);
        if (CurrentHeadPosition != 0 && SpaceView[CurrentHeadPosition - 1].ID != cont.ID) SpaceView[CurrentHeadPosition - 1].Right.SetActive(true);
        var a = _findNextFreePosition();
        if (a >= 0)
        {
            CurrentHeadPosition = a;
        }
    }


    public int AddContentPiece(Content cont)
    {
        if (SpaceView[CurrentHeadPosition].ID == 0) {
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

    public void RemoveContent(Content cont)
    {
        for (int i = 0; i < SpaceView.Length; i++)
        {
            if (SpaceView[i].ID == cont.ID)
            {
                SpaceView[i].ID = 0;
                SpaceView[i].content = null;
                SpaceView[i].Left.SetActive(false);
                SpaceView[i].Right.SetActive(false);
            }
        }
    }

    public void Clear()
    {
        for (int i = 0; i < SpaceView.Length; i++)
        {
            SpaceView[i].ID = 0;
            SpaceView[i].content = null;
            SpaceView[i].Left.SetActive(false);
            SpaceView[i].Right.SetActive(false);
        }
        CurrentHeadPosition= 0;
    }
}
