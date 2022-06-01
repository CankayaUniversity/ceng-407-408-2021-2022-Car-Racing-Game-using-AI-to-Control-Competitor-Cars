using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class car
{

    public GameObject sentObj;
    public int pos;
    public car(GameObject d_sentObj, int d_pos)
    {
        sentObj = d_sentObj;
        pos = d_pos;


    }


}

public class ManagingRanking : MonoBehaviour
{
    public List<car> cars = new List<car>();
    public TextMeshProUGUI rank;

    public void Send(GameObject sentObj, int actRot)
    {
        cars.Add(new car(sentObj, actRot));


        if (cars.Count == 4)
        {

            CheckRanking();

        }


    }

    public void UpdateRank(GameObject sentCar, int pos)
    {
        for (int i = 0; i < cars.Count; i++)
        {

            if (cars[i].sentObj == sentCar)
            {
                cars[i].pos = pos;


            }


        }
        CheckRanking();


    }


    public void CheckRanking()
    {
        cars = cars.OrderBy(w => w.pos).ToList();


        rank.text = "";

        for (int i = 0; i < cars.Count; i++)
        {

            switch (i)
            {

                case 0:
                    if (cars[i].sentObj.name == "Me")
                    {
                        rank.text = "4/4";
                        cars[i].sentObj.GetComponent<RankingSc>().pos = 4;


                    }

                    break;

                case 1:

                    if (cars[i].sentObj.name == "Me")
                    {
                        rank.text = "3/4";
                        cars[i].sentObj.GetComponent<RankingSc>().pos = 3;


                    }


                    break;

                case 2:

                    if (cars[i].sentObj.name == "Me")
                    {
                        rank.text = "2/4";
                        cars[i].sentObj.GetComponent<RankingSc>().pos = 2;


                    }


                    break;

                case 3:

                    if (cars[i].sentObj.name == "Me")
                    {
                        rank.text = "1/4";
                        cars[i].sentObj.GetComponent<RankingSc>().pos = 1;


                    }


                    break;

            }


        }

        /*foreach (var car in cars)
        {
            rank.text += car.sentObj.name + "-" + car.pos + "<br>";


        }*/

    }


    

}
