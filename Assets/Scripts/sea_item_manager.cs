using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]

//생성스팟 2차배열의 하위배열 
public class array_spot
{
    public GameObject[] spot;
}

public class sea_item_manager : MonoBehaviour
{
    public sea_item[] sea_item;
    public sea_item shell, seaweed, starfish, shrimp, jellyfish, crab, octopus, abalone, turtle;
    public array_spot[] sea1_spot = new array_spot[2]; //얕은바다 생성스팟 2차배열 전체
    public array_spot[] sea2_spot = new array_spot[2]; //중간바다 생성스팟 2차배열 전체
    public array_spot[] sea3_spot = new array_spot[2]; //깊은바다 생성스팟 2차배열 전체
    public int level;
    public int[] arr; //난수 저장할 배열
    public List<int> list; //빈 스팟 인덱스를 담을 리스트
    public float delay_time; //자원 리젠 시간 
    public Image block_touch;
    public AudioSource item_create;

    // 코인 
    public Transform[] coin_spot;
    public GameObject coin_gold;
    public GameObject coin_silver;
    public GameObject coin_bronze;
    public GameObject[] coin_clone;
    public float coin_time; // 코인 생성 시간 간격

    // 코인 생성 전체 
    public IEnumerator create_coins() 
    {
        int clone_idx = 0;

        yield return new WaitWhile(() => block_touch.gameObject.activeSelf); //터치 방지가 활성화되어있는 동안 대기
        yield return StartCoroutine(wait(4f)); //시작버튼 누른 후로 4초 대기

        while (true)
        {
            creat_coin(clone_idx); // 코인 생성 함수 호출

            coin_time = Haenyeo.coin_time; // 물안경 레벨에 따라 코인 생성 시간 다르게 함
            yield return StartCoroutine(wait(coin_time)); //리젠시간만큼 대기 후 다시 루프 실행
            clone_idx++;
        }

        IEnumerator wait(float delay) //delay만큼 대기 (WaitForSeconds는 타이머 일시정지가 안돼서 만듦)
        {
            float time = 0f;
            while (time < delay) //wait의 실행시간이 delay가 될때까지
            {
                yield return null; //프레임은 변하지 않으면서
                if (!block_touch.gameObject.activeSelf) //터치 방지가 비활성화 상태여야만
                {
                    time += Time.deltaTime; //시간이 가게 함
                }
            }
        }
    }

    // 코인 생성 함수
    public void creat_coin(int clone_idx)
    {
        int idx = Random.Range(0, coin_spot.Length); // 코인 스팟들 중에 랜덤으로 하나 선택
        int coin_color = Random.Range(0, 3); // 코인 색깔 중에 랜덤으로 하나 선택  
        switch (coin_color)
        {
            case 0:
                coin_clone[clone_idx] = Instantiate(coin_gold, coin_spot[idx].position, Quaternion.identity); // 선택된 스팟에 생성하는 코드
                break;
            case 1:
                coin_clone[clone_idx] = Instantiate(coin_silver, coin_spot[idx].position, Quaternion.identity); // 선택된 스팟에 생성하는 코드
                break;
            case 2:
                coin_clone[clone_idx] = Instantiate(coin_bronze, coin_spot[idx].position, Quaternion.identity); // 선택된 스팟에 생성하는 코드
                break;
        }
    }

    void Update()
    {
        for (int i = 0; i < coin_clone.Length; i++) // 생성된 모든 코인 클론에 대해
        {
            if (!block_touch.gameObject.activeSelf && coin_clone[i] != null) //터치 방지가 비활성화 상태이고 코인이 생성돼있을때만
            {
                coin_clone[i].transform.Translate(0, -3f, 0); // 코인이 아래로 이동
            }
        }

        for (int i = 0; i < coin_clone.Length; i++) // 생성된 모든 코인 클론에 대해
        {
            if (coin_clone[i] != null && coin_clone[i].transform.position.y < -400) // 코인이 생성돼있고 화면 벗어나면 
            {
                Destroy(coin_clone[i]); // 클론 제거
            }
        }
    }

