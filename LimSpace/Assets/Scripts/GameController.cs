using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    Storage storage;
    void Start()
    {
        storage= new Storage();
        storage.InitSpace(10);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void placeContent()
    {
        ContentFactory.CreateContent(3);
    }
}
