using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenKeyboard : MonoBehaviour
{
    public TouchScreenKeyboard keyboard;

    public void OpenKeyboardInHololens()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false);
    }
}