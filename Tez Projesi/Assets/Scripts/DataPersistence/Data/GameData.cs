using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class GameData
{
    // Oyundaki ipucular� say�sal olarak tutulmas� i�in...
    public static int totalInfo = 7;
    public static int collectedInfo;
    // Oyundaki skorun say�sal olarak tutulmas� i�in...
    public static int levelScore = 0;
    public static int necessaryScore = 100;
    // Tamamlanan level say�s�n�n tutulmas� i�in...
    public static int completedLevel = 0;
    // Test i�in...
    public static bool isFailed;

}