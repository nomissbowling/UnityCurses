// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;
using System.Text;
using Assets.Maxwolf.OregonTrail.Entity;
using Assets.Maxwolf.WolfCurses.Window;
using Assets.Maxwolf.WolfCurses.Window.Form;
using Assets.Maxwolf.WolfCurses.Window.Form.Input;

namespace Assets.Maxwolf.OregonTrail.Window.Travel.Hunt
{
    /// <summary>
    ///     Tabulates results about the hunting session after it ends, depending on the performance of the player and how many
    ///     animals they killed, if any will be calculated in weight. Players can only ever take 100 pounds of meat back to
    ///     the vehicle so this discourages mass killing.
    /// </summary>
    [ParentWindow(typeof(Travel))]
    public sealed class HuntingResult : InputForm<TravelInfo>
    {
        /// <summary>
        ///     References the total weight of all the animals the player killed so we only have to reference it once.
        /// </summary>
        private int _finalKillWeight;

        /// <summary>
        ///     Holds all of the data for our final hunting result before rendering out to player.
        /// </summary>
        private StringBuilder _huntScore;

        /// <summary>
        ///     Initializes a new instance of the <see cref="InputForm{T}" /> class.
        ///     This constructor will be used by the other one
        /// </summary>
        /// <param name="window">The window.</param>
        public HuntingResult(IWindow window) : base(window)
        {
            _huntScore = new StringBuilder();
        }

        /// <summary>
        ///     Fired after the state has been completely attached to the simulation letting the state know it can browse the user
        ///     data and other properties below it.
        /// </summary>
        public override void OnFormPostCreate()
        {
            base.OnFormPostCreate();

            // After hunting we roll the dice on the party and player and skip a day.
            OregonTrailApp.Instance.TakeTurn(false);
        }

        /// <summary>
        ///     Fired when dialog prompt is attached to active game Windows and would like to have a string returned.
        /// </summary>
        /// <returns>
        ///     The dialog prompt text.<see cref="string" />.
        /// </returns>
        protected override string OnDialogPrompt()
        {
            // Clear previous hunting score information.
            _huntScore = new StringBuilder();

            // Calculate total weight of all animals killed by player during hunt.
            var killWeight = UserData.Hunt.KillWeight;

            // Depending on kill weight we change response and message.
            if (killWeight <= 0)
            {
                _huntScore.AppendLine(string.Format("{0}You were unable to shoot any", Environment.NewLine));
                _huntScore.AppendLine(string.Format("food.{0}", Environment.NewLine));
            }
            else if (killWeight > 0)
            {
                // Message to let the player know they killed prey.
                _huntScore.AppendLine(string.Format("{0}From the animals you shot, you", Environment.NewLine));
                _huntScore.AppendLine(string.Format("got {0:N0} pounds of meat.{1}", killWeight,
                    Environment.NewLine));

                // Adds the killing weight since it is safe at this point.
                _finalKillWeight = killWeight;

                // Player can only take MAXFOOD amount from hunt regardless of total weight.
                if (killWeight <= HuntManager.MAXFOOD)
                    return _huntScore.ToString();

                // Forces the weight of the kill to become
                _finalKillWeight = HuntManager.MAXFOOD;

                // Player killed to many animals.
                _huntScore.AppendLine("However, you were only able to");
                _huntScore.AppendLine(string.Format("carry {0} pounds back to the", _finalKillWeight));
                _huntScore.AppendLine(string.Format("wagon.{0}", Environment.NewLine));
            }

            // Return the hunting result to text renderer.
            return _huntScore.ToString();
        }

        /// <summary>
        ///     Fired when the dialog receives favorable input and determines a response based on this. From this method it is
        ///     common to attach another state, or remove the current state based on the response.
        /// </summary>
        /// <param name="reponse">The response the dialog parsed from simulation input buffer.</param>
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            // Transfers the total finalized kill weight we calculated to vehicle inventory as food in pounds.
            if (_finalKillWeight > 0)
                OregonTrailApp.Instance.Vehicle.Inventory[Entities.Food].AddQuantity(_finalKillWeight);

            // Destroys all hunting related data now that we are done with it.
            UserData.DestroyHunt();

            // Returns to the travel menu so the player can continue down the trail.
            ClearForm();
        }
    }
}