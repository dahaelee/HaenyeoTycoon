using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class betting_rsp : MonoBehaviour
{
    //int you = -1;
    int seller = -1;
    //public Image rockSelect;
    //public Image objStore;
    public GameObject[] select;
    public GameObject hide, question;
    public GameObject[] seller_rsp;
    public float time = 0;
    public int rsp_result = -1;

    public void result(int myHand) // 상인 랜덤으로 가위바위보 정하고 해녀 중심 결과 rsp_result에 저장
    {
        for(int i = 0; i < 3; i++)
        {
            seller_rsp[i].SetActive(false);
        }
        seller = Random.Range(0, 3); // 0 = rock, 1 = sci, 2 = paper
        seller_rsp[seller].SetActive(true);
        Debug.Log(seller);

        int k = (3 + myHand - seller) % 3;
        if(k == 0)
        {
            rsp_result = 0; // 비김            
        }
        else if(k == 1)
        {
            rsp_result = 1; // 짐
        }
        else
        {
            rsp_result = 2; // 이김
        }
    }
    public void clickRock()
    {
        select[0].SetActive(true);
        hide_effect();
        //hide.SetActive(false);
        result(0);
    }
    public void clickSci()
    {
        select[1].SetActive(true);
        hide_effect();
        //hide.SetActive(false);
        result(1);
    }
    public void clickPaper()
    {
        select[2].SetActive(true);
        hide_effect();
        //hide.SetActive(false);
        result(2);
    }

    public void hide_effect() // 가림막 없어지는 효과
    {
        hide.transform.localScale = new Vector3(0, 0.8f, 1) + new Vector3(0.8f, 0, 0) * (1 - 2 * time);

        if (time > 0.5f)
        {
            time = 0;
            hide.gameObject.SetActive(false);
        }
        time += Time.deltaTime;
    }
    // Start is called before the first frame update
    void Start()
    {
       
        for (int i = 0; i < 3; i++)
        {
            select[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (rsp_result > 0)
        {
            question.gameObject.SetActive(false);
            hide_effect();
            if (rsp_result == 0)
            {
                Debug.Log("비겼습니다.");
            }
            else if (rsp_result == 1)
            {
                Debug.Log("졌습니다.");
            }
            else
            {
                Debug.Log("이겼습니다.");
            }
        }
    }
    
}
