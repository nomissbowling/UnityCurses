// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;
using System.Text;
using Assets.Engine.Window;
using Assets.Engine.Window.Form;
using Assets.Engine.Window.Form.Input;

namespace Assets.OregonTrail.Window.Travel.Store.Help
{
    /// <summary>
    ///     If the player cannot afford to leave the store because they have attempted to purchase more items than they are
    ///     capable of carrying and or purchasing this will be displayed to inform the user they need to pay up.
    /// </summary>
    [ParentWindow(typeof(Travel))]
    public sealed class StoreDebtWarning : InputForm<TravelInfo>
    {
        /// <summary>
        ///     The store debt.
        /// </summary>
        private StringBuilder storeDebt;

        /// <summary>
        ///     Initializes a new instance of the <see cref="StoreDebtWarning" /> class.
        ///     This constructor will be used by the other one
        /// </summary>
        /// <param name="window">The window.</param>
        public StoreDebtWarning(IWindow window) : base(window)
        {
            storeDebt = new StringBuilder();
        }

        /// <summary>
        ///     Fired when dialog prompt is attached to active game Windows and would like to have a string returned.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        protected override string OnDialogPrompt()
        {
            storeDebt = new StringBuilder();
            storeDebt.AppendLine(string.Format("{0}Whoa there partner!", Environment.NewLine));
            storeDebt.AppendLine(
                string.Format("I see you got {0} items worth {1}.", UserData.Store.Transactions.Count,
                    UserData.Store.TotalTransactionCost));
            storeDebt.AppendLine(string.Format("You only got {0}!", OregonTrailApp.Instance.Vehicle.Balance));
            storeDebt.AppendLine(string.Format("Put some items back in order to leave the store...{0}",
                Environment.NewLine));
            return storeDebt.ToString();
        }

        /// <summary>
        ///     Fired when the dialog receives favorable input and determines a response based on this. From this method it is
        ///     common to attach another state, or remove the current state based on the response.
        /// </summary>
        /// <param name="reponse">The response the dialog parsed from simulation input buffer.</param>
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            SetForm(typeof(Store));
        }
    }
}