using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class sea_catch : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static GameObject target;
    public static List<GameObject> targets;
    public Image block_touch;
    public sea_item[] sea_item;
    public sea_item shell, seaweed, starfish, shrimp, jellyfish, crab, octopus, abalone, turtle;
    public GameObject obj, gagebar_bg, gagebar_in, gagebar_ball, fail_string, catch_string, catch_double_string, player, ink1, ink2, ink3, net; //obj는 제어할 게임오브젝트(sea)
    public sea component; //제어할 스크립트 타입의 참조변수
    public float catch_min, catch_max, speed;
    public bool catchable;
    public int difficulty, idx; //채집할 자원의 인덱스
    public AudioSource item_catch, item_fail, ink_1, ink_2, net_sound1;

    void Start()
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

        targets = new List<GameObject>();

        catchable = true;
        gagebar_ball.transform.localPosition = new Vector3(-0.0f, 0.0f, -2.0f); //게이지바 구슬 중앙으로
        gagebar_bg.gameObject.SetActive(false);
        fail_string.gameObject.SetActive(false);
        catch_string.gameObject.SetActive(false);
        catch_double_string.gameObject.SetActive(false);
        ink1.gameObject.SetActive(false);
        ink2.gameObject.SetActive(false);
        ink3.gameObject.SetActive(false);
        net.gameObject.SetActive(false);
        component = obj.GetComponent<sea>();
        item_catch.volume = PlayerPrefs.GetFloat("Effect_volume", 1);
        item_fail.volume = PlayerPrefs.GetFloat("Effect_volume", 1);
        ink_1.volume = PlayerPrefs.GetFloat("Effect_volume", 1);
        ink_2.volume = PlayerPrefs.GetFloat("Effect_volume", 1);
        net_sound1.volume = PlayerPrefs.GetFloat("Effect_volume", 1)/2;
    }

    void Update()
    {
        //터치 중 충돌 벗어날 때 게이지바 없애기 위함
        if (target == null)
            gagebar_bg.gameObject.SetActive(false);
    }

    public void item_net()
    {
        Haenyeo.item_inven[0] -= 1;

        targets = new List<GameObject>();

        StartCoroutine(start_net());
        StartCoroutine(net_effect1());
    }

    public IEnumerator start_net()
    {
        net_sound1.PlayOneShot(net_sound1.clip);
        yield return StartCoroutine(wait(5f));

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

    public IEnumerator net_effect1()
    {
        float scale = 0f;
        int i = 0;

        net.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        net.gameObject.SetActive(true); //그물 활성화
        catchable = false;

        while (i <= 50)
        {
            yield return new WaitForSeconds(0.01f);
            net.transform.localScale = new Vector3(0.5f + scale, 0.5f + scale, 1);
            scale += 0.01f;
            i++;
            if (i == 50) // 그물 제일 커졌을 때
            {
                net_effect2(); // 범위 안에 있는 자원들 한번에 잡기
                yield return new WaitForSeconds(0.1f);
            }
        }
        net.gameObject.SetActive(false); //그물 비활성화
    }

    public void net_effect2() 
    {
        //그물 벗어난 것, 중복 요소 제거
        targets = targets.Where(x => x.GetComponent<Animator>().GetBool("collided") == true).ToList();
        targets = targets.Distinct().ToList();

        // 그물 범위 내의 자원들 찍어보기 (나중에 삭제)
        for (int i = 0; i < targets.Count; i++)
            Debug.Log(targets[i].GetComponent<sea_item>().item_name);

        // 거품 순차적으로 터지는 효과
        StartCoroutine("net_effect3");

        // 그물 안에 있는 것들만 먹기
        for (int j = 0; j < targets.Count; j++)
        {
            target = targets[j];

            // 아이템 인덱스 알아내기
            for (int i = 0; i < sea_item.Length; i++)
            {
                if (target.gameObject.GetComponent<sea_item>().item_name == sea_item[i].name)
                    idx = i;
            }

            if (sea.is_double)
            {
                component.item_num[idx] += 2; //sea스크립트 상의 해녀 그물망에 추가
                Haenyeo.sea_item_number[idx] += 2; //해녀의 보유 아이템 개수에 추가
            }
            else
            {
                component.item_num[idx] += 1; //sea스크립트 상의 해녀 그물망에 추가
                Haenyeo.sea_item_number[idx] += 1; //해녀의 보유 아이템 개수에 추가
            }
        }
    }

    public IEnumerator net_effect3()
    {
        if (targets.Count == 0) // 그물 안에 뭐 없으면
            catchable = true; // 바로 터치 가능

        for (int j = 0; j < targets.Count; j++)
        {
            target = targets[j];
            StartCoroutine("bubble"); 
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if ((target != null) && catchable)
        {
            difficulty = target.GetComponent<sea_item>().difficulty; //채집난이도 받아오기
            show_gagebar(difficulty); //난이도에 따라 게이지바 띄우기
            target.GetComponent<sea_spots>().targeted = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if ((target != null) && catchable)
        {
            if (target.GetComponent<sea_spots>().targeted == true)
            {
                gagebar_bg.gameObject.SetActive(false); //게이지바 비활성화
                catch_or_fail(difficulty); //난이도에 따라 성공, 실패 판정 및 등등
            }
        }
    }

    public void show_gagebar(int difficulty)
    {
        gagebar_ball.transform.localPosition = new Vector3(-0.0f, 0.0f, -2.0f); //게이지바 구슬 원위치

        switch (difficulty)
        {
            case 1:
                gagebar_in.transform.localScale = new Vector3(3.3f, 1.0f, 1.0f);
                gagebar_bg.gameObject.SetActive(true);
                break;
            case 2:
                gagebar_in.transform.localScale = new Vector3(2.3f, 1.0f, 1.0f);
                gagebar_bg.gameObject.SetActive(true);

                break;
            case 3:
                gagebar_in.transform.localScale = new Vector3(1.8f, 1.0f, 1.0f);
                gagebar_bg.gameObject.SetActive(true);

                break;
            case 4:
                gagebar_in.transform.localScale = new Vector3(1.2f, 1.0f, 1.0f);
                gagebar_bg.gameObject.SetActive(true);
                break;
        }
    }

    public void catch_or_fail(int difficulty)
    {
        float pos = gagebar_ball.transform.localPosition.x;

        //아이템의 인덱스 알아내기 
        for (int i = 0; i < sea_item.Length; i++)
        {
            if (target.gameObject.GetComponent<sea_item>().item_name == sea_item[i].name)
                idx = i;
        }

        switch (difficulty)
        {
            case 1:
                catch_min = -0.54f;
                catch_max = 1.34f;
                break;
            case 2:
                catch_min = -0.255f;
                catch_max = 1.055f;
                break;
            case 3:
                catch_min = -0.113f;
                catch_max = 0.913f;
                break;
            case 4:
                catch_min = 0.0584f;
                catch_max = 0.742f;
                break;
        }

        if (pos > catch_min && pos < catch_max) //노란색 영역의 범위에 있으면
        {
            if (sea.is_double)
            {
                component.item_num[idx] += 2; //sea스크립트 상의 해녀 그물망에 추가
                Haenyeo.sea_item_number[idx] += 2; //해녀의 보유 아이템 개수에 추가
            }
            else
            {
                component.item_num[idx] += 1; //sea스크립트 상의 해녀 그물망에 추가
                Haenyeo.sea_item_number[idx] += 1; //해녀의 보유 아이템 개수에 추가
            }

            catchable = false; //터치 불가능
            StartCoroutine("bubble"); //거품 이펙트
            StartCoroutine("catch_effect"); //성공 이펙트
        }
        else
        {
            item_fail.PlayOneShot(item_fail.clip);
            catchable = false; //터치 불가능
            StartCoroutine("fade_out"); //자원 페이드아웃
            StartCoroutine("fail_effect"); //실패 이펙트

            if (target.gameObject.GetComponent<sea_item>().item_name == "octopus") // 실패한 자원이 문어면 
            {
                StartCoroutine("octopus_ink"); //먹물 이펙트
            }
        }
    }

    public IEnumerator catch_effect()
    {
        GameObject effect_string;

        if (sea.is_double)
            effect_string = catch_double_string;
        else
            effect_string = catch_string;

        float string_time = 0.3f;
        float time = 0f;
        Vector3 string_pos = effect_string.transform.localPosition;
        string_pos.y = 0f;

        effect_string.gameObject.SetActive(true); //문구 활성화


        while (string_pos.y < 0.85)
        {
            time += Time.deltaTime / string_time;
            string_pos.y = Mathf.Lerp(-0f, 0.85f, time);
            effect_string.transform.localPosition = new Vector3(0, string_pos.y, 0);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        effect_string.gameObject.SetActive(false); //문구 비활성화
    }

    public IEnumerator bubble()
    {
        GameObject target_now = target;

        float scale = 0f;
        int i = 0;

        Transform bubble = target_now.transform.GetChild(0);
        bubble.localScale = new Vector3(1, 1, 1);
        bubble.gameObject.SetActive(true);

        while (i <= 8)
        {
            yield return new WaitForSeconds(0.05f);
            bubble.transform.localScale = new Vector3(1 + scale, 1 + scale, 1);
            scale += 0.2f;
            i++;
            if (i == 6)
            {
                item_catch.PlayOneShot(item_catch.clip);
                target_now.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0); //자원 안보이게
            }
        }

        bubble.gameObject.SetActive(false); //거품 비활성화
        yield return new WaitForSeconds(0.6f);
        target_now.GetComponent<sea_spots>().targeted = false;
        target_now.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1); //자원 다시 보이게 
        target_now.gameObject.SetActive(false); //자원 비활성화
        catchable = true; //터치 가능
    }

    public IEnumerator fail_effect()
    {
        float string_time = 0.3f;
        float time = 0f;
        Vector3 string_pos = fail_string.transform.localPosition;
        string_pos.y = 0f;
        fail_string.gameObject.SetActive(true); //문구 활성화 

        player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f); // 해녀 깜빡깜빡
        player.GetComponent<Rigidbody2D>().drag = 20; // 해녀 저항 증가 -> 속도 감소

        while (string_pos.y < 0.85)
        {
            time += Time.deltaTime / string_time;
            string_pos.y = Mathf.Lerp(-0f, 0.85f, time);
            fail_string.transform.localPosition = new Vector3(0, string_pos.y, 0);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        fail_string.gameObject.SetActive(false); //문구 비활성화

        // 해녀 깜빡깜빡
        for (int i = 0; i < 2; i++)
        {
            player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.5f);

            player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(0.5f);
        }

        player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1); // 해녀 투명도 원래대로
        player.GetComponent<Rigidbody2D>().drag = 5; // 해녀 저항 원래대로 -> 속도 원래대로
    }

    public IEnumerator octopus_ink()
    {
        GameObject[] inks = { ink1, ink2, ink3 };
        float expand_time = 0.03f;
        float fade_time = 0.5f;
        float time;

        // 먹물 작은 거 두개 커지면서 튀기기
        for (int i = 0; i < 2; i++)
        {
            ink_1.PlayOneShot(ink_1.clip);
            time = 0f;
            inks[i].gameObject.SetActive(true);
            while (inks[i].transform.localScale.x < 50f)
            {
                time += Time.deltaTime / expand_time;
                float scale = Mathf.Lerp(1f, 50f, time);
                inks[i].transform.localScale = new Vector3(scale, scale, 1);
                yield return null;
            }
            yield return new WaitForSeconds(0.15f);
        }

        // 먹물 큰 거 한개 커지면서 튀기기
        ink_2.PlayOneShot(ink_2.clip);
        time = 0f;
        ink3.gameObject.SetActive(true);
        while (ink3.transform.localScale.x < 100f)
        {
            time += Time.deltaTime / expand_time;
            float scale = Mathf.Lerp(1f, 100f, time);
            ink3.transform.localScale = new Vector3(scale, scale, 1);
            yield return null;
        }

        yield return new WaitForSeconds(2f); // 먹물 유지하는 시간

        // 먹물 차례로 페이드아웃
        for (int i = 0; i < 3; i++)  
        {
            time = 0f;

            Color fade_color = inks[i].GetComponent<SpriteRenderer>().color;
            while (fade_color.a > 0)
            {
                time += Time.deltaTime / fade_time;
                fade_color.a = Mathf.Lerp(1f, 0f, time);
                inks[i].GetComponent<SpriteRenderer>().color = fade_color;
                yield return null;
            }
            inks[i].gameObject.SetActive(false); // 먹물 비활성화 
            inks[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1); // 먹물 투명도 복구
        }

        // 먹물 크기 복구 
        ink1.transform.localScale = new Vector3(20, 20, 1); 
        ink2.transform.localScale = new Vector3(20, 20, 1); 
        ink3.transform.localScale = new Vector3(50, 50, 1); 
    }

    public IEnumerator fade_out()
    {
    GameObject target_now = target;

    float fade_time = 1f;
    float time = 0f;
    Color fade_color = target_now.GetComponent<SpriteRenderer>().color;

    while (fade_color.a > 0)
    {
        time += Time.deltaTime / fade_time;
        fade_color.a = Mathf.Lerp(1f, 0f, time);
        target_now.GetComponent<SpriteRenderer>().color = fade_color;
        yield return null;
    }

    target_now.GetComponent<sea_spots>().targeted = false;
    target_now.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    target_now.gameObject.SetActive(false); //자원 비활성화
    catchable = true; //터치 가능
    }
}
