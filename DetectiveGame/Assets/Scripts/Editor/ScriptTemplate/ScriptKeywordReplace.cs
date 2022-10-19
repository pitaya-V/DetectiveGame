#region HeadComments
// ********************************************************************
//  Copyright (C) 2016 DefaultCompany
//  作    者：V_XSLIAO-PC0
//  文件路径：Assets/Editor/NssProject/ScriptKeywordReplace.cs
//  创建日期：2016/05/14 09:54:48
//  功能描述：脚本关键字替换
//
//  [修改日志]
//  修改者：V_XSLIAO-PC0 时间：2016/05/14 09:54:48 修改内容：创建脚本
//
// *********************************************************************
#endregion

using UnityEditor;
using UnityEngine;

public class ScriptKeywordReplace : UnityEditor.AssetModificationProcessor
{
    public static void OnWillCreateAsset(string path)
    {
        path = path.Replace(".meta", "");
        int index = path.LastIndexOf(".");
        if (index == -1) return;
        string file = path.Substring(index);
        if (file != ".cs") return;
        index = Application.dataPath.LastIndexOf("Assets");
        path = Application.dataPath.Substring(0, index) + path;
        file = System.IO.File.ReadAllText(path);
        path = path.Replace(Application.dataPath, "Assets");

        //file = file.Replace("#AUTHOR#", System.Environment.MachineName);
        file = file.Replace("#AUTHOR#", System.Environment.UserName);
        file = file.Replace("#FILEPATH#", path);
        file = file.Replace("#YEAR#", System.DateTime.Now.ToString("yyyy"));
        file = file.Replace("#CREATIONDATE#", System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
        file = file.Replace("#PROJECTNAME#", PlayerSettings.productName);
        file = file.Replace("#DEVELOPERS#", PlayerSettings.companyName);

        System.IO.File.WriteAllText(path, file, System.Text.Encoding.UTF8);
        AssetDatabase.Refresh();
    }
}
