using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EviController : MonoBehaviour
{
    public GameObject obs_prefal;

    public Transform obs_parent;

    public GameObject obsOld;

    public List<ObsContronler> SpawnedObs
    {
        private set; get;
    } 
    // Start is called before the first frame update
    void Start()
    {
        SpanwnFirst5obs();
    }
    public void SpanwnFirst5obs()
    {
        if(SpawnedObs == null)
        {
            SpawnedObs = new List<ObsContronler>();

            // dùng for để tạo ra các obs clone
            for (int i = 0; i < 5; i++)
            {
                SpanwnNextObs();
            }
        }else
        {
            // dùng for để tạo ra các obs clone
            for (int i = 0; i < SpawnedObs.Count; i++)
            {
                SpawnedObs[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < 5; i++)
            {
                SpanwnNextObs();
            }
        }    
        
    }
    public void SpanwnNextObs()
    {
       

        //tạo ra độ dài của obs randomlength 
        int rand = UnityEngine.Random.Range(1, 4);
        

        //tạo ra khoảng cách giữa 2 obs 
        int rand_pos = UnityEngine.Random.Range(1, Const.MAX_OBS_DIS);
        

        //Tạo obs mới 
        GameObject goj = SpawnObs(rand*2);

        // Gắn chiều dài của obs đó
        goj.GetComponent<ObsContronler>().ObsLength = rand;
        goj.SetActive(true);
        //set vị trí cho cột đó bằng (vị trí cuối cùng + chuyền dài của cột + khoảng cách 
        goj.transform.position = new Vector3(0, -3.9f,
            MyGameManager.Instance.LastSpawnedPos
            + rand
            + rand_pos
            );
        goj.gameObject.GetComponent<Renderer>().material = MyGameManager.Instance.uIController.material[MyGameManager.Instance.NextObsColorType];

        MyGameManager.Instance.LastSpawnedPos += rand*2 + rand_pos;
        App.Trace("vị trí cuối cùng= "+MyGameManager.Instance.LastSpawnedPos, App.DebugColor.green);
        


    }
    public GameObject SpawnObs(int rand_length)
    {
        for (int i = 0; i < SpawnedObs.Count; i++)
        {
            if (SpawnedObs[i].ObsLength == rand_length && !SpawnedObs[i].gameObject.activeInHierarchy)
            {
                Debug.Log("lấy lại obs");
                return SpawnedObs[i].gameObject; ;
            }
        }
        Debug.Log("Tạo mới obs");
        GameObject goj = Instantiate(obs_prefal, obs_parent, false);
        //set scale cho cột
        goj.transform.localScale = new Vector3(
            goj.transform.localScale.x,
            goj.transform.localScale.y,
            rand_length
            );
        ObsContronler obsContronler = goj.GetComponent<ObsContronler>();
        obsContronler.ObsLength = rand_length;
        SpawnedObs.Add(obsContronler);
        App.Trace("length = " + SpawnedObs.Count, App.DebugColor.white);
        return goj;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
