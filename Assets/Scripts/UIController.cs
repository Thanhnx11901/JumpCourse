using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI countDownText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScore;
    public TextMeshProUGUI score;
    public Material[] material;
    public GameObject[] gojs;
    public TextMeshProUGUI[] missionText;
    public GameObject btnMission;

    
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0;i < gojs.Length; i++)
        {
            gojs[i].SetActive(false);
        }
        gojs[0].SetActive(true);
        gojs[0].transform.position = gojs[2].transform.position;
    }
    public void PlayGame()
    {
        gojs[0].SetActive(false);
        MyGameManager.Instance.StartTheGame();
    }
    public void ShowGameOver()
    {
        gojs[1].SetActive(true);
        gojs[1].transform.position = gojs[2].transform.position;
        highScore.text = "HIGH SCORE: " + MyGameManager.Instance.HighScore.ToString();
        score.text = "SCORE: " + MyGameManager.Instance.Score.ToString();

    }
    public void ChoosePlayorColor(int i)
    {
        MyGameManager.Instance.playerController.gameObject.GetComponent<Renderer>().material = material[i];
    }
    public void ChooseGroundColor(int type)
    {
        for(int i=0; i < MyGameManager.Instance.eviController.SpawnedObs.Count; i++)
        {
            MyGameManager.Instance.eviController.SpawnedObs[i].gameObject.GetComponent<Renderer>().material = material[type];
        }
        MyGameManager.Instance.NextObsColorType = type;
        MyGameManager.Instance.eviController.obsOld.gameObject.GetComponent<Renderer>().material = material[type];
    }

    public void ShowChooseColorPop()
    {
        gojs[4].SetActive(true);
        gojs[4].transform.position = gojs[2].transform.position;
    }

    public void ShowMisson()
    {
        btnMission.transform.position = gojs[2].transform.position;
        Debug.Log(btnMission.transform.position);
    }
}
