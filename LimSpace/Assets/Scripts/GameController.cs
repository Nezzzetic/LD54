using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using TMPro;

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
    public float LifeTimeMax;
    public float LifeLenghtMulty;
    public float Points;
    public float MaxPoints;
    public float DivisionsMulty;
    public float DefragMulty;
    public Text watchTimerText;
    public Text lifeTimerText;
    public TMP_Text pointsText;
    public Text maxpointsText;
    public Image desireText;
    public Image timeProgressBar;
    public int Size;
    public int Setup;
    public int SelectedType;
    public Vector2Int Type0Size;
    public Vector2Int Type1Size;
    public Vector2Int Type2Size;
    public bool LoadActive;
    public int DesireType;
    public int[] DesireTypesLimits;
    public Color[] TypeToColor;
    public Sprite[] TypeToSprite;
    public int ColorIndex;
    public List<Vector2Int> StartSet;
    public List<AudioClip[]> ContentAudioClips; 
    public AudioClip[] ImageAudioClips;
    public AudioClip[] MusicAudioClips;
    public AudioClip[] VideoAudioClips;
    public AudioSource ContentAudioSource;
    public AudioSource FailAudioSource;
    public AudioSource DefragAudioSource;
    void Start()
    {
        ContentAudioClips = new List<AudioClip[]>
        {
            ImageAudioClips,
            MusicAudioClips,
            VideoAudioClips
        };

        contentList =new List<Content>();
        watchedContentList=new List<Content>();
        storage.SpaceTransform = transform;
        storage.BitView = BitViewPrefab;
        storage.InitSpace(Size);
        for (int i = 0; i < Size; i++)
        {
            storage.SpaceView[i].OnBitClick += consumeContent;
            storage.SpaceView[i].TypeToSprite = TypeToSprite;
            storage.SpaceView[i].TypeToColor = TypeToColor;
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
        var a = StartSetupFromConfig(StartSet);
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
            timeProgressBar.fillAmount= LifeTimer/ LifeTimeMax;
            lifeTimerText.text = LifeTimer.ToString();
            pointsText.text = Points+"/200";
            maxpointsText.text = MaxPoints.ToString();
            if (LifeTimer <= 0)
            {
                LifeTimer= - 1;
                SceneManager.LoadScene(1);
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
        CurrentContent.Color = TypeToColor[ColorIndex];
        ColorIndex++;
        if (ColorIndex == TypeToColor.Length) ColorIndex = 0;
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
        CurrentContent.Color = TypeToColor[ColorIndex];
        ColorIndex++;
        if (ColorIndex == TypeToColor.Length) ColorIndex = 0;
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
        var delta = cont.Coords.Count * LifeLenghtMulty - (cont.Divisions - 1) * DivisionsMulty;
        Points += delta;
        LifeTimer = LifeTime;
        if (MaxPoints <= Points) MaxPoints = Points;
        if (Points>=200) SceneManager.LoadScene(2);
        if (delta < 0)
            FailAudioSource.Play();
        else
        {
            var rnd = Random.Range(0, ContentAudioClips[DesireType].Length);
            ContentAudioSource.clip = ContentAudioClips[DesireType][rnd];
            ContentAudioSource.Play();
        }
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
        desireText.sprite = TypeToSprite[DesireType];
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
        var res2= new List<Vector2Int>();
        var used = 0;
        while (used < Setup)
        {
            RollDesire();
            var size = getSizeByType(DesireType);
            used += size;
            res.Add(new Vector2Int(DesireType, size));
        }
        var rnd = new System.Random();
        var randomized = res.OrderBy(item => rnd.Next());
        foreach (var item in randomized)
        {
            res2.Add(item);
        }
        return res2;
    }

    private List<Vector2Int> StartSetupFromConfig(List<Vector2Int> setup)
    {
        var res2 = new List<Vector2Int>();
        var rnd = new System.Random();
        var randomized = setup.OrderBy(item => rnd.Next());
        foreach (var item in randomized)
        {
            res2.Add(item);
        }
        return res2;
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
        DefragAudioSource.Play();
    }

}
