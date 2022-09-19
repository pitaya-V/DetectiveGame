﻿#region HeadComments
// ********************************************************************
//  Copyright (C) 2022 DefaultCompany
//  作    者：unity
//  文件路径：Assets/GameMain/Scripts/Procedure/BuiltinDataComponent.cs
//  创建日期：2022/09/19 14:15:19
//  功能描述：
//
// *********************************************************************
#endregion

using UnityEngine;
using System.Collections;
using GameFramework;
using UnityGameFramework.Runtime;

namespace DetectiveGame
{
    public class BuiltinDataComponent : GameFrameworkComponent 
    {
        [SerializeField]
        private TextAsset m_BuildInfoTextAsset = null;

        [SerializeField]
        private TextAsset m_DefaultDictionaryTextAsset = null;

        [SerializeField]
        //private UIUpdateResourceForm m_UpdateResourceFormTemplate = null;

        private BuildInfo m_BuildInfo = null;

        public BuildInfo BuildInfo
        {
            get
            {
                return m_BuildInfo;
            }
        }
        public void InitBuildInfo()
        {
            if (m_BuildInfoTextAsset == null || string.IsNullOrEmpty(m_BuildInfoTextAsset.text))
            {
                Log.Info("Build info can not be found or empty.");
                return;
            }

            m_BuildInfo = Utility.Json.ToObject<BuildInfo>(m_BuildInfoTextAsset.text);
            if (m_BuildInfo == null)
            {
                Log.Warning("Parse build info failure.");
                return;
            }
        }

        public void InitDefaultDictionary()
        {
            if (m_DefaultDictionaryTextAsset == null || string.IsNullOrEmpty(m_DefaultDictionaryTextAsset.text))
            {
                Log.Info("Default dictionary can not be found or empty.");
                return;
            }

            if (!GameEntry.Localization.ParseData(m_DefaultDictionaryTextAsset.text))
            {
                Log.Warning("Parse default dictionary failure.");
                return;
            }
        }
        
    }
    
}

