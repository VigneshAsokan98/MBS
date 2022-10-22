using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class ObjectController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Indicator"))
        {
            GameManager.instance.Colliding(true);
            Debug.LogError("OBJECT HIT");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Indicator"))
        {
            GameManager.instance.Colliding(false);
            Debug.LogError("OBJECT Exit");
        }
    }
}
