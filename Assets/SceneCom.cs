using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SceneCom : MonoBehaviour
{
    public Text firstscenename;
    public Text secondscenename;
    string names;

    public void callname()
    {
        names = firstscenename.text;
        GetName(names);

    }
    public void ExitGame()
    {
        Application.Quit();
    }


    public static void GetName(string bilgiler)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/name.txt";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, bilgiler);
        stream.Close();
    }
}
