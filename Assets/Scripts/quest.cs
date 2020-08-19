using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 양식장 처음 들어가면 퀘스트 아이콘만 클릭 가능하게, 나머지는 회색 씌우기

// 보상 이펙트..

public class quest : MonoBehaviour
{
    public static int quest_number = 0, quest_state;
    public Image[] quest_ui;
    public Image quest_new, quest_ing, quest_done, reward_on, reward_off;
    public Text quest1_ing, quest2_ing, quest3_ing, quest5_ing_suit, quest5_ing_goggle, quest5_ing_flipper, quest4_ing, quest6_ing_suit, quest6_ing_goggle, quest6_ing_flipper;
    public GameObject quest_parent, quest_last;
    public Image reward_effect_ui;

    public AudioSource bgm, quest_icon, reward_click;

    void Awake()
    {
        check_quest();
        
    }

    void Start()
    {
        check_quest();
    }
    void Update()
    {

        check_quest();
        if (quest_number < 6)
        {
            switch (quest_state)
            {
                case 0:
                    quest_new.gameObject.SetActive(true);
                    quest_ing.gameObject.SetActive(false);
                    quest_done.gameObject.SetActive(false);
                    break;
                case 1:
                    quest_new.gameObject.SetActive(false);
                    quest_ing.gameObject.SetActive(true);
                    quest_done.gameObject.SetActive(false);
                    break;
                case 2:
                    quest_new.gameObject.SetActive(false);
                    quest_ing.gameObject.SetActive(false);
                    quest_done.gameObject.SetActive(true);
                    break;

            }
        }

    }



    //퀘스트 완료 됐는지 매번 확인하는 함수
    public void check_quest()
    {
        switch (quest_number)       //내가 현재 진행중인 퀘스트 번호
        {
            case 0:
                if (Haenyeo.sea_item_number[0] >= 1) //조개 1개 모아오기 성공했을 때
                {
                    quest1_ing.text = "1";
                    quest_state = (int)Quest_state.DONE;
                    reward_on.gameObject.SetActive(true);   //보상받기 버튼 활성화
                    reward_off.gameObject.SetActive(false);

                }
                else
                {
                    quest1_ing.text = Haenyeo.sea_item_number[0].ToString();
                    quest_state = (int)Quest_state.ING;
                    reward_on.gameObject.SetActive(false);
                    reward_off.gameObject.SetActive(true);  //보상받기 버튼 비활성화

                }
                break;

            case 1: // 조개 한 번 수확하기
                
                if (farm_manager.quest_test2 == true)
                {
                    quest2_ing.text = "1";
                    quest_state = (int)Quest_state.DONE;
                    reward_on.gameObject.SetActive(true);   //보상받기 버튼 활성화
                    reward_off.gameObject.SetActive(false);

                }
                else
                {
                    quest2_ing.text = "0";
                    quest_state = (int)Quest_state.ING;
                    reward_on.gameObject.SetActive(false);
                    reward_off.gameObject.SetActive(true);  //보상받기 버튼 비활성화
                }

                break;

            case 2: // 송금하기
                
                if (farm_manager.quest_test3 == true)
                {
                    quest3_ing.text = "1";
                    quest_state = (int)Quest_state.DONE;
                    reward_on.gameObject.SetActive(true);   //보상받기 버튼 활성화
                    reward_off.gameObject.SetActive(false);

                }
                else
                {
                    quest3_ing.text = "0";
                    quest_state = (int)Quest_state.ING;
                    reward_on.gameObject.SetActive(false);
                    reward_off.gameObject.SetActive(true);  //보상받기 버튼 비활성화
                    
                }
                break;
			case 3: // 양식장 확장

                if (farm_manager.quest_test4 == true)
                {
                    quest4_ing.text = "1";
                    quest_state = (int)Quest_state.DONE;
                    reward_on.gameObject.SetActive(true);   //보상받기 버튼 활성화
                    reward_off.gameObject.SetActive(false);
                }
                else
                {
                    quest4_ing.text = "0";
                    quest_state = (int)Quest_state.ING;
                    reward_on.gameObject.SetActive(false);   //보상받기 버튼 비활성화
                    reward_off.gameObject.SetActive(true);
                    
                }
                break;
            case 4: // 장비 업그레이드

                if (equipment_upgrade.my_suit == 1 && equipment_upgrade.my_goggle == 1 && equipment_upgrade.my_flipper == 1)
                {
                    quest5_ing_suit.text = "1";
                    quest5_ing_goggle.text = "1";
                    quest5_ing_flipper.text = "1";
                    quest_state = (int)Quest_state.DONE;
                    reward_on.gameObject.SetActive(true);   //보상받기 버튼 활성화
                    reward_off.gameObject.SetActive(false);
                }
                else
                {
                    quest5_ing_suit.text = "0";
                    quest5_ing_goggle.text = "0";
                    quest5_ing_flipper.text = "0";
                    if (equipment_upgrade.my_suit == 1)
                    {
                        quest5_ing_suit.text = "1";
                    }
                    if (equipment_upgrade.my_goggle == 1)
                    {
                        quest5_ing_goggle.text = "1";
                    }
                    if (equipment_upgrade.my_flipper == 1)
                    {
                        quest5_ing_flipper.text = "1";
                    }
                    quest_state = (int)Quest_state.ING;
                    reward_on.gameObject.SetActive(false);
                    reward_off.gameObject.SetActive(true);  //보상받기 버튼 비활성화
                }
                break;

            

            case 5: // 장비 업그레이드 

                if (equipment_upgrade.my_suit == 2 && equipment_upgrade.my_goggle == 2 && equipment_upgrade.my_flipper == 2)
                {
                    quest6_ing_suit.text = "1";
                    quest6_ing_goggle.text = "1";
                    quest6_ing_flipper.text = "1";
                    quest_state = (int)Quest_state.DONE;
                    reward_on.gameObject.SetActive(true);   //보상받기 버튼 활성화
                    reward_off.gameObject.SetActive(false);

                }
                else
                {
                    quest_state = (int)Quest_state.ING;
                    quest6_ing_suit.text = "0";
                    quest6_ing_goggle.text = "0";
                    quest6_ing_flipper.text = "0";
                    if (equipment_upgrade.my_suit == 2)
                    {
                        quest6_ing_suit.text = "1";
                    }
                    if (equipment_upgrade.my_goggle == 2)
                    {
                        quest6_ing_goggle.text = "1";
                    }
                    if (equipment_upgrade.my_flipper == 2)
                    {
                        quest6_ing_flipper.text = "1";
                    }

                    reward_on.gameObject.SetActive(false);
                    reward_off.gameObject.SetActive(true);  //보상받기 버튼 비활성화
                    
                }
                break;

            case 6:
                quest_new.gameObject.SetActive(false);
                quest_ing.gameObject.SetActive(false);
                quest_done.gameObject.SetActive(false);
                break;
        }
    }

