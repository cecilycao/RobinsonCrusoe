using UnityEngine;
//using UnityEditor;

public class Texture_Change : MonoBehaviour
{
    //人物MeshRender
    public GameObject PlayerMeshRender;
    //走路贴图
    public Texture[] Walk;
    //待机贴图
    public Texture IDLE;

    public void Walk_Texture()
    {
        Material PlayerMAT = PlayerMeshRender.GetComponent<MeshRenderer>().material;
        PlayerMAT.SetTexture("_BaseMap", Walk[0]);

        Animator PlayerAnim = PlayerMeshRender.GetComponent<Animator>();
        PlayerAnim.SetBool("IsWalk", true);
    }

    public void IDLE_Texture()
    {
        Material PlayerMAT = PlayerMeshRender.GetComponent<MeshRenderer>().material;
        PlayerMAT.SetTexture("_BaseMap", IDLE);

        Animator PlayerAnim = PlayerMeshRender.GetComponent<Animator>();
        PlayerAnim.SetBool("IsWalk", false);
    }

    public GameObject Island;
    public Transform parent;
    public void Creat()
    {
        Instantiate(Island, parent);
    }
}

//[CustomEditor(typeof(Texture_Change))]
//public class Texture_Change_Editor : Editor {
//    public override void OnInspectorGUI() {
//        DrawDefaultInspector();
//        Texture_Change myScript = (Texture_Change)target;
//        if(GUILayout.Button("Walk")) {
//            myScript.Walk_Texture();
//        }
//        if(GUILayout.Button("IDLE")) {
//            myScript.IDLE_Texture();
//        }
//        if(GUILayout.Button("CreatIsland")) {
//            myScript.Creat();
//        }
//    }
//}