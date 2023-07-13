using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public InputField inputField;
    public Button button;
    
    string searchQuery;
    public AmiiboManager amiiboManager;

    private void Start()
    {
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        string inputText = inputField.text;
        searchQuery = inputText;
        amiiboManager.SearchAmiibo(searchQuery);
    }
}
