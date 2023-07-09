using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerController.Instance.IsHolding == true) return;
            transform.parent = PlayerController.Instance.Hands.transform;
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles= new(0f,90f,0f);
            PlayerController.Instance.ChangePlayerHolding(true);
        }
    }
}
