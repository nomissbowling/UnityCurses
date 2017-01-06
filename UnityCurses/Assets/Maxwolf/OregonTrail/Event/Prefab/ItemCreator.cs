﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Maxwolf.OregonTrail.Entity;
using Assets.Maxwolf.OregonTrail.Module.Director;
using Assets.Maxwolf.OregonTrail.Window.RandomEvent;

namespace Assets.Maxwolf.OregonTrail.Event.Prefab
{
    /// <summary>
    ///     Prefab class that is used to destroy some items at random from the vehicle inventory. Will return a list of items
    ///     and print them to the screen and allow for a custom prompt message to be displayed so it can be different for each
    ///     implementation that wants to use it.
    /// </summary>
    public abstract class ItemCreator : EventProduct
    {
        /// <summary>
        ///     String builder that will hold all the data from event execution.
        /// </summary>
        private StringBuilder _eventText;

        /// <summary>
        ///     Fired when the event is created by the event factory, but before it is executed. Acts as a constructor mostly but
        ///     used in this way so that only the factory will call the method and there is no worry of it accidentally getting
        ///     called by creation.
        /// </summary>
        public override void OnEventCreate()
        {
            base.OnEventCreate();

            // Init the string builder that will hold representation of event action to display for debugging.
            _eventText = new StringBuilder();
        }

        /// <summary>
        ///     Fired when the event handler associated with this enum type triggers action on target entity. Implementation is
        ///     left completely up to handler.
        /// </summary>
        /// <param name="eventExecutor">
        ///     Entities which the event is going to directly affect. This way there is no confusion about
        ///     what entity the event is for. Will require casting to correct instance type from interface instance.
        /// </param>
        public override void Execute(RandomEventInfo eventExecutor)
        {
            // Clear out the text from the string builder.
            _eventText = new StringBuilder();

            // Add the pre-create message if it exists.
            var preCreatePrompt = OnPreCreateItems();
            if (!string.IsNullOrEmpty(preCreatePrompt))
                _eventText.AppendLine(preCreatePrompt);

            // Init some items at random and get a list back of what and how much.
            var createdItems = OregonTrailApp.Instance.Vehicle.CreateRandomItems();

            // Add the post create message if it exists.
            var postCreatePrompt = OnPostCreateItems(createdItems);
            if (!string.IsNullOrEmpty(postCreatePrompt))
                _eventText.AppendLine(postCreatePrompt);

            // Skip if created items count is zero.
            if (createdItems != null && !(createdItems.Count > 0))
                return;

            // Loop through all of the created items and add them to string builder.
            foreach (var createdItem in createdItems)
                if (createdItems.Last().Equals(createdItem))
                    _eventText.Append(string.Format("{0:N0} {1}", createdItem.Value, createdItem.Key));
                else
                    _eventText.AppendLine(string.Format("{0:N0} {1}", createdItem.Value, createdItem.Key));
        }

        /// <summary>Fired by the event prefab after the event has executed.</summary>
        /// <param name="createdItems">Items that were created and added to vehicle inventory.</param>
        /// <returns>The <see cref="string" />.</returns>
        protected abstract string OnPostCreateItems(IDictionary<Entities, int> createdItems);

        /// <summary>
        ///     Fired by the event prefab before the event has executed.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        protected abstract string OnPreCreateItems();

        /// <summary>
        ///     Fired when the simulation would like to render the event, typically this is done AFTER executing it but this could
        ///     change depending on requirements of the implementation.
        /// </summary>
        /// <param name="userData"></param>
        /// <returns>Text user interface string that can be used to explain what the event did when executed.</returns>
        protected override string OnRender(RandomEventInfo userData)
        {
            return _eventText.ToString();
        }
    }
}