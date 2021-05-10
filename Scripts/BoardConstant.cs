﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoardConstant 
{ 
  public static int currentLevel = 0;

    // Start is called before the first frame update
  public static int[] level1 = new int[81] {
        0, 0, 0, 0, 0, 1, 1, 1, 1,
        1, 1, 1, 1, 0, 1, 1, 1, 1,
        1, 1, 1, 1, 0, 1, 1, 1, 1,
        1, 1, 1, 1, 0, 1, 1, 1, 1,
        1, 1, 1, 1, 0, 1, 1, 1, 1,
        1, 1, 1, 1, 0, 1, 1, 1, 1,
        1, 1, 1, 1, 0, 1, 1, 1, 1,
        1, 0, 0, 0, 0, 0, 0, 0, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1
  };
   public static int[] level2 = new int[81] {
        0, 0, 0, 0, 0, 1, 1, 1, 1,
        1, 1, 1, 1, 0, 1, 1, 1, 1,
        1, 1, 1, 1, 0, 0, 0, 0, 0,
        1, 1, 1, 1, 0, 1, 1, 1, 0,
        1, 1, 1, 1, 0, 0, 0, 0, 0,
        1, 1, 1, 1, 0, 1, 1, 1, 1,
        1, 1, 1, 1, 0, 1, 1, 1, 1,
        1, 0, 0, 0, 0, 0, 0, 0, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1
  };
   public static int[] level3 = new int[81] {
        0, 0, 0, 0, 0, 0, 0, 0, 0,
        1, 1, 1, 1, 1, 1, 1, 1, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 1, 1, 1, 1, 1, 1, 1, 1,
        0, 0, 0, 0, 0, 0, 0, 0, 0,
        1, 1, 1, 1, 1, 1, 1, 1, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 1, 1, 1, 1, 1, 1, 1, 1,
        0, 0, 0, 0, 0, 0, 0, 0, 0,
  };
  public static int[] level4 = new int[81] {
        0, 0, 0, 0, 0, 1, 1, 1, 1,
        1, 1, 1, 1, 0, 1, 1, 1, 1,
        1, 1, 1, 1, 0, 1, 1, 1, 1,
        1, 1, 1, 1, 0, 1, 1, 1, 1,
        1, 1, 1, 1, 0, 1, 1, 1, 1,
        1, 1, 1, 1, 0, 1, 1, 1, 1,
        1, 1, 1, 1, 0, 1, 1, 1, 1,
        1, 0, 0, 0, 0, 0, 0, 0, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1
  };
    public static int[] level5 = new int[81] {
        0, 0, 0, 0, 0, 1, 1, 1, 1,
        1, 1, 1, 1, 0, 1, 1, 1, 1,
        1, 1, 1, 1, 0, 1, 1, 1, 1,
        1, 1, 1, 1, 0, 1, 1, 1, 1,
        1, 1, 1, 1, 0, 1, 1, 1, 1,
        1, 1, 1, 1, 0, 1, 1, 1, 1,
        1, 1, 1, 1, 0, 1, 1, 1, 1,
        1, 0, 0, 0, 0, 0, 0, 0, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1
  };
  public static List<int[]> levelList = new List<int[]> { level1, level2, level3, level4, level5 };

}