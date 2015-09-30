using UnityEngine;
using System;
using System.Collections;

[Serializable]
public enum UnitType
{
    None, Ashigaru, Engineer, Gunner, Junker, Knight, Magi, Pirate, Scout, Valkyrie, Wark, Witch
}

[Serializable]
public enum Size
{
    None=0,Small=1,Medium=2,Large=3,Huge=4
}

[Serializable]
public class UnitClass
{
    public UnitType unitType = UnitType.None;
    public Size size = Size.Medium;



}
