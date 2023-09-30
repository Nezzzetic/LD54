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
    Content CurrentWatchContent;
    List<Content> contentList;
    List<Content> watchedContentList;
    public BitView BitViewPrefab;
    public float DownloadTime;
    public float DownloadTimer;
    public float WatchTimer;
    public float WatchTime;
    public float LifeTimer;
    public float LifeTime;
    public float LifeLenghtMulty;
    public float DivisionsMulty;
    public Text watchTimerText;
    public Text lifeTimerText;
    public Text desireText;
    public int Size;
    public int Setup;
    public int SelectedType;
    public Vector2Int Type0Size;
    public Vector2Int Type1Size;
    public Vector2Int Type2Size;
    public bool LoadActive;
    public int DesireType;
    public int[] DesireTypesLimits;
    void Start()
    {
        contentList=new List<Content>();
        watchedContentList=new List<Content>();
        storage.SpaceTransform = transform;
        storage.BitView = BitViewPrefab;
        storage.InitSpace(Size);
        LifeTimer = LifeTime/2;
        LoadActive = true;
        for (int i = 0; i < Size; i++)
        {
            storage.SpaceView[i].OnBitClick += consumeContent;
        }
        DownloadTimer = DownloadTime;
        for (int i = 0; i < Setup; i++)
        {
            if (CurrentContent == null || CurrentContent.Placed) {
                SelectedType = Random.Range(0, 3);
                CreateContent(SelectedType); 
            }
            SelectedType = 0;
            placeContent();
        }
        RollDesire();
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
        UpdateLife();
    }

    private void UpdateDownload()
    {
        if (!LoadActive) return;
        if (DownloadTimer > 0)
        {
            DownloadTimer -= Time.deltaTime;
            if (DownloadTimer <= 0)
            {
                if (CurrentContent == null || CurrentContent.Placed) { CreateContent(SelectedType); }
                placeContent();
                DownloadTimer = DownloadTime;
            }
        }
    }

    private void UpdateLife()
    {
        if (LifeTimer > 0)
        {
            LifeTimer -= Time.deltaTime;
            lifeTimerText.text = LifeTimer.ToString();
            if (LifeTimer <= 0)
            {
                LifeTimer= - 1;
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
                deleteContent(CurrentWatchContent);
                RollDesire();
            }
        }
    }

    void CreateContent(int type)
    {
        int rnd = 0;
        if (type==0) rnd = Random.Range(Type0Size.x, Type0Size.y);
        if (type==1) rnd = Random.Range(Type1Size.x, Type1Size.y);
        if (type==2) rnd = Random.Range(Type2Size.x, Type2Size.y);
        CurrentContent = ContentFactory.CreateContent(rnd, type);
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
        LifeTimer += cont.Coords.Count * LifeLenghtMulty;
        if (LifeTimer > LifeTime) LifeTimer = LifeTime;
        Debug.Log(cont.Divisions);
        ContentWindow.SetActive(true);
        viewStorage();
    }

    void consumeContent(BitView bit)
    {
        if (WatchTimer > 0) return;
        var cont = bit.content;
        if (DesireType != cont.Type) return;
        watchedContentList.Add(cont);
        contentList.Remove(cont);
        cont.Watched = true;
        WatchTimer = WatchTime * cont.Divisions * DivisionsMulty * cont.Coords.Count;
        LifeTimer += cont.Coords.Count * LifeLenghtMulty;
        if (LifeTimer > LifeTime) LifeTimer = LifeTime;
        Debug.Log(cont.Divisions);
        ContentWindow.SetActive(true);
        CurrentWatchContent = cont;
        viewStorage();
    }

    public void deleteContent(Content content)
    {
        storage.RemoveContent(content);
        if (watchedContentList.Contains(content)) watchedContentList.Remove(content);
        if (contentList.Contains(content)) contentList.Remove(content);
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

    public void ChangeContentType(int type)
    {
        if (SelectedType == type && LoadActive)
            LoadActive = false;
        else {
            LoadActive = true;
            if (SelectedType != type)
            {
                CurrentContent = null;

            }
            SelectedType = type;
        }
        
    }

    private void RollDesire()
    {
        var rnd = Random.Range(0, 100);
        DesireType = -1;
        for (int i = 0; i < DesireTypesLimits.Length; i++)
        {
            if (rnd< DesireTypesLimits[i])
            {
                DesireType = i; break;
            }
        }
        if (DesireType == -1) DesireType = 2;
        desireText.text = DesireType.ToString();
    }
}
