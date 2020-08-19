using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sea_coin_spots : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 10f);
    }
}
