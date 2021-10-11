using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.Video;

public class StartMenu : MonoBehaviour
{
    public GameObject loadFileScreen;

    [Header("Loading")]
    public Slider progressBar;
    public TextMeshProUGUI progressText;
    //public Animator fadeScreen;
    public VideoPlayer flyStart;
    public GameObject loadingScreen;
    AsyncOperation operation;
    float progress;
    bool downloading;

    [Header("Select Menu")]
    public GameObject selectMenu;
    public Image newGameBox;
    public Image loadFileBox;
    public Image exitGameBox;
    public Transform footPrintMark;
    public Sprite yelloBox, brownBox;
    int onSelectIndex; // 0 newGame,  1 loadFile, 2 exitGame;
    bool upArrow, downArrow, enter;

    private void Update()
    {
        if (!downloading)
        {
            DetectInput();
            HandleInput();
        }
        else if (!flyStart.isPlaying)
        {
            //開始下載 且影片已播完
            operation.allowSceneActivation = true;
            if (progress < 1)
            {
                //下載還沒完成
                loadingScreen.SetActive(false);
            }
        }
        else if (progress >= 1)
        {
            //開始下載 影片還沒播完下載已經完成
            progressBar.gameObject.SetActive(false);
        }
    }

    public void DetectInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            upArrow = true;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            downArrow = true;
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            enter = true;
        }
    }

    public void HandleInput()
    {
        if (selectMenu.activeSelf)
        {
            if (upArrow)
            {
                upArrow = false;
                OnSelectChange(true);
            }
            if (downArrow)
            {
                downArrow = false;
                OnSelectChange(false);
            }
            if (enter)
            {
                if (onSelectIndex == 0)
                {
                    selectMenu.SetActive(false);
                    LoadGame();
                }
                else if (onSelectIndex == 1)
                {
                    selectMenu.SetActive(false);
                    loadFileScreen.SetActive(true);
                }
                enter = false;
            }
        }
        else if (loadFileScreen.activeSelf)
        {
            //TODO 選擇讀取檔案
            if (upArrow)
            {
                upArrow = false;
            }
            if (downArrow)
            {
                downArrow = false;
            }
            if (enter)
            {

            }
        }
    }

    public void OnSelectChange(bool isUp)
    {
        AudioManager.instance.buttonSound.Play();
        if (isUp)
        {
            if (onSelectIndex < 1)
            {
                return;
            }
            else
            {
                onSelectIndex -= 2;
            }
        }
        else
        {
            if (onSelectIndex > 1)
            {
                return;
            }
            else
            {
                onSelectIndex += 2;
            }
        }


        switch (onSelectIndex)
        {
            case 0:
                // newGame
                newGameBox.sprite = yelloBox;
                loadFileBox.sprite = brownBox;
                exitGameBox.sprite = brownBox;
                footPrintMark.SetParent(newGameBox.transform);
                footPrintMark.localPosition = new Vector3(-180, 0, 0);
                break;
            case 1:
                // loadFile
                newGameBox.sprite = brownBox;
                loadFileBox.sprite = yelloBox;
                exitGameBox.sprite = brownBox;
                footPrintMark.SetParent(loadFileBox.transform);
                footPrintMark.localPosition = new Vector3(-180, 0, 0);
                break;
            case 2:
                // exit
                newGameBox.sprite = brownBox;
                loadFileBox.sprite = brownBox;
                exitGameBox.sprite = yelloBox;
                footPrintMark.SetParent(exitGameBox.transform);
                footPrintMark.localPosition = new Vector3(-180, 0, 0);
                break;
        }
    }


    public void LoadGame()
    {
        downloading = true;
        selectMenu.SetActive(false);
        loadingScreen.SetActive(true);
        Debug.Log("loadingScreen active");
        progressBar.gameObject.SetActive(true);
        StartCoroutine(AudioFade.FadeIn("MainVolume", 1.5f, Mathf.SmoothStep));
        StartCoroutine(AudioFade.FadeOut("MenuVolume", 1.5f, Mathf.SmoothStep));
        StartCoroutine(LoadAsynchronously());
    }

    IEnumerator LoadAsynchronously()
    {
        operation = SceneManager.LoadSceneAsync(1);
        operation.allowSceneActivation = false;
        while (!operation.isDone)
        {
            progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressBar.value = progress;
            progressText.text = "Loading... " + Mathf.FloorToInt(progress * 100f) + "%";
            yield return null;
        }
    }
}
