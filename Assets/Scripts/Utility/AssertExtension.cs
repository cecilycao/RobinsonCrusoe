using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

public static class AssertExtension
{
    /// <summary>
    /// 如果var不为null，执行runCode的代码
    /// </summary>
    /// <param name="var"></param>
    /// <param name="runCode"></param>
    public static void NotNullRun(object var,Action runCode)
    {
        if (var != null)
        {
            runCode();
        }
    }
}
