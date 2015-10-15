using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
    public Board board;
    public Controller player0;
    public Controller player1;
    public GameMenu gameMenu;
    void Start()
    {
        MoonSharp.Interpreter.UserData.RegisterAssembly();
        player0 = new PlayerController(Tile.State.PLAYER_0, board);
        //new PlayerController(Tile.State.PLAYER_1, board);
        //System.IO.Directory.GetFiles(Application.dataPath + "/Resources")
        string[] scripts = System.IO.Directory.GetFiles(Application.streamingAssetsPath + "/AIScripts/", "*.txt");
        //object [] scripts = Resources.LoadAll("MoonSharp/Scripts/");
        for (int i = 0; i < scripts.Length; i++)
        {

            string s = scripts[i];
            gameMenu.scriptSelection.options.Add(new UnityEngine.UI.Dropdown.OptionData(s.Substring(s.LastIndexOf("/")+1).Replace(".txt", "")));
        }
        gameMenu.scriptSelection.captionText.text = gameMenu.scriptSelection.options[0].text;
        
    }

    public void StartGame()
    {
        gameMenu.gameObject.SetActive(false);
        string target = gameMenu.scriptSelection.captionText.text;//gameMenu.scriptSelection.options[gameMenu.scriptSelection.value].text;
        AIController ai = new AIController(Tile.State.PLAYER_1, board, this, target);
        player1 = ai;
        StartCoroutine(StartSession());
    }

    private IEnumerator StartSession()
    {
        yield return null;
        yield return StartCoroutine(player0.OnGameStart());
        yield return StartCoroutine(player1.OnGameStart());
        bool running = true;
        while (running)
        {
            if (board.CanPlaceAnyMore(player0.owner))
            {
                yield return StartCoroutine(player0.TakeTurn());
                if (board.CanPlaceAnyMore(player1.owner))
                {
                    yield return StartCoroutine(player1.TakeTurn());
                }
                else
                {
                    running = false;
                }
            }
            else
            {
                running = false;
            }
        }
        Debug.Log("End of match");
        Debug.Log("Score:");
        int p0 = 0;
        int p1 = 0;

        //Debug.Log("Player 1: " + player0.board.)
    }
}
