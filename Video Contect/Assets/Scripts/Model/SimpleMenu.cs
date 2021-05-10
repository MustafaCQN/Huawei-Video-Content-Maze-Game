using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SimpleMenu<T> : Menu<T> where T : SimpleMenu<T>
{
    public static void Show()
    {
        Open();
    }

    public static void Hide()
    {
        Close();
    }
}
