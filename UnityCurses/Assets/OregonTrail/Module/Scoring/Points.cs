// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;
using Assets.Engine.Utility;
using Assets.OregonTrail.Entity.Item;

namespace Assets.OregonTrail.Module.Scoring
{
    /// <summary>
    ///     Represents a given type of point that can take in a object of a given type in the concrete handler and we will
    ///     process and calculate total points for this item and display the information in a to string override so it is easy
    ///     to get to and visualize to user as a list of objects (such as a table).
    /// </summary>
    public sealed class Points
    {
        /// <summary>
        ///     Some things like cash don't have any equivalent type other than being a base POCO object, this allows us to have
        ///     nice display name for it regardless.
        /// </summary>
        private readonly string _optionalDisplayName;

        /// <summary>
        ///     Defines the quantity of the type of item that must be located in inventory for points awarded to be returned.
        /// </summary>
        private readonly int _perAmount;

        private readonly int _pointsAwarded;

        private readonly SimItem _resource;

        /// <summary>Initializes a new instance of the <see cref="T:Assets.OregonTrail.Module.Scoring.Points" /> class.</summary>
        /// <param name="resource">The resource.</param>
        /// <param name="optionalDisplayName">The optional Display Name.</param>
        public Points(SimItem resource, string optionalDisplayName)
        {
            // Complain if the per amount is zero, the developer is doing it wrong.
            if (resource.PointsPerAmount <= 0)
                throw new ArgumentException("Per amount is less than zero, default value is one for a reason!");

            // Setup point tabulator basics.
            _resource = resource;
            _pointsAwarded = resource.PointsAwarded;
            _optionalDisplayName = optionalDisplayName;
            _perAmount = resource.PointsPerAmount;
        }

        /// <summary>
        ///     Total points the player will get for this item being in their inventory multiplied by the quantity owned.
        /// </summary>
        public int PointsAwarded
        {
            get { return _pointsAwarded; }
        }

        /// <summary>
        ///     Represents the item we will be comparing with other items for, cost is not evaluated in any of these calculations.
        /// </summary>
        private SimItem Resource
        {
            get { return _resource; }
        }

        /// <summary>
        ///     Representation of the point scoring as a string that can be displayed visually to user so they understand the
        ///     scoring mechanism.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public override string ToString()
        {
            // Check if optional display name is being used.
            var displayName = Resource.Name;
            if (!string.IsNullOrEmpty(_optionalDisplayName))
                displayName = _optionalDisplayName;

            // Check if per amount is default value of one.
            return _perAmount == 1
                ? string.Format("{0}", displayName)
                : string.Format("{0} (per {1})", displayName, _perAmount);
        }
    }
}