//对Volume 组件进行动态修改的示例文件
using UnityEngine;
using UnityEngine.Rendering;//必须引用
using System.Collections.Generic;

public class PostProcessController : MonoBehaviour
{
    //Volume组件
     public Volume PostProcess;
     //Volume组件下的Profile
     private List<VolumeComponent> Profiles;

    private void Start()
    {
        //这里展示获取profile中的Override
        Profiles = PostProcess.profile.components;
        foreach(VolumeComponent file in Profiles)
        {
            Debug.Log(file.ToString());//输出Override的名字
        }
    }

    // Update is called once per frame
    void Update()
    {
        //动态修改Profile的权重的测试
        float weigth = Mathf.PingPong(Time.time, 1);
        PostProcess.weight = weigth;
    }
}


