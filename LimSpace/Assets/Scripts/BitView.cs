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
    public GameObject Border;

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
        if (ID == 0) { Renderer.color = Color.gray; Icon.enabled=false; Border.SetActive(false);
        } else {
            Icon.enabled = true;
            if (content.Placed) {
                Renderer.color = content.Color;
                Icon.color = Color.white;
            } else
            {
                Icon.color = Color.gray;
                Renderer.color = Color.black;
            }
            Icon.sprite = TypeToSprite[content.Type];
            Border.SetActive(content.Divisions > 1);
        }


    }

    private void OnMouseUp()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
            OnBitClick(this);
    }

}
