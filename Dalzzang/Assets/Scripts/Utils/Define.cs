using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum UIEvent
    {
        Click,
        Drag,
        EndDrag,
    }

    public enum PointEvent
    {
        Enter,
        Exit,
    }
    public enum Scenes
    {
        Title, Game, After,
    }

    public enum Reward
    {
        Miss,
        Gem1, Gem2, Gem3, Gem4, Gem5, Gem6, Gem7, Gem8, Gem9, Gem10, Power, Speed
    }
}
