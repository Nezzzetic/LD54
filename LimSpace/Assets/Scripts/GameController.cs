using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    Storage storage;
    Content CurrentContent;
    List<Content> contentList;
    void Start()
    {
        contentList=new List<Content>();
        storage = new Storage();
        storage.InitSpace(10);
        viewStorage();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CurrentContent==null || CurrentContent.Placed) { CreateContent();}
            placeContent();
        } 
    }

    void CreateContent()
    {
        CurrentContent = ContentFactory.CreateContent(3);
        CurrentContent.OnPlaced += ContentPlaced;
    }

    void placeContent()
    {
        storage.AddContentPiece(CurrentContent);
        viewStorage();
    }

    void viewStorage()
    {
        var s = "";
        for (int i = 0; i < storage.Space.Length; i++)
        {
            s += storage.Space[i] + "; ";
        }
        Debug.Log(s);
    }

    void ContentPlaced(Content cont)
    {
        contentList.Add(cont);
    }
}
