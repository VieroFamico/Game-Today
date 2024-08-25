using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player_Menus_Manager : MonoBehaviour
{
    public static Player_Menus_Manager instance;

    [Header("Main Menu References")]
    public Animator mainMenu;
    public Button startButton;
    public Button exitGameButton;
    [Header("Death Menu References")]
    public Animator deathMenu;
    public Button retryButton;
    public Button exitToMenuButton;
    [Header("Pause Menu References")]
    public Animator pauseMenu;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        retryButton.onClick.AddListener(Restart);
    }

    void Update()
    {

    }

    public void ShowDeathMenu()
    {
        deathMenu.SetTrigger("Show");
    }

    public void Restart()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {

    }

    public void InitializePlayer()
    {
        PlayerState_Manager.instance.InitializePlayer();
    }
}
