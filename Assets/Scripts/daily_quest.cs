using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class daily_quest : MonoBehaviour
{
    static int daily;

    // Start is called before the first frame update
    void Start()
    {
        daily = PlayerPrefs.GetInt("daily", 1);
    }

    // Update is called once per frame
    void Update()
    {
        give_quest();
    }

    public void give_quest() {
        //임시 일일 퀘스트
        if (daily==1&& Haenyeo.hp < 95) {

        }
    }
}
