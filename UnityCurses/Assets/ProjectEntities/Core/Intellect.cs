// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 06/18/2016@1:06 AM

using System;
using System.ComponentModel;
using Assets.ProjectCommon;
using Assets.ProjectEntities.Characters;
using UnityEngine;

namespace Assets.ProjectEntities.Core
{
    /// <summary>
    ///     This takes the form of either AI (Artificial Intelligence) or player control over a unit .
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         There is inherit AI base base for an computer-controlled intellect.
    ///         For example, there is the <see cref="GameCharacterAI" /> class which is designed for the
    ///         management of a game character.
    ///     </para>
    ///     <para>
    ///         Control by a live player (<see cref="PlayerIntellect" />) is achieved through the commands
    ///         of pressed keys or the mouse for control of the unit or turret.
    ///     </para>
    /// </remarks>
    public abstract class Intellect : MonoBehaviour
    {
        private static int controlKeyCount;

        ///////////////////////////////////////////

        [SerializeField]
        private bool allowTakeItems;

        private float[] controlKeysStrength;

        [SerializeField]
        private GameObject controlledObject;

        [SerializeField]
        private Faction faction;

        public Intellect()
        {
            //calculate controlKeyCount
            if (controlKeyCount == 0)
            {
                foreach (var value in Enum.GetValues(typeof(GameControlKeys)))
                {
                    var controlKey = (GameControlKeys)value;
                    if ((int)controlKey >= controlKeyCount)
                        controlKeyCount = (int)controlKey + 1;
                }
            }

            controlKeysStrength = new float[controlKeyCount];
        }

        public bool AllowTakeItems
        {
            get { return allowTakeItems; }
            set { allowTakeItems = value; }
        }

        [Browsable(false)]
        public GameObject ControlledObject
        {
            get { return controlledObject; }
            set
            {
                var oldObject = controlledObject;
                controlledObject = value;
                ResetControlKeys();

                OnControlledObjectChange(oldObject);
            }
        }

        [Browsable(false)]
        public Faction Faction
        {
            get { return faction; }
            set { faction = value; }
        }

        public float GetControlKeyStrength(GameControlKeys key)
        {
            return controlKeysStrength[(int)key];
        }

        public bool IsControlKeyPressed(GameControlKeys key)
        {
            return GetControlKeyStrength(key) != 0.0f;
        }

        protected virtual void OnControlledObjectChange(GameObject oldObject)
        {
        }

        protected void ControlKeyPress(GameControlKeys controlKey, float strength)
        {
            if (strength <= 0.0f)
                Debug.LogError("Intellect: ControlKeyPress: Invalid \"strength\".");

            if (GetControlKeyStrength(controlKey) == strength)
                return;

            controlKeysStrength[(int)controlKey] = strength;

            //if (controlledObject != null)
            //    controlledObject.DoIntellectCommand(new Command(controlKey, true));
        }

        protected void ControlKeyRelease(GameControlKeys controlKey)
        {
            if (!IsControlKeyPressed(controlKey))
                return;

            controlKeysStrength[(int)controlKey] = 0;

            //if (controlledObject != null)
            //    controlledObject.DoIntellectCommand(new Command(controlKey, false));
        }

        private void ResetControlKeys()
        {
            for (var n = 0; n < controlKeysStrength.Length; n++)
                controlKeysStrength[n] = 0;
        }

        protected virtual void OnControlledObjectRenderFrame()
        {
        }

        public void DoControlledObjectRenderFrame()
        {
            OnControlledObjectRenderFrame();
        }

        protected virtual void OnControlledObjectRender(Camera camera)
        {
        }

        public void DoControlledObjectRender(Camera camera)
        {
            OnControlledObjectRender(camera);
        }

        public virtual bool IsActive()
        {
            return false;
        }

        public virtual bool IsAlwaysRun()
        {
            return false;
        }

        ///////////////////////////////////////////

        public struct Command
        {
            private GameControlKeys key;
            private bool keyPressed;

            internal Command(GameControlKeys key, bool keyPressed)
            {
                this.key = key;
                this.keyPressed = keyPressed;
            }

            public GameControlKeys Key
            {
                get { return key; }
            }

            public bool KeyPressed
            {
                get { return keyPressed; }
            }

            public bool KeyReleased
            {
                get { return !keyPressed; }
            }
        }
    }
}