    void Start()
    {
        // 코인
        StartCoroutine(create_coins());

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

        //생성 스팟 전부 비활성화 
        for (int i = 0; i < sea1_spot.Length; i++) 
        {
            for (int j = 0; j < sea1_spot[i].spot.Length; j++) 
            {
                sea1_spot[i].spot[j].gameObject.SetActive(false);
            }
            for (int j = 0; j < sea2_spot[i].spot.Length; j++)
            {
                sea2_spot[i].spot[j].gameObject.SetActive(false);
            }
            for (int j = 0; j < sea3_spot[i].spot.Length; j++)
            {
                sea3_spot[i].spot[j].gameObject.SetActive(false);
            }
        }

        level = Haenyeo.level;

        //해녀 레벨에 따라 초기 자원 생성 및 추가 자원 생성
        if (level == 1)
        {
            create_initial(1);
            StartCoroutine(create_plus(1, 1));
        }
        if (level == 2)
        {
            create_initial(1);
            create_initial(2);
            StartCoroutine(create_plus(2, 1));
            StartCoroutine(create_plus(2, 2));
        }
        if (level == 3)
        {
            create_initial(1);
            create_initial(2);
            create_initial(3);
            StartCoroutine(create_plus(3, 1));
            StartCoroutine(create_plus(3, 2));
            StartCoroutine(create_plus(3, 3));
        }

        item_create.volume = PlayerPrefs.GetFloat("item_creat", 1);
    }

