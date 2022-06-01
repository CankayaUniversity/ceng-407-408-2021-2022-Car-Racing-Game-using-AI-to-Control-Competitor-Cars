using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Car;
using TMPro;

public class GeneralSettings : MonoBehaviour
{

    public GameObject[] cars;
    public GameObject spawnPlace;
    Transform pivot;
    Transform position;

    public GameObject[] AI_spawn;
    public GameObject[] AI_cars;
    List<GameObject> createdCars = new List<GameObject>();
    public TextMeshProUGUI countDown;
    float second = 3f;
    public AudioSource[] audios;
    bool time = true;
    Coroutine myTiming;
    public GameObject finishPanel;
    



    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        StartFunc();
        myTiming = StartCoroutine(TimeControl());


    }




    void StartFunc()
    {
        countDown.text = second.ToString();
        GameObject myCar = Instantiate(cars[PlayerPrefs.GetInt("ChosenCar")], spawnPlace.transform.position, spawnPlace.transform.rotation);

        pivot = myCar.transform.Find("Pivot");
        position = myCar.transform.Find("Position");

        GameObject.Find("Main Camera").GetComponent<CameraControl>().target[0] = position;
        GameObject.Find("Main Camera").GetComponent<CameraControl>().target[1] = pivot;



        GameObject.Find("GameController").GetComponent<CameraSwitch>().cams[1] = myCar.transform.Find("Camera/front").gameObject;
        GameObject.Find("GameController").GetComponent<CameraSwitch>().cams[2] = myCar.transform.Find("Camera/inside").gameObject;


        for (int i = 0; i < 3; i++)
        {
            int randValue = Random.Range(0, AI_cars.Length - 1);

            GameObject createdCar = Instantiate(AI_cars[randValue], AI_spawn[i].transform.position, AI_spawn[i].transform.rotation);
            createdCar.GetComponent<AI_CarController>().spawnPointId = i;

        }

    }


    public void Send(GameObject sentObj)
    {
        createdCars.Add(sentObj);


    }

    public void GameOver(int pos)
    {

        finishPanel.SetActive(true);
        finishPanel.transform.Find("Panel/rank").GetComponent<TextMeshProUGUI>().text = pos.ToString() + ". finished";
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        audios[0].Stop();
        audios[1].Stop();
        GetComponent<CarAudio>().enabled = false;
        


    }

    IEnumerator TimeControl()
    {
        while (time)
        {

            yield return new WaitForSeconds(1f);
            second--;
            countDown.text = Mathf.Round(second).ToString();
            audios[1].Play();
            if (second < 0)
            {
                foreach (var car in createdCars)
                {
                    if (car.gameObject.name == "Me")
                    {

                        car.GetComponentInParent<CarUserControl>().enabled = true;

                    }
                    else
                    {
                        car.GetComponentInParent<CarAIControl>().enabled = true;


                    }

                }
                countDown.enabled = false;
                time = false;
                StopCoroutine(myTiming);
            }




        }



    }
    









}
