using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace ModTemplate
{
    public static class Main
    {

        public static bool IsEnabled = false;
        public static UnityModManager.ModEntry.ModLogger Logger;
        public static Harmony harmony;
        public static Setting setting;
        public static TestGUI testGUI;
        
        //EntryMethod 시작할 때
        internal static void Setup(UnityModManager.ModEntry modEntry)
        {
            //설정 로드
            setting = new Setting();
            setting = UnityModManager.ModSettings.Load<Setting>(modEntry);
            
            Logger = modEntry.Logger;
            modEntry.OnToggle = OnToggle;
            
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            IsEnabled = value;
            if (value)
            {
                //모드 킬때
                harmony = new Harmony(modEntry.Info.Id);
                harmony.PatchAll(Assembly.GetExecutingAssembly());
                
                modEntry.OnGUI = OnGUI;
                modEntry.OnSaveGUI = OnSaveGUI;
                
                //UI 생성
                testGUI = new GameObject().AddComponent<TestGUI>();
                UnityEngine.Object.DontDestroyOnLoad(testGUI);
            }
            else
            {
                //UI 파괴
                if(testGUI!=null) UnityEngine.Object.DestroyImmediate(testGUI);
                testGUI = null;
                
                //모드 끌때
                harmony.UnpatchAll(modEntry.Info.Id);
            }
            return true;
        }

        private static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            //모드 설정창
            GUILayout.Label("모드 테스트");

            if (GUILayout.Button("무한~"))
            {
                Logger.Log("무야호~");
            }
        }
        
        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            //설정 저장
            setting.Save(modEntry);
        }


    }
}