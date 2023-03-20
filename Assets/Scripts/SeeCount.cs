using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SeeCount : MonoBehaviour
{
    #region Expose
    [SerializeField]
    private IntVariable _seeObject;
    #endregion

    #region Unity Life Cycle
    private void Awake()
    {
        _enemySeeText = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        _enemySeeText.text = _seeObject.m_value.ToString();
    }
    #endregion

    #region methode

    #endregion

    #region Private & Protected
    private TextMeshProUGUI _enemySeeText;
    #endregion
}
