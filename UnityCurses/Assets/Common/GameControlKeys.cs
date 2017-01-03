// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 06/18/2016@1:06 AM

using UnityEngine;

namespace Assets.Common
{
    public enum GameControlKeys
    {
        ///////////////////////////////////////////
        //Moving

        [DefaultKeyboardMouseValue(KeyCode.W)] [DefaultKeyboardMouseValue(KeyCode.UpArrow)] Forward,

        [DefaultKeyboardMouseValue(KeyCode.S)] [DefaultKeyboardMouseValue(KeyCode.DownArrow)] Backward,

        [DefaultKeyboardMouseValue(KeyCode.A)] [DefaultKeyboardMouseValue(KeyCode.LeftArrow)] Left,

        [DefaultKeyboardMouseValue(KeyCode.D)] [DefaultKeyboardMouseValue(KeyCode.RightArrow)] Right,

        ///////////////////////////////////////////
        //Looking

        //MouseMove (in the PlayerIntellect)
        LookUp,

        //MouseMove (in the PlayerIntellect)
        LookDown,

        //MouseMove (in the PlayerIntellect)
        LookLeft,

        //MouseMove (in the PlayerIntellect)
        LookRight,

        ///////////////////////////////////////////
        //Actions

        [DefaultKeyboardMouseValue(KeyCode.Mouse0)] Fire1,

        [DefaultKeyboardMouseValue(KeyCode.Mouse1)] Fire2,

        [DefaultKeyboardMouseValue(KeyCode.R)] Reload,

        [DefaultKeyboardMouseValue(KeyCode.E)] Use,

        [DefaultKeyboardMouseValue(KeyCode.LeftBracket)] PreviousWeapon,

        [DefaultKeyboardMouseValue(KeyCode.RightBracket)] NextWeapon,

        [DefaultKeyboardMouseValue(KeyCode.H)] Holster,

        [DefaultKeyboardMouseValue(KeyCode.Alpha0)] Weapon0,

        [DefaultKeyboardMouseValue(KeyCode.Alpha1)] Weapon1,

        [DefaultKeyboardMouseValue(KeyCode.Alpha2)] Weapon2,

        [DefaultKeyboardMouseValue(KeyCode.Alpha3)] Weapon3,

        [DefaultKeyboardMouseValue(KeyCode.Alpha4)] Weapon4,

        [DefaultKeyboardMouseValue(KeyCode.Alpha5)] Weapon5,

        ///////////////////////////////////////////
        //Dialogs

        [DefaultKeyboardMouseValue(KeyCode.Tab)] PersonalDigitalAssistant
    }
}