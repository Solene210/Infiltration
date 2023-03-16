using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : MonoBehaviour
{
    #region Expose
    [SerializeField] private GameObject _victoryScreenUI;
    [SerializeField] private GameObject _player;
    #endregion

    #region Unity Life Cycle
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _player = other.gameObject;
            _victoryScreenUI.SetActive(true);
            Time.timeScale = 0;
        }
    }
    #endregion

    #region methods

    #endregion

    #region Private & Protected

    #endregion
}
