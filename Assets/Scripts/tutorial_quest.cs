﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class tutorial_quest : MonoBehaviour
{
    //공통 오브젝트
    public GameObject quest_ui;
    public Image touch_bg, quest_bg, text_window, next_triangle;
    public Text text, hilight_text;
    public static int step, quest_num;
    public static int IsQuest, item_num = 0;     //퀘스트 확인용

    //farm object
    public GameObject hilight_parent, bubble_parent;
    public Image[] hilight;
    public Image sache, speech_bubble;
    public Text bubble_text;
    public string[] tutorial_texts;
    public Image sea_icon_fake;

    public void Initialize()
    {
        touch_bg.gameObject.SetActive(false);
        quest_bg.gameObject.SetActive(false);
        sache.gameObject.SetActive(false);
        text_window.gameObject.SetActive(false);
        next_triangle.gameObject.SetActive(false);
        hilight_text.gameObject.SetActive(false);
        hilight_parent.SetActive(false);
        bubble_parent.gameObject.SetActive(false);
        sea_icon_fake.gameObject.SetActive(false);
        quest_ui.gameObject.SetActive(false);
    }

    //다음 텍스트
    public void Next_text()
    {
        step++;
        switch (quest_num)
        {
            case 0:
                Initialize();
                break;
            case 1:
                switch (step)
                {
                    case 2:
                        hilight_text.gameObject.SetActive(true);
                        text.text = "우선 5일을 주마.\n                        을 갚지 못한다면.. \n어떻게 되는진 말하지 않아도 알겠지 ? ";
                        hilight_text.text = "5일동안 20만원";
                        break;
                    default:
                        Initialize();
                        Daddy1();
                        break;
                }
                break;
            case 2:
                if (step == 2)
                {
                    quest_bg.gameObject.SetActive(false);
                    hilight[0].gameObject.SetActive(true);

                    bubble_parent.transform.position = new Vector3(640, 260, 0); // 화면 하단 위치
                    bubble_text.text = tutorial_texts[1];
                }
                else
                {
                    if (step < 8)
                    {
                        hilight[step - 3].gameObject.SetActive(false);
                        hilight[step - 2].gameObject.SetActive(true);
                        bubble_text.text = tutorial_texts[step - 1];
                    }
                    //말풍선 위치 조정
                    if (step == 5) bubble_parent.transform.position = new Vector3(640, 450, 0);
                    if (step == 8)
                    {
                        bubble_text.text = tutorial_texts[step - 1];
                        sea_icon_fake.gameObject.SetActive(true);
                    }
                }
                break;
            case 3:
                Initialize();
                break;
        }
    }

    // 사채업자의 첫번째 빚재촉
    public void Sache1()
    {
        Initialize();
        quest_manager.quest_contents.Clear(); //퀘스트 목록 지우기
        touch_bg.gameObject.SetActive(true);
        quest_bg.gameObject.SetActive(true);
        sache.gameObject.SetActive(true);
        text_window.gameObject.SetActive(true);
        next_triangle.gameObject.SetActive(true);
        hilight_text.gameObject.SetActive(false);
        bubble_parent.gameObject.SetActive(false);


        quest_manager.quest_contents.Add(1, new Quest_Content(1, "사채업자의 빚재촉", "5일동안 20만원 갚기", 0));   // 퀘스트 목록에 추가하기
        GameObject.Find("quest_manager").GetComponent<quest_manager>().quest_contents_update();//실시간 반영
        step = 1; quest_num = 1;
        text.text = "너가 대신 아버지빚을 갚겠다고? \n마음은 기특하지만,\n과연 너가 돈을 갚을 수 있을진 의심이 되는군";
    }

    //아빠 1 - 게임 ui 설명
    public void Daddy1()
    {
        tutorial_texts = new string[]
        {
            "아빠 때문에 네가 고생하는 것 같아 마음이 편하지가 않구나..\n아직 양식장 구성에 대해 잘 모를테니 몇가지 설명을 해주마..",
            "이건 너의 소지금을 나타낸단다...",
            "이건.. 한 달동안 갚아야할 돈의 액수란다..",
            "이건 너의 체력을 나타내.. 체력이 0이되면, 하루를 마무리 짓게 된단다.",
            "이걸 누르면, 해야 할일의 목록과 내용을 확인할 수 있단다..",
            "상점 아이콘을 누르면, 상점으로 갈 수 있단다..",
            "바다 아이콘을 누르면, 바다로 갈 수 있단다..",
            "이 참에 바다에서 물질하는 법도 알려주마… 바다 아이콘을 눌러보렴"
        };

        touch_bg.gameObject.SetActive(true);
        quest_bg.gameObject.SetActive(true);
        bubble_parent.SetActive(true);
        hilight_parent.SetActive(true);
        for (int i = 0; i < hilight.Length; i++)
        {
            hilight[i].gameObject.SetActive(false);
        }

        quest_manager.quest_contents.Add(2, new Quest_Content(1, "아빠의 가르침", "바다에서 자원 1개 이상 채집하기", 0));   // 퀘스트 목록에 추가하기
        GameObject.Find("quest_manager").GetComponent<quest_manager>().quest_contents_update(); //실시간 반영
        step = 1; quest_num = 2;
        bubble_parent.transform.position = new Vector3(640, 450, 0); // 화면 상단 위치
        bubble_text.text = "아빠 때문에 네가 고생하는 것 같아 마음이 편하지가 않구나..\n아직 양식장 구성에 대해 잘 모를테니 몇가지 설명을 해주마..";
    }

    //fake_sea_icon 눌렀을 때 함수
    public void sea_open()
    {
        Initialize();
        PlayerPrefs.SetInt("isQuest", 3);
        SceneManager.LoadScene("sea"); // 바다로 이동
        hilight[5].gameObject.SetActive(false);
    }


    public void Update()
    {
        IsQuest = PlayerPrefs.GetInt("isQuest", 1);
        Check_quest(IsQuest);     // quest 완료 검사
    }

    // 퀘스트 완료 확인
    public void Check_quest(int IsQuest)
    {
        switch (IsQuest)
        {
            case 3:
                for (int i = 0; i < 9; i++)
                {
                    item_num += Haenyeo.sea_item_number[i];
                }
                if (item_num > 0)
                {
                    Initialize();
                    touch_bg.gameObject.SetActive(true);
                    quest_bg.gameObject.SetActive(true);
                    bubble_parent.SetActive(true);

                    quest_manager.quest_contents.Remove(2);   // quest 2 삭제
                    quest_manager.quest_contents.Add(3, new Quest_Content(1, "아빠의 가르침", "자원 양식하기", 0));   // 퀘스트 목록에 추가하기
                    GameObject.Find("quest_manager").GetComponent<quest_manager>().quest_contents_update(); ; //실시간 반영

                    step = 1; quest_num = 3;
                    bubble_parent.transform.position = new Vector3(640, 450, 0); // 화면 상단 위치
                    bubble_text.text = "역시 우리 해녀로구나.. 이제 잡은 자원을 양식해보렴";

                    item_num = 0;
                    PlayerPrefs.SetInt("isQuest", 4);
                }
                break;
            case 4:
                for (int i = 0; i < 9; i++)
                {
                    item_num += Haenyeo.farm_item_number[i];
                }
                if (item_num > 0)
                {
                    bubble_parent.SetActive(true);


                    quest_manager.quest_contents.Remove(3);   // quest 3 삭제
                    quest_manager.quest_contents.Add(4, new Quest_Content(1, "아빠의 가르침", "상인 아저씨와 대화하기", 0));   // 퀘스트 목록에 추가하기
                    GameObject.Find("quest_manager").GetComponent<quest_manager>().quest_contents_update(); //실시간 반영

                    step = 1; quest_num = 3;
                    bubble_parent.transform.position = new Vector3(640, 450, 0); // 화면 상단 위치
                    bubble_text.text = "그렇지~ 자원은 그렇게 양식하는 거란다.. 상인 아저씨가 찾던데! 어서 가보렴";

                    item_num = 0;
                    PlayerPrefs.SetInt("isQuest", 4);
                }
                break;
        }
    }

    //quest_ui open
    public void quest_ui_open()
    {
        quest_bg.gameObject.SetActive(true);
        quest_ui.gameObject.SetActive(true);
    }

    public void quest_ui_close()
    {
        quest_bg.gameObject.SetActive(false);
        quest_ui.gameObject.SetActive(false);
    }

    // quest_data를 gameobject로 바꿔줌
    public void Awake()
    {
        GameObject.Find("quest_manager").GetComponent<quest_manager>().quest_contents_update();
    }

    public void Start()
    {
        StartCoroutine("triangle_effect");
    }

    //삼각형 깜박깜박 이펙트
    IEnumerator triangle_effect()
    {
        while (text_window.IsActive())
        {
            next_triangle.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            next_triangle.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.3f);
        }
    }

}