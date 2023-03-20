using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Expose
    [SerializeField]
    private IntVariable _enemySee; 
    [SerializeField]
    private IntVariable _cameraSee;
    #endregion

    #region Unity Life Cycle
    void Start()
    {
        _cameraSee.m_value = 0;
        _enemySee.m_value = 0;
    }

    #endregion

    #region methods
    public void Quit()
    {
        Application.Quit();
    }

    public void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
    #endregion

    #region Private & Protected

    #endregion
}
