using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class tutorial_quest : MonoBehaviour
{
    //공통 오브젝트
    public GameObject quest_ui,quest_box;
    public Image touch_bg, quest_bg,text_box,next_triangle;
    public Text text,hilight_text;
    public static int step, quest_num;
    public static int IsQuest, item_num = 0;     //퀘스트 확인용
    public AudioSource item_click, icon_click;

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
        quest_box.SetActive(false);
        hilight_parent.SetActive(false);
        bubble_parent.gameObject.SetActive(false);
        sea_icon_fake.gameObject.SetActive(false);
        quest_ui.gameObject.SetActive(false);
    }

    //다음 텍스트
    public void Next_text()
    {
        item_click.PlayOneShot(item_click.clip);

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

                        Initialize();
                        Daddy1();
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
                    bubble_text.text = tutorial_texts[1];
                }
                else if (step == 3)
                {
                    quest_bg.gameObject.SetActive(false);
                    hilight[0].gameObject.SetActive(true);

                    bubble_parent.transform.position = new Vector3(640, 260, 0); // 화면 하단 위치
                    bubble_text.text = tutorial_texts[2];
                }
                else
                {
                    if (step < 9)
                    {
                        hilight[step - 4].gameObject.SetActive(false);
                        hilight[step - 3].gameObject.SetActive(true);
                        bubble_text.text = tutorial_texts[step - 1];
                    }
                    //말풍선 위치 조정
                    if (step == 6) bubble_parent.transform.position = new Vector3(640, 450, 0);
                    if (step == 9)
                    {
                        bubble_text.text = tutorial_texts[step - 1];
                        sea_icon_fake.gameObject.SetActive(true);
                    }
                }
                break;
            case 3:
                Initialize();
                break;
            case 4:
                if (step == 2)
                {
                    text.text = "다음은 15일 까지 누적 송금액이 200만원 이상이 되도록 해";
                    hilight_text.text = "15일 까지 200만원 이상 송금하기";
                }
                if (step == 3)
                {
                    SceneManager.LoadScene("ending_bad");
                }
                else Initialize();
                break;
        }
    }

    // 사채업자의 첫번째 빚재촉
    public void Sache1()
    {
        Initialize();
        touch_bg.gameObject.SetActive(true);
        quest_bg.gameObject.SetActive(true);
        quest_box.SetActive(true);
        sache.gameObject.SetActive(true);
        hilight_text.text = "";
        step = 1; quest_num = 1;

        text.text = "어이 꼬맹이 너가 대신 빚을 갚겠다고? 그래 좋다.. 대신 매일 돈을 송금해야 할거다. 두고보자";
    }

    //아빠 1 - 게임 ui 설명
    public void Daddy1()
    {
        tutorial_texts = new string[]
        {
            "아빠 때문에 네가 고생하는 것 같아 마음이 편하지가 않구나..",
            "아직 양식장 구성에 대해 잘 모를테니 몇가지 설명을 해주마..",
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

        quest_Data.tutorial_quest_list[0].state = 1;    //진행중인 퀘스트로 변경
        GameObject.Find("quest_manager").GetComponent<quest_manager>().quest_contents_update(); //실시간 반영
        step = 1; quest_num = 2;
        bubble_parent.transform.position = new Vector3(640, 450, 0); // 화면 상단 위치
        bubble_text.text = tutorial_texts[0];
    }

    //fake_sea_icon 눌렀을 때 함수
    public void sea_open()
    {
        icon_click.PlayOneShot(icon_click.clip);
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

                    quest_Data.tutorial_quest_list[0].state = -1;    //퀘스트 목록에서 삭제
                    quest_Data.tutorial_quest_list[1].state = 1;    //진행중인 퀘스트로 변경
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
                    Initialize();
                    touch_bg.gameObject.SetActive(true);
                    quest_bg.gameObject.SetActive(true);
                    bubble_parent.SetActive(true);

                    quest_Data.tutorial_quest_list[1].state = -1;    //퀘스트 목록에서 삭제
                    quest_Data.tutorial_quest_list[2].state = 1;    //진행중인 퀘스트로 변경
                    GameObject.Find("quest_manager").GetComponent<quest_manager>().quest_contents_update(); //실시간 반영

                    step = 1; quest_num = 3;
                    bubble_parent.transform.position = new Vector3(640, 450, 0); // 화면 상단 위치
                    bubble_text.text = "그렇지~ 자원은 그렇게 양식하는 거란다.. 상점도 한번 둘러보겠니?";
                    //PlayerPrefs.SetInt("storeNew", 1);

                    if(PlayerPrefs.GetInt("storeNew",1)==0) quest_Data.tutorial_quest_list[2].state = -1;

                    item_num = 0;
                    PlayerPrefs.SetInt("isQuest", 5);
                }
                break;
        }
    }

    //quest_ui open
    public void quest_ui_open()
    {
        icon_click.PlayOneShot(icon_click.clip);
        GameObject.Find("quest_manager").GetComponent<quest_manager>().quest_contents_update(); //실시간 반영
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
        while (text_box.IsActive())
        {
            next_triangle.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            next_triangle.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.3f);
        }
    }
}
