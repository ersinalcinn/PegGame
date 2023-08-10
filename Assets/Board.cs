using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement; 

public class Board : MonoBehaviour
{
    [SerializeField] GameObject piyonPrefab;
    [SerializeField] GameObject HolePrefab;

    Piyon[,] piyons = new Piyon[7, 7];
    Hole[,] holes = new Hole[7, 7];

    string[,] temp2dBall = new string[7,7];
    string[] tempBall = new string[50];

    Piyon chosenPiyon;

    Vector3 piyonOffset = new Vector3(-0.5f, 0f, -0.5f);

    Vector2 mouseOver;
    Vector2 startDrag;
    Vector2 endDrag;

    private AudioSource pop;

    public Rigidbody piyon;
    public Transform target;
    public Text pegs;
    public Text name2;

    string temp;
    bool checkSave;
    void Start()
    {

        pop = GetComponent<AudioSource>();
        createTabla();
    }

    //I created 7*7 piyon and hole for createPiyon and createHole function
    //I wrote the necessary if condition so that the corners and middle ones are not removed.
    private void createTabla()
    {
        for (int x = 0; x < 7; x++)
        {
            for (int y = 0; y < 7; y++)
            {
                if (x != 3 || y != 3)
                    if (x > 1 && x < 5 || y > 1 && y < 5)
                    {
                        CreatePiyon(x, y);
                        CreateHole(x, y);

                    }

            }
        }
    }

    //in this function i create and save piyons as objects
    private void CreatePiyon(int x, int y)
    {
        GameObject go = Instantiate(piyonPrefab) as GameObject;
        Piyon p = go.GetComponent<Piyon>();
        piyons[x, y] = p;
        MovePiyon(p, x, y);

    }

    //in this function i create and save holes as objects
    private void CreateHole(int x, int y)
    {
        GameObject go1 = Instantiate(HolePrefab) as GameObject;
        Hole p = go1.GetComponent<Hole>();
        holes[x, y] = p;
        MoveHole(p, x, y);
    }

    //I set how far the piyons will go
    private void MovePiyon(Piyon p, int x, int y)
    {

        p.transform.position = (Vector3.forward * x) + (Vector3.right * y);
    }
    //I set how far the holes will go
    private void MoveHole(Hole p, int x, int y)
    {
        p.transform.position = (Vector3.forward * x) + (Vector3.right * y);
    }
    //I've finished creating piyons and holes while moving piyons




    private void Update()
    {
        UpdateMouseOver();


        int x = (int)mouseOver.x;
        int y = (int)mouseOver.y;

        if (chosenPiyon != null)
        {
            if (Input.GetMouseButtonDown(0))
                move((int)startDrag.x, (int)startDrag.y, x, y);
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                PiyonChosen(x, y);

                if (chosenPiyon != null)
                {
                    UpdatePiyonDrag(chosenPiyon);
                }
            }
        }

