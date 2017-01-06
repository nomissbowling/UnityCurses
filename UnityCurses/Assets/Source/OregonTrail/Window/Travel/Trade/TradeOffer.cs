// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using Assets.Source.OregonTrail.Entity.Item;
using Assets.Source.OregonTrail.Entity.Vehicle;

namespace Assets.Source.OregonTrail.Window.Travel.Trade
{
    /// <summary>
    ///     Represents an offer that automatically generates itself when constructor is called. Randomly selects a want, and
    ///     then a offer both of which are simulation items. Depending on the inventory of the vehicle this may or may not be
    ///     possible depending on totals.
    /// </summary>
    public sealed class TradeOffer
    {
        private readonly SimItem _offeredItem;
        private readonly SimItem _wantedItem;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Assets.Source.OregonTrail.Window.Travel.Trade.TradeOffer" /> class.
        /// </summary>
        public TradeOffer()
        {
            // Select a random item default inventory might have which the emigrant wants.
            _wantedItem = Vehicle.CreateRandomItem();

            // Select random item from default inventory which the emigrant offers up in exchange.
            _offeredItem = Vehicle.CreateRandomItem();
        }

        /// <summary>
        ///     Wanted item from the players vehicle inventory in order to get the offered item.
        /// </summary>
        public SimItem WantedItem
        {
            get { return _wantedItem; }
        }

        /// <summary>
        ///     Offers up an item in exchange for the traders wanted item.
        /// </summary>
        public SimItem OfferedItem
        {
            get { return _offeredItem; }
        }
    }
}