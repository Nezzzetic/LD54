using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Storage storage;
    Content CurrentContent;
    List<Content> contentList;
    List<Content> watchedContentList;
    public BitView BitViewPrefab;
    void Start()
    {
        contentList=new List<Content>();
        watchedContentList=new List<Content>();
        storage.SpaceTransform = transform;
        storage.BitView = BitViewPrefab;
        storage.InitSpace(15);
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
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            consumeContent();
        }
    }

    void CreateContent()
    {
        int rnd = Random.Range(2, 5);
        CurrentContent = ContentFactory.CreateContent(rnd);
        CurrentContent.OnPlaced += ContentPlaced;
    }

    void placeContent()
    {
        storage.AddContentPiece(CurrentContent);
        viewStorage();
    }

    void consumeContent()
    {
        if (contentList.Count == 0) return;
        int rnd=Random.Range(0, contentList.Count);
        var cont = contentList[rnd];
        watchedContentList.Add(cont);
        contentList.RemoveAt(rnd);
        cont.Watched = true;
        viewStorage();
    }

    public void deleteContent(Content content)
    {
        
        storage.RemoveContent(content);
        watchedContentList.Remove(content);
        viewStorage();
    }

    void viewStorage()
    {
        for (int i = 0; i < storage.SpaceView.Length; i++)
        {
           storage.SpaceView[i].UpdateState();
        }
    }

    void ContentPlaced(Content cont)
    {
        contentList.Add(cont);
    }
}
