using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CarChoosing : MonoBehaviour
{
    public GameObject[] cars;
    public Text carNames;
    int actCarId = 0;
    public ParticleSystem effect;

    void Start()
    {
        cars[actCarId].SetActive(true);
        carNames.text = cars[actCarId].GetComponent<CarInfos>().carName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Next()
    {
        if (actCarId != cars.Length -1 )
        {
            
            cars[actCarId].SetActive(false);
            actCarId++;
            cars[actCarId].SetActive(true);
            carNames.text = cars[actCarId].GetComponent<CarInfos>().carName;
            effect.Play();


        }
        else
        {
           
            cars[actCarId].SetActive(false);
            actCarId = 0;
            cars[actCarId].SetActive(true);
            carNames.text = cars[actCarId].GetComponent<CarInfos>().carName;
            effect.Play();
        }



        


    }

    public void Back()
    {
        if (actCarId != 0)
        {
            cars[actCarId].SetActive(false);
            actCarId--;
            cars[actCarId].SetActive(true);


        }
        else
        {
            cars[actCarId].SetActive(false);
            actCarId = cars.Length - 1;
            cars[actCarId].SetActive(true);

        }


    }

    public void Play()
    {
        PlayerPrefs.SetInt("ChosenCar", actCarId);
        SceneManager.LoadScene("mapChoosing");

    }


}
