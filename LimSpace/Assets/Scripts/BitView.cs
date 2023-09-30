using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BitView : MonoBehaviour
{
    public int ID;
    public SpriteRenderer Renderer;
    public Content content;
    public GameObject Left;
    public GameObject Right;
    public Action<BitView> OnBitClick = delegate { };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateState()
    {
        if (ID == 0) { Renderer.color = Color.white; return; }
        if (content.Placed) {
            if (content.Watched) { Renderer.color = Color.cyan; return; }
            else {
                if (content.Type == 0) { Renderer.color = Color.green; }
                if (content.Type == 1) { Renderer.color = Color.blue; }
                if (content.Type == 2) { Renderer.color = Color.red; }
            }
            return; 
        }
        Renderer.color = Color.gray;

    }

    private void OnMouseUp()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
            OnBitClick(this);
    }

}
