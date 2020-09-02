using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Tutorial_Serialization
{

    [SerializeField] List<Tutorial_quest_form> tutorial_list;    //진행중인 퀘스트 목록
    public List<Tutorial_quest_form> toList() { return tutorial_list; }

    public Tutorial_Serialization(List<Tutorial_quest_form> tutorial_list)
    {
        this.tutorial_list = tutorial_list;
    }
}

[Serializable]
public class Daily_Serialization
{

    [SerializeField] List<Daily_quest_form> daily_list;    //진행중인 퀘스트 목록
    public List<Daily_quest_form> toList() { return daily_list; }

    public Daily_Serialization(List<Daily_quest_form> daily_list)
    {
        this.daily_list = daily_list;
    }
}

[System.Serializable]
public class Tutorial_quest_form
{
    [SerializeField] public int state;   //0. new 1. ing 2. done -1. 
    [SerializeField] public string summary;
    [SerializeField] public string type;

    public Tutorial_quest_form(int state, string type, string summary)
    {
        this.state = state;
        this.type = type;
        this.summary = summary;
    }

}

[System.Serializable]
public class Daily_quest_form
{
    [SerializeField] public int state;//0. new 1. ing 2. done, -1 미확인 퀘스트
    [SerializeField] public string summary;
    [SerializeField] public string type;
    [SerializeField] public int person; //0. 사채업자 1. 아빠 2. 상인아저씨
    [SerializeField] public string text; // 말풍선 텍스트
    [SerializeField] public string todo; // 해야할 일
    [SerializeField] public string reward;   // 보상

    public Daily_quest_form(int state, string summary, string type, int person, string text, string todo, string reward)
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

public class quest_Data : MonoBehaviour
{
    [SerializeField] public static List<Tutorial_quest_form> tutorial_quest_list = new List<Tutorial_quest_form>();  //튜토리얼 퀘스트 리스트
    [SerializeField] public static List<Daily_quest_form> daily_quest_list = new List<Daily_quest_form>();  //일일 퀘스트 리스트

    public void newStart()
    {
        //튜토리얼 퀘스트 추가
        tutorial_quest_list.Add(new Tutorial_quest_form(-1, "사채업자의 빚재촉", "5일동안 20만원 갚기"));  
        tutorial_quest_list.Add(new Tutorial_quest_form(-1, "아빠의 가르침", "바다에서 자원 1개 이상 채집하기"));
        tutorial_quest_list.Add(new Tutorial_quest_form(-1, "아빠의 가르침", "자원 양식하기"));
        tutorial_quest_list.Add(new Tutorial_quest_form(-1, "아빠의 가르침", "상인 아저씨와 대화하기"));

        //일일 퀘스트 추가
        daily_quest_list.Add(new Daily_quest_form(0,"조개 3개, 미역2개 채집하기","오늘의 점심 식사",1,"점심식사 맛나게 해보자꾸나","조개 /5\t미역 /2","체력보충제"));
        
        //첫 튜토 시작
        GameObject.Find("tutorial_quest").GetComponent<tutorial_quest>().Sache1();

    }
}
