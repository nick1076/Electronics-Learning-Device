using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="New Mode", fileName="newMode")]
public class ModeData : ScriptableObject
{
    public string modeName = "Mode Name";
    public List<string> permittedWindows = new List<string>();
}