        if (gameOver())
        {
            Debug.Log("Pegs:" + PiyonCounter());
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
    void UpdateMouseOver()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("Tabla")))
        {
            mouseOver.y = (int)(hit.point.x - piyonOffset.x);
            mouseOver.x = (int)(hit.point.z - piyonOffset.z);
        }
        else
        {
            mouseOver.x = -100;
            mouseOver.y = -100;
        }
    }


    private void PiyonChosen(int x, int y)
    {
        //Out of bounds
        if (x < 0 || x >= 7 || y < 0 || y >= 7)
            return;

        Piyon p = piyons[x, y];

        if (p != null)
        {
            chosenPiyon = p;
            startDrag = mouseOver;
        }

    }

    private void UpdatePiyonDrag(Piyon p)
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("Tabla")))
            p.transform.position = hit.point + (Vector3.up * 2);

    }
    //piyon counter
    int PiyonCounter()
    {
        int sayac = 0;

        for (int i = 0; i < 7; i++)
            for (int j = 0; j < 7; j++)
                if (piyons[i, j] != null)
                    sayac++;

        return sayac;
    }

    private void move(int x1, int y1, int x2, int y2)
    {

        startDrag = new Vector2(x1, y1);
        endDrag = new Vector2(x2, y2);
        chosenPiyon = piyons[x1, y1];

        //Out of bounds
        if (x2 < 0 || x2 >= 7 || y2 < 0 || y2 >= 7)
        {
            if (chosenPiyon != null)
                MovePiyon(chosenPiyon, x1, y1);

            startDrag = Vector2.zero;
            chosenPiyon = null;
            return;
        }

        if (chosenPiyon != null)
        {
            // If he wants to move to the same place, the move is canceled
            if (endDrag == startDrag)
            {
                MovePiyon(chosenPiyon, x1, y1);

                startDrag = Vector2.zero;
                chosenPiyon = null;
                return;

            }

            // Will it be portable
            if (MoveControl(x1, y1, x2, y2))
            {
                pop.Play();
                if (x1 == x2)
                {
                    Destroy(piyons[x1, (y1 + y2) / 2].gameObject);
                    piyons[x1, (y1 + y2) / 2] = null;
                }
                else
                {
                    Destroy(piyons[(x1 + x2) / 2, y1].gameObject);
                    piyons[(x1 + x2) / 2, y1] = null;
                }


                pegs.text = System.Convert.ToString("Pegs remaining : " + PiyonCounter());
                piyons[x2, y2] = chosenPiyon;
                piyons[x1, y1] = null;
                MovePiyon(chosenPiyon, x2, y2);
                chosenPiyon = null;
                startDrag = Vector2.zero;
            }
        }

    }
    //function to check whether the game is over
    bool gameOver()
    {
        for (int x = 0; x < 7; x++)
        {
            for (int y = 0; y < 7; y++)
            {
                if (x < 5)
                {
                    if (MoveControl(x, y, x + 2, y))
                    {
                        return false;
                    }
                }

                if (y < 5)
                {
                    if (MoveControl(x, y, x, y + 2))
                    {
                        return false;
                    }
                }

                if (x > 1)
                {
                    if (MoveControl(x, y, x - 2, y))
                    {
                        return false;
                    }
                }

                if (y > 1)
                {
                    if (MoveControl(x, y, x, y - 2))
                    {
                        return false;
                    }
                }
            }
        }

        int sayac = PiyonCounter();
        if (PiyonCounter() >= 9)
            sayac = 0;
        SendResult();
        SceneManager.LoadScene("gameover");

        //////////////

        return true;
    }

    //checks if the piyon is allowed to go locations
    private bool MoveControl(int x1, int y1, int x2, int y2)
    {
        //yasaklý konumlar
        if (x2 == 0 && y2 == 0 || x2 == 0 && y2 == 1 || x2 == 0 && y2 == 5 || x2 == 0 && y2 == 6
         || x2 == 1 && y2 == 0 || x2 == 1 && y2 == 1 || x2 == 1 && y2 == 5 || x2 == 1 && y2 == 6
         || x2 == 5 && y2 == 0 || x2 == 5 && y2 == 1 || x2 == 5 && y2 == 5 || x2 == 5 && y2 == 6
         || x2 == 6 && y2 == 0 || x2 == 6 && y2 == 1 || x2 == 6 && y2 == 5 || x2 == 6 && y2 == 6)
            return false;

        if (x1 == x2)
        {
            int y3;

            if (y1 > y2)
            {
                if (y1 - y2 > 2 || y1 - y2 < 2)
                    return false;

                y3 = y1 - 1;
            }
            else
            {
                if (y2 - y1 > 2 || y2 - y1 < 2)
                    return false;

                y3 = y2 - 1;
            }

            if (piyons[x1, y1] != null && piyons[x2, y2] == null && piyons[x1, y3] != null)
                return true;
        }
        else if (y1 == y2)
        {
            int x3;

            if (x1 > x2)
            {
                if (x1 - x2 > 2 || x1 - x2 < 2)
                    return false;

                x3 = x1 - 1;
            }
            else
            {
                if (x2 - x1 > 2 || x2 - x1 < 2)
                    return false;

                x3 = x2 - 1;
            }

            if (piyons[x1, y1] != null && piyons[x2, y2] == null && piyons[x3, y1] != null)
                return true;
        }

        return false;
    }


    //Save game 
    public static void RecordPiyon(string bilgiler)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.bin";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, bilgiler);
        stream.Close();
    }
    //Load game
    public static string LoadPiyon()
    {
        string data = "";
        string path = Application.persistentDataPath + "/player.bin";
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
    //Exit button
    public void exit()
    {
        Application.Quit();
    }
    //function to send the remaining piyon number to the last scene
    public void SendResult()
    {
        GetResult(PiyonCounter().ToString());

    }
    public static void GetResult(string sonuc)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/sonuc.txt";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, sonuc);
        stream.Close();
    }
    //if you want to start a new game(new game button)
    public void NewGame()
    {
        checkSave = false;
        for (int i = 0; i < 7; i++)
        {
            for (int g = 0; g < 7; g++)
            {
                if (piyons[i, g] != null)
                {
                    Destroy(piyons[i, g].gameObject);
                    piyons[i, g] = null;
                }

            }
        }
        pegs.text = System.Convert.ToString("Pegs remaining : 32");
        Start();

    }
    /*  The functions where it performs operations between temporary arrays when saving or loading the game */
    public void CallArraySave()
    {
        TakeArray();
        checkSave = true;
    }
    public void LoadArray()
    {
        temp = LoadPiyon();
        if (checkSave == true)
        {
            CreateBoard();
        }


    }
    public void CreateBoard()
    {
        int y = 0;
        tempBall = temp.Split('|');
        for (int x = 0; x < 7; x++)
        {
            for (int j = 0; j < 7; j++)
            {
                temp2dBall[x, j] = tempBall[y];
                y++;
            }
        }
        for (int i = 0; i < 7; i++)
        {
            for (int g = 0; g < 7; g++)
            {
                if (piyons[i, g] != null)
                {
                    Destroy(piyons[i, g].gameObject);
                    piyons[i, g] = null;
                }

            }
        }

        for (int t = 0; t < 7; t++)
        {
            for (int f = 0; f < 7; f++)
            {
                if (temp2dBall[t, f] == "1")
                {
                    CreatePiyon(t, f);
                }
            }
        }

        pegs.text = tempBall[49];
    }
    void TakeArray()
    {
        string a = "";
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                if (piyons[i, j] != null)
                {
                    temp2dBall[i, j] = "1";
                }
                else
                {
                    temp2dBall[i, j] = "0";
                }

            }
        }


        for (int o = 0; o < 7; o++)
        {
            for (int d = 0; d < 7; d++)
            {
                a = a + temp2dBall[o, d] + "|";
            }
        }
        a = a + pegs.text;
        RecordPiyon(a);
    }  
}
