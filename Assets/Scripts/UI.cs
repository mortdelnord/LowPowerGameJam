using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    float endTimer = 0f;

    public GameObject player;
    public Image batteryMeter;
    public GameObject baterMeterObject;
    public GameObject cameraFrame;
    public GameObject credits;
    public GameObject startScreen;

    public Button creditsButton;
    public Button backButton;
    public Button startButton;

    
    public Player playerScript;

    private bool isCredit = false;
    private bool isEnd = false;

    public void Start()
    {
        DoorScript1.Instance.OnLastDoorOpen += Instance_OnLastDoorOpen;

        isEnd = false;
        startScreen.SetActive(true);
        playerScript = player.GetComponent<Player>();
        playerScript.isDisabled = true;
        baterMeterObject.SetActive(false);
        cameraFrame.SetActive(false);
        Time.timeScale = 0f;

        creditsButton.onClick.AddListener(Credits);
        backButton.onClick.AddListener(Credits);
        startButton.onClick.AddListener(StartScreen);
    }

    private void Instance_OnLastDoorOpen(object sender, System.EventArgs e)
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        isCredit = true;
        isEnd = true;

    }

    public void Update()
    {
        batteryMeter.fillAmount = playerScript.energyAmount / 100;
        //Debug.Log(playerScript.energyAmount);

        credits.SetActive(isCredit);

        if (isEnd)
        {
            Ending();
        }
    }

    

    public void StartScreen()
    {
        
        playerScript.isDisabled = false;
        startScreen.SetActive(false);
        baterMeterObject.SetActive(true);
        cameraFrame.SetActive(true);

        Time.timeScale = 1f;
    }

    public void Credits()
    {
        isCredit = !isCredit;
    }
    
    public void Ending()
    {
        startScreen.SetActive(true);
        endTimer += Time.deltaTime;
        Debug.Log(endTimer);
        if (endTimer >= 5f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}