    //바다의 깊이에 따른 초기 자원 생성 함수
    public void create_initial(int sea_num)
    {
        int num1, num2, num3;

        switch (sea_num)
        {
            case 1:
                {
                    //sea1_no moving에서 난수 3개 중복 없이 뽑아 조개,미역,불가사리 생성
                    arr = get_random(3, 0, sea1_spot[0].spot.Length);
                    get_values_sea1(0, arr[0], 0);
                    sea1_spot[0].spot[arr[0]].gameObject.SetActive(true);
                    get_values_sea1(0, arr[1], 1);
                    sea1_spot[0].spot[arr[1]].gameObject.SetActive(true);
                    get_values_sea1(0, arr[2], 2);
                    sea1_spot[0].spot[arr[2]].gameObject.SetActive(true);

                    //sae1_moving에서 난수 2개 중복 없이 뽑아 새우, 해파리 생성
                    arr = get_random(2, 0, sea1_spot[1].spot.Length);
                    get_values_sea1(1, arr[0], 3);
                    sea1_spot[1].spot[arr[0]].gameObject.SetActive(true);
                    get_values_sea1(1, arr[1], 4);
                    sea1_spot[1].spot[arr[1]].gameObject.SetActive(true);

                    //움직임 여부, 생성스팟, 생성자원 순으로 난수 뽑아 자원 1개 생성
                    num1 = Random.Range(0,2);
                    if (num1 == 0)
                    {
                        do
                        { num2 = Random.Range(0, sea1_spot[0].spot.Length); } while (sea1_spot[0].spot[num2].activeSelf); //생성스팟 뽑을 때 빈 자리 나올때까지 반복
                        do
                        { num3 = Random.Range(0, sea_item.Length); } while (num3 != 0 && num3 != 1 && num3 != 2); //생성자원 뽑을 때 얕은바다, 안움직이는거 나올때까지 반복
                        get_values_sea1(num1, num2, num3);
                        sea1_spot[0].spot[num2].gameObject.SetActive(true);
                    }
                    else
                    {
                        do
                        { num2 = Random.Range(0, sea1_spot[1].spot.Length); } while (sea1_spot[1].spot[num2].activeSelf); //생성스팟 뽑을 때 빈 자리 나올때까지 반복
                        do
                        { num3 = Random.Range(0, sea_item.Length); } while (num3 != 3 && num3 != 4); //생성자원 뽑을 때 얕은바다, 움직이는거 나올때까지 반복
                        get_values_sea1(num1, num2, num3);
                        sea1_spot[1].spot[num2].gameObject.SetActive(true);
                    }
                    break;
                }
            case 2 :
                {
                    //sea2_no moving에서 난수 3개 중복 없이 뽑아 조개,미역,꽃게 생성
                    arr = get_random(3, 0, sea2_spot[0].spot.Length);
                    get_values_sea2(0, arr[0], 0);
                    sea2_spot[0].spot[arr[0]].gameObject.SetActive(true);
                    get_values_sea2(0, arr[1], 1);
                    sea2_spot[0].spot[arr[1]].gameObject.SetActive(true);
                    get_values_sea2(0, arr[2], 5);
                    sea2_spot[0].spot[arr[2]].gameObject.SetActive(true);

                    //sae2_moving에서 난수 2개 중복없이 뽑아 새우,문어 생성 
                    arr = get_random(2, 0, sea2_spot[1].spot.Length);
                    get_values_sea2(1, arr[0], 3);
                    sea2_spot[1].spot[arr[0]].gameObject.SetActive(true);
                    get_values_sea2(1, arr[1], 6);
                    sea2_spot[1].spot[arr[1]].gameObject.SetActive(true);

                    //움직임 여부, 생성스팟, 생성자원 순으로 난수 뽑아 자원 1개 생성
                    num1 = Random.Range(0, 2);
                    if (num1 == 0)
                    {
                        do
                        { num2 = Random.Range(0, sea2_spot[0].spot.Length); } while (sea2_spot[0].spot[num2].activeSelf); //생성스팟 뽑을 때 빈 자리 나올때까지 반복
                        do
                        { num3 = Random.Range(0, sea_item.Length); } while (num3 != 0 && num3 != 1 && num3 != 5); //생성자원 뽑을 때 중간바다, 안움직이는거 나올때까지 반복
                        get_values_sea2(num1, num2, num3);
                        sea2_spot[0].spot[num2].gameObject.SetActive(true);
                    }
                    else
                    {
                        do
                        { num2 = Random.Range(0, sea2_spot[1].spot.Length); } while (sea2_spot[1].spot[num2].activeSelf); //생성스팟 뽑을 때 빈 자리 나올때까지 반복
                        do
                        { num3 = Random.Range(0, sea_item.Length); } while (num3 != 3 && num3 != 6); //생성자원 뽑을 때 중간바다, 움직이는거 나올때까지 반복 
                        get_values_sea2(num1, num2, num3);
                        sea2_spot[1].spot[num2].gameObject.SetActive(true);
                    }
                    break;
                }

            case 3 :
                {
                    //sea3_no moving에서 난수 3개 중복 없이 뽑아 조개,미역, 전복 생성
                    arr = get_random(3, 0, sea1_spot[0].spot.Length);
                    get_values_sea3(0, arr[0], 0);
                    sea3_spot[0].spot[arr[0]].gameObject.SetActive(true);
                    get_values_sea3(0, arr[1], 1);
                    sea3_spot[0].spot[arr[1]].gameObject.SetActive(true);
                    get_values_sea3(0, arr[2], 7);
                    sea3_spot[0].spot[arr[2]].gameObject.SetActive(true);

                    //sae3_moving에서 난수 2개 중복 없이 뽑아 거북이,새우 생성
                    arr = get_random(2, 0, sea3_spot[1].spot.Length);
                    get_values_sea3(1, arr[0], 8);
                    sea3_spot[1].spot[arr[0]].gameObject.SetActive(true);
                    get_values_sea3(1, arr[1], 3);
                    sea3_spot[1].spot[arr[1]].gameObject.SetActive(true);

                    //num2 = Random.Range(0, sea3_spot[1].spot.Length);
                    //get_values_sea3(1, num2, 8);
                    //sea3_spot[1].spot[num2].gameObject.SetActive(true);

                    //움직임 여부는 무조건 0(바다스팟 두개엔 거북이, 새우 넣으면 끝), 생성스팟, 생성자원 순으로 난수 뽑아 자원 1개 생성
                    //num1 = Random.Range(0, 2);
                    num1 = 0;
                    //if (num1 == 0)
                    //{
                        do
                        { num2 = Random.Range(0, sea3_spot[0].spot.Length); } while (sea3_spot[0].spot[num2].activeSelf); //생성스팟 뽑을 때 빈 자리 나올때까지 반복
                        do
                        { num3 = Random.Range(0, sea_item.Length); } while (num3 != 0 && num3 != 1 && num3 != 7); //생성자원 뽑을 때 깊은바다, 안움직이는거 나올때까지 반복
                        get_values_sea3(num1, num2, num3);
                        sea3_spot[0].spot[num2].gameObject.SetActive(true);
                    //}

                    //else if ((num1 == 1) && (sea3_spot[1].spot[0].activeSelf) && (sea3_spot[1].spot[1].activeSelf)) //움직이는 자원 뽑혔고, 바다에 자리가 없는 경우
                    //{
                    //    num1 = 0; //안움직이는 자원으로 변경하고 num==0 일 때처럼 동작
                    //    do
                    //    { num2 = Random.Range(0, sea3_spot[0].spot.Length); } while (sea3_spot[0].spot[num2].activeSelf); //생성스팟 뽑을 때 빈 자리 나올때까지 반복
                    //    do
                    //    { num3 = Random.Range(0, sea_item.Length); } while (num3 != 0 && num3 != 1 && num3 != 7); //생성자원 뽑을 때 깊은바다, 안움직이는거 나올때까지 반복
                    //    get_values_sea3(num1, num2, num3);
                    //    sea3_spot[0].spot[num2].gameObject.SetActive(true);
                    //}
                    //else 
                    //{
                    //    do
                    //    { num2 = Random.Range(0, sea3_spot[1].spot.Length); } while (sea3_spot[1].spot[num2].activeSelf); //생성스팟 뽑을 때 빈 자리 나올때까지 반복
                    //    do
                    //    { num3 = Random.Range(0, sea_item.Length); } while (num3 != 3 && num3 != 8); //생성자원 뽑을 때 깊은바다, 움직이는거 나올때까지 반복 
                    //    get_values_sea3(num1, num2, num3);
                    //    sea3_spot[1].spot[num2].gameObject.SetActive(true);
                    //}

                    break;
                }
        }
    }

