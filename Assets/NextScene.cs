using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextScene : MonoBehaviour
{
    public InputField name1;
    public void SecondScene()
    {

        if (string.IsNullOrEmpty(name1.text) || name1.text == "Please enter your name !!")
        {
            name1.text = "Please enter your name !!";
        }
        else
        {
            SceneManager.LoadScene("Peq");

        }
    }


}
