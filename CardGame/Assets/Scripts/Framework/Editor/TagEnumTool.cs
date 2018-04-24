using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AMG.Framework;
using System.IO;

namespace AMG.Framework.Tools
{
    public class TagEnumTool : EditorWindow
    {
        [SerializeField]
        string[] m_stringEnum;
        [SerializeField]
        TextAsset m_targetEnumFile = null;

        [MenuItem("AMG/Entity/Enum Tool")]
        public static void OpenWindow()
        {
            TagEnumTool window = (TagEnumTool)EditorWindow.GetWindow(typeof(TagEnumTool));
            window.titleContent.text = "Entity tag tool";
            window.Show();
        }

        private void BuildStringArray()
        {
            m_stringEnum = Enum.GetNames(typeof(TagEnum));
        }

        private void OnEnable()
        {
            BuildStringArray();
        }

        private void OnGUI()
        {
            // "target" can be any class derrived from ScriptableObject 
            // (could be EditorWindow, MonoBehaviour, etc)
            ScriptableObject target = this;
            SerializedObject so = new SerializedObject(target);
            SerializedProperty stringsProperty = so.FindProperty("m_stringEnum");

            EditorGUILayout.PropertyField(stringsProperty, true); // True means show children
            
            so.ApplyModifiedProperties(); // Remember to apply modified properties

            if (GUILayout.Button("Write Enum"))
            {
                WriteEnum();
            }
        }

        private void WriteEnum()
        {
            string assetPath = AssetDatabase.GetAssetPath(m_targetEnumFile);
            string namespaceName = "AMG.Framework";
            string enumName = "TagEnum";


            using (StreamWriter streamWriter = new StreamWriter(assetPath))
            {
                streamWriter.WriteLine("namespace " + namespaceName );
                streamWriter.WriteLine("{");
                streamWriter.WriteLine("\tpublic enum " + enumName);
                streamWriter.WriteLine("\t{");
                for (int i = 0; i < m_stringEnum.Length; i++)
                {
                    streamWriter.WriteLine("\t\t" + m_stringEnum[i] + ",");
                }
                streamWriter.WriteLine("\t}");
                streamWriter.WriteLine("}");
            }
            AssetDatabase.Refresh();
        }
    }
}