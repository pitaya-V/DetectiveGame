#region HeadComments
// ********************************************************************
//  Copyright (C) 2017 DefaultCompany
//  作    者：V_XSLIAO-PC0
//  文件路径：Assets/Editor/ScriptTemplate/ReplaceScriptTemplates.cs
//  创建日期：2017/02/13 10:06:18
//  功能描述：替换脚本模板
//
// *********************************************************************
#endregion

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class ReplaceScriptTemplates
{
    private const string EDITOR_SCRIPTS_TEMPLATE_PATH = "GameMain/Scripts/Editor/ScriptTemplate/Templates";

    //遍历文件
    public static FileInfo[] TraverseFiles(string dir, string searchPattern = "*")
    {
        if (!Directory.Exists(dir))
            return null;

        DirectoryInfo dirInfo = new DirectoryInfo(dir);
        FileInfo[] fileInfos = dirInfo.GetFiles(searchPattern, SearchOption.AllDirectories);
        return fileInfos;
    }

    static ReplaceScriptTemplates()
    {
        bool isReplaceScriptTemplates = PlayerPrefs.GetInt("IsReplaceScriptTemplates", 0) == 1;
        if (isReplaceScriptTemplates)
            return;

        string editorScriptTemplatesPath = string.Format("{0}/{1}", Application.dataPath, EDITOR_SCRIPTS_TEMPLATE_PATH);
        string appScriptTemplatesPath = string.Format("{0}/{1}",
            EditorApplication.applicationContentsPath, "Resources/ScriptTemplates");

        FileInfo[] fileInfos = ReplaceScriptTemplates.TraverseFiles(editorScriptTemplatesPath);
        if (fileInfos != null)
        {
            List<string> editorScriptTemplatePaths = new List<string>();
            for (int i = 0; i < fileInfos.Length; i++)
            {
                FileInfo fileInfo = fileInfos[i];
                if (fileInfo == null)
                    continue;

                string path = fileInfo.FullName.Replace("\\", "/");
                string extension = Path.GetExtension(path);
                if (extension != ".txt")
                    continue;

                editorScriptTemplatePaths.Add(path);
            }

            for (int i = 0; i < editorScriptTemplatePaths.Count; i++)
            {
                string editorScriptTemplatePath = editorScriptTemplatePaths[i];
                string scriptTemplateFileName = Path.GetFileName(editorScriptTemplatePath);

                string appScriptTemplatePath = string.Format("{0}/{1}", appScriptTemplatesPath, scriptTemplateFileName);
                if (!File.Exists(appScriptTemplatePath))
                    continue;

                //string leftMD5 = NssFile.GetFileMD5(editorScriptTemplatePath);
                //string rightMD5 = NssFile.GetFileMD5(appScriptTemplatePath);
                //if (leftMD5 != rightMD5)
                try
                {
                    File.Copy(editorScriptTemplatePath, appScriptTemplatePath, true);
                }
                catch (System.Exception)
                {
                    
                    Debug.LogError(string.Format("没有管理员权限，请手动复制{0}到{1}文件夹",editorScriptTemplatePath,appScriptTemplatePath));
                }
                    
            }

            PlayerPrefs.SetInt("IsReplaceScriptTemplates", 1);
        }
    }
}