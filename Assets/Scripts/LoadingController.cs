using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour
{
    public TextMeshProUGUI LoadingText;
    public int dotCount;

    // Start is called before the first frame update
    void Start()
    {
        //delay 2s mới load game
        //Invoke("LoadGame", 3f);
        InvokeRepeating("ChangeText", .25f, .25f);
        StartCoroutine(LoadGameSceneAsync());
    }
    void LoadGame()
    {
        //SceneManager.LoadScene("Game");
    }
    void ChangeText()
    {
        dotCount--;
        if (dotCount == -1)
        {
            dotCount = 3;
        }
        LoadingText.text = "";
        for(int i = 1; i < dotCount; i++)
        {
            LoadingText.text += ".";
        }

    }
    IEnumerator LoadGameSceneAsync()
    {
        yield return new WaitForSeconds(2f);
        AsyncOperation async =  SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
        while (!async.isDone)
        {
            yield return null;
        }
        Scene scene = SceneManager.GetSceneByName("Game");
        if(scene != null && scene.isLoaded)
        {
            SceneManager.SetActiveScene(scene);
            gameObject.SetActive(false);
        }

    }
}
