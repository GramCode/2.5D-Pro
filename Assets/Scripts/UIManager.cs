using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
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
    private TMP_Text _coinsText, _gameCompleteText, _coinsCollected, _inputText;

    private void Awake()
    {
        _instance = this;
    }

    public void UpdateCoinsText(int count)
    {
        _coinsText.text = "x " + count;
    }

    public void DisplayGameCompleteText()
    {
        _gameCompleteText.gameObject.SetActive(true);
        StartCoroutine(FlickerRoutine(_gameCompleteText.gameObject));
    }

    public void DisplayCoinsCollectedText()
    {
        _coinsCollected.gameObject.SetActive(true);
        StartCoroutine(FlickerRoutine(_coinsCollected.gameObject));
    }

    public void DisplayInputText()
    {
        _inputText.gameObject.SetActive(true);
        StartCoroutine(FlickerRoutine(_inputText.gameObject));
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
}
