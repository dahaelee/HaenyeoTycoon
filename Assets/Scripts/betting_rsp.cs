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

    public Image[] result_ui;
    public GameObject result_mask;
    public Image target;

    public int rand_text = -1;
    public string[] rsp_text;
    public Text seller_text;
    public Image bubble_seller;

    public AudioSource win, lose;

    public void Awake()
    {
        win.volume = PlayerPrefs.GetFloat("Effect_volume", 1);
        lose.volume = PlayerPrefs.GetFloat("Effect_volume", 1);
    }

    public void initial_rsp()
    {
        result_mask.SetActive(false);
        hide.SetActive(true);
        for (int i = 0; i < 3; i++)
        {
            seller_rsp[i].SetActive(false);
            select[i].SetActive(false);
            result_ui[i].gameObject.SetActive(false);
        }
        hide_time = 0;
        result_time = 0;
        seller = -1;
        hide.transform.localScale = new Vector3(0.8f, 0.8f, 1);
        question.SetActive(true);
        blank_bg.SetActive(false);
        rsp_result = -1;
        rsp_talk();
    }

    public void rsp_talk()
    {
        rsp_text = new string[]
        {
            "허허~ 이번엔 무엇을 내볼까~?",
            "흐음, 가위를 내볼까나?",
            "흐음, 바위를 내볼까나?",
            "흐음, 보를 내볼까나?",
            "가위를 내보는건 어떠냐?",
            "바위를 내보는건 어떠냐?",
            "보를 내보는건 어떠냐?",
            "왠지 가위가 내고 싶은걸?",
            "왠지 바위가 내고 싶은걸?",
            "왠지 보가 내고 싶은걸?",
            "허허~ 아무거나 내보렴!"
        };
        rand_text = Random.Range(0, rsp_text.Length);
        seller_text.text = rsp_text[rand_text];
        StartCoroutine(UI_On(bubble_seller, seller_text));
    }
    public IEnumerator UI_On(Image image, Text text)
    {
        yield return new WaitForSeconds(0.1f);
        image.gameObject.SetActive(true);
        text.gameObject.SetActive(true);
        for (int i = 0; i < 5; i++)
        {
            image.rectTransform.localScale = new Vector3((float)(1.30 + i * 0.01), (float)(1.30 + i * 0.01), (float)(1.30 + i * 0.01));
            text.rectTransform.localScale = new Vector3((float)(0.95 + i * 0.01), (float)(0.95 + i * 0.01), (float)(0.95 + i * 0.01));
            yield return 0;
        }
        yield return new WaitForSeconds(2.5f);
    }
    public IEnumerator result_effect(Image result_ui)
    {
        result_ui.gameObject.SetActive(true);
        for (float i = 0.1f; i <= 1; i += 0.1f)
        {
            Color color = new Vector4(1, 1, 1, i);
            result_ui.color = color;
            yield return new WaitForSeconds(0);
        }
        yield return new WaitForSeconds(0.001f);
    }

    public void result(int myHand) // 상인 랜덤으로 가위바위보 정하고 해녀 중심 결과 rsp_result에 저장
    {
        for(int i = 0; i < 3; i++)
        {
            seller_rsp[i].SetActive(false);
        }
        seller = Random.Range(0, 3); // 0 = rock, 1 = sci, 2 = paper
        seller_rsp[seller].SetActive(true);
        
        int k = (3 + myHand - seller) % 3;
        if(k == 0)
        {
            rsp_result = 0; // 비김
            seller_text.text = "비겼구만! 그렇다면 제값에 받도록 하마.";
        }
        else if(k == 1)
        {
            rsp_result = 1; // 짐
            seller_text.text = "아이고~ 어쩌니. 아저씨가 이겨버렸네. 내기는 내기니까, 반값으로 잘 받아가마 해녀야~";
            lose.PlayOneShot(lose.clip);

            //효민 - 5번 퀘스트 관련
            if (quest_Data.daily_quest_list[5].state != -1 && quest_Data.daily_quest_list[5].state != 2)
            {
                if (PlayerPrefs.GetInt("quest5", 0) >= 0) PlayerPrefs.SetInt("quest5", PlayerPrefs.GetInt("quest5") - 1);
            }
        }
        else
        {
            rsp_result = 2; // 이김
            seller_text.text = "이런~ 내가 졌구나. 기분이다! 다섯배로 쳐주마!";
            win.PlayOneShot(win.clip);

            //효민 - 5번 퀘스트 관련
            if (quest_Data.daily_quest_list[5].state != -1 && quest_Data.daily_quest_list[5].state != 2)
            {
                if (PlayerPrefs.GetInt("quest5", 0) < 3) PlayerPrefs.SetInt("quest5", PlayerPrefs.GetInt("quest5") + 1);
            }
        }
    }
    
    public void clickRock()
    {
        result_mask.SetActive(true);
        select[0].SetActive(true);
        result(0);
        blank_bg.SetActive(true);
    }
    public void clickSci()
    {
        result_mask.SetActive(true);
        select[1].SetActive(true);
        result(1);
        blank_bg.SetActive(true);
    }
    public void clickPaper()
    {
        result_mask.SetActive(true);
        select[2].SetActive(true);
        result(2);
        blank_bg.SetActive(true);
    }

    public void hide_effect() // 가림막 없어지는 효과
    {
        hide.transform.localScale = new Vector3(0, 0.8f, 1) + new Vector3(0.8f, 0, 0) * (1 - 2 * hide_time);
        if (hide_time > 0.5f)
        {
            hide.gameObject.SetActive(false);
        }
        hide_time += Time.deltaTime;
        StartCoroutine(result_effect(result_ui[rsp_result]));
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //win.volume = PlayerPrefs.GetFloat("Effect_volume", 1);
        //lose.volume = PlayerPrefs.GetFloat("Effect_volume", 1);
        rsp_talk();
        initial_rsp();
        result_mask.SetActive(false);
        blank_bg.SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            select[i].SetActive(false);
            result_ui[i].gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        rsp_result = -1;
    }
    // Update is called once per frame
    void Update()
    {
        if (rsp_result >= 0)
        {
            question.gameObject.SetActive(false);
            hide_effect();
        }
    }
    
}
