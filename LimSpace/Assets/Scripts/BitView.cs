using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BitView : MonoBehaviour
{
    public int ID;
    public SpriteRenderer Renderer;
    public SpriteRenderer Icon;
    public Content content;
    public GameObject Left;
    public GameObject Right;
    public Action<BitView> OnBitClick = delegate { };
    public Color[] TypeToColor;
    public Sprite[] TypeToSprite;
    public Sprite DefaultSprite;

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
        if (ID == 0) { Renderer.color = Color.white; Icon.sprite = DefaultSprite; return; }
        if (content.Placed) {
            Renderer.color = content.Color;
        } else
        {
            Renderer.color = Color.gray;
        }
        Icon.sprite = TypeToSprite[content.Type];
        


    }

    private void OnMouseUp()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
            OnBitClick(this);
    }

}
