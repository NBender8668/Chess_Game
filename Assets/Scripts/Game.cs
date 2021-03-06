using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    //Reference from Unity IDE
    public GameObject chesspiece;

    //Matrices needed, positions of each of the GameObjects
    //Also separate arrays for the players in order to easily keep track of them all
    //Keep in mind that the same objects are going to be in "positions" and "playerBlack"/"playerWhite"
    private GameObject[,] positions = new GameObject[8, 8];
    private GameObject[] playerBlack = new GameObject[16];
    private GameObject[] playerWhite = new GameObject[16];

    //current turn
    private string currentPlayer = "white";

    //Game Ending
    private bool gameOver = false;

    //Chess960 gamemode
    public bool isChess960;

    List<int> positionlist = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 };
    List<int> blackpositons = new List<int>();


    //Unity calls this right when the game starts, there are a few built in functions
    //that Unity can call for you
    public void Start()
    {  
        if(isChess960)
        {
            playerWhite = new GameObject[] { Create("white_bishop", PickPosition(positionlist), 0), Create("white_bishop", PickPosition(positionlist), 0),
            Create("white_rook",PickPosition(positionlist), 0), Create("white_rook", PickPosition(positionlist), 0), Create("white_king", PickPosition(positionlist), 0),
            Create("white_knight", PickPosition(positionlist), 0), Create("white_knight", PickPosition(positionlist), 0), Create("white_queen", PickPosition(positionlist), 0),
            Create("white_pawn", 0, 1), Create("white_pawn", 1, 1), Create("white_pawn", 2, 1),
            Create("white_pawn", 3, 1), Create("white_pawn", 4, 1), Create("white_pawn", 5, 1),
            Create("white_pawn", 6, 1), Create("white_pawn", 7, 1) };
            playerBlack = new GameObject[] { Create("black_bishop", blackpositons[0], 7), Create("black_bishop",blackpositons[1],7),
            Create("black_rook",blackpositons[2],7), Create("black_rook",blackpositons[3],7), Create("black_king",blackpositons[4],7),
            Create("black_knight",blackpositons[5],7), Create("black_knight",blackpositons[6],7), Create("black_queen",blackpositons[7],7),
            Create("black_pawn", 0, 6), Create("black_pawn", 1, 6), Create("black_pawn", 2, 6),
            Create("black_pawn", 3, 6), Create("black_pawn", 4, 6), Create("black_pawn", 5, 6),
            Create("black_pawn", 6, 6), Create("black_pawn", 7, 6) };
        }
        else
        {
            playerWhite = new GameObject[] { Create("white_rook", 0, 0), Create("white_knight", 1, 0),
            Create("white_bishop", 2, 0), Create("white_queen", 3, 0), Create("white_king", 4, 0),
            Create("white_bishop", 5, 0), Create("white_knight", 6, 0), Create("white_rook", 7, 0),
            Create("white_pawn", 0, 1), Create("white_pawn", 1, 1), Create("white_pawn", 2, 1),
            Create("white_pawn", 3, 1), Create("white_pawn", 4, 1), Create("white_pawn", 5, 1),
            Create("white_pawn", 6, 1), Create("white_pawn", 7, 1) };
            playerBlack = new GameObject[] { Create("black_rook", 0, 7), Create("black_knight",1,7),
            Create("black_bishop",2,7), Create("black_queen",3,7), Create("black_king",4,7),
            Create("black_bishop",5,7), Create("black_knight",6,7), Create("black_rook",7,7),
            Create("black_pawn", 0, 6), Create("black_pawn", 1, 6), Create("black_pawn", 2, 6),
            Create("black_pawn", 3, 6), Create("black_pawn", 4, 6), Create("black_pawn", 5, 6),
            Create("black_pawn", 6, 6), Create("black_pawn", 7, 6) };
        }

        //Set all piece positions on the positions board
        for (int i = 0; i < playerBlack.Length; i++)
        {
            SetPosition(playerBlack[i]);
            Debug.Log(playerBlack[i].transform.position);
            SetPosition(playerWhite[i]);
            Debug.Log(playerWhite[i].transform.position);
        }
    }

    public GameObject Create(string name, int x, int y)
    {
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity);
        Chessman cm = obj.GetComponent<Chessman>(); //We have access to the GameObject, we need the script
        cm.name = name; //This is a built in variable that Unity has, so we did not have to declare it before
        cm.SetXBoard(x);
        cm.SetYBoard(y);
        cm.Activate(); //It has everything set up so it can now Activate()

        Debug.Log(obj);
        return obj;
    }
    
    public void SetPosition(GameObject obj)
    {
        Chessman cm = obj.GetComponent<Chessman>();

        //Overwrites either empty space or whatever was there
        positions[cm.GetXBoard(), cm.GetYBoard()] = obj;
    }

    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null;
    }

    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
        
    }

    
    public int PickPosition(List<int> list)
    {
        var rand = new System.Random();
        int index = rand.Next(list.Count);
        var value = list[index];

        if (list[index] % 2 == 0)
        {
            rand.Next(list.Count);
        }
        else
        {
            rand.Next(list.Count);
        }

        blackpositons.Add(value);
       
        list.RemoveAt(index);

        Debug.Log(value);

        return value;
    }

    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1)) return false;
        return true;
    }

    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void NextTurn()
    {
        if (currentPlayer == "white")
        {
            currentPlayer = "black";
        }
        else
        {
            currentPlayer = "white";
        }
    }

    public void Update()
    {
        if (gameOver == true && Input.GetMouseButtonDown(0))
        {
            gameOver = false;

            //Using UnityEngine.SceneManagement is needed here
            SceneManager.LoadScene("MainMenu"); //Restarts the game by loading the scene over again
        }
    }
    
    public void Winner(string playerWinner)
    {
        gameOver = true;

        //Using UnityEngine.UI is needed here
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = true;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = playerWinner + " is the winner";

        GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = true;
    }
}
