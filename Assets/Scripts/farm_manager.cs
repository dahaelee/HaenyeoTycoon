using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Diagnostics;

public class farm_manager : MonoBehaviour
{
    public UI_manager UI_manager;       //UI 관련 매니저
    public farm[] farms;
    public GameObject[] farmable_items;
    public GameObject sea_item_parent;
    public static GameObject[] sea_item;
    public Text name_info, time_info, day, money, debt, debt_repay, money_repay, sending_amount_repay, entire_debt, interest, payed, balance, today, interest_warning;
    public int chosen_item, chosen_farm, trash_farm_num, activation_cost, sending_int, sending_limit, limit_day;
    public bool is_chosen;
    public static bool is_sea_locked, is_repay_locked, is_repayAnim_Activated;
    public string sending_str = "";
    public Slider bgm_volume, effect_volume;
    public Text[] item_count;
    //사운드 관련 필드
    public AudioSource bgm, button_click, popup_click, expand_click, request_denied, get_money, trashing, next_day, debt_sending, num_pad, icon_click, item_click, farm_money;
    public AudioClip farm_night_BGM, farm_day_BGM;
    public Image repay_icon, farm_night, repay_inactive, sea_inactive, Switch;
    public Image[] items;

    IEnumerator current_Info;

    public enum farms_index
    {
        farm0 = 0,
        farm1,
        farm2,
        farm3,
        farm4,
        farm5,
        farm6,
        farm7
    }

    void OnEnable()
    {
        sea_item = new GameObject[9];
        for (int i = 0; i < sea_item.Length; i++)
        {
            sea_item[i] = sea_item_parent.transform.GetChild(i).gameObject;
        }
        data_load();
    }
    void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        int isNew = PlayerPrefs.GetInt("isNew", 1);     //새로운거인지 확인
        if (isNew == 1)     //첫 시작이면 퀘스트 뜨게 함
        {
            //효민 - 빚쟁이 텍스트
            GameObject.Find("quest_Data").GetComponent<quest_Data>().newStart();
            PlayerPrefs.SetInt("isNew", 0);
        }



        activation_cost = 50000;        //양식장 확장 비용
        limit_day = 20;
        sending_int = 0;
        sending_str = "0";
        sending_limit = 100000;
        sending_amount_repay.GetComponent<Text>().text = sending_str;
        day.GetComponent<Text>().text = Haenyeo.day.ToString();
        money.GetComponent<Text>().text = Haenyeo.money.ToString();
        debt.GetComponent<Text>().text = Haenyeo.debt.ToString();

        for (int i = 0; i < sea_item.Length; i++)
        {
            sea_item[i].transform.GetChild(2).GetComponent<Text>().text = sea_item[i].GetComponent<sea_item>().item_name_kor;
            sea_item[i].transform.GetChild(3).GetComponent<Text>().text = sea_item[i].GetComponent<sea_item>().farm_price.ToString();
            sea_item[i].transform.GetChild(4).GetComponent<Text>().text = sea_item[i].GetComponent<sea_item>().farm_time.ToString();
        }

