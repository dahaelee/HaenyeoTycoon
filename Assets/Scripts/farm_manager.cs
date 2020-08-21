using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Diagnostics;

public class farm_manager : MonoBehaviour
{
    public bool isTest = false;
    public farm[] farms;
    public GameObject[] farmable_items;
    public static sea_item[] sea_item;
    public sea_item starfish, seaweed, shell, shrimp, jellyfish, crab, octopus, abalone, turtle;
    public Text name_info, time_info, day, money, debt, debt_repay, money_repay, sending_amount_repay;
    public Image farm_info, farmable_item_ui, farming_start_bttn, UI_background, trash_ui, expand_ui, repay_ui, go_sea_ui, send_money_warning, no_money_ui, ask_quit_ui, setting_ui, restart_ui, ending, no_item;
    public int chosen_item, chosen_farm, trash_farm_num, activation_cost, sending_int, sending_limit, limit_day;
    public bool is_chosen, is_infoUI_On, is_farmableUI_On, is_trashUI_On, is_expandUI_On, is_repayUI_On, is_goseaUI_On, is_warning_Effect_On;
    public static bool is_sea_locked, is_repay_locked;
    public string sending_str = "";
    public Slider bgm_volume, effect_volume;
    public Text[] item_count;
    //사운드 관련 필드
    public AudioSource bgm, button_click, popup_click, expand_click, request_denied, get_money, trashing, next_day, debt_sending, num_pad, icon_click, item_click, farm_money;

    IEnumerator current_Info;


    public enum farms_index
    {
        farm0 = 0,
        farm1,
        farm2,
        farm3,
        farm4,
        farm5,
        farm6
    }

    void OnEnable()
    {
        sea_item = new sea_item[9];
        sea_item[0] = shell;
        sea_item[1] = seaweed;
        sea_item[2] = starfish;
        sea_item[3] = shrimp;
        sea_item[4] = jellyfish;
        sea_item[5] = crab;
        sea_item[6] = octopus;
        sea_item[7] = abalone;
        sea_item[8] = turtle;
        data_load();
    }
    void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
<<<<<<< HEAD
=======


        int isNew = PlayerPrefs.GetInt("isNew", 1);     //새로운거인지 확인
        if (isNew == 1)     //첫 시작이면 퀘스트 뜨게 함
        {
            //효민 - 빚쟁이 텍스트
            GameObject.Find("new_quest").GetComponent<new_quest>().Sache1();

            //tutorial_parent.SetActive(true);
            //for (int k = 0; k < tutorials.Length; k++)
            //{
            //    tutorials[k].gameObject.SetActive(false);
            //}
            //tutorials[0].gameObject.SetActive(true);

            PlayerPrefs.SetInt("isNew", 0);
        }
        else
        {
            tutorial_parent.SetActive(false);
        }



>>>>>>> 75aeed78bf612a3a5a6ff187450ec2f343706c8e
        activation_cost = 50000;        //양식장 확장 비용
        limit_day = 20;
        sending_int = 0;
        sending_str = "0";
        sending_limit = 100000;
        sending_amount_repay.GetComponent<Text>().text = sending_str;
        day.GetComponent<Text>().text = Haenyeo.day.ToString();
        money.GetComponent<Text>().text = Haenyeo.money.ToString();
        debt.GetComponent<Text>().text = Haenyeo.debt.ToString();


