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

    //빚 관련
    public enum quest_debt
    {
        debt1=500000,
        debt2=2000000,
        debt3=5000000
    }

    public enum quest_endDay
    {
        endDay1=5,
        endDay2=10,
        endDay3=15
    }

    public void newStart()
    {
        tutorial_quest_list.Clear();
        daily_quest_list.Clear();

        //튜토리얼 퀘스트 추가
        tutorial_quest_list.Add(new Tutorial_quest_form(-1, "사채업자의 빚재촉", "5일동안 최소 50만원 이상 상환하기"));  
        tutorial_quest_list.Add(new Tutorial_quest_form(-1, "아빠의 가르침", "바다에서 자원 1개 이상 채집하기"));
        tutorial_quest_list.Add(new Tutorial_quest_form(-1, "아빠의 가르침", "자원 양식하기"));
        tutorial_quest_list.Add(new Tutorial_quest_form(-1, "아빠의 가르침", "상점 방문하기"));

        //일일 퀘스트 추가
        daily_quest_list.Add(new Daily_quest_form(-1,"조개 5개 채집하기","제철 해산물",2, "과일도 제철 과일이 맛있고,\n해산물도 제철 해산물이 맛있지.\n그런고로, 오늘은 조개를 가져오면 짭짤하게 쳐주마!\n그럼 오늘도 잘 부탁한다, 해녀야~~","","2만원"));
        daily_quest_list.Add(new Daily_quest_form(-1, "양식 미역 3번 수확하기", "양식의 달인", 1, "해녀 생활을 훌륭하게 잘 해내고 있구나\n양식 하는 건 아직 미숙하던데\n미역 3번만 수확해 보겠니?", "", "3만원"));
        daily_quest_list.Add(new Daily_quest_form(-1, "새우 2개, 꽃게 1개 채집하기", "오늘의 점심", 1, "해녀야,, 오늘따라 얼큰한 해물라면이 끌리지 않니..?\n몇 가지 재료만 가져다 주면 아빠가 끓여줄게..\n그럼.. 부탁할게..", "", "5만원"));

        //첫 튜토 시작
        GameObject.Find("tutorial_quest").GetComponent<tutorial_quest>().Sache1();

    }
}
