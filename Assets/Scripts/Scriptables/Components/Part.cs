using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : FighterComponent
{
    public PartType partType;
    public Sprite sprite;
    public string partName = "Part 1";
    public Ability[] abilities = new Ability[2];
}