        farm_info.gameObject.SetActive(false);
        farmable_item_ui.gameObject.SetActive(false);
        UI_background.gameObject.SetActive(false);
        trash_ui.gameObject.SetActive(false);
        expand_ui.gameObject.SetActive(false);
        repay_ui.gameObject.SetActive(false);
        go_sea_ui.gameObject.SetActive(false);
        send_money_warning.gameObject.SetActive(false);
        no_money_ui.gameObject.SetActive(false);
        ask_quit_ui.gameObject.SetActive(false);
        setting_ui.gameObject.SetActive(false);
        ending.gameObject.SetActive(false);
        no_item.gameObject.SetActive(false);

    }
    void Update()
    {
        day.GetComponent<Text>().text = "D-" + (limit_day - Haenyeo.day + 1).ToString();
        money.GetComponent<Text>().text = string.Format("{0:#,###0}", Haenyeo.money);
        debt.GetComponent<Text>().text = string.Format("{0:#,###0}", Haenyeo.debt);
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            StartCoroutine(UI_On(ask_quit_ui));
        }

        effect_sound_ctrl();
        bgm_sound_ctrl();


    }

    //수확하기 버튼 눌렀을 때 실행

    public void farming_money(int index)    //양식장 번호 파라미터
    {
        get_money.PlayOneShot(get_money.clip);  //수확할 때 사운드
        //수확됨
    }


    IEnumerator FadeOut(Image image, Image image2 = null, Image image3 = null, float sec = 0) //페이드 아웃 되듯이 사라지는 이펙트 함수
    {
        for (float i = 1f; i >= 0; i -= 0.1f)
        {
            Color color = new Vector4(1, 1, 1, i);
            image.color = color;
            yield return new WaitForSeconds(sec);
        }
        image.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
<<<<<<< HEAD
        image2.gameObject.SetActive(false);
=======
        //Debug.Log("하이 헬로");
        image2.gameObject.SetActive(false);
        //Debug.Log("하이 헬로2");
>>>>>>> 75aeed78bf612a3a5a6ff187450ec2f343706c8e
        yield return new WaitForSeconds(0.05f);
        image3.gameObject.SetActive(true);
    }



    //양식할 자원 선택
    public void choose_item(int index)
    {
        item_click.PlayOneShot(item_click.clip);
        for (int i = 0; i < 9; i++)
        {
            farmable_items[i].GetComponent<sea_item>().chosen_mark.gameObject.SetActive(false);//체크 마크 지움

        }
        farmable_items[index].GetComponent<sea_item>().chosen_mark.gameObject.SetActive(true);  //선택된 자원만 체크마크 표시

        chosen_item = index;    //선택된 자원 저장

    }

    //선택된 자원으로 양식 시작
    public void start_farming()
    {
        int farm_index = chosen_farm;
        int item_index = chosen_item;
        if (Haenyeo.sea_item_number[item_index] > 0)    //1개 이상만 양식 가능
        {

            button_click.PlayOneShot(button_click.clip);
            UI_off();

            farms[farm_index].isFarming = true;    //양식 시작한걸로 하기
            Haenyeo.sea_item_number[item_index] -= 1;   //자원 1개 차감 
            farms[farm_index].plus.gameObject.SetActive(false); //플러스 표시 빼기
            farmable_item_ui.gameObject.SetActive(false);   //양식 가능한 자원 보여주는 창 닫기

            farms[farm_index].item = sea_item[item_index];    //양식장에 자원 넣기
            farms[farm_index].item1.sprite = Resources.Load<Sprite>(sea_item[item_index].name);   //자원 이미지 바꾸기
            farms[farm_index].item2.sprite = Resources.Load<Sprite>(sea_item[item_index].name);   //자원 이미지 바꾸기
            farms[farm_index].item3.sprite = Resources.Load<Sprite>(sea_item[item_index].name);   //자원 이미지 바꾸기

            farms[farm_index].item1.gameObject.SetActive(true);
            farms[farm_index].item2.gameObject.SetActive(false);
            farms[farm_index].item3.gameObject.SetActive(false);

            farms[farm_index].item_generating = Wait_generating(farms[farm_index]);
            StartCoroutine(farms[farm_index].item_generating);

<<<<<<< HEAD
=======
            /*
            if (farms[farm_index].remaining_time < haenyeo.sea_item[item_index].farm_time / 3*2) //시간이 3분의 1 지났을 경우
            {
                farms[farm_index].item2.gameObject.SetActive(true);
            }
            if (farms[farm_index].remaining_time < haenyeo.sea_item[item_index].farm_time /3) //시간이 3분의 2 지났을 경우
            {
                farms[farm_index].item3.gameObject.SetActive(true);
            }
            */

        }
        else
        {   //3개 미만인 경우 양식 안함
            //Debug.Log("양식 못해열");
>>>>>>> 75aeed78bf612a3a5a6ff187450ec2f343706c8e
        }
    }


    //양식 가능한 자원 보여주기
    public void show_farmable_item(int farm_index)
    {

        popup_click.PlayOneShot(popup_click.clip);      //팝업창 사운드

        for (int k = 0; k < item_count.Length; k++)            //자원 개수 보여주기
        {
            item_count[k].text = Haenyeo.sea_item_number[k].ToString();
        }
        int count = 0;
        for (int j = 0; j < 9; j++)
        {
            farmable_items[j].SetActive(false);
        }
        for (int i = 0; i < 9; i++)
        {
            if (Haenyeo.sea_item_number[i] > 0)    //1개 이상인 자원만 보여주기
            {
                count++;
            }
        }
        if (count < 1)
        {
            request_denied.PlayOneShot(request_denied.clip);        //요청 거절 사운드
            StartCoroutine(warning_UI_effect(no_item));
        }
        else
        {

            if (!is_farmableUI_On)
            {
                UI_off();
                is_farmableUI_On = true;
                StartCoroutine(UI_On(farmable_item_ui));    //양식 가능한 자원 보여주는 ui창 띄우기
            }
            for (int i = 0; i < 9; i++)
            {

                if (Haenyeo.sea_item_number[i] > 0)    //5개 이상인 자원만 보여주기
                {
                    count++;
                    farmable_items[i].SetActive(true);
                    if (!is_chosen)
                    {
                        is_chosen = true;
                        choose_item(i);     //맨 처음꺼 선택된걸로 두기
                    }
                }
                else
                {
                    farmable_items[i].SetActive(false);
                }
            }
            if (!is_chosen)//자원이 5개 이상이 없으면 양식 불가
            {
                farming_start_bttn.gameObject.SetActive(false);
            }
            chosen_farm = farm_index;   //양식장 번호 저장
        }


    }

    public void UI_off()
    {
        if (is_infoUI_On)
        {
            is_infoUI_On = false;
            farm_info.gameObject.SetActive(false);
            StopCoroutine(current_Info);
        }
        if (is_farmableUI_On)
        {
            is_farmableUI_On = false;
            farmable_item_ui.gameObject.SetActive(false);
        }
        if (is_trashUI_On)
        {
            is_trashUI_On = false;
            trash_ui.gameObject.SetActive(false);
        }
        if (is_expandUI_On)
        {
            is_expandUI_On = false;
            expand_ui.gameObject.SetActive(false);
        }
        repay_ui.gameObject.SetActive(false);

        if (is_goseaUI_On)
        {
            is_goseaUI_On = false;
            go_sea_ui.gameObject.SetActive(false);
        }
        is_chosen = false;
        send_money_warning.gameObject.SetActive(false);
        no_money_ui.gameObject.SetActive(false);
        ask_quit_ui.gameObject.SetActive(false);
        setting_ui.gameObject.SetActive(false);
        restart_ui.gameObject.SetActive(false);
        no_item.gameObject.SetActive(false);
        UI_background.gameObject.SetActive(false);

    }

    //양식 정보 보여주기
    public void farming_info(int index) //양식장 index 파라미터
    {
        popup_click.PlayOneShot(popup_click.clip);      //팝업창 사운드
        trash_farm_num = index;
        if (farms[index].isFarming)
        {
            switch (farms[index].item.number)
            {
                case 0:
                    name_info.text = "조개 양식장";
                    break;
                case 1:
                    name_info.text = "미역 양식장";
                    break;
                case 2:
                    name_info.text = "불가사리 양식장";
                    break;
                case 3:
                    name_info.text = "새우 양식장";
                    break;
                case 4:
                    name_info.text = "해파리 양식장";
                    break;
                case 5:
                    name_info.text = "꽃게 양식장";
                    break;
                case 6:
                    name_info.text = "문어 양식장";
                    break;
                case 7:
                    name_info.text = "전복 양식장";
                    break;
                case 8:
                    name_info.text = "거북이 양식장";
                    break;

            }

            //양식정보 UI 창 띄우기
            if (!is_infoUI_On)
            {
                UI_off();
                is_infoUI_On = true;
                StartCoroutine(UI_On(farm_info));

            }

            //생성까지 남은 시간 보여주기
            current_Info = Farm_remaining_time(index);
            StartCoroutine(current_Info);
        }

    }

    //생성까지 시간 차감하며 기다리기
    IEnumerator Wait_generating(farm farm)
    {
        farm.isFarming = true;
        farm.farm_opportunity--;
        farm.remaining_time = farm.item.farm_time;
        farm.item1.gameObject.SetActive(true);
        farm.item2.gameObject.SetActive(false);
        farm.item3.gameObject.SetActive(false);
        if (farm.item.name == "turtle")
        {
            farm.item1.rectTransform.localScale = new Vector3((float)1.2, (float)0.8, (float)0.8);
            farm.item2.rectTransform.localScale = new Vector3((float)1.2, 1, 1);
            farm.item3.rectTransform.localScale = new Vector3((float)1.2, 1, 1);
        }
        else
        {
            farm.item1.rectTransform.localScale = new Vector3(1, 1, 1);
            farm.item2.rectTransform.localScale = new Vector3(1, 1, 1);
            farm.item3.rectTransform.localScale = new Vector3(1, 1, 1);
        }
        StartCoroutine(item_effect(farm.item1));

        bool item2_effect_On = false;
        bool item3_effect_On = false;
        while (farm.remaining_time > 0)
        {
            if ((farm.remaining_time < (farm.item.farm_time / 3) * 2) && (farm.remaining_time >= farm.item.farm_time / 3))
            {
                farm.item2.gameObject.SetActive(true);
                if (!item2_effect_On)
                {
                    StartCoroutine(item_effect(farm.item2));        //움찔움찔 이펙트
                    item2_effect_On = true;
                }
            }
            if (farm.remaining_time < farm.item.farm_time / 3)
            {
                farm.item3.gameObject.SetActive(true);
                if (!item3_effect_On)
                {
                    StartCoroutine(item_effect(farm.item3));        //움찔움찔 이펙트
                    item3_effect_On = true;
                }
            }
            yield return new WaitForSeconds(1);


            farm.remaining_time--;
        }
        farm.isFarming = false;
        farm.money.gameObject.SetActive(true);


    }


    //양식장 비우기 확인 UI창 띄우기
    public void Ask_trash()
    {
        button_click.PlayOneShot(button_click.clip);    //버튼클릭 사운드
        if (!is_trashUI_On)
        {
            UI_off();
            is_trashUI_On = true;
            StartCoroutine(UI_On(trash_ui));
        }
    }

    //양식장 비우기
    public void trash_farm()
    {
        trashing.PlayOneShot(trashing.clip);
        UI_off();
        farms[trash_farm_num].item = null;
        farms[trash_farm_num].isFarming = false;         //양식중 아님으로 바꾸기
        farms[trash_farm_num].farm_opportunity = 5;          //양식 횟수 초기화
        farms[trash_farm_num].item1.gameObject.SetActive(false);     //자원들 다 안보이게 하기
        farms[trash_farm_num].item2.gameObject.SetActive(false);
        farms[trash_farm_num].item3.gameObject.SetActive(false);
        farms[trash_farm_num].money.gameObject.SetActive(false);
        farms[trash_farm_num].plus.gameObject.SetActive(true);       //양식하기 플러스 아이콘 보이게하기
        StopCoroutine(farms[trash_farm_num].item_generating);        //자원 생성중인 코루틴 함수 중단


    }

    //양식장 확장하기
    public void Ask_expand_farm(int index)
    {

        if (!is_expandUI_On)
        {
            popup_click.PlayOneShot(popup_click.clip);
            UI_off();
            is_expandUI_On = true;
            chosen_farm = index;
            StartCoroutine(UI_On(expand_ui));
        }
    }

    //양식장 활성화
    public void Activate_farm()
    {
        if (Haenyeo.money >= activation_cost)
        {
            int index = chosen_farm;
            expand_click.PlayOneShot(expand_click.clip);      //확장클릭 사운드
            UI_off();
            farms[index].is_farm_Activated = true;
            Haenyeo.money -= activation_cost;
            StartCoroutine(FadeOut(farms[index].lock_img, farms[index].locked_bg, farms[index].plus, 0.1f));

        }
        else
        {
            //소지금이 부족합니다 UI 띄우기
            request_denied.PlayOneShot(request_denied.clip);        //요청 거절 사운드
            UI_off();
            StartCoroutine(warning_UI_effect(no_money_ui));
        }

    }

    //수확까지 남은 시간 보여주기
    IEnumerator Farm_remaining_time(int index)
    {
        while (is_infoUI_On)
        {
            if ((farms[index].remaining_time / 60) > 0)
            {
                time_info.text = "양식까지 남은 시간 : " + (farms[index].remaining_time / 60).ToString() + "분 " + (farms[index].remaining_time % 60).ToString() + "초";
            }
            else
            {
                time_info.text = "양식까지 남은 시간 : " + (farms[index].remaining_time % 60).ToString() + "초";
            }
            yield return new WaitForSeconds(1);

        }

    }

    //송금하기 UI 띄우기
    public void repay()
    {


        //만약 is_repay_locked 이면 바다 가야한다고 UI 띄우기
        if (is_repay_locked)
        {

            request_denied.PlayOneShot(request_denied.clip);        //요청 거절 사운드

        }
        else
        {
            icon_click.PlayOneShot(icon_click.clip);        //아이콘 클릭시 사운드
            UI_off();
            StartCoroutine(UI_On(repay_ui));
            debt_repay.GetComponent<Text>().text = string.Format("{0:#,###0}", Haenyeo.debt);
            money_repay.GetComponent<Text>().text = string.Format("{0:#,###0}", Haenyeo.money);
            sending_int = 0;
            sending_str = "0";
            sending_amount_repay.GetComponent<Text>().text = sending_str;

        }
    }

    //송금할 때 번호 키패드 각각 누르기
    public void number_pad(string s)        //번호를 string s로 받아옴
    {
        num_pad.PlayOneShot(num_pad.clip);      //키패드 누를 때 사운드
        sending_str += s;
        sending_int = int.Parse(sending_str);
        if (sending_int > Haenyeo.money)
        {

            sending_int = Haenyeo.money;
            sending_str = sending_int.ToString();

        }
        sending_amount_repay.GetComponent<Text>().text = string.Format("{0:#,###0}", sending_int);
    }

    //송금액 초기화 버튼
    public void repay_clear()
    {
        num_pad.PlayOneShot(num_pad.clip);      //키패드 누를 때 사운드
        sending_int = 0;
        sending_str = "";
        sending_amount_repay.GetComponent<Text>().text = "0";
    }

    //숫자패드 백스페이스
    public void backspace()
    {
        num_pad.PlayOneShot(num_pad.clip);      //키패드 누를 때 사운드
        sending_int = sending_int / 10;
        sending_str = sending_int.ToString();
        sending_amount_repay.GetComponent<Text>().text = string.Format("{0:#,###0}", sending_int);
    }

    //송금하기 버튼 누르면 실행되는 함수
    public void send_money()
    {
        UI_off();
        if (sending_int < sending_limit)
        {
            button_click.PlayOneShot(button_click.clip);
            send_money_warning.gameObject.SetActive(true);
            UI_background.gameObject.SetActive(true);

        }
        else
        {
            debt_sending.PlayOneShot(debt_sending.clip);
            next_day.PlayDelayed((float)0.5f);
            Haenyeo.money -= sending_int;
            Haenyeo.debt -= sending_int;
            if (Haenyeo.debt < 0)
            {
                Haenyeo.debt = 0;
            }
            Haenyeo.day++;
            is_repay_locked = true;
            is_sea_locked = false;      //바다 아이콘 활성화
            PlayerPrefs.SetInt("is_repay_locked", 1);
            if (isTest)
            {
                if (Haenyeo.day > 30)
                {
                    StartCoroutine("bad_ending");
                }

                if (Haenyeo.debt < 1)
                {
                    StartCoroutine("happy_ending");
                }
            }
            else
            {
                if (Haenyeo.day > limit_day && Haenyeo.debt > 0)
                {
                    StartCoroutine("bad_ending");
                }

                if (Haenyeo.day > limit_day && Haenyeo.debt < 1)
                {
                    StartCoroutine("happy_ending");

                }

                if (Haenyeo.debt < 1)
                {
                    StartCoroutine("happy_ending");
                }
            }

        }

    }

    //최소송금액보다 부족할 경우에 이자늘어남과 함께 송금되는 함수
    public void repay_interest()
    {
        debt_sending.PlayOneShot(debt_sending.clip);
        Haenyeo.money -= sending_int;
        Haenyeo.day++;
        Haenyeo.debt -= sending_int;
        if (Haenyeo.debt <= 0)
        {
            Haenyeo.debt = 0;
        }
        Haenyeo.debt += (int)(Haenyeo.debt * 0.01);     //이자율 1%
        UI_off();
        is_repay_locked = true;
        is_sea_locked = false;      //바다 아이콘 활성화
        if (isTest)
        {
            if (Haenyeo.day > 5)
            {
                StartCoroutine("bad_ending");
            }

            if (Haenyeo.debt < 1)
            {
                StartCoroutine("happy_ending");
            }
        }
        else
        {
            if (Haenyeo.day > limit_day && Haenyeo.debt > 0)
            {
                StartCoroutine("bad_ending");
            }

            if (Haenyeo.day > limit_day && Haenyeo.debt < 1)
            {
                StartCoroutine("happy_ending");

            }

            if (Haenyeo.debt < 1)
            {
                StartCoroutine("happy_ending");
            }
        }

    }


    //바다 가기 아이콘 눌렀을 때 실행되는 함수
    public void sea_ui()
    {


        if (is_sea_locked)  //잠겨있으면
        {
            //송금을 해야한다고 말하기

            request_denied.PlayOneShot(request_denied.clip);        //요청 거절 사운드

        }
        else
        {
            if (!is_goseaUI_On)
            {
                icon_click.PlayOneShot(icon_click.clip);        //아이콘 클릭시 사운드
                is_goseaUI_On = true;
                StartCoroutine(UI_On(go_sea_ui));   //바다가시겠습니까 물어보는 UI띄움
            }
        }
    }

    //아이콘 비활성화 시에 뜨는 알림UI 이펙트
    IEnumerator warning_UI_effect(Image image)
    {
        is_warning_Effect_On = true;
        image.gameObject.SetActive(true);
        for (int i = 0; i < 10; i++)
        {
            image.rectTransform.localScale = new Vector3((float)(0.7 + i * 0.03), (float)(0.7 + i * 0.03), (float)(0.7 + i * 0.03));
            yield return 0;
        }
        yield return new WaitForSeconds(2f);
        image.gameObject.SetActive(false);
        is_warning_Effect_On = false;
    }

    //UI 창 오픈하는 코드와 이펙트
    IEnumerator UI_On(Image image)
    {
<<<<<<< HEAD
=======
        //Debug.Log("ffff");
>>>>>>> 75aeed78bf612a3a5a6ff187450ec2f343706c8e
        yield return new WaitForSeconds(0.1f);
        UI_background.gameObject.SetActive(true);
        image.gameObject.SetActive(true);
        for (int i = 0; i < 5; i++)
        {
            image.rectTransform.localScale = new Vector3((float)(0.95 + i * 0.01), (float)(0.95 + i * 0.01), (float)(0.95 + i * 0.01));
            yield return 0;
        }

    }

    //자원 생성될 때 째깍 거리는 이펙트
    IEnumerator item_effect(Image image)
    {
        image.gameObject.transform.Rotate(Vector3.back * 20);
        yield return new WaitForSeconds(0.3f);
        image.gameObject.transform.Rotate(Vector3.forward * 20);
        yield return new WaitForSeconds(0.3f);
        image.gameObject.transform.Rotate(Vector3.back * 20);
        yield return new WaitForSeconds(0.3f);
        image.gameObject.transform.Rotate(Vector3.forward * 20);
        yield return new WaitForSeconds(0.3f);
    }

    //게임 설정 창 UI
    public void setting()
    {
        Haenyeo.hp -= 1;
        //Debug.Log(Haenyeo.hp);
        icon_click.PlayOneShot(icon_click.clip);        //아이콘 클릭시 사운드
        setting_ui.gameObject.SetActive(true);
        UI_background.gameObject.SetActive(true);

    }

    public void restart_bttn()
    {
        button_click.PlayOneShot(button_click.clip);
        StartCoroutine(UI_On(restart_ui));
    }

    public void ask_quit()
    {
        button_click.PlayOneShot(button_click.clip);
        StartCoroutine(UI_On(ask_quit_ui));
    }


    //바다로 가기
    public void go_sea()
    {
        data_save();
        SceneManager.LoadScene("sea"); //바다 씬으로 전환

    }

    //상점 가기
    public void go_market()
    {
        data_save();
        icon_click.PlayOneShot(icon_click.clip);        //아이콘 클릭시 사운드
        SceneManager.LoadScene("store");
    }

    //이펙트 볼륨 조절
    public void effect_sound_ctrl()
    {
        popup_click.volume = effect_volume.value;
        expand_click.volume = effect_volume.value;
        request_denied.volume = effect_volume.value;
        get_money.volume = effect_volume.value;
        trashing.volume = effect_volume.value;
        next_day.volume = effect_volume.value;
        debt_sending.volume = effect_volume.value;
        num_pad.volume = effect_volume.value;
        button_click.volume = effect_volume.value;
        icon_click.volume = effect_volume.value;
        item_click.volume = effect_volume.value;
        farm_money.volume = effect_volume.value;
    }
    public void bgm_sound_ctrl()
    {
        bgm.volume = bgm_volume.value;
    }

    public void button_sound()
    {
        button_click.PlayOneShot(button_click.clip);
    }

    //배드 엔딩으로 넘어감
    IEnumerator bad_ending()
    {
        ending.gameObject.SetActive(true);        //중간 장면
        ending.color = new Vector4(1, 1, 1, 0);
        for (float i = 0f; i <= 1; i += 0.1f)
        {
            ending.color = new Vector4(1, 1, 1, i);
            yield return new WaitForSeconds(0.1f);
        }

        SceneManager.LoadScene("ending_bad");
    }

    //해피 엔딩으로 넘어감
    IEnumerator happy_ending()
    {
        ending.gameObject.SetActive(true);        //중간 장면
        ending.color = new Vector4(1, 1, 1, 0);
        for (float i = 0f; i <= 1; i += 0.1f)
        {
            ending.color = new Vector4(1, 1, 1, i);
            yield return new WaitForSeconds(0.2f);
        }
        SceneManager.LoadScene("ending_happy");
    }



    //게임 종료 버튼
    public void game_quit()
    {
        button_click.PlayOneShot(button_click.clip);
        data_save();
        Application.Quit();
    }

    public void farm_reset()
    {
        button_click.PlayOneShot(button_click.clip);
        PlayerPrefs.DeleteAll();
        for (int i = 0; i < farms.Length; i++)
        {
            farms[i].item = null;
            farms[i].isFarming = false;         //양식중 아님으로 바꾸기
            farms[i].farm_opportunity = 5;          //양식 횟수 초기화
            farms[i].item1.gameObject.SetActive(false);     //자원들 다 안보이게 하기
            farms[i].item2.gameObject.SetActive(false);
            farms[i].item3.gameObject.SetActive(false);
            farms[i].plus.gameObject.SetActive(true);       //양식하기 플러스 아이콘 보이게하기
            if (farms[i].item_generating != null)
            {
                StopCoroutine(farms[i].item_generating);        //자원 생성중인 코루틴 함수 중단
            }
        }
        SceneManager.LoadScene("start");
    }


    public void data_save()
    {
        //데이터 저장

        PlayerPrefs.SetInt("isNew", 0);     //새로운게 아니라고 표시

        PlayerPrefs.SetInt("Haenyeo" + "_" + "money", Haenyeo.money);
        PlayerPrefs.SetInt("Haenyeo_debt", Haenyeo.debt);
        PlayerPrefs.SetInt("Haenyeo_diving_time", Haenyeo.diving_time);
        PlayerPrefs.SetInt("Haenyeo_moving_speed", Haenyeo.moving_speed);
        PlayerPrefs.SetInt("Haenyeo_day", Haenyeo.day);
        PlayerPrefs.SetInt("Haenyeo_level", Haenyeo.level);
        PlayerPrefs.SetFloat("Haenyeo_hp", Haenyeo.hp);
        PlayerPrefs.SetString("lasttime", System.DateTime.Now.ToString());

        if (is_repay_locked)
        {
            PlayerPrefs.SetInt("is_repay_locked", 1);
            PlayerPrefs.SetInt("is_sea_locked", 0);
        }
        else
        {
            PlayerPrefs.SetInt("is_repay_locked", 0);
            PlayerPrefs.SetInt("is_sea_locked", 1);
        }
        PlayerPrefs.SetInt("is_sea_locked", 0);
        PlayerPrefs.SetInt("Haenyeo_sea_item_number0", Haenyeo.sea_item_number[0]);
        PlayerPrefs.SetInt("Haenyeo_sea_item_number1", Haenyeo.sea_item_number[1]);
        PlayerPrefs.SetInt("Haenyeo_sea_item_number2", Haenyeo.sea_item_number[2]);
        PlayerPrefs.SetInt("Haenyeo_sea_item_number3", Haenyeo.sea_item_number[3]);
        PlayerPrefs.SetInt("Haenyeo_sea_item_number4", Haenyeo.sea_item_number[4]);
        PlayerPrefs.SetInt("Haenyeo_sea_item_number5", Haenyeo.sea_item_number[5]);
        PlayerPrefs.SetInt("Haenyeo_sea_item_number6", Haenyeo.sea_item_number[6]);
        PlayerPrefs.SetInt("Haenyeo_sea_item_number7", Haenyeo.sea_item_number[7]);
        PlayerPrefs.SetInt("Haenyeo_sea_item_number8", Haenyeo.sea_item_number[8]);
        PlayerPrefs.SetFloat("Bgm_volume", bgm_volume.value);
        PlayerPrefs.SetFloat("Effect_volume", effect_volume.value);

        for (int i = 0; i < farms.Length; i++)
        {
            if (farms[i].item != null)
            {
                PlayerPrefs.SetInt("farm" + i + "_sea_item", farms[i].item.number);
            }
            if (farms[i].is_farm_Activated)
            {
                PlayerPrefs.SetInt("farm" + i + "_is_farm_activated", 1);     //true면 1저장
            }
            else
            {
                if (farms[i].farm_number == 2 || farms[i].farm_number == 3 || farms[i].farm_number == 4 || farms[i].farm_number == 5)
                {
                    PlayerPrefs.SetInt("farm" + i + "_is_farm_activated", 1);
                }
                else
                {
                    PlayerPrefs.SetInt("farm" + i + "_is_farm_activated", 0);
                }
            }

            if (farms[i].isFarming)
            {
                PlayerPrefs.SetInt("farm" + i + "_isFarming", 1);     //true면 1저장
            }
            else
            {
                PlayerPrefs.SetInt("farm" + i + "_isFarming", 0);
            }
            if (farms[i].is_money_on)
            {
                PlayerPrefs.SetInt("farm" + i + "_is_money_on", 1);     //true면 1저장
            }
            else
            {
                PlayerPrefs.SetInt("farm" + i + "_is_money_on", 0);
            }
            PlayerPrefs.SetInt("farm" + i + "_opportunity", farms[i].farm_opportunity);
            PlayerPrefs.SetInt("farm" + i + "_remaining_time", farms[i].remaining_time);
        }
        PlayerPrefs.Save();

    }


    public void data_load()
    {
        if (PlayerPrefs.GetInt("is_repay_locked", 1) == 0)
        {
            is_repay_locked = false;
            is_sea_locked = true;
        }
        else if (PlayerPrefs.GetInt("is_repay_locked", 1) == 1)
        {
            is_repay_locked = true;
            is_sea_locked = false;
        }

        if (isTest)
        {

            Haenyeo.money = PlayerPrefs.GetInt("Haenyeo_money", 50000);
            Haenyeo.debt = PlayerPrefs.GetInt("Haenyeo_debt", 1000000);
            Haenyeo.diving_time = PlayerPrefs.GetInt("Haenyeo_diving_time", 70);
            Haenyeo.moving_speed = PlayerPrefs.GetInt("Haenyeo_moving_speed", 7);
            Haenyeo.day = PlayerPrefs.GetInt("Haenyeo_day", 1);
            Haenyeo.level = PlayerPrefs.GetInt("Haenyeo_level", 2);
            Haenyeo.hp = PlayerPrefs.GetFloat("Haenyeo_hp", 100);
            bgm_volume.value = PlayerPrefs.GetFloat("Bgm_volume", 1);
            effect_volume.value = PlayerPrefs.GetFloat("Effect_volume", 1);


            //해녀 보유한 자원 개수 초기화

            Haenyeo.sea_item_number[0] = PlayerPrefs.GetInt("Haenyeo_sea_item_number0", 0);
            Haenyeo.sea_item_number[1] = PlayerPrefs.GetInt("Haenyeo_sea_item_number1", 0);
            Haenyeo.sea_item_number[2] = PlayerPrefs.GetInt("Haenyeo_sea_item_number2", 0);
            Haenyeo.sea_item_number[3] = PlayerPrefs.GetInt("Haenyeo_sea_item_number3", 0);
            Haenyeo.sea_item_number[4] = PlayerPrefs.GetInt("Haenyeo_sea_item_number4", 0);
            Haenyeo.sea_item_number[5] = PlayerPrefs.GetInt("Haenyeo_sea_item_number5", 0);
            Haenyeo.sea_item_number[6] = PlayerPrefs.GetInt("Haenyeo_sea_item_number6", 0);
            Haenyeo.sea_item_number[7] = PlayerPrefs.GetInt("Haenyeo_sea_item_number7", 0);
            Haenyeo.sea_item_number[8] = PlayerPrefs.GetInt("Haenyeo_sea_item_number8", 0);
        }
        else
        {
            Haenyeo.money = PlayerPrefs.GetInt("Haenyeo_money", 0);
            Haenyeo.debt = PlayerPrefs.GetInt("Haenyeo_debt", 5000000);
            Haenyeo.diving_time = PlayerPrefs.GetInt("Haenyeo_diving_time", 60);
            Haenyeo.moving_speed = PlayerPrefs.GetInt("Haenyeo_moving_speed", 7);
            Haenyeo.day = PlayerPrefs.GetInt("Haenyeo_day", 1);
            Haenyeo.level = PlayerPrefs.GetInt("Haenyeo_level", 3); // 다해 : 바다 다 열려고 레벨 3으로 설정 해놨음
            Haenyeo.hp = PlayerPrefs.GetFloat("Haenyeo_hp", 100);
            bgm_volume.value = PlayerPrefs.GetFloat("Bgm_volume", 1);
            effect_volume.value = PlayerPrefs.GetFloat("Effect_volume", 1);


            //해녀 보유한 자원 개수 초기화

            Haenyeo.sea_item_number[0] = PlayerPrefs.GetInt("Haenyeo_sea_item_number0", 0);
            Haenyeo.sea_item_number[1] = PlayerPrefs.GetInt("Haenyeo_sea_item_number1", 0);
            Haenyeo.sea_item_number[2] = PlayerPrefs.GetInt("Haenyeo_sea_item_number2", 0);
            Haenyeo.sea_item_number[3] = PlayerPrefs.GetInt("Haenyeo_sea_item_number3", 0);
            Haenyeo.sea_item_number[4] = PlayerPrefs.GetInt("Haenyeo_sea_item_number4", 0);
            Haenyeo.sea_item_number[5] = PlayerPrefs.GetInt("Haenyeo_sea_item_number5", 0);
            Haenyeo.sea_item_number[6] = PlayerPrefs.GetInt("Haenyeo_sea_item_number6", 0);
            Haenyeo.sea_item_number[7] = PlayerPrefs.GetInt("Haenyeo_sea_item_number7", 0);
            Haenyeo.sea_item_number[8] = PlayerPrefs.GetInt("Haenyeo_sea_item_number8", 0);
        }
    }




}