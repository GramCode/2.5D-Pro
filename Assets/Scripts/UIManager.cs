using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public enum CurrentText
    {
        GameCompleteText,
        AllCoinsCollectedText,
        RestartText,
    }

    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("The UI Manager is NULL");
            return _instance;
        }
    }

    [SerializeField]
    private TMP_Text _coinsText;
    [SerializeField]
    private TMP_Text _interactionText;
    [SerializeField]
    private TMP_Text[] _textArray; // 0 = Game Complete Text, 1 = All Coins Collected Text, 2 = Restart Text
    [SerializeField]
    private GameObject _menu;

    private void Awake()
    {
        _instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            DisplayMenu(true);
        }

        if (Input.GetKeyUp(KeyCode.M))
        {
            DisplayMenu(false);
        }
    }

    private void DisplayMenu(bool display)
    {
        _menu.SetActive(display);
    }

    public void UpdateCoinsText(int count)
    {
        _coinsText.text = "x " + count;
    }

    IEnumerator FlickerRoutine(GameObject obj)
    {
        while (true)
        {
            obj.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            obj.SetActive(true);
            yield return new WaitForSeconds(0.6f);
        }
        
    }

    public void DisplayText(CurrentText currentText)
    {
        GameObject text = null;

        switch (currentText)
        {
            case CurrentText.GameCompleteText:
                _textArray[0].gameObject.SetActive(true);
                text = _textArray[0].gameObject;
                break;
            case CurrentText.AllCoinsCollectedText:
                _textArray[1].gameObject.SetActive(true);
                text = _textArray[1].gameObject;
                break;
            case CurrentText.RestartText:
                _textArray[2].gameObject.SetActive(true);
                text = _textArray[2].gameObject;
                break;
        }
        
        StartCoroutine(FlickerRoutine(text));
    }

    public void DisplayInteractionText(bool display)
    {
        _interactionText.gameObject.SetActive(display);
    }
}
