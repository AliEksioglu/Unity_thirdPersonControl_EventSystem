using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public static class EventManager 
{
    public static event Action OnSpecialZoneTrigger;
    public static void Fire_OnSpecialZoneTrigger() { OnSpecialZoneTrigger?.Invoke(); }

    public static event Action<float, float> OnPlayerHPChanged;
    public static void Fire_OnPlayerHPChanged(float currentHP , float maxHP) { OnPlayerHPChanged?.Invoke(currentHP, maxHP); }

    public static event Action<Vector3> OnPlayerPosChanged;
    public static void Fire_OnPlayerPosChanged(Vector3 wordlPos) { OnPlayerPosChanged?.Invoke(wordlPos); }

}
