﻿using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
    public Board board;
    public Controller player0;
    public Controller player1;
    public GameMenu gameMenu;
    void Start()
    {
        Debug.Log("---- GAME_CONTROLLER.Start()");
        //MoonSharp.Interpreter.UserData.RegisterAssembly();
        Debug.Log("---- END GAME_CONTROLLER.Start()");
        //player0 = new PlayerController(Tile.State.PLAYER_0, board);
        //new PlayerController(Tile.State.PLAYER_1, board);
        //System.IO.Directory.GetFiles(Application.dataPath + "/Resources")
        
        
    }

    public void StartMainMenu()
    {
        gameMenu.mainmenuPanel.SetActive(true);
        gameMenu.endgamePanel.SetActive(false);
    }

    public void StartGame()
    {
        gameMenu.mainmenuPanel.SetActive(false);
        board.Reset();
        if (gameMenu.player0Dropdown.AISelected())
        {
            player0 = new AIController(Tile.State.PLAYER_0, board, this, gameMenu.player0Dropdown.GetSelectedScriptName());
        }
        else
        {
            player0 = new PlayerController(Tile.State.PLAYER_0, board);
        }
        if (gameMenu.player1Dropdown.AISelected())
        {
            player1 = new AIController(Tile.State.PLAYER_1, board, this, gameMenu.player1Dropdown.GetSelectedScriptName());
        }
        else
        {
            player1 = new PlayerController(Tile.State.PLAYER_1, board);
        }
        StartCoroutine(StartSession());
    }

    private void UpdateScores()
    {
        BasicBoard.Score score = board.getScore();
        gameMenu.scorePlayer0.text = score[BasicTile.State.PLAYER_0].ToString();
        gameMenu.scorePlayer1.text = score[BasicTile.State.PLAYER_1].ToString();
    }

    private IEnumerator StartSession()
    {
        yield return null;
        UpdateScores();
        yield return StartCoroutine(player0.OnGameStart());
        yield return StartCoroutine(player1.OnGameStart());
        bool running = true;
        while (running)
        {
            if (board.CanPlaceAnyMore(player0.owner))
            {
                yield return StartCoroutine(player0.TakeTurn());
                UpdateScores();
                if (board.CanPlaceAnyMore(player1.owner))
                {
                    yield return StartCoroutine(player1.TakeTurn());
                    UpdateScores();
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
        BasicBoard.Score s = board.getScore();
        int p0 = s[BasicTile.State.PLAYER_0];
        int p1 = s[BasicTile.State.PLAYER_1];
        Debug.Log("Player 1: " + p0);
        Debug.Log("player 2: " + p1);
        gameMenu.endgamePanel.SetActive(true);
        //Debug.Log("Player 1: " + player0.board.)
    }
}
