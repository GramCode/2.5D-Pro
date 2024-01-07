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
    private TMP_Text _coinsText;

    private void Awake()
    {
        _instance = this;
    }

    public void UpdateCoinsText(int count)
    {
        _coinsText.text = "x " + count;
    }
}
