using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class betting_rsp : MonoBehaviour
{
    int seller = -1;
    public GameObject[] select;
    public GameObject hide, question, blank_bg;
    public GameObject[] seller_rsp;
    public float hide_time = 0, result_time = 0;
    public static int rsp_result = -1;

    public GameObject[] result_ui;
    public GameObject result_mask;

    public void initial_rsp()
    {
        hide.SetActive(true);
        for (int i = 0; i < 3; i++)
        {
            seller_rsp[i].SetActive(false);
            select[i].SetActive(false);
        }
        hide_time = 0;
        seller = -1;
        hide.transform.localScale = new Vector3(0.8f, 0.8f, 1);
        question.SetActive(true);
        blank_bg.SetActive(false);
        rsp_result = -1;
    }
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
            result_ui[0].SetActive(true);
        }
        else if(k == 1)
        {
            rsp_result = 1; // 짐
            result_ui[1].SetActive(true);
        }
        else
        {
            rsp_result = 2; // 이김
            result_ui[2].SetActive(true);
        }
    }
    public void clickRock()
    {
        select[0].SetActive(true);
        result(0);
        blank_bg.SetActive(true);
    }
    public void clickSci()
    {
        select[1].SetActive(true);
        result(1);
        blank_bg.SetActive(true);
    }
    public void clickPaper()
    {
        select[2].SetActive(true);
        result(2);
        blank_bg.SetActive(true);
    }

    public void hide_effect() // 가림막 없어지는 효과
    {
        hide.transform.localScale = new Vector3(0, 0.8f, 1) + new Vector3(0.8f, 0, 0) * (1 - 2 * hide_time);
        if (hide_time > 0.5f)
        {
            hide_time = 0;
            hide.gameObject.SetActive(false);
        }
        hide_time += Time.deltaTime;
    }

    public void result_effect(int result) // 가위바위보 결과 내려오는 이펙트
    {
        result_mask.transform.localScale = new Vector3(1, 0, 1) + new Vector3(0, 1, 0) * (1 - 2 * result_time);
        if (result_time > 0.5f)
        {
            result_time = 0;
        }
        result_time += Time.deltaTime;
    }
    // Start is called before the first frame update
    void Start()
    {
        blank_bg.SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            select[i].SetActive(false);
            result_ui[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (rsp_result > 0)
        {
            question.gameObject.SetActive(false);
            hide_effect();
        }
    }
    
}
