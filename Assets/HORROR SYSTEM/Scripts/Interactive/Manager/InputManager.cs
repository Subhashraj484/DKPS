using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteractionSystem
{
    public class InputManager : MonoBehaviour
    {
        [Header("MAIN BUTTON")]
        [Space(10)]
        public KeyCode mainButton;

        [Header("FLASHLIGHT BUTTON")]
        [Space(10)]
        public KeyCode flashlightSwitch;
        public KeyCode reloadBattery;

        [Header("MACHETE BUTTON")]
        [Space(10)]
        public KeyCode machete;

        [Header("TREATMENT BUTTON")]
        [Space(10)]
        public KeyCode treatment;

        [Header("RUNNNING BUTTON")]
        [Space(10)]
        public KeyCode running;

        [Header("MESHRENDER BUTTON")]
        [Space(10)]
        public KeyCode meshRender;


        public static InputManager instance;

        private void Awake()
        {
            if (instance != null) 
            { 
                Destroy(gameObject); 
            }
            else 
            { 
                instance = this; 
            }
        }
    }
}
