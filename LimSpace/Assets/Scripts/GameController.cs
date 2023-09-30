using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Storage storage;
    public GameObject ContentWindow;
    Content CurrentContent;
    List<Content> contentList;
    List<Content> watchedContentList;
    public BitView BitViewPrefab;
    public float DownloadTime;
    public float DownloadTimer;
    public float WatchTimer;
    public float WatchTime;
    public float DivisionsMulty;
    public Text watchTimerText;
    void Start()
    {
        contentList=new List<Content>();
        watchedContentList=new List<Content>();
        storage.SpaceTransform = transform;
        storage.BitView = BitViewPrefab;
        storage.InitSpace(15);
        for (int i = 0; i < 15; i++)
        {
            storage.SpaceView[i].OnBitClick += deleteContent;
        }
        DownloadTimer = DownloadTime;
        viewStorage();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (WatchTimer<=0) { consumeContent(); }
        }
        UpdateDownload();
        UpdateWatching();
    }

    private void UpdateDownload()
    {
        if (DownloadTimer > 0)
        {
            DownloadTimer -= Time.deltaTime;
            if (DownloadTimer <= 0)
            {
                if (CurrentContent == null || CurrentContent.Placed) { CreateContent(); }
                placeContent();
                DownloadTimer = DownloadTime;
            }
        }
    }

    private void UpdateWatching()
    {
        if (WatchTimer > 0)
        {
            WatchTimer -= Time.deltaTime;
            watchTimerText.text = WatchTimer.ToString();
            if (WatchTimer <= 0)
            {
                WatchTimer = 0;
                ContentWindow.SetActive(false);
            }
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
        WatchTimer = WatchTime * cont.Divisions* DivisionsMulty*cont.Coords.Count;
        Debug.Log(cont.Divisions);
        ContentWindow.SetActive(true);
        viewStorage();
    }

    public void deleteContent(Content content)
    {
        
        storage.RemoveContent(content);
        watchedContentList.Remove(content);
        viewStorage();
    }

    public void deleteContent(BitView bit)
    {
        if (WatchTimer > 0) return;
        var content = bit.content;
        storage.RemoveContent(content);
        if (watchedContentList.Contains(content)) watchedContentList.Remove(content);
        if (contentList.Contains(content)) contentList.Remove(content);
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