    //바다의 깊이에 따른 추가 자원 생성 함수
    public void create_plus_item(int sea_num)
    {
        int num1, num2, num3;

        switch (sea_num)
        {
            case 1:
                num3 = what_to_create(sea_num); //바다 깊이에 따라 생성할 자원 선택하는 함수 호출

                //선택된 자원 num3의 움직임 여부인 num1
                if (!sea_item[num3].moving) //안움직이는 자원이면
                {
                    num1 = 0;
                }
                else //움직이는 자원이면
                {
                    num1 = 1;
                }

                //움직임 여부 num1에 해당하는 스팟들의 활성화 여부를 확인하고 빈 스팟만 리스트에 담기
                list = new List<int>(); //빈 스팟 인덱스를 담을 리스트
                for (int i = 0; i < sea1_spot[num1].spot.Length; i++) //해당 스팟들의 개수만큼 반복
                {
                    if (!sea1_spot[num1].spot[i].activeSelf) //활성화 안돼있으면
                    {
                        list.Add(i); //리스트에 추가
                    }
                }

                if (list.Count > 0) //리스트의 개수가 0이 아니면 (빈 스팟이 있으면)
                {
                    do
                    { num2 = Random.Range(0, sea1_spot[num1].spot.Length); } while (sea1_spot[num1].spot[num2].activeSelf); //빈 스팟 나올 때까지 반복해 뽑은 스팟 인덱스 num2
                    get_values_sea1(num1, num2, num3);
                    sea1_spot[num1].spot[num2].gameObject.SetActive(true);
                    item_create.PlayOneShot(item_create.clip);
                }
                else //빈 스팟이 없으면
                {
                    break;
                }

                break;

            case 2:
                num3 = what_to_create(sea_num); //바다 깊이에 따라 생성할 자원 선택하는 함수 호출

                //선택된 자원 num3의 움직임 여부인 num1
                if (!sea_item[num3].moving) //안움직이는 자원이면
                {
                    num1 = 0;
                }
                else //움직이는 자원이면
                {
                    num1 = 1;
                }

                //움직임 여부 num1에 해당하는 스팟들의 활성화 여부를 확인하고 빈 스팟만 리스트에 담기
                list = new List<int>(); //빈 스팟 인덱스를 담을 리스트
                for (int i = 0; i < sea2_spot[num1].spot.Length; i++) //해당 스팟들의 개수만큼 반복
                {
                    if (!sea2_spot[num1].spot[i].activeSelf) //활성화 안돼있으면
                    {
                        list.Add(i); //리스트에 추가
                    }
                }

                if (list.Count > 0) //리스트의 개수가 0이 아니면 (빈 스팟이 있으면)
                {
                    do
                    { num2 = Random.Range(0, sea2_spot[num1].spot.Length); } while (sea2_spot[num1].spot[num2].activeSelf); //빈 스팟 나올 때까지 반복해 뽑은 스팟 인덱스 num2
                    get_values_sea2(num1, num2, num3);
                    sea2_spot[num1].spot[num2].gameObject.SetActive(true);
                    item_create.PlayOneShot(item_create.clip);
                }
                else //빈 스팟이 없으면
                {
                    break;
                }

                break;

            case 3:
                num3 = what_to_create(sea_num); //바다 깊이에 따라 생성할 자원 선택하는 함수 호출

                //선택된 자원 num3의 움직임 여부인 num1
                if (!sea_item[num3].moving) //안움직이는 자원이면
                {
                    num1 = 0;
                }
                else //움직이는 자원이면
                {
                    num1 = 1;
                }

                //움직임 여부 num1에 해당하는 스팟들의 활성화 여부를 확인하고 빈 스팟만 리스트에 담기
                list = new List<int>(); //빈 스팟 인덱스를 담을 리스트
                for (int i = 0; i < sea3_spot[num1].spot.Length; i++) //해당 스팟들의 개수만큼 반복
                {
                    if (!sea3_spot[num1].spot[i].activeSelf) //활성화 안돼있으면
                    {
                        list.Add(i); //리스트에 추가
                    }
                }

                if (list.Count > 0) //리스트의 개수가 0이 아니면 (빈 스팟이 있으면)
                {
                    do
                    { num2 = Random.Range(0, sea3_spot[num1].spot.Length); } while (sea3_spot[num1].spot[num2].activeSelf); //빈 스팟 나올 때까지 반복해 뽑은 스팟 인덱스 num2
                    get_values_sea3(num1, num2, num3);
                    sea3_spot[num1].spot[num2].gameObject.SetActive(true);
                    item_create.PlayOneShot(item_create.clip);
                }
                else //빈 스팟이 없으면
                {
                    break;
                }

                break;
        }
    }

