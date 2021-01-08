using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class quest_manager : MonoBehaviour
{
    public GameObject new_exist,quest_icon,done_exist;
    public AudioSource button_click, reward_click, item_click;
    
    //퀘스트 content 관련 오브젝트
    public GameObject content_parent, content;
    public Text summary_text, type_text;
    public Image touch_x,touch;
    public Image[] quest_states;
    public Button finish_active, finish_inactive,reward_active,reward_inactive;

    //퀘스트 popup 관련 오브젝트
    public GameObject popup;
    public GameObject[] persons; //상반식 캐릭터
    public Text text, todo_text, reward_text;
    public Button cancel_button;

    //퀘스트 텍스트 창 관련 오브젝트
    public GameObject quest_bg, quest_box_touch,quest_box,triangle;
    public GameObject[] quest_giver;
    public Text hilight_text,box_text;
    public static int text_done;
    public IEnumerator text_coroutine, triangle_coroutine, icon_effect_coroutine=null;

    //reward effect 관련 오브젝트
    public Image reward_item,reward_money,reward_image;
    public Text  money_text;

    public Animator dad_anim, seller_anim;

    void Start()
    {
        string tutorial_quest_load = PlayerPrefs.GetString("tutorial_quest", "new");
        string daily_quest_load = PlayerPrefs.GetString("daily_quest", "new");

        if (!tutorial_quest_load.Equals("new"))
        {
            quest_Data.tutorial_quest_list = JsonUtility.FromJson<Tutorial_Serialization>(tutorial_quest_load).toList();
            quest_Data.daily_quest_list = JsonUtility.FromJson<Daily_Serialization>(daily_quest_load).toList();
            quest_contents_update();
        }

        quest_contents_update();

        button_click.volume = PlayerPrefs.GetFloat("Effect_volume", 1);
        reward_click.volume = PlayerPrefs.GetFloat("Effect_volume", 1);
        item_click.volume = PlayerPrefs.GetFloat("Effect_volume", 1);
    }

    void Update()
    {
        if (text_done == 0)
        {
            if(quest_giver[1].activeSelf)dad_anim.SetBool("talking", true);
            if (quest_giver[2].activeSelf) seller_anim.SetBool("talking", true);
        }
        else {
            if (quest_giver[1].activeSelf) dad_anim.SetBool("talking", false);
            if (quest_giver[2].activeSelf) seller_anim.SetBool("talking", false);
        }
    }

    //퀘스트 목록 업데이트 
    public void quest_contents_update()
    {
        //초기화
        popup.SetActive(false);
        content.SetActive(false);
        new_exist.gameObject.SetActive(false);
        done_exist.gameObject.SetActive(false);

        //기존 자식 오브젝트들 삭제하고 다시 생성
        for (int i = 0; i < content_parent.transform.childCount; i++)
        {
            Destroy(content_parent.transform.GetChild(i).gameObject);
        }
        content_parent.transform.DetachChildren();

        // 튜토리얼 퀘스트
        for(int i=0;i<quest_Data.tutorial_quest_list.Count;i++)
        {
            Tutorial_quest_form data = quest_Data.tutorial_quest_list[i];

            if (data.state != -1)   //퀘스트 목록에 추가할 퀘스트라면
            {
                for (int j = 0; j < 3; j++) quest_states[j].gameObject.SetActive(false);
                finish_inactive.gameObject.SetActive(false);
                finish_active.gameObject.SetActive(false);
                touch.gameObject.SetActive(false);
                //퀘스트 state
                quest_states[data.state].gameObject.SetActive(true);
                touch_x.gameObject.SetActive(true);

                type_text.text = data.type; summary_text.text = data.summary;

                GameObject temp_content = Instantiate(content, new Vector3(0, 0, -376), Quaternion.identity);
                temp_content.transform.SetParent(content_parent.transform);
                temp_content.SetActive(true);
            }
        }

        //일일 퀘스트
        for (int i = 0; i < quest_Data.daily_quest_list.Count; i++)
        {
            Daily_quest_form data = quest_Data.daily_quest_list[i];

            if (data.state != -1)
            {
                if (data.state == 0) new_exist.gameObject.SetActive(true);
                if (data.state == 2) done_exist.gameObject.SetActive(true);
                
                for (int j = 0; j < 3; j++) quest_states[j].gameObject.SetActive(false);
                finish_inactive.gameObject.SetActive(false);
                finish_active.gameObject.SetActive(false);
                touch_x.gameObject.SetActive(false);
                touch.gameObject.SetActive(true);
                quest_states[data.state].gameObject.SetActive(true);
                if (data.state == 2) finish_active.gameObject.SetActive(true);
                else finish_inactive.gameObject.SetActive(true);

                type_text.text = data.type; summary_text.text = data.summary;

                //content와 popup복제해서 부모 설정하기
                GameObject temp_content = Instantiate(content, new Vector3(0, 0, -376), Quaternion.identity);
                temp_content.transform.SetParent(content_parent.transform);
                temp_content.name = i.ToString();      //이름을 인덱스로 설정
                temp_content.SetActive(true);
            }
        }
        
        if (new_exist.activeSelf == true || done_exist.activeSelf == true) StartCoroutine("quest_icon_effect");
        
        //json타입 변환 후 저장
        if (quest_Data.daily_quest_list.Count != 0 && quest_Data.tutorial_quest_list.Count != 0)
        {
            string tutorial_quest_save = JsonUtility.ToJson(new Tutorial_Serialization(quest_Data.tutorial_quest_list));
            string daily_quest_save = JsonUtility.ToJson(new Daily_Serialization(quest_Data.daily_quest_list));
            PlayerPrefs.SetString("tutorial_quest", tutorial_quest_save);
            PlayerPrefs.SetString("daily_quest", daily_quest_save);
        }
        
    }

    //퀘스트 목록에서 클릭했을 시
    public void quest_popup_open()
    {
        button_click.PlayOneShot(button_click.clip);

        //클릭한 오브젝트 찾아서 idx 찾기
        string content_name = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.name;
        int idx = Int32.Parse(content_name);

        Daily_quest_form data=quest_Data.daily_quest_list[idx];
        for (int i = 0; i < 3; i++) persons[i].SetActive(false);
        persons[data.person].SetActive(true);
        text.text = data.text; todo_text.text = data.todo; reward_text.text = data.reward;
        
        //state변경 및 content update
        if(quest_Data.daily_quest_list[idx].state == 0 )quest_Data.daily_quest_list[idx].state = 1;
        quest_contents_update();

        //reward button 활성화
        reward_active.gameObject.SetActive(false);
        reward_inactive.gameObject.SetActive(false);
        if (quest_Data.daily_quest_list[idx].state == 2) reward_active.gameObject.SetActive(true);
        else reward_inactive.gameObject.SetActive(true);

        popup.name = idx.ToString();
        popup.SetActive(true);
    }

    public void quest_popup_close()
    {
       popup.SetActive(false);
    } 

    public void done_check()
    {
        bool flag = false;

        for (int idx = 0; idx < quest_Data.daily_quest_list.Count; idx++)
        {
            if (quest_Data.daily_quest_list[idx].state == 2) flag = true;
        }

        if (flag)
        {
            done_exist.SetActive(true);
            StartCoroutine("quest_icon_effect");
        }
        if(!flag) done_exist.SetActive(false);
    }

    public void clicked_reward_button()
    {
        reward_click.PlayOneShot(reward_click.clip);

        //클릭한 오브젝트 찾아서 idx 찾기
        string content_name = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.name;
        int idx = Int32.Parse(content_name);
        quest_Data.daily_quest_list[idx].state = -1;

        //퀘스트에 따른 보상
        GameObject.Find("daily_quest_manager").GetComponent<daily_quest_manager>().quest_reward(idx);
        quest_Data.daily_quest_list[idx].state = -1;
    }

    //화면 어두워지면서 퀘스트 주는 거
    public void show_quest_box(Daily_quest_form day_quest)
    {
        quest_bg.SetActive(true);
        quest_giver[day_quest.person].SetActive(true);
        quest_box.gameObject.SetActive(true);
        hilight_text.gameObject.SetActive(true);
        hilight_text.text ="";
        quest_box_touch.SetActive(true);

        text_coroutine = text_effect(day_quest);
        StartCoroutine(text_coroutine);

        triangle_coroutine = triangle_effect();
        StartCoroutine(triangle_coroutine);
    }

    IEnumerator text_effect(Daily_quest_form day_quest)
    {
        text_done = 0;
        for (int i = 0; i < day_quest.text.Length; i++)
        {
            box_text.text = day_quest.text.Substring(0, i + 1);
            yield return new WaitForSeconds(0.05f);
        }
        hilight_text.text = day_quest.summary;
        text_done = 1;
    }
    
    IEnumerator triangle_effect()
    {
        while (true)
        {
            triangle.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            triangle.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.3f);
        }
    }

    public void quest_box_close()
    {
        if (text_done == 0)
        {
            StopCoroutine(text_coroutine);
            text_coroutine = null;
            text_done = 1;
            box_text.text = quest_Data.daily_quest_list[Haenyeo.day - 1].text;
            hilight_text.text = quest_Data.daily_quest_list[Haenyeo.day - 1].summary;
        }
        else
        {
            StopCoroutine(triangle_coroutine);
            triangle_coroutine = null;
            item_click.PlayOneShot(item_click.clip);
            quest_box_touch.gameObject.SetActive(false);
            quest_bg.gameObject.SetActive(false);
            for (int i = 0; i < 3; i++) quest_giver[i].SetActive(false);
            quest_box.gameObject.SetActive(false);
        }
    }
    
    //새로운 퀘스트 있을 때 아이콘 깜박깜박
    IEnumerator quest_icon_effect()
    {
        if (icon_effect_coroutine == null)
        {
            icon_effect_coroutine = quest_icon_effect();
            int time = 0;
            while (time < 5)
            {
                quest_icon.gameObject.transform.Rotate(Vector3.back * 5);
                yield return new WaitForSeconds(0.2f);
                quest_icon.gameObject.transform.Rotate(Vector3.forward * 5);
                yield return new WaitForSeconds(0.2f);
                time++;
            }
            yield return new WaitForSeconds(3.0f);

            icon_effect_coroutine = null;
        }
    }

    //보상 버튼 이펙트
    public IEnumerator reward_effect(string reward, int value)
    {
        if (reward == "money")
        {
            reward_item.gameObject.SetActive(false);
            reward_money.gameObject.SetActive(true);
        }
        else
        {
            reward_item.GetComponent<Image>().sprite = Resources.Load<Sprite>(reward);
            reward_item.gameObject.SetActive(true);
            reward_money.gameObject.SetActive(false);
        }

        reward_image.gameObject.SetActive(true);
        money_text.text =value.ToString();
        Vector2 startPos = reward_image.transform.localPosition;
        Vector2 endPos = startPos + new Vector2(0, 40);
        float LerpT = 0;
        float speed = 3;

        while (LerpT <= 1)
        {
            reward_image.transform.localPosition = Vector2.Lerp(startPos, endPos, LerpT);

            LerpT += Time.deltaTime * speed;
            yield return null;
        }
        yield return new WaitForSeconds(0.3f);
        reward_image.gameObject.SetActive(false);
        reward_image.transform.localPosition = startPos;

        popup.SetActive(false);
        quest_contents_update();
    }
}