using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : FighterComponent
{
    public PartType slot;
    public string partName = "Part 1";
    public Ability[] abilities = new Ability[2];
}