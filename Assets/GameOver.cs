using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameOver : MonoBehaviour
{
    public Text GetResult;
    public Text GetPegs;
    string[] ResultPoint = new string[10];
    int point = 200;
    void Start()
    {
        //Debug.Log(LoadResult());
        for (int i = 1; i < ResultPoint.Length; i++)
        {
            ResultPoint[i] = point.ToString();
            point -= 25;
        }

        for (int j = 0; j < ResultPoint.Length; j++)
        {
            if (j.ToString() == LoadResult())
            {
                GetResult.text = "Score : " + ResultPoint[j];
                GetPegs.text = "Pegs  : " + j;
            }
        }


    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            Application.Quit();
        }
       
        
    }
    public static string LoadResult()
    {
        string data = "";
        string path = Application.persistentDataPath + "/sonuc.txt";
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
