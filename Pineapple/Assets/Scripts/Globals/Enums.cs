﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enums
{
    public enum Weight { lite = 5, medium = 10, heavy = 20 };
    public enum BubbleSize { sm, md, lg };
    public enum TouchState { pressedRight, pressedLeft, tapped, none };
    public enum InteractColor { none, activate, deactivate };
    public enum ObstacleSpawnPoint { top, mid, bot};
}
