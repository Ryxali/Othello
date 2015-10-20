using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GameMenu : MonoBehaviour {
    public PlayerTypeDropdown player0Dropdown;
    public PlayerTypeDropdown player1Dropdown;
    public Button StartButton;
    public GameObject mainmenuPanel;
    public GameObject endgamePanel;
    public Text scorePlayer0;
    public Text scorePlayer1;
    void Start()
    {
        
        //object [] scripts = Resources.LoadAll("MoonSharp/Scripts/");
        /**/
        
#if UNITY_WEBPLAYER || UNITY_WEBGL
        //StartCoroutine(Init());
#else
        string[] scripts = System.IO.Directory.GetFiles(Application.streamingAssetsPath + "/AIScripts/", "*.lua");
        for (int i = 0; i < scripts.Length; i++)
        {

            string s = scripts[i];
            player0Dropdown.scriptselect.options.Add(new UnityEngine.UI.Dropdown.OptionData(s.Substring(s.LastIndexOf("/") + 1).Replace(".lua", "")));
            player1Dropdown.scriptselect.options.Add(new UnityEngine.UI.Dropdown.OptionData(s.Substring(s.LastIndexOf("/") + 1).Replace(".lua", "")));
        }
        player0Dropdown.scriptselect.captionText.text = player0Dropdown.scriptselect.options[0].text;
        player1Dropdown.scriptselect.captionText.text = player1Dropdown.scriptselect.options[0].text;
#endif
    }

    private IEnumerator Init()
    {
        WWW scripts = new WWW("http://www.ryxali.com/Othello/StreamingAssets/AIScripts/fetch.php");
        yield return scripts;
        Debug.Log(scripts.text);
        JSONObject t = new JSONObject(scripts.text);
        Debug.Log(t);
        Debug.Log(t[0]);
        for (int i = 0; i < t.Count; i++)
        {
            string s = t[i][0].str;
            string code = "";
            for (int j = 0; j < t[i][1].Count; j++)
            {
                Debug.Log(t[i][1][j].str);
                code += t[i][1][j].str.Replace("\\", "") + "\n";
            }
            Debug.Log(code);
            BoardProspector.scriptsAvailable.Add(s, code);
            player0Dropdown.scriptselect.options.Add(new UnityEngine.UI.Dropdown.OptionData(s));
            player1Dropdown.scriptselect.options.Add(new UnityEngine.UI.Dropdown.OptionData(s));
        }
        player0Dropdown.scriptselect.captionText.text = player0Dropdown.scriptselect.options[0].text;
        player1Dropdown.scriptselect.captionText.text = player1Dropdown.scriptselect.options[0].text;
    }
}
