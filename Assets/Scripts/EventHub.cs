using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventHub
{
    public static event Action OnHPChange;
    public static event Action OnLose;

    public static void RaiseLose()
        => OnLose?.Invoke();

    public static void ChangeHP()
        => OnHPChange?.Invoke();
}
