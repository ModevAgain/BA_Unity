using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData", order = 0)]
public class GameData : ScriptableObject
{
    [Header("Duration")]
    public int GameDuration;
    public int TestDuration;

    [Header("Inputs")]
    public bool Gamepad;
    public bool MK;
    public bool Touch;


    [Header("GameMode")]
    public bool IsTest;


    public int GetGameDuration()
    {
        if (IsTest)
            return TestDuration;
        else return GameDuration;
    }

}
