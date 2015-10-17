using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour {
    public PlayerTypeDropdown player0Dropdown;
    public PlayerTypeDropdown player1Dropdown;
    public Button StartButton;
    public GameObject mainmenuPanel;
    public GameObject endgamePanel;

    void Start()
    {
        string[] scripts = System.IO.Directory.GetFiles(Application.streamingAssetsPath + "/AIScripts/", "*.lua");
        //object [] scripts = Resources.LoadAll("MoonSharp/Scripts/");
        for (int i = 0; i < scripts.Length; i++)
        {

            string s = scripts[i];
            player0Dropdown.scriptselect.options.Add(new UnityEngine.UI.Dropdown.OptionData(s.Substring(s.LastIndexOf("/") + 1).Replace(".lua", "")));
            player1Dropdown.scriptselect.options.Add(new UnityEngine.UI.Dropdown.OptionData(s.Substring(s.LastIndexOf("/") + 1).Replace(".lua", "")));
        }
        player0Dropdown.scriptselect.captionText.text = player0Dropdown.scriptselect.options[0].text;
        player1Dropdown.scriptselect.captionText.text = player1Dropdown.scriptselect.options[0].text;
    }
}