    //해녀의 레벨과 바다의 깊이에 따라 자원을 추가 생성하는 코루틴
    public IEnumerator create_plus(int level, int sea_num)
    {
        yield return new WaitWhile(() => block_touch.gameObject.activeSelf); //터치 방지가 활성화되어있는 동안 대기
        yield return StartCoroutine(wait(4f)); //시작버튼 누른 후로 4초 대기

        while (true)
        {
            create_plus_item(sea_num); //바다의 깊이에 따라 추가 자원 생성 함수 호출
            delay_time = Random.Range((float)(level + 1), (float)(level + 3)); //추가 자원 생성 함수를 호출할 때마다 리젠시간을 현 레벨의 범위 내에서 랜덤하게
            yield return StartCoroutine(wait(delay_time)); //리젠시간만큼 대기 후 다시 루프 실행
        }

        IEnumerator wait(float delay) //delay만큼 대기 (WaitForSeconds는 타이머 일시정지가 안돼서 만듦)
        {
            float time = 0f;
            while (time < delay) //wait의 실행시간이 delay가 될때까지
            {
                yield return null; //프레임은 변하지 않으면서
                if (!block_touch.gameObject.activeSelf) //터치 방지가 비활성화 상태여야만
                {
                    time += Time.deltaTime; //시간이 가게 함
                }
            }
        }
    }

    //얕은바다 자원의 변수값들 가져오기 
    public void get_values_sea1(int moving_idx, int spot_idx, int item_idx)
    {
        sea1_spot[moving_idx].spot[spot_idx].GetComponent<sea_item>().item_name = sea_item[item_idx].item_name;
        sea1_spot[moving_idx].spot[spot_idx].GetComponent<sea_item>().sea1_prob = sea_item[item_idx].sea1_prob;
        sea1_spot[moving_idx].spot[spot_idx].GetComponent<sea_item>().sea2_prob = sea_item[item_idx].sea2_prob;
        sea1_spot[moving_idx].spot[spot_idx].GetComponent<sea_item>().sea3_prob = sea_item[item_idx].sea3_prob;
        sea1_spot[moving_idx].spot[spot_idx].GetComponent<sea_item>().moving = sea_item[item_idx].moving;
        sea1_spot[moving_idx].spot[spot_idx].GetComponent<sea_item>().difficulty = sea_item[item_idx].difficulty;
        sea1_spot[moving_idx].spot[spot_idx].GetComponent<Animator>().runtimeAnimatorController = sea_item[item_idx].anim.runtimeAnimatorController;
    }

