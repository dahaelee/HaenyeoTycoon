using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class new_quest : MonoBehaviour
{
    //퀘스트 ui 관련 오브젝트
    public GameObject quest_ui;
    public GameObject content_parent,content;
    public Text todo_text, type_text;
    public Image quest_ing, quest_done;
    public Button reward_button,cancle_button;

    //공통 오브젝트
    public Image touch_bg,quest_bg, text_window, next_triangle;
    public Text text, hilight_text;
    public static int step, quest_num;
    public static int IsQuest, item_num = 0;     //퀘스트 확인용

    //farm object
    public GameObject hilight_parent, bubble_parent;
    public Image[] hilight;
    public Image sache, daddy , speech_bubble;
    public Text bubble_text;
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
                switch (step)
                {
                    case 2:
                        quest_bg.gameObject.SetActive(false);
                        hilight[0].gameObject.SetActive(true);

                        bubble_parent.transform.position = new Vector3(640, 180, 0); // 화면 하단 위치
                        bubble_text.text = "이건 너의 소지금을 나타낸단다...";
                        break;
                    case 3:
                        hilight[0].gameObject.SetActive(false);
                        hilight[1].gameObject.SetActive(true);

                        bubble_text.text = "이건.. 한 달동안 갚아야할 돈의 액수란다..";
                        break;
                    case 4:
                        hilight[1].gameObject.SetActive(false);
                        hilight[2].gameObject.SetActive(true);

                        bubble_text.text = "이건 너의 체력을 나타내.. 체력이 0이되면, 하루를 마무리 짓게 된단다.";
                        break;
                    case 5:
                        hilight[2].gameObject.SetActive(false);
                        hilight[3].gameObject.SetActive(true);

                        bubble_text.text = "이건 남은 기한을 나타낸단다..";
                        break;
                    case 6:
                        hilight[3].gameObject.SetActive(false);
                        hilight[4].gameObject.SetActive(true);

                        bubble_parent.transform.position = new Vector3(640, 540, 0);
                        bubble_text.text = "이걸 누르면, 해야 할일의 목록과 내용을 확인할 수 있단다..";
                        break;
                    case 7:
                        hilight[4].gameObject.SetActive(false);
                        hilight[5].gameObject.SetActive(true);

                        bubble_text.text = "상점 아이콘을 누르면, 상점으로 갈 수 있단다..";
                        break;
                    case 8:
                        hilight[5].gameObject.SetActive(false);
                        hilight[6].gameObject.SetActive(true);

                        bubble_text.text = "바다 아이콘을 누르면, 바다로 갈 수 있단다..";
                        break;
                    case 9:
                        sea_icon_fake.gameObject.SetActive(true);
                        
                        bubble_text.text = "이 참에 바다에서 물질하는 법도 알려주마… 바다 아이콘을 눌러보렴";
                        break;
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
        touch_bg.gameObject.SetActive(true);
        quest_bg.gameObject.SetActive(true);
        sache.gameObject.SetActive(true);
        text_window.gameObject.SetActive(true);
        next_triangle.gameObject.SetActive(true);
        hilight_text.gameObject.SetActive(false);
        bubble_parent.gameObject.SetActive(false);

        quest_content_generate(1, "사채업자의 빚재촉", "5일동안 20만원 갚기");
        step = 1; quest_num = 1;
        text.text = "너가 대신 아버지빚을 갚겠다고? \n마음은 기특하지만,\n과연 너가 돈을 갚을 수 있을진 의심이 되는군";
    }

    //아빠 1 - 게임 ui 설명
    public void Daddy1()
    {
        touch_bg.gameObject.SetActive(true);
        quest_bg.gameObject.SetActive(true);
        bubble_parent.SetActive(true);
        hilight_parent.SetActive(true);
        for (int i = 0;  i < hilight.Length; i++)
        {
            hilight[i].gameObject.SetActive(false);
        }

        quest_content_generate(2, "아빠의 가르침", "바다에서 자원 1개 이상 채집하기");
        step = 1; quest_num = 2;
        bubble_parent.transform.position = new Vector3(640, 540, 0); // 화면 상단 위치
        bubble_text.text = "아빠 때문에 네가 고생하는 것 같아 마음이 편하지가 않구나..\n아직 양식장 구성에 대해 잘 모를테니 몇가지 설명을 해주마..";
    }

    //fake_sea_icon 눌렀을 때 함수
    public void sea_open()
    {
        Initialize();
        PlayerPrefs.SetInt("isQuest", 3);
        SceneManager.LoadScene("sea"); // 바다로 이동
        hilight[6].gameObject.SetActive(false);
    }


    public void Update()
    {
        IsQuest = PlayerPrefs.GetInt("isQuest", 1);
        Check_quest(IsQuest);     // quest 완료 검사
    }

    // 퀘스트 완료 확인
    public void Check_quest(int IsQuest) {
        switch (IsQuest) {
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

                    Destroy(GameObject.FindWithTag("quest2"));   //quest2 삭제
                    quest_content_generate(3, "아빠의 가르침", "자원 양식하기");

                    step = 1; quest_num = 3;
                    bubble_parent.transform.position = new Vector3(640, 540, 0); // 화면 상단 위치
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

                    Destroy(GameObject.FindWithTag("quest3"));   //quest3 삭제
                    quest_content_generate(4, "아빠의 가르침", "상인 아저씨와 대화하기");

                    step = 1; quest_num = 3;
                    bubble_parent.transform.position = new Vector3(640, 540, 0); // 화면 상단 위치
                    bubble_text.text = "그렇지~ 자원은 그렇게 양식하는 거란다.. 상인 아저씨가 찾던데! 어서 가보렴";

                    item_num = 0;
                    PlayerPrefs.SetInt("isQuest", 4);
                }
                break;
        }
    }

    public void quest_content_generate(int quest_num,string type, string todo) {
        //quest content 설정
        quest_ing.gameObject.SetActive(true);
        quest_done.gameObject.SetActive(false);
        type_text.text = type;  todo_text.text = todo;
        content.tag = "quest"+quest_num.ToString();

        GameObject temp_content = Instantiate(content, new Vector3(0, 0, 0), Quaternion.identity);
        temp_content.transform.SetParent(content_parent.transform);
        temp_content.SetActive(true);
    }

    //quest_ui open
    public void quest_ui_open() {
        quest_bg.gameObject.SetActive(true);
        quest_ui.gameObject.SetActive(true);
    }

    public void quest_ui_close()
    {
        quest_bg.gameObject.SetActive(false);
        quest_ui.gameObject.SetActive(false);
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
                                                                                                 