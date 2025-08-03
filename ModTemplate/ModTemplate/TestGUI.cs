using System;
using UnityEngine;

namespace ModTemplate
{
    public class TestGUI : MonoBehaviour
    {
        public string text = "";
        
        public void OnGUI()
        {
            GUI.Label(new Rect(0,0,Screen.width,Screen.height),text);
        }
    }
}