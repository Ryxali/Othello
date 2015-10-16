using UnityEngine;
using System.Collections;
using UnityEngine.UI;
[RequireComponent(typeof(Dropdown))]
public class PlayerTypeDropdown : MonoBehaviour {
    
    [SerializeField]
    public Dropdown scriptselect;
    private Dropdown self;
    void Start() {
        self = GetComponent<Dropdown>();
        scriptselect.gameObject.SetActive(false);
    }
    public void OnValueChange(int newVal) {
        scriptselect.gameObject.SetActive(newVal == 1);
    }

    public string GetValueText()
    {
        return self.captionText.text;
    }

    public int GetValue()
    {
        return self.value;
    }

    public bool AISelected()
    {
        return self.value == 1;
    }

    public string GetSelectedScriptName()
    {
        return scriptselect.captionText.text;
    }
}
