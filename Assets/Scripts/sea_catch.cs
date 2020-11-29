using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class sea_catch : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static GameObject target;
    public sea_item[] sea_item;
    public sea_item shell, seaweed, starfish, shrimp, jellyfish, crab, octopus, abalone, turtle;
    public GameObject obj, gagebar_bg, gagebar_in, gagebar_ball, fail_string, catch_string, catch_double_string, bubble_img, player, ink1, ink2, ink3; //obj는 제어할 게임오브젝트(sea)
    public sea component; //제어할 스크립트 타입의 참조변수
    public float catch_min, catch_max, speed;
    public bool catchable;
    public int difficulty, idx; //채집할 자원의 인덱스
    public AudioSource item_catch, item_fail, ink_1, ink_2;

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

        catchable = true;
        gagebar_ball.transform.localPosition = new Vector3(-0.0f, 0.0f, -2.0f); //게이지바 구슬 중앙으로
        gagebar_bg.gameObject.SetActive(false);
        fail_string.gameObject.SetActive(false);
        catch_string.gameObject.SetActive(false);
        catch_double_string.gameObject.SetActive(false);
        bubble_img.gameObject.SetActive(false);
        ink1.gameObject.SetActive(false);
        ink2.gameObject.SetActive(false);
        ink3.gameObject.SetActive(false);
        component = obj.GetComponent<sea>();
        item_catch.volume = PlayerPrefs.GetFloat("Effect_volume", 1);
        item_fail.volume = PlayerPrefs.GetFloat("Effect_volume", 1);
        ink_1.volume = PlayerPrefs.GetFloat("Effect_volume", 1);
        ink_2.volume = PlayerPrefs.GetFloat("Effect_volume", 1);
    }

    void Update()
    {
        //터치 중 충돌 벗어날 때 게이지바 없애기 위함
        if (target == null)
        {
            gagebar_bg.gameObject.SetActive(false);
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
            {
                idx = i;
            }
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
            if(sea.is_double)
                component.item_num[idx] += 2; //sea스크립트 상의 해녀 그물망에 추가
            else
                component.item_num[idx] += 1; //sea스크립트 상의 해녀 그물망에 추가

            Haenyeo.sea_item_number[idx] += 1; //해녀의 보유 아이템 개수에 추가
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
        float string_time = 0.3f;
        float time = 0f;
        Vector3 string_pos = catch_string.transform.localPosition;
        string_pos.y = 0f;

        //yield return new WaitForSeconds(0.5f);
        catch_string.gameObject.SetActive(true); //문구 활성화 

        while (string_pos.y < 0.85)
        {
            time += Time.deltaTime / string_time;
            string_pos.y = Mathf.Lerp(-0f, 0.85f, time);
            catch_string.transform.localPosition = new Vector3(0, string_pos.y, 0);
            yield return null;
        }

        //yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(0.5f);
        catch_string.gameObject.SetActive(false); //문구 비활성화
    }

    public IEnumerator bubble()
    {
        GameObject target_now = target;

        float scale = 0f;
        int i = 0;
        bubble_img.transform.localScale = new Vector3(100, 100, 1);

        bubble_img.transform.position = target_now.transform.position; //거품의 위치는 현재 자원의 위치
        bubble_img.gameObject.SetActive(true); //거품 활성화

        while (i <= 4)
        {
            yield return new WaitForSeconds(0.1f);
            bubble_img.transform.localScale = new Vector3(100 + scale, 100 + scale, 1);
            scale += 40;
            i++;
            if (i == 3)
            {
                item_catch.PlayOneShot(item_catch.clip);
                target_now.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0); //자원 안보이게
            }
        }

        bubble_img.gameObject.SetActive(false); //거품 비활성화
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

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
