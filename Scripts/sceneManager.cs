using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{
   

    public Animator anim;
    public AudioSource audios;


    private void Start()
    {
        audios.Play();


    }

    public void changeTheScene(int value)
    {
        StartCoroutine(passScene(value));
       


    }
    public void quitTheGame()
    {

        Application.Quit();


    }
    IEnumerator passScene(int value)
    {
        anim.SetTrigger("pass");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(value);


    }

    



}
