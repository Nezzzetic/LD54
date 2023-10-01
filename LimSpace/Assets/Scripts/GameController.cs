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
    public float Points;
    public float MaxPoints;
    public float DivisionsMulty;
    public float DefragMulty;
    public Text watchTimerText;
    public Text lifeTimerText;
    public Text pointsText;
    public Text maxpointsText;
    public Image desireText;
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
        for (int i = 0; i < Size; i++)
        {
            storage.SpaceView[i].OnBitClick += consumeContent;
        }
        DownloadTimer = DownloadTime;
        //for (int i = 0; i < Setup; i++)
        //{
        //    if (CurrentContent == null || CurrentContent.Placed) {
        //        RollDesire();
        //        CreateContent(DesireType); 
        //    }
        //    SelectedType = 0;
        //    placeContent();
        //}
        var a = StartSetup();
        PlaceSetup(a);

        RollDesire();
        viewStorage();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Defragmetation(); 
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
                if (CurrentContent.Placed) LoadActive = false;
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
            pointsText.text = Points.ToString();
            maxpointsText.text = MaxPoints.ToString();
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
        CurrentContent = ContentFactory.CreateContent(getSizeByType(type), type);
        CurrentContent.OnPlaced += ContentPlaced;
    }

    int getSizeByType(int type)
    {
        int rnd = 0;
        if (type == 0) rnd = Random.Range(Type0Size.x, Type0Size.y);
        if (type == 1) rnd = Random.Range(Type1Size.x, Type1Size.y);
        if (type == 2) rnd = Random.Range(Type2Size.x, Type2Size.y);
        return rnd;
    }

    void CreateContent(int type, int size)
    {
        CurrentContent = ContentFactory.CreateContent(size, type);
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
        WatchTimer = WatchTime * cont.Divisions* DivisionsMulty * cont.Coords.Count;
        Points += cont.Coords.Count * LifeLenghtMulty;
        if (MaxPoints <= Points) MaxPoints = Points;
        LifeTimer = LifeTime;
        Debug.Log(cont.Divisions);
        ContentWindow.SetActive(true);
        viewStorage();
    }

    void consumeContent(BitView bit)
    {
        if (WatchTimer > 0) return;
        var cont = bit.content;
        if (cont == null ) return;
        if (DesireType != cont.Type) return;

        Points += cont.Coords.Count * LifeLenghtMulty-(cont.Divisions-1)*DivisionsMulty;
        LifeTimer = LifeTime;
        if (MaxPoints <= Points) MaxPoints = Points;

        storage.RemoveContent(cont);
        if (watchedContentList.Contains(cont)) watchedContentList.Remove(cont);
        if (contentList.Contains(cont)) contentList.Remove(cont);
        ChangeContentType(DesireType);
        RollDesire();
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
        if (DesireType == 0) { desireText.color = Color.green; }
        if (DesireType == 1) { desireText.color = Color.blue; }
        if (DesireType == 2) { desireText.color = Color.red; }
    }

    private void PlaceSetup(List<Vector2Int> setup)
    {
        storage.Clear();
        for (int i = 0; i < setup.Count; i++)
        {
            var type = setup[i].x;
            var size = setup[i].y;
            CreateContent(type, size);
            for (int j = 0; j < size; j++)
            {
                placeContent();
            }
        }
    }
    private List<Vector2Int> StartSetup()
    {
        var res= new List<Vector2Int>();
        var used = 0;
        while (used < Setup)
        {
            RollDesire();
            var size = getSizeByType(DesireType);
            used += size;
            res.Add(new Vector2Int(DesireType, size));
        }
        return res;
    }

    private List<Vector2Int> GetSetupFromContent()
    {
        var res = new List<Vector2Int>();
        foreach (var item in contentList)
        {
            res.Add(new Vector2Int(item.Type, item.Coords.Count));
        }
        return res;
    }

    public void Defragmetation()
    {
        var setup = GetSetupFromContent();
        storage.Clear();
        contentList.Clear();
        PlaceSetup(setup);
        LifeTime=LifeTime * DefragMulty;
        if (LifeTimer> LifeTime) LifeTimer=LifeTime;
    }
}
