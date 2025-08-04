using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ModTemplate
{
    /// <summary>
    /// Minimal in-game camera timeline editor inspired by the standalone
    /// pygame prototype.  When the ChartEditor scene loads a controller is
    /// spawned which renders a very small IMGUI based timeline allowing
    /// keyframes to be added, moved and previewed.  The implementation is
    /// intentionally lightweight but mirrors the Python data model so that
    /// additional functionality can be ported over easily.
    /// </summary>
    public static class CameraTimeline
    {
        // Data model ---------------------------------------------------------
        public class Keyframe
        {
            public int time;      // milliseconds
            public Vector2 pos;
            public float zoom;
            public float angle;
            public string ease = "Linear";
        }

        public class Track
        {
            public readonly List<Keyframe> keyframes = new List<Keyframe>();
            public int? selectedIndex;

            public Keyframe Current
            {
                get => selectedIndex.HasValue ? keyframes[selectedIndex.Value] : null;
            }

            public void Add(int time, Vector2 pos, float zoom, float angle)
            {
                var kf = new Keyframe { time = time, pos = pos, zoom = zoom, angle = angle };
                keyframes.Add(kf);
                keyframes.Sort((a,b)=>a.time.CompareTo(b.time));
                selectedIndex = keyframes.IndexOf(kf);
            }
        }

        // Runtime controller -------------------------------------------------
        public class Controller : MonoBehaviour
        {
            public Track track = new Track();
            bool playing;
            float currentMs;

            void Update()
            {
                if (playing)
                    currentMs += Time.unscaledDeltaTime * 1000f;
            }

            void OnGUI()
            {
                GUILayout.BeginArea(new Rect(10,10,300,120), "CameraTimeline", GUI.skin.window);
                GUILayout.Label($"Keyframes: {track.keyframes.Count}");
                if (GUILayout.Button("Add keyframe"))
                {
                    track.Add((int)currentMs, Vector2.zero, 100f, 0f);
                }
                if (GUILayout.Button(playing?"Pause":"Play")) playing=!playing;
                GUILayout.EndArea();
            }
        }

        // Runtime registration ---------------------------------------------
        static GameObject current;

        public static void Enable()
        {
            SceneManager.activeSceneChanged += OnSceneChanged;
        }

        public static void Disable()
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;
            if (current != null) UnityEngine.Object.DestroyImmediate(current);
            current = null;
        }

        static void OnSceneChanged(Scene prev, Scene next)
        {
            if (next.name == "ChartEditor")
            {
                if (current == null)
                {
                    current = new GameObject("CameraTimeline");
                    UnityEngine.Object.DontDestroyOnLoad(current);
                    current.AddComponent<Controller>();
                }
            }
            else if (current != null)
            {
                UnityEngine.Object.Destroy(current);
                current = null;
            }
        }
    }
}
