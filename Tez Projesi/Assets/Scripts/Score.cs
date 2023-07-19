using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour, IDataPersistence
{
    [SerializeField] private TMP_Text infoScoreText;
    [SerializeField] private GameObject progressBarGreen;
    [SerializeField] private GameObject testButtonActivation;

    private bool takenInfo = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            takenInfo = true;
        }
    }
    private void Update()
    {
        if (takenInfo && Input.GetKeyDown(KeyCode.F) && gameObject.CompareTag("Ground") && GameData.collectedInfo < 7)
        {
            Debug.Log("collected info artt�r�lmadan �nce: " + GameData.collectedInfo);
            GameData.collectedInfo++;
            GameData.levelScore += 75;
            Debug.Log("Local Score: " + GameData.levelScore);
            infoScoreText.text = GameData.collectedInfo.ToString();
            progressBarGreen.transform.GetChild(GameData.collectedInfo - 1).gameObject.SetActive(true);
            Debug.Log("collected info artt�r�ld�ktan sonra: " + GameData.collectedInfo);

            gameObject.tag = "Collected";
            
            if (GameData.collectedInfo == 4)
            {
                testButtonActivation.SetActive(true);
            }

            if (GameData.collectedInfo == 7)
            {
                //GameData.levelScore += 100;
            }
        }
        else
        {
            Debug.Log("Toplanan t�m info: " + GameData.collectedInfo);
        }
        
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            takenInfo = false;
        }
    }

    public void LoadData(GameData gameData)
    {
        GameData.collectedInfo = gameData.collectedInfo2;
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.collectedInfo2  = GameData.collectedInfo;
    }
}
