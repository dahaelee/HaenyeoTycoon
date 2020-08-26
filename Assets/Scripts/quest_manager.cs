using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quest_Content
{
    public int state;   //0. new 1. ing 2. done
    public string summary;
    public string type;
    public int finish; //0. inactive 1. active  
    public int touchable; //0. dont touch 1.can touch

    public Quest_Content(int state, string type, string summary,int touchable)
    {
        this.state = state;
        this.type = type;
        this.summary = summary;
        this.touchable = touchable;
    }
}

public class Quest_Popup
{
    public int person; //0. 사채업자 1. 아빠 2. 상인아저씨
    public string text; // 말풍선 텍스트
    public string todo; // 해야할 일
    public string state; // 현재 진행 상황
    public string reward;   // 보상

    public Quest_Popup(int person, string text, string todo, string reward)
    {
        this.person = person;
        this.text = text;
        this.todo = todo;
        this.reward = reward;
    }
}

public class quest_manager : MonoBehaviour
{
    public static SortedList<int, Quest_Content> quest_contents = new SortedList<int, Quest_Content>();   // 퀘스트 목록
    public GameObject new_exist;

    //퀘스트 content 관련 오브젝트
    public GameObject content_parent, content;
    public Text summary_text, type_text;
    public Image touch_x;
    public Image[] quest_states;
    public Button finish_active, finish_inactive;

    //퀘스트 popup 관련 오브젝트
    public GameObject popup_ui;
    public GameObject[] persons; //상반식 캐릭터
    public Text text, todo_text, reward_text;

    public void quest_contents_update()
    {
        new_exist.gameObject.SetActive(false);

        //기존 자식 오브젝트들 삭제하고 다시 생성
        for (int i = 0; i < content_parent.transform.childCount; i++)
        {
            Destroy(content_parent.transform.GetChild(i).gameObject);
        }
        content_parent.transform.DetachChildren();

        foreach (var tmp in quest_contents)
        {
            Quest_Content data = tmp.Value;

            if (data.state == 0) new_exist.gameObject.SetActive(true);  //새로운 퀘스트 있으면 퀘스트 아이콘에 ! 뜸
            
            finish_active.gameObject.SetActive(false);
            finish_inactive.gameObject.SetActive(false);
            for (int i = 0; i < 3; i++) quest_states[i].gameObject.SetActive(false);
            touch_x.gameObject.SetActive(false);

            //popup창 안뜨는 퀘스트
            if (data.touchable == 0) touch_x.gameObject.SetActive(true);

            else
            {
                if (data.state == 3) finish_active.gameObject.SetActive(true);
                else finish_inactive.gameObject.SetActive(true);
            }

            //퀘스트 state
            quest_states[data.state].gameObject.SetActive(true);

            type_text.text = data.type; summary_text.text = data.summary;

            GameObject temp_content = Instantiate(content, new Vector3(0, 0, 0), Quaternion.identity);
            temp_content.transform.SetParent(content_parent.transform);
            temp_content.SetActive(true);
        }
    }

    public void quest_popup_open(Quest_Popup quest_popup)
    {
        popup_ui.gameObject.SetActive(true);

        // 캐릭터 상반신 띄우기
        for (int i = 0; i < 3; i++) persons[i].gameObject.SetActive(false);
        persons[quest_popup.person].gameObject.SetActive(true);

        text.text = quest_popup.text;
        todo_text.text = quest_popup.todo;
        reward_text.text = quest_popup.reward;
    }
}