    //중간바다 자원의 변수값들 가져오기 
    public void get_values_sea2(int moving_idx, int spot_idx, int item_idx)
    {
        sea2_spot[moving_idx].spot[spot_idx].GetComponent<sea_item>().item_name = sea_item[item_idx].item_name;
        sea2_spot[moving_idx].spot[spot_idx].GetComponent<sea_item>().sea1_prob = sea_item[item_idx].sea1_prob;
        sea2_spot[moving_idx].spot[spot_idx].GetComponent<sea_item>().sea2_prob = sea_item[item_idx].sea2_prob;
        sea2_spot[moving_idx].spot[spot_idx].GetComponent<sea_item>().sea3_prob = sea_item[item_idx].sea3_prob;
        sea2_spot[moving_idx].spot[spot_idx].GetComponent<sea_item>().moving = sea_item[item_idx].moving;
        sea2_spot[moving_idx].spot[spot_idx].GetComponent<sea_item>().difficulty = sea_item[item_idx].difficulty;
        sea2_spot[moving_idx].spot[spot_idx].GetComponent<Animator>().runtimeAnimatorController = sea_item[item_idx].anim.runtimeAnimatorController;
    }

    //깊은바다 자원의 변수값들 가져오기 
    public void get_values_sea3(int moving_idx, int spot_idx, int item_idx)
    {
        sea3_spot[moving_idx].spot[spot_idx].GetComponent<sea_item>().item_name = sea_item[item_idx].item_name;
        sea3_spot[moving_idx].spot[spot_idx].GetComponent<sea_item>().sea1_prob = sea_item[item_idx].sea1_prob;
        sea3_spot[moving_idx].spot[spot_idx].GetComponent<sea_item>().sea2_prob = sea_item[item_idx].sea2_prob;
        sea3_spot[moving_idx].spot[spot_idx].GetComponent<sea_item>().sea3_prob = sea_item[item_idx].sea3_prob;
        sea3_spot[moving_idx].spot[spot_idx].GetComponent<sea_item>().moving = sea_item[item_idx].moving;
        sea3_spot[moving_idx].spot[spot_idx].GetComponent<sea_item>().difficulty = sea_item[item_idx].difficulty;
        sea3_spot[moving_idx].spot[spot_idx].GetComponent<Animator>().runtimeAnimatorController = sea_item[item_idx].anim.runtimeAnimatorController;
    }

    //생성확률을 고려해 생성할 자원을 선택하기 
    public int what_to_create(int sea_num)
    {
        float[] arr_prob = new float[9]; //자원 9개의 생성 확률을 담을 배열 
        for (int i = 0; i < arr_prob.Length; i++) //바다 깊이에 따른 자원 9개의 생성확률을 arr_prob에 담기
        {
            if (sea_num == 1)
            {
                arr_prob[i] = sea_item[i].sea1_prob;
            }
            else if (sea_num == 2)
            {
                arr_prob[i] = sea_item[i].sea2_prob;
            }
            else
            {
                arr_prob[i] = sea_item[i].sea3_prob;
            }
        }

        float total = 1f; //확률 합은 항상 1
        float random_point = Random.value * total; //확률들을 이은 선상의 랜덤한 점 추출 

        for (int i = 0; i < arr_prob.Length; i++) //추출된 점이 어떤 요소의 확률범위 안에 있는지 계산하기
        {
            if (random_point < arr_prob[i])
            {
                return i; //해당 요소의 인덱스를 반환
            }
            else
            {
                random_point -= arr_prob[i]; 
            }
        }
        return arr_prob.Length - 1; //Random_point가 1인 경우 마지막 요소의 인덱스를 반환
    }

    //중복 없이 난수 뽑기
    public int[] get_random(int length, int min, int max)
    {
        int[] randArray = new int[length];
        bool isSame;

        for (int i = 0; i < length; ++i)
        {
            while (true)
            {
                randArray[i] = Random.Range(min, max);
                isSame = false;
                for (int j = 0; j < i; ++j)
                {
                    if (randArray[j] == randArray[i])
                    { 
                        isSame = true;
                        break;
                    }
                }
                if (!isSame)
                    break;
            }
        }
        return randArray;
    }
}