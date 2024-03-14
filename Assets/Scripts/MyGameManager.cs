using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MyGameManager : MonoBehaviour
{
    
    public float LastSpawnedPos { get; set; }
    public GameState CurGameState { get; set; }
    //lưu vị trí trước khi chết
    public Vector3 playerBeforeDeathPos { get; set; }

    public long HighScore { get; set; }
    public long Score { get; set; }
    public int NextObsColorType;
    public static MyGameManager Instance { private set; get; }

    public PlayerController playerController;

    public EviController eviController;

    public UIController uIController;

    public long MissonTotalJumpAmount { get; set; }
    public long MissonJumpAmountInOneRun { get; set; }
    public long MissonScoreAmountReach { get; set; }
    public AudioClip[] audioClips;
    public AudioSource audioSource;

    private UserInfo userInfo;


    private void Awake()
    {
        Instance = this;

        LastSpawnedPos = Const.LAST_POS_ORI;

        CurGameState = GameState.ready;
        NextObsColorType = UnityEngine.Random.Range(0,uIController.material.Length);
        playerController.gameObject.GetComponent<Renderer>().material = uIController.material[UnityEngine.Random.Range(0, uIController.material.Length)];
        eviController.obsOld.gameObject.GetComponent<Renderer>().material = uIController.material[NextObsColorType];

    }

    // Start is called before the first frame update
    void Start()
    {
        //set user info
        string userInforStr =  PlayerPrefs.GetString("user_info", "{\"HighScore\":8,\"MissonTotalJumpAmount\":8,\"MissonJumpAmountInOneRun\":10,\"MissonScoreAmountReach\":11}");
        userInfo = JsonUtility.FromJson<UserInfo>(userInforStr);
        App.Trace(userInfo.HighScore+";" +userInfo.MissonTotalJumpAmount+"; "+userInfo.MissonJumpAmountInOneRun+"; "+userInfo.MissonScoreAmountReach);
        uIController.missionText[0].text = "Total Jump Amount: " + MissonTotalJumpAmount + "/50";
        uIController.missionText[1].text = "Jump Amount In One Run: " + MissonJumpAmountInOneRun + "/50";
        uIController.missionText[2].text = "Score Amount Reach: " + MissonScoreAmountReach + "/50";

        Debug.Log("Start game after 2s");
        //Invoke("StartTheGame", 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartTheGame()
    {
        uIController.gojs[3].SetActive(true);
        StartCoroutine(CountDown(delegate {
            Debug.Log("Start the game");
            CurGameState = GameState.playing;
        }));
        
    }

    public void Death()
    {
        audioSource.PlayOneShot(audioClips[0]);
        CurGameState = GameState.death;
        uIController.gojs[3].SetActive(false);
        //dung lai
        playerController.rb.useGravity = false;
        playerController.rb.velocity = Vector3.zero;

        //show game over
        uIController.ShowGameOver();
        MissonScoreAmountReach += Score;
        uIController.missionText[2].text = "Score Amount Reach: " + (MissonScoreAmountReach) + "/1500";

        //save user info
        userInfo.HighScore = HighScore;
        userInfo.MissonJumpAmountInOneRun = MissonJumpAmountInOneRun;
        userInfo.MissonScoreAmountReach = MissonScoreAmountReach;
        userInfo.MissonTotalJumpAmount = MissonTotalJumpAmount;
        PlayerPrefs.SetString("user_info", JsonUtility.ToJson(userInfo));
        PlayerPrefs.Save();
    }
    public void ContinueGame()
    {
        playerController.transform.position = new Vector3(playerBeforeDeathPos.x, 2f, playerBeforeDeathPos.z);
        uIController.gojs[1].SetActive(false);
        uIController.gojs[3].SetActive(true);
        StartCoroutine(CountDown(delegate {
            CurGameState = GameState.playing;
            playerController.rb.useGravity = true;
        }));
        
    }
    public void NewGame()
    {
        MissonJumpAmountInOneRun = 0;
        uIController.gojs[1].SetActive(false);
        uIController.gojs[3].SetActive(true);
        LastSpawnedPos = Const.LAST_POS_ORI;
        Debug.Log(LastSpawnedPos);
        eviController.SpanwnFirst5obs();
        playerController.transform.position = new Vector3(0, 2f, 0);
        uIController.scoreText.text = "";
        StartCoroutine(CountDown(delegate {
            CurGameState = GameState.playing;
            playerController.rb.useGravity = true;
        }));
        
    }
    IEnumerator CountDown(Action callback)
    {
        uIController.scoreText.text = "";
        int time = 3;
        while(time > 0)
        {
            uIController.countDownText.text = time.ToString();
            yield return new WaitForSeconds(1f);
            time--;

        }
        uIController.countDownText.text = "";
        if(callback != null)
        {
            callback();
        }
    }
    public void UpdateScore(long score)
    {
        uIController.scoreText.text = score.ToString();
        //MissonScoreAmountReach = Score;
        Score = score;
        if (score > HighScore)
        {
            HighScore = score;
        }
    }
    public void UpdateJumpMission()
    {
        MissonTotalJumpAmount++;
        MissonJumpAmountInOneRun++;
        uIController.missionText[0].text = "Total Jump Amount: " + MissonTotalJumpAmount + "/500";
        uIController.missionText[1].text = "Jump Amount In One Run: " + MissonJumpAmountInOneRun + "/500";
    }

    public enum GameState
    {
        ready, playing, death
    }

    public class UserInfo
    {
        public long HighScore;
        public long MissonTotalJumpAmount;
        public long MissonJumpAmountInOneRun;
        public long MissonScoreAmountReach;
    }
}