    public void exit()
    {
        quest_last.gameObject.SetActive(false);
    }

    //보상받기 버튼 누르면 실행하는 함수
    public void next_quest()
    {
        reward_on.gameObject.SetActive(false);
        reward_off.gameObject.SetActive(true);
        reward_click.PlayOneShot(reward_click.clip);
        StartCoroutine(reward_effect());
        switch (quest_number)       //내 현재 진행중인 번호에 따른 보상
        {
            case 0:
                Haenyeo.money += 10000;
                break;
            case 1:
                Haenyeo.money += 10000;
                break;
            case 2:
                Haenyeo.money += 10000;
                break;
            case 3:
                Haenyeo.money += 10000;
                break;
            case 4:
                Haenyeo.level = 2;
                Haenyeo.diving_time = 70;
                PlayerPrefs.SetInt("Haenyeo_diving_time", Haenyeo.diving_time);
                break;
                
            case 5:
                Haenyeo.level = 3;
                Haenyeo.diving_time = 80;
                PlayerPrefs.SetInt("Haenyeo_diving_time", Haenyeo.diving_time);
                quest_last.gameObject.SetActive(true);
                break;
        }
        quest_state = (int)Quest_state.NEW;
        StartCoroutine(BlinkQuest());
        quest_parent.SetActive(false);
        quest_number++;     //다음 퀘스트 번호로 넘어감
        quest_ui_open();


    }

    //퀘스트 아이콘 클릭했을 때 나오는 함수
    public void quest_ui_open()
    {
        quest_parent.SetActive(true);
        if (quest_state == 0)
        {
            quest_state = (int)Quest_state.ING;
        }
        switch (quest_number)
        {
            case 0:
                quest_ui[0].gameObject.SetActive(true);
                break;
            case 1:
                quest_ui[1].gameObject.SetActive(true);
                break;
            case 2:
                quest_ui[2].gameObject.SetActive(true);
                break;
            case 3:
                quest_ui[3].gameObject.SetActive(true);
                break;
            case 4:
                quest_ui[4].gameObject.SetActive(true);
                break;
            case 5:
                quest_ui[5].gameObject.SetActive(true);
                break;
            case 6:
                break;
        }
    }

    public void no_icon()
    {
        quest_new.gameObject.SetActive(false);
        quest_ing.gameObject.SetActive(false);
        quest_done.gameObject.SetActive(false);
    }

    // 퀘스트 UI에서 나머지 누르면 나가기
    public void quest_ui_close()
    {
        quest_ui[quest_number].gameObject.SetActive(false);
    }


    public enum Quest_state
    {
        NEW = 0,
        ING = 1,
        DONE = 2

    }

    IEnumerator reward_effect()
    {
        reward_effect_ui.gameObject.SetActive(true);
        reward_effect_ui.color = new Vector4(1, 1, 1, 1);
        for (int i = 0; i < 30; i++)
        {
            reward_effect_ui.rectTransform.localPosition = new Vector3(290, -100 + i, 0);
            yield return new WaitForSeconds(0.0005f);
        }
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(FadeOut(reward_effect_ui));
    }


    IEnumerator FadeOut(Image image, Image image2 = null, Image image3 = null, float sec = 0)
    {
        for (float i = 1f; i >= 0; i -= 0.1f)
        {
            Color color = new Vector4(1, 1, 1, i);
            image.color = color;
            yield return new WaitForSeconds(sec);
        }
        image.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        image2.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.05f);
        image3.gameObject.SetActive(true);
    }

    IEnumerator BlinkQuest()
    {
        int count = 0;
        while (count < 3)
        {
            quest_new.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            quest_new.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            count++;
        }
    }


}


