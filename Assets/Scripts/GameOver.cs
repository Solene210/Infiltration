using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameOver : MonoBehaviour
{
    #region Expose
    [SerializeField] private GameObject _gameOverScreenUI;
    [SerializeField] private GameObject _player;
    #endregion

    #region Unity Life Cycle
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _player = collision.gameObject;
            _gameOverScreenUI.SetActive(true);
            Time.timeScale = 0;
        }
    }
    #endregion

    #region methods

    #endregion

    #region Private & Protected

    #endregion
}
