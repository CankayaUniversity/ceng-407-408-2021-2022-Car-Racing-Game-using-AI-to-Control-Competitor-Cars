using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingSc : MonoBehaviour
{
    public int actRank = 1;
    ManagingRanking ranking;
    public int pos;
    GeneralSettings settings;


    void Start()
    {
        ranking = GameObject.Find("GameController").GetComponent<ManagingRanking>();
        ranking.Send(gameObject, actRank);

        settings = GameObject.Find("GameController").GetComponent<GeneralSettings>();
        settings.Send(gameObject);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("forRotation"))
        {

            actRank = int.Parse(other.transform.gameObject.name);
            ranking.UpdateRank(gameObject, actRank);
        }

        if(gameObject.name == "Me")
        {

            if (other.CompareTag("Finish"))
            {

                settings.GameOver(pos);
            }


        }





    }


}
