using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    public BitView[] SpaceView;
    public int CurrentHeadPosition;
    public Transform SpaceTransform;
    public BitView BitView;

    public void InitSpace(int Size)
    {
        SpaceView = new BitView[Size];
        for (int i = 0; i < Size; i++)
        {
            SpaceView[i] = Instantiate(BitView, SpaceTransform);
            SpaceView[i].transform.position += Vector3.right * 1.1f * i + Vector3.left * 8;
            SpaceView[i].ID = 0;
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
        if (CurrentHeadPosition== SpaceView.Length-1 || SpaceView[CurrentHeadPosition + 1].ID != 0) SpaceView[CurrentHeadPosition].Right.SetActive(true);
        if (CurrentHeadPosition == 0 || SpaceView[CurrentHeadPosition - 1].ID != cont.ID) SpaceView[CurrentHeadPosition].Left.SetActive(true);
        if (CurrentHeadPosition != 0 && SpaceView[CurrentHeadPosition - 1].ID != cont.ID) SpaceView[CurrentHeadPosition-1].Right.SetActive(true);
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
}