        UI_manager.AllUIoff();
        if (Haenyeo.todayState == Haenyeo.TodayState.night)
        {
            Color color = new Vector4(1, 1, 1, 1);
            farm_night.color = color;
            farm_night.gameObject.SetActive(true);
            Switch.sprite = Resources.Load<Sprite>("switch_night");
        }
        else
        {
            Switch.sprite = Resources.Load<Sprite>("switch_day");
            farm_night.gameObject.SetActive(false);
        }

    }
    void Update()
    {
        day.GetComponent<Text>().text = "D-" + (limit_day - Haenyeo.day + 1).ToString();
        money.GetComponent<Text>().text = string.Format("{0:#,###0}", Haenyeo.money);
        debt.GetComponent<Text>().text = string.Format("{0:#,###0}", Haenyeo.debt + Haenyeo.interest - Haenyeo.payed);
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            StartCoroutine(UI_manager.UI_On(UI_manager.UIstate.ask_quit));
        }

        effect_sound_ctrl();
        bgm_sound_ctrl();
        if (Haenyeo.hp <= 0)
        {
            sea_inactive.gameObject.SetActive(true);    //바다 못가요
            repay_inactive.gameObject.SetActive(false);    //바다 못가요
            if (Haenyeo.todayState == Haenyeo.TodayState.day)
            {
                StartCoroutine(GoNight());
            }
            if (!is_repayAnim_Activated)
            {
                StartCoroutine(repayAnim());
            }
            if (bgm.clip.name == "BGM_farm")
            {
                bgm.clip = farm_night_BGM;
                bgm.Play();
            }
        }
        else
        {
            sea_inactive.gameObject.SetActive(false);
            repay_inactive.gameObject.SetActive(true);    //바다 못가요
            is_repayAnim_Activated = false;
        }
        if (Haenyeo.todayState == Haenyeo.TodayState.night)
        {
            Switch.sprite = Resources.Load<Sprite>("switch_night");
        }
        else
        {
            Switch.sprite = Resources.Load<Sprite>("switch_day");
        }
    }
    //수확하기 버튼 눌렀을 때 실행

    public void farming_money(int index)    //양식장 번호 파라미터
    {
        get_money.PlayOneShot(get_money.clip);  //수확할 때 사운드
        //양식자원 하나 증가
        int item_num = farms[index].item.number;
        Haenyeo.farm_item_number[item_num]++;
        //양식장 비우기
        StartCoroutine(farming_effect(index));

    }

    //버블 커지는 효과
    public IEnumerator farming_effect(int index)
    {
        farms[index].money.transform.GetChild(0).transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        Vector3 d = farms[index].money.transform.GetChild(0).transform.localScale;
        for (int i = 0; i < 10; i++)
        {
            d = new Vector3(d.x * 1.02f, d.y * 1.02f, d.z * 1.02f);
            farms[index].money.transform.GetChild(0).transform.localScale = d;
            yield return new WaitForSeconds(0.02f);
        }
        farms[index].farmReset();
        farms[index].money.transform.GetChild(0).transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

    }

    public IEnumerator repayAnim()
    {
        is_repayAnim_Activated = true;
        while (Haenyeo.hp <= 0)
        {
            repay_icon.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -5));
            yield return new WaitForSeconds(0.2f);
            repay_icon.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 5));
            yield return new WaitForSeconds(0.2f);
            repay_icon.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -5));
            yield return new WaitForSeconds(0.2f);
            repay_icon.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 5));
            yield return new WaitForSeconds(0.2f);
            repay_icon.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            yield return new WaitForSeconds(2f);
        }

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
        image2.gameObject.SetActive(false);
        image2.gameObject.SetActive(false);
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
            UI_manager.AllUIoff();

            farms[farm_index].isFarming = true;    //양식 시작한걸로 하기
            Haenyeo.sea_item_number[item_index] -= 1;   //자원 1개 차감 
                                                        //            StartCoroutine(UI_manager.HPminus(1));      //체력 -1 이펙트
            farms[farm_index].plus.gameObject.SetActive(false); //플러스 표시 빼기

            farms[farm_index].item = sea_item[item_index].GetComponent<sea_item>();    //양식장에 자원 넣기
            farms[farm_index].item1.sprite = Resources.Load<Sprite>(sea_item[item_index].name);   //자원 이미지 바꾸기
            farms[farm_index].item2.sprite = Resources.Load<Sprite>(sea_item[item_index].name);   //자원 이미지 바꾸기
            farms[farm_index].item3.sprite = Resources.Load<Sprite>(sea_item[item_index].name);   //자원 이미지 바꾸기

            farms[farm_index].item1.GetComponent<Animator>().runtimeAnimatorController = farmable_items[item_index].GetComponent<sea_item>().anim.runtimeAnimatorController;    //자원 애니메이터 넣기
            farms[farm_index].item2.GetComponent<Animator>().runtimeAnimatorController = farmable_items[item_index].GetComponent<sea_item>().anim.runtimeAnimatorController;    //자원 애니메이터 넣기
            farms[farm_index].item3.GetComponent<Animator>().runtimeAnimatorController = farmable_items[item_index].GetComponent<sea_item>().anim.runtimeAnimatorController;    //자원 애니메이터 넣기
            farms[farm_index].money.sprite = Resources.Load<Sprite>(sea_item[item_index].name); //버블 속 자원 아이템 이미지 바꾸기

            farms[farm_index].item1.gameObject.SetActive(true);
            farms[farm_index].item2.gameObject.SetActive(false);
            farms[farm_index].item3.gameObject.SetActive(false);

            farms[farm_index].item_generating = farms[farm_index].Wait_generating();
            StartCoroutine(farms[farm_index].item_generating);


        }
        else
        {   //3개 미만인 경우 양식 안함
            //Debug.Log("양식 못해열");
        }
    }


    //양식 가능한 자원 보여주기
    public void show_farmable_item(int farm_index)
    {
        if (Haenyeo.hp <= 0)
        {
            if (UI_manager.currentState != UI_manager.UIstate.no_HP)
            {
                StartCoroutine(UI_manager.UI_On(UI_manager.UIstate.no_HP, true));
            }
        }
        else
        {


            is_chosen = false;
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
                StartCoroutine(UI_manager.UI_On(UI_manager.UIstate.no_item, true));
            }
            else
            {
                if (UI_manager.currentState != UI_manager.UIstate.farmable_item)
                {
                    StartCoroutine(UI_manager.UI_On(UI_manager.UIstate.farmable_item));    //양식 가능한 자원 보여주는 ui창 띄우기
                }

                for (int i = 0; i < 9; i++)
                {

                    if (Haenyeo.sea_item_number[i] > 0)
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
                chosen_farm = farm_index;   //양식장 번호 저장
            }
        }
    }


    //양식 정보 보여주기
    public void farming_info(int index) //양식장 index 파라미터
    {
        if (current_Info != null)
        {
            StopCoroutine(current_Info);
        }
        current_Info = null;
        if (farms[index].isFarming)
        {
            popup_click.PlayOneShot(popup_click.clip);      //팝업창 사운드
            trash_farm_num = index;
            if (farms[index].isFarming)
            {
                name_info.text = farms[index].item.item_name_kor + " 양식장";
            }

            if (UI_manager.currentState != UI_manager.UIstate.farm_info)
            {
                StartCoroutine(UI_manager.UI_On(UI_manager.UIstate.farm_info));
            }

            //생성까지 남은 시간 보여주기
            current_Info = Farm_remaining_time(index);
            StartCoroutine(current_Info);
        }
    }


    //양식장 비우기 확인 UI창 띄우기
    public void Ask_trash()
    {
        button_click.PlayOneShot(button_click.clip);    //버튼클릭 사운드

        if (UI_manager.currentState != UI_manager.UIstate.trash)
        {
            UI_manager.AllUIoff();
            StartCoroutine(UI_manager.UI_On(UI_manager.UIstate.trash));
            UnityEngine.Debug.Log("trash 열어라");
        }
    }

    //양식장 비우기
    public void trash_farm()
    {
        trashing.PlayOneShot(trashing.clip);
        UI_manager.AllUIoff();
        farms[trash_farm_num].farmReset();

    }

    //양식장 확장하기
    public void Ask_expand_farm(int index)
    {
        popup_click.PlayOneShot(popup_click.clip);
        chosen_farm = index;
        if (UI_manager.currentState != UI_manager.UIstate.expand)
        {
            StartCoroutine(UI_manager.UI_On(UI_manager.UIstate.expand));
        }
    }

    //양식장 활성화
    public void Activate_farm()
    {
        if (Haenyeo.money >= activation_cost)
        {
            int index = chosen_farm;
            expand_click.PlayOneShot(expand_click.clip);      //확장클릭 사운드
            UI_manager.AllUIoff();
            farms[index].is_farm_Activated = true;
            Haenyeo.money -= activation_cost;
            StartCoroutine(FadeOut(farms[index].lock_img, farms[index].locked_bg, farms[index].plus, 0.1f));

        }
        else
        {
            //소지금이 부족합니다 UI 띄우기
            request_denied.PlayOneShot(request_denied.clip);        //요청 거절 사운드

            if (UI_manager.currentState != UI_manager.UIstate.no_money)
            {
                StartCoroutine(UI_manager.UI_On(UI_manager.UIstate.no_money, true));
            }
        }

    }

    //수확까지 남은 시간 보여주기
    IEnumerator Farm_remaining_time(int index)
    {
        while (UI_manager.currentState == UI_manager.UIstate.farm_info)
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

    public IEnumerator GoNight()

    {
        Haenyeo.todayState = Haenyeo.TodayState.night;
        StartCoroutine(fadein(farm_night));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(UI_manager.UI_On(UI_manager.UIstate.todayFinished, true, 3f));
        yield return new WaitForSeconds(2f);

    }

    public IEnumerator GoDay()
    {
        Haenyeo.todayState = Haenyeo.TodayState.day;
        bgm.clip = farm_day_BGM;
        bgm.Play();
        StartCoroutine(fadeout(farm_night));
        yield return new WaitForSeconds(0f);
    }

    public IEnumerator fadein(Image img)
    {
        img.gameObject.SetActive(false);    //이코드 이상할수도...
        float time = 0;
        Color fadecolor = img.color;
        fadecolor.a = 0f;
        img.color = fadecolor;
        img.gameObject.SetActive(true);
        while (img.color.a < 1)
        {
            time += Time.deltaTime;

            fadecolor.a = Mathf.Lerp(0, 1, time);
            img.color = fadecolor;
            yield return null;
        }
    }

    public IEnumerator fadeout(Image img)
    {
        float time = 0;
        Color fadecolor = img.color;
        fadecolor.a = 1f;
        img.color = fadecolor;
        img.gameObject.SetActive(true);
        while (img.color.a > 0)
        {
            time += Time.deltaTime;

            fadecolor.a = Mathf.Lerp(1, 0, time);
            img.color = fadecolor;
            UnityEngine.Debug.Log(img.color.a);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        fadecolor.a = 1f;
        img.color = fadecolor;

        img.gameObject.SetActive(false);

    }



    //하루정산 UI 띄우기
    public void today_work()
    {
        icon_click.PlayOneShot(icon_click.clip);
        UI_manager.AllUIoff();
        entire_debt.text = string.Format("{0:#,###0}", Haenyeo.debt);
        interest.text = string.Format("{0:#,###0}", Haenyeo.interest);
        payed.text = string.Format("{0:#,###0}", Haenyeo.payed);
        balance.text = string.Format("{0:#,###0}", (Haenyeo.debt - Haenyeo.payed + Haenyeo.interest));
        today.text = Haenyeo.day.ToString();
        StartCoroutine(UI_manager.UI_On(UI_manager.UIstate.today_work));

    }

    //송금하기 UI 띄우기
    public void repay()
    {
        icon_click.PlayOneShot(icon_click.clip);        //아이콘 클릭시 사운드
        UI_manager.AllUIoff();
        StartCoroutine(UI_manager.UI_On(UI_manager.UIstate.repay));
        debt_repay.GetComponent<Text>().text = string.Format("{0:#,###0}", Haenyeo.debt);
        money_repay.GetComponent<Text>().text = string.Format("{0:#,###0}", Haenyeo.money);
        sending_int = 0;
        sending_str = "0";
        sending_amount_repay.GetComponent<Text>().text = sending_str;
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
        UI_manager.AllUIoff();
        if (sending_int < sending_limit)
        {
            button_click.PlayOneShot(button_click.clip);
            interest_warning.text = "10만원보다 적은 금액 송금시\n 이자"+((int)((Haenyeo.debt + Haenyeo.interest - Haenyeo.payed) * 0.03)).ToString()+"원이 붙습니다.";
            StartCoroutine(UI_manager.UI_On(UI_manager.UIstate.interest_warning));
        }
        else
        {
            debt_sending.PlayOneShot(debt_sending.clip);
            next_day.PlayDelayed((float)0.5f);
            Haenyeo.money -= sending_int;
            Haenyeo.payed += sending_int;
            if (Haenyeo.debt < 0)
            {
                Haenyeo.debt = 0;
            }
            PlayerPrefs.SetInt("questReady", 1);

            Haenyeo.day++;
            Haenyeo.hp = 100;
            Haenyeo.todayState = Haenyeo.TodayState.day;
            is_repay_locked = true;
            is_sea_locked = false;      //바다 아이콘 활성화
            PlayerPrefs.SetInt("is_repay_locked", 1);
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

            StartCoroutine(GoDay());    //다음날로 바뀌는 이펙트



        }

    }

    //최소송금액보다 부족할 경우에 이자늘어남과 함께 송금되는 함수
    public void repay_interest()
    {
        PlayerPrefs.SetInt("questReady", 1);

        debt_sending.PlayOneShot(debt_sending.clip);
        Haenyeo.money -= sending_int;
        Haenyeo.day++;
        Haenyeo.hp = 100;
        Haenyeo.todayState = Haenyeo.TodayState.day;
        Haenyeo.payed += sending_int;
        if (Haenyeo.debt <= 0)
        {
            Haenyeo.debt = 0;
        }
        Haenyeo.interest += (int)((Haenyeo.debt + Haenyeo.interest - Haenyeo.payed) * 0.03);     //이자율 3%
        UI_manager.AllUIoff();
        is_repay_locked = true;
        is_sea_locked = false;      //바다 아이콘 활성화
        
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
        StartCoroutine(GoDay());    //다음날로 바뀌는 이펙트

    }


    //바다 가기 아이콘 눌렀을 때 실행되는 함수
    public void sea_ui()
    {
        if (Haenyeo.hp > 0)
        {
            if (UI_manager.currentState != UI_manager.UIstate.go_sea)
            {
                icon_click.PlayOneShot(icon_click.clip);        //아이콘 클릭시 사운드
                StartCoroutine(UI_manager.UI_On(UI_manager.UIstate.go_sea));
            }
        }
        else
        {
            if (UI_manager.currentState != UI_manager.UIstate.no_HP)
            {
                StartCoroutine(UI_manager.UI_On(UI_manager.UIstate.no_HP, true));
            }
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
        icon_click.PlayOneShot(icon_click.clip);        //아이콘 클릭시 사운드
        StartCoroutine(UI_manager.UI_On(UI_manager.UIstate.setting));
    }

    public void restart_bttn()
    {
        button_click.PlayOneShot(button_click.clip);
        StartCoroutine(UI_manager.UI_On(UI_manager.UIstate.restart));
    }

    public void ask_quit()
    {
        button_click.PlayOneShot(button_click.clip);
        StartCoroutine(UI_manager.UI_On(UI_manager.UIstate.ask_quit));
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
        StartCoroutine(UI_manager.UI_On(UI_manager.UIstate.ending));
        UI_manager.UIs[(int)UI_manager.UIstate.ending].color = new Vector4(1, 1, 1, 0);
        for (float i = 0f; i <= 1; i += 0.1f)
        {
            UI_manager.UIs[(int)UI_manager.UIstate.ending].color = new Vector4(1, 1, 1, i);
            yield return new WaitForSeconds(0.1f);
        }

        SceneManager.LoadScene("ending_bad");
    }

    //해피 엔딩으로 넘어감
    IEnumerator happy_ending()
    {
        StartCoroutine(UI_manager.UI_On(UI_manager.UIstate.ending));        //중간 장면
        UI_manager.UIs[(int)UI_manager.UIstate.ending].color = new Vector4(1, 1, 1, 0);
        for (float i = 0f; i <= 1; i += 0.1f)
        {
            UI_manager.UIs[(int)UI_manager.UIstate.ending].color = new Vector4(1, 1, 1, i);
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
        Haenyeo.todayState = Haenyeo.TodayState.day;
        PlayerPrefs.DeleteAll();
        for (int i = 0; i < farms.Length; i++)
        {
            farms[i].farmReset();
        }
        SceneManager.LoadScene("start");
    }


    public void data_save()
    {
        //데이터 저장

        PlayerPrefs.SetInt("isNew", 0);     //새로운게 아니라고 표시

        PlayerPrefs.SetInt("Haenyeo" + "_" + "money", Haenyeo.money);
        PlayerPrefs.SetInt("Haenyeo_debt", Haenyeo.debt);
        PlayerPrefs.SetInt("Haenyeo_payed", Haenyeo.payed);
        PlayerPrefs.SetInt("Haenyeo_interest", Haenyeo.interest);
        PlayerPrefs.SetInt("Haenyeo_diving_time", Haenyeo.diving_time);
        PlayerPrefs.SetInt("Haenyeo_day", Haenyeo.day);
        PlayerPrefs.SetInt("Haenyeo_level", Haenyeo.level);
        PlayerPrefs.SetFloat("Haenyeo_hp", Haenyeo.hp);
        PlayerPrefs.SetString("lasttime", System.DateTime.Now.ToString());

        PlayerPrefs.SetFloat("Haenyeo_moving_speed", Haenyeo.moving_speed);
        PlayerPrefs.SetInt("Haenyeo_coin_time", Haenyeo.coin_time);
        PlayerPrefs.SetFloat("Haenyeo_hp_ratio", Haenyeo.hp_ratio);

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

        PlayerPrefs.SetInt("Haenyeo_farm_item_number0", Haenyeo.farm_item_number[0]);
        PlayerPrefs.SetInt("Haenyeo_farm_item_number1", Haenyeo.farm_item_number[1]);
        PlayerPrefs.SetInt("Haenyeo_farm_item_number2", Haenyeo.farm_item_number[2]);
        PlayerPrefs.SetInt("Haenyeo_farm_item_number3", Haenyeo.farm_item_number[3]);
        PlayerPrefs.SetInt("Haenyeo_farm_item_number4", Haenyeo.farm_item_number[4]);
        PlayerPrefs.SetInt("Haenyeo_farm_item_number5", Haenyeo.farm_item_number[5]);
        PlayerPrefs.SetInt("Haenyeo_farm_item_number6", Haenyeo.farm_item_number[6]);
        PlayerPrefs.SetInt("Haenyeo_farm_item_number7", Haenyeo.farm_item_number[7]);
        PlayerPrefs.SetInt("Haenyeo_farm_item_number8", Haenyeo.farm_item_number[8]);


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

        Haenyeo.money = PlayerPrefs.GetInt("Haenyeo_money", 0);
        Haenyeo.debt = PlayerPrefs.GetInt("Haenyeo_debt", 5000000);
        Haenyeo.payed = PlayerPrefs.GetInt("Haenyeo_payed", 0);
        Haenyeo.interest = PlayerPrefs.GetInt("Haenyeo_interest", 0);
        Haenyeo.diving_time = PlayerPrefs.GetInt("Haenyeo_diving_time", 60);
        Haenyeo.day = PlayerPrefs.GetInt("Haenyeo_day", 1);
        Haenyeo.level = PlayerPrefs.GetInt("Haenyeo_level", 3); // 다해 : 바다 다 열려고 레벨 3으로 설정 해놨음
        Haenyeo.hp = PlayerPrefs.GetFloat("Haenyeo_hp", 100);
        bgm_volume.value = PlayerPrefs.GetFloat("Bgm_volume", 1);
        effect_volume.value = PlayerPrefs.GetFloat("Effect_volume", 1);

        Haenyeo.moving_speed = PlayerPrefs.GetFloat("Haenyeo_moving_speed", 7);
        Haenyeo.coin_time = PlayerPrefs.GetInt("Haenyeo_coin_time", 8);
        Haenyeo.hp_ratio = PlayerPrefs.GetFloat("Haenyeo_hp_ratio", 2);

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


        Haenyeo.farm_item_number[0] = PlayerPrefs.GetInt("Haenyeo_farm_item_number0", 0);
        Haenyeo.farm_item_number[1] = PlayerPrefs.GetInt("Haenyeo_farm_item_number1", 0);
        Haenyeo.farm_item_number[2] = PlayerPrefs.GetInt("Haenyeo_farm_item_number2", 0);
        Haenyeo.farm_item_number[3] = PlayerPrefs.GetInt("Haenyeo_farm_item_number3", 0);
        Haenyeo.farm_item_number[4] = PlayerPrefs.GetInt("Haenyeo_farm_item_number4", 0);
        Haenyeo.farm_item_number[5] = PlayerPrefs.GetInt("Haenyeo_farm_item_number5", 0);
        Haenyeo.farm_item_number[6] = PlayerPrefs.GetInt("Haenyeo_farm_item_number6", 0);
        Haenyeo.farm_item_number[7] = PlayerPrefs.GetInt("Haenyeo_farm_item_number7", 0);
        Haenyeo.farm_item_number[8] = PlayerPrefs.GetInt("Haenyeo_farm_item_number8", 0);

    }
}