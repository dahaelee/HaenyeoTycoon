using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tutorial_quest_form
{
    public int state;   //0. new 1. ing 2. done
    public string summary;
    public string type;

    public Tutorial_quest_form(int state, string type, string summary)
    {
        this.state = state;
        this.type = type;
        this.summary = summary;
    }

}

public class Daily_quest_form
{
    public int state;//0. new 1. ing 2. done
    public string summary;
    public string type;
    public int person; //0. 사채업자 1. 아빠 2. 상인아저씨
    public string text; // 말풍선 텍스트
    public string todo; // 해야할 일
    public string reward;   // 보상

    public Daily_quest_form(int state,string summary, string type, int person, string text, string todo, string reward)
    {
        this.state = state;
        this.summary = summary;
        this.type = type;
        this.person = person;
        this.text = text;
        this.todo = todo;
        this.reward = reward;
    }
}

public class quest_manager : MonoBehaviour
{
    public static SortedList<int, Tutorial_quest_form> tutorial_quest_list = new SortedList<int, Tutorial_quest_form>();    //튜토리얼 퀘스트 목록
    public static SortedList<int, Daily_quest_form> daily_quest_list = new SortedList<int, Daily_quest_form>(); // 일일 퀘스트 목록
    public GameObject new_exist;

    //퀘스트 content 관련 오브젝트
    public GameObject content_parent, content;
    public Text summary_text, type_text;
    public Image touch_x;
    public Image[] quest_states;
    public Button finish_active, finish_inactive;

    //퀘스트 popup 관련 오브젝트
    public GameObject popup;
    public GameObject[] persons; //상반식 캐릭터
    public Text text, todo_text, reward_text;
    public Button cancel_button;

    //퀘스트 텍스트 창 관련 오브젝트
    public GameObject quest_bg, quest_box_touch,quest_box;
    public GameObject[] quest_giver;
    public Text hilight_text,box_text;

    public static int day;
    
    void Update()
    {
        day = PlayerPrefs.GetInt("day", 1);
        give_daily_quest(); //일일 퀘스트 제공
    }

    //퀘스트 목록 업데이트
    public void quest_contents_update()
    {
        //초기화
        popup.SetActive(false);
        content.SetActive(false);
        new_exist.gameObject.SetActive(false);

        //기존 자식 오브젝트들 삭제하고 다시 생성
        for (int i = 0; i < content_parent.transform.childCount; i++)
        {
            Destroy(content_parent.transform.GetChild(i).gameObject);
        }
        content_parent.transform.DetachChildren();

        // 튜토리얼 퀘스트
        foreach (var tmp in tutorial_quest_list)
        {
            Tutorial_quest_form data = tmp.Value;

            for (int i = 0; i < 3; i++)
            {
                quest_states[i].gameObject.SetActive(false);
                persons[i].gameObject.SetActive(false);
            }
            finish_inactive.gameObject.SetActive(false);
            finish_active.gameObject.SetActive(false);
            //퀘스트 state
            quest_states[data.state].gameObject.SetActive(true);
            touch_x.gameObject.SetActive(true);
            
            type_text.text = data.type; summary_text.text = data.summary;

            GameObject temp_content = Instantiate(content, new Vector3(0, 0, 0), Quaternion.identity);
            temp_content.transform.SetParent(content_parent.transform);
            temp_content.name = "tutorial" + tmp.Key.ToString();
            temp_content.SetActive(true);
        }
        
        //일일 퀘스트
        foreach (var tmp in daily_quest_list)
        {
            Daily_quest_form data = tmp.Value;

            if (data.state == 0) new_exist.SetActive(true);

            for (int i = 0; i < 3; i++)
            {
                quest_states[i].gameObject.SetActive(false);
                persons[i].gameObject.SetActive(false);
            }
            finish_inactive.gameObject.SetActive(false);
            finish_active.gameObject.SetActive(false);
            touch_x.gameObject.SetActive(false);
            quest_states[data.state].gameObject.SetActive(true);
            if (data.state == 2) finish_active.gameObject.SetActive(true);
            else finish_inactive.gameObject.SetActive(true);

            type_text.text = data.type; summary_text.text = data.summary;
            
            //content와 popup복제해서 부모 설정하기
            GameObject temp_content = Instantiate(content, new Vector3(0, 0, 0), Quaternion.identity);
            temp_content.transform.SetParent(content_parent.transform);
            temp_content.name = tmp.Key.ToString();      //이름을 key로 설정
            temp_content.SetActive(true);
        }
    }

    //퀘스트 목록에서 클릭했을 시
    public void quest_popup_open()
    {
        //클릭한 오브젝트 찾아서 key 찾기
        string content_name = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.name;
        int key = Int32.Parse(content_name);

        Daily_quest_form data=daily_quest_list[key];
        for (int i = 0; i < 2; i++) persons[i].SetActive(false);
        persons[data.person].SetActive(true);
        text.text = data.text; todo_text.text = data.todo; reward_text.text = data.reward;

        //state변경 및 content update
        daily_quest_list[key].state = 1;
        quest_contents_update();

        popup.SetActive(true);
    }

    public void quest_popup_close()
    {
       popup.SetActive(false);
    }

    //일일 퀘스트 제공
    public void give_daily_quest()
    {   //임시 퀘스트
        if (day == 1 && Haenyeo.hp < 90)
        {
            PlayerPrefs.SetInt("day", 2);

            Daily_quest_form day_quest = daily_quest.daily_quest1;
            show_quest_box(day_quest); // 화면 어두워지고 텍스트창 뜨기
            daily_quest_list.Add(0, new Daily_quest_form(0,day_quest.summary, day_quest.type, day_quest.person, day_quest.text,day_quest.todo,day_quest.reward));
            quest_contents_update();
        }
    }

    //화면 어두워지면서 퀘스트 주는 거
    public void show_quest_box(Daily_quest_form day_quest)
    {
        quest_bg.SetActive(true);
        quest_giver[day_quest.person].SetActive(true);
        quest_box.gameObject.SetActive(true);
        hilight_text.gameObject.SetActive(true);
        box_text.text = day_quest.text;
        hilight_text.text = day_quest.summary;
        quest_box_touch.SetActive(true);
    }

    public void quest_box_close()
    {
        quest_box_touch.gameObject.SetActive(false);
        quest_bg.gameObject.SetActive(false);
        for (int i = 0; i < 3; i++) quest_giver[i].SetActive(false);
        quest_box.gameObject.SetActive(false);
    }
    
}
