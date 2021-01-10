/*
 * 対象モデルの概要を表示するUnityエディタ拡張
 *
 * (C)2021 slip
 * This software is released under the MIT License.
 * http://opensource.org/licenses/mit-license.php
 * [Twitter]: https://twitter.com/kjmch2s/
 *
 * 利用規約：
 *  作者に無断で改変、再配布が可能で、利用形態（商用、18禁利用等）
 *  についても制限はありません。
 *  このスクリプトはもうあなたのものです。
 * 
 */

using System.Text;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.IO;

public class SummarizeMeshProperty: ScriptableWizard
{

    [SerializeField]
    //対象になるモデル
    private GameObject targetModel;

    private int materials = 0;
    private int vertices = 0;
    private int polygons = 0; 

    public static void CreateWizard()
    {
        var wiz = ScriptableWizard.DisplayWizard<SummarizeMeshProperty>(
            "SummarizeMeshProperty", "Show");
        var go = Selection.activeObject as GameObject;

    }

    void OnWizardCreate()
    {
        //SkinnedMeshRenderer
        SkinnedMeshRenderer[] skinnedMeshRenderers
         = targetModel.GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach(SkinnedMeshRenderer skin in skinnedMeshRenderers){
            //マテリアル数
            materials += skin.sharedMaterials.Length;
            //頂点数
            vertices += skin.sharedMesh.vertices.Length;
            //ポリゴン数
            polygons += skin.sharedMesh.triangles.Length / 3;

        }

        //MeshRenderer
        MeshRenderer[] meshRenderers
         = targetModel.GetComponentsInChildren<MeshRenderer>();

        foreach(MeshRenderer renderer in meshRenderers){
            //マテリアル数
            materials += renderer.sharedMaterials.Length;
        }

        //MeshFilter
        MeshFilter[] meshFilters
         = targetModel.GetComponentsInChildren<MeshFilter>();

        foreach(MeshFilter filter in meshFilters){
            //頂点数
            vertices += filter.sharedMesh.vertices.Length;
            //ポリゴン数
            polygons += filter.sharedMesh.triangles.Length / 3;
        }

        EditorUtility.DisplayDialog("Summary", "materials/マテリアル数：" + materials
        + "\nverts/頂点数       ：" + vertices
        + "\npolygons/ポリゴン数：" + polygons, "OK");
    }    

    void OnDestroy(){
        //終了時の処理を記載する
        //DestroyImmediate(m_Wizard);
    }   
}

public static class SummarizeMeshPropertyMenu
{
    const string ADD_OPTIONOBJECT_KEY = "MeshProperty/Show";

    [MenuItem(ADD_OPTIONOBJECT_KEY)]
    private static void Menu()
    {
        SummarizeMeshProperty.CreateWizard();
    }
}
