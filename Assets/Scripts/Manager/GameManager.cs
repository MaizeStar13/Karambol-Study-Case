using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] public GameObject _pausepanel;
    [SerializeField] public GameObject _winCond;
    [SerializeField] public GameObject _loseCond;

    [SerializeField] private GameObject[] _uidisable;

    private void Awake()
    {
        _pausepanel.SetActive(false);
    }


    public void pause()
    {
        _pausepanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Continue()
    {
        _pausepanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Sample");
    }

    public void Losing()
    {
        Time.timeScale = 0f;
        _loseCond.SetActive(true);

        foreach (GameObject uiElemet in _uidisable)
        {
            if(uiElemet != null)
            {
                uiElemet.SetActive(false);
            }
        }
    }

    public void Winning()
    {
        Time.timeScale = 0f;
        _winCond.SetActive(true);
    }
}
