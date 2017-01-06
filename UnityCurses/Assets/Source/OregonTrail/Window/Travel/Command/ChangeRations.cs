﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;
using System.Text;
using Assets.Source.Engine.Utility;
using Assets.Source.Engine.Window;
using Assets.Source.Engine.Window.Form;
using Assets.Source.OregonTrail.Entity.Person;

namespace Assets.Source.OregonTrail.Window.Travel.Command
{
    /// <summary>
    ///     Allows the player to change the amount of food their party members will have access to in a given day, the purpose
    ///     of which is to limit the amount they take in to slow the loss of food per pound. This has many affects on the
    ///     simulation such as disease, chance for breaking body parts, and or complete death from starvation.
    /// </summary>
    [ParentWindow(typeof(Travel))]
    public sealed class ChangeRations : Form<TravelInfo>
    {
        /// <summary>
        ///     Builds up the ration information and selection text.
        /// </summary>
        private StringBuilder _ration;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ChangeRations" /> class.
        ///     This constructor will be used by the other one
        /// </summary>
        /// <param name="window">The window.</param>
        public ChangeRations(IWindow window) : base(window)
        {
        }

        /// <summary>
        ///     Fired after the state has been completely attached to the simulation letting the state know it can browse the user
        ///     data and other properties below it.
        /// </summary>
        public override void OnFormPostCreate()
        {
            base.OnFormPostCreate();

            _ration = new StringBuilder();
            _ration.AppendLine(string.Format("{0}Change food rations", Environment.NewLine));
            _ration.AppendLine(
                string.Format("(currently \"{0}\"){1}",
                    OregonTrailApp.Instance.Vehicle.Ration.ToDescriptionAttribute(), Environment.NewLine));
            _ration.AppendLine("The amount of food the people in");
            _ration.AppendLine("your party eat each day can");
            _ration.AppendLine(string.Format("change. These amounts are:{0}", Environment.NewLine));
            _ration.AppendLine("1. filling - meals are large and");
            _ration.AppendLine(string.Format("   generous.{0}", Environment.NewLine));
            _ration.AppendLine("2. meager - meals are small, but");
            _ration.AppendLine(string.Format("   adequate.{0}", Environment.NewLine));
            _ration.AppendLine("3. bare bones - meals are very");
            _ration.Append("   small, everyone stays hungry.");
        }

        /// <summary>
        ///     Returns a text only representation of the current game Windows state. Could be a statement, information, question
        ///     waiting input, etc.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public override string OnRenderForm()
        {
            return _ration.ToString();
        }

        /// <summary>Fired when the game Windows current state is not null and input buffer does not match any known command.</summary>
        /// <param name="input">Contents of the input buffer which didn't match any known command in parent game Windows.</param>
        public override void OnInputBufferReturned(string input)
        {
            switch (input.ToUpperInvariant())
            {
                case "1":
                    OregonTrailApp.Instance.Vehicle.ChangeRations(RationLevel.Filling);
                    ClearForm();
                    break;
                case "2":
                    OregonTrailApp.Instance.Vehicle.ChangeRations(RationLevel.Meager);
                    ClearForm();
                    break;
                case "3":
                    OregonTrailApp.Instance.Vehicle.ChangeRations(RationLevel.BareBones);
                    ClearForm();
                    break;
                default:
                    SetForm(typeof(ChangeRations));
                    break;
            }
        }
    }
}