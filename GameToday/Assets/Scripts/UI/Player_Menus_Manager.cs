using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player_Menus_Manager : MonoBehaviour
{
    public static Player_Menus_Manager instance;

    [Header("Main Menu References")]
    public Animator mainMenu;
    public Image mainMenuImage;
    public Button startButton;
    public Button exitGameButton;

    [Header("Intro Text")]
    public Image dialogBackground; 
    public Dialog introDialog;
    public Button skipIntroDialog;

    [Header("Death Menu References")]
    public Animator deathMenu;
    public Button retryButton;
    public Button exitToMenuButton;
    [Header("Pause Menu References")]
    public Animator pauseMenu;
    public Button pauseMenuButton;
    public Button resumeButton;
    public Button restartButton;
    public Button exitToMainMenuButton;

    private bool inMainMenu = true;
    private bool isPaused = false;
    private bool isDied = false;

    public float panelFadeDuration = 0.5f;
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
        DialogManager.instance.introDialogEnded.AddListener(OnIntroDialogEnded);

        startButton.onClick.AddListener(CloseMainMenu);
        exitGameButton.onClick.AddListener(ExitGame);

        skipIntroDialog.onClick.AddListener(DialogManager.instance.DisplayNextIntroSentence);

        pauseMenuButton.onClick.AddListener(TogglePause);
        resumeButton.onClick.AddListener(TogglePause);
        restartButton.onClick.AddListener(Restart);


        mainMenu.gameObject.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    } 

    public void Restart()
    {
        Scene_Nav_Manager.instance.Restart();
    }

    public void GoToMainMenu()
    {
        mainMenu.SetTrigger("Show");
    }

    private void ShowMenu()
    {

    }
    
    

    #region StartGame
    private void CloseMainMenu()
    {
        mainMenu.SetTrigger("Close");
        StartCoroutine(TurnOffPanelAfterDelay(mainMenuImage));
        DialogManager.instance.StartIntroDialog(introDialog);
        inMainMenu = false;

    }
    private void OnIntroDialogEnded()
    {
        StartCoroutine(TurnOffPanelAfterDelay(dialogBackground));
        InitializePlayer();
    }
    public void InitializePlayer()
    {
        PlayerState_Manager.instance.InitializePlayer();
    }
    #endregion

    #region Exit Game
    public void ExitGame()
    {
        Application.Quit();
    }
    #endregion

    #region Pause Menu

    private void TogglePause()
    {
        if (isDied)
        {
            return;
        }

        if(!isPaused)
        {
            Pause();

        }
        else
        {
            UnPause();
        }
    }

    private void Pause()
    {
        pauseMenu.SetTrigger("Show");
        isPaused = true;
        pauseMenuButton.gameObject.SetActive(false);
    }
    private void UnPause()
    {
        pauseMenu.SetTrigger("Hide");
        isPaused = false;
        pauseMenuButton.gameObject.SetActive(true);
    }

    #endregion

    public void ShowDeathMenu()
    {
        if(isPaused)
        {
            UnPause();
        }
        deathMenu.SetTrigger("Show");
        isDied = true;
    }

    #region Animation/Visual
    private IEnumerator TurnOffPanelAfterDelay(Image image)
    {
        yield return new WaitForSeconds(0.5f);

        float temp = 1f;
        float fadeRate = 1f / panelFadeDuration;

        Color color = image.color;

        while (temp > 0f)
        {
            temp -= Time.deltaTime * fadeRate;
            color.a = Mathf.Clamp01(temp);  // Clamp to ensure the alpha stays between 0 and 1
            image.color = color;

            yield return null;  // Wait for the next frame
        }

        image.gameObject.SetActive(false);

        if (image == mainMenuImage)
        {
            dialogBackground.gameObject.SetActive(true);
        }
    }
    #endregion
}
