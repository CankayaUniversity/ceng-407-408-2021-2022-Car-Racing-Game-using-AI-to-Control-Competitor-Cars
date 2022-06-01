using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{

    public GameObject[] cams;
    int activeCam = 0;


    void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            TurnCamsOff();
            activeCam++;
          
            if(activeCam > cams.Length - 1)
            {
                activeCam = 0;
                

            }
            cams[activeCam].SetActive(true);


        }

        
    }
    void TurnCamsOff()
    {
        foreach (var cam in cams)
        {
            cam.SetActive(false);


        }


    }


}
