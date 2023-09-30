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
        if (content.Placed) { Renderer.color = Color.green; return; }
        if (!content.Placed) { Renderer.color = Color.gray; return; }
        if (!content.Watched) { Renderer.color = Color.cyan; }

    }

    private void OnMouseUp()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
            OnBitClick(this);
    }

}
