using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapManagement : MonoBehaviour
{

    string chosenMapName;
    public Button playButton;
    public void ChosenMap(string mapName)
    {
        chosenMapName = mapName;

    }
    void Start()
    {
        
    }
    void Update()
    {
        if (chosenMapName != null)
        {
            playButton.interactable = true;

        }

   
    }

    public void play()
    {
      
        SceneManager.LoadScene(chosenMapName);

    }

}
