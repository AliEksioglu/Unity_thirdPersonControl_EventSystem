using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class SpecialZone : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            //other.GetComponentInParent<Player>()?.IncreaseHP();
            EventManager.Fire_OnSpecialZoneTrigger();
        }
    }


}
