using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewRank : MonoBehaviour
{
    [SerializeField] private Text newRankText;
    [SerializeField] private InputField inputField;
    private GameManager code;

    // Use this for initialization
    void Start()
    {
        code = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        newRankText.text = "You ranked #" + (code.PlayerRank + 1).ToString() + ".\nEnter your initial";
    }

    public void SetNewRank()
    {
        if (inputField.text == string.Empty)
        {
            code.SetLoaderboard(code.PlayerRank);
        }
        else
        {
            code.SetLoaderboard(code.PlayerRank, inputField.text.ToUpper());
        }
        inputField.text = string.Empty;
    }
}
