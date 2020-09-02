using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class quest_manager : MonoBehaviour
{
    public GameObject new_exist,quest_icon;
    
    //퀘스트 content 관련 오브젝트
    public GameObject content_parent, content;
    public Text summary_text, type_text;
    public Image touch_x,touch;
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
    }

    void Update()
    {
        //give_daily_quest(); //일일 퀘스트 제공
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
        for(int i=0;i<quest_Data.tutorial_quest_list.Count;i++)
        {
            Tutorial_quest_form data = quest_Data.tutorial_quest_list[i];

            if (data.state != -1)   //퀘스트 목록에 추가할 퀘스트라면
            {
                for (int j = 0; j < 3; j++) quest_states[i].gameObject.SetActive(false);
                finish_inactive.gameObject.SetActive(false);
                finish_active.gameObject.SetActive(false);
                touch.gameObject.SetActive(false);
                //퀘스트 state
                quest_states[data.state].gameObject.SetActive(true);
                touch_x.gameObject.SetActive(true);

                type_text.text = data.type; summary_text.text = data.summary;

                GameObject temp_content = Instantiate(content, new Vector3(0, 0, 0), Quaternion.identity);
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
                if (data.state == 0)
                {
                    new_exist.SetActive(true);
                    StartCoroutine("quest_icon_effect");
                }
                for (int j = 0; j < 3; j++) quest_states[i].gameObject.SetActive(false);
                finish_inactive.gameObject.SetActive(false);
                finish_active.gameObject.SetActive(false);
                touch_x.gameObject.SetActive(false);
                touch.gameObject.SetActive(true);
                quest_states[data.state].gameObject.SetActive(true);
                if (data.state == 2) finish_active.gameObject.SetActive(true);
                else finish_inactive.gameObject.SetActive(true);

                type_text.text = data.type; summary_text.text = data.summary;

                //content와 popup복제해서 부모 설정하기
                GameObject temp_content = Instantiate(content, new Vector3(0, 0, 0), Quaternion.identity);
                temp_content.transform.SetParent(content_parent.transform);
                temp_content.name = i.ToString();      //이름을 인덱스로 설정
                temp_content.SetActive(true);
            }
        }

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
        //클릭한 오브젝트 찾아서 key 찾기
        string content_name = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.name;
        int idx = Int32.Parse(content_name);

        Daily_quest_form data=quest_Data.daily_quest_list[idx];
        for (int i = 0; i < 3; i++) persons[i].SetActive(false);
        persons[data.person].SetActive(true);
        text.text = data.text; todo_text.text = data.todo; reward_text.text = data.reward;

        //state변경 및 content update
        quest_Data.daily_quest_list[idx].state = 1;
        quest_contents_update();

        popup.SetActive(true);
    }

    public void quest_popup_close()
    {
       popup.SetActive(false);
    }

    //일일 퀘스트 제공
    public void give_daily_quest()
    {  
         Daily_quest_form day_quest = new Daily_quest_form(1,"2","3",1,"2","3","1");        //임시, 수정
         show_quest_box(day_quest); // 화면 어두워지고 텍스트창 뜨기
         
         quest_contents_update();
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

    //새로운 퀘스트 있을 때 아이콘 깜박깜박
    IEnumerator quest_icon_effect()
    {
        int time = 0;
        while (time < 5)
        {
            quest_icon.gameObject.transform.Rotate(Vector3.back * 10);
            yield return new WaitForSeconds(0.2f);
            quest_icon.gameObject.transform.Rotate(Vector3.forward * 10);
            yield return new WaitForSeconds(0.2f);
            time++;
        }
    }
}
