using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class new_quest : MonoBehaviour
{
    //farm,sea 공통 오브젝트
    public Image touch_bg,quest_bg, text_window, next_triangle;
    public Text text, hilight_text;
    public static int step, quest_num;

    //farm object
    public GameObject hilight_parent, bubble_parent;
    public Image[] hilight;
    public Image debtor, daddy , speech_bubble;
    public Text bubble_text;
    public Image sea_icon_fake;

    //sea object
    public Image daddy_big; // 아빠 상반신 이미지
    
    public int IsQuest, item_num=0;     //퀘스트 중인지 아닌지

    public void Initialize()
    {
        touch_bg.gameObject.SetActive(false);
        quest_bg.gameObject.SetActive(false);
        debtor.gameObject.SetActive(false);
        text_window.gameObject.SetActive(false);
        next_triangle.gameObject.SetActive(false);
        hilight_text.gameObject.SetActive(false);
        hilight_parent.SetActive(false);
        bubble_parent.gameObject.SetActive(false);
        sea_icon_fake.gameObject.SetActive(false);
    }

    public void sea_initialize() {
        touch_bg.gameObject.SetActive(false);
        quest_bg.gameObject.SetActive(false);
        daddy_big.gameObject.SetActive(false);
        text_window.gameObject.SetActive(false);
        next_triangle.gameObject.SetActive(false);
    }

    // 빚쟁이의 첫번째 빚재촉
    public void Debtor1()
    {
        touch_bg.gameObject.SetActive(true);
        quest_bg.gameObject.SetActive(true);
        debtor.gameObject.SetActive(true);
        text_window.gameObject.SetActive(true);
        next_triangle.gameObject.SetActive(true);
        hilight_text.gameObject.SetActive(false);
        bubble_parent.gameObject.SetActive(false);
        step = 1; quest_num = 1;

        text.text = "너가 대신 아버지빚을 갚겠다고? \n마음은 기특하지만,\n과연 너가 돈을 갚을 수 있을진 의심이 되는군";
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
                switch (step) {
                    case 2:

                        break;
                }
                break;
        }
    }

    public void sea_open()
    {
        IsQuest = PlayerPrefs.GetInt("isQuest", 1);
        SceneManager.LoadScene("sea"); // 바다로 이동
        Initialize();
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
        step = 1; quest_num = 2;

        bubble_parent.transform.position = new Vector3(640, 540, 0); // 화면 상단 위치
        bubble_text.text = "해녀야... 아빠 때문에 네가 고생하는 것 같아 마음이 편하지가 않구나..\n아직 양식장 구성에 대해 잘 모를테니 몇가지 설명을 해주마..";
    }

    public void Start()
    {
        StartCoroutine("triangle_effect");
    }

    public void Update() {
        for (int i = 0; i < 9; i++) {
            item_num+=Haenyeo.sea_item_number[i];
        }

        if (IsQuest == 1 && item_num == 1) {
            sea_initialize();
            quest_bg.gameObject.SetActive(true);
            daddy_big.gameObject.SetActive(true);
            text_window.gameObject.SetActive(true);
            next_triangle.gameObject.SetActive(true);

            quest_num = 3; step = 1;
            text.text = "제법이구나 역시 우리 딸이야..! \n이런식으로 자원을 채집하는 거란다..\n자원 하나만 더 채집해 보겠니 ?";
        }
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
                                                                                                 