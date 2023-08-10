using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class SceneCom2 : MonoBehaviour
{
    public Text getname;

    private void Start()
    {
        getname.text = LoadName();

    }
    public static string LoadName()
    {
        string data = "";
        string path = Application.persistentDataPath + "/name.txt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            data = System.Convert.ToString(formatter.Deserialize(stream));
            stream.Close();
            return data;
        }
        return data;
    }
    
}
