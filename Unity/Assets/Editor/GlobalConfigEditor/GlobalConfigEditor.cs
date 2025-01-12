﻿using System.IO;
using Model;
using UnityEditor;
using UnityEngine;

namespace MyEditor
{
    public class GlobalProtoEditor: EditorWindow
    {
        const string path = @".\Assets\streamingAssets\GlobalProto.txt";

        private GlobalProto globalProto;

        [MenuItem("Tools/全局配置")]
        public static void ShowWindow()
        {
            GetWindow<GlobalProtoEditor>();
        }

        public void Awake()
        {
            if (File.Exists(path))
            {
                this.globalProto = MongoHelper.FromJson<GlobalProto>(File.ReadAllText(path));
            }
            else
            {
                this.globalProto = new GlobalProto();
            }
        }

        public void OnGUI()
        {
            this.globalProto.AssetBundleServerUrl = EditorGUILayout.TextField("资源路径:", this.globalProto.AssetBundleServerUrl);
            this.globalProto.Address = EditorGUILayout.TextField("服务器地址:", this.globalProto.Address);
            this.globalProto.RealmAddress = EditorGUILayout.TextField("登录服务器地址:", this.globalProto.RealmAddress);
            this.globalProto.GateAddress = EditorGUILayout.TextField("网管服务器地址:", this.globalProto.GateAddress);

            if (GUILayout.Button("保存"))
            {
                File.WriteAllText(path, MongoHelper.ToJson(this.globalProto));
                AssetDatabase.Refresh();
            }
        }
    }
}
