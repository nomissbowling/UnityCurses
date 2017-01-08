﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Maxwolf.Engine;
using Assets.Maxwolf.OregonTrail.Event;
using Assets.Maxwolf.ProjectCommon.Utility;
using UnityEngine;

namespace Assets.Maxwolf.OregonTrail.Module.Director
{
    /// <summary>
    ///     Factory pattern for creating director event items from type references.
    /// </summary>
    public sealed class EventFactory
    {
        private readonly Dictionary<EventKey, Type> _eventReference;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Assets.Maxwolf.OregonTrail.Module.Director.EventFactory" /> class.
        /// </summary>
        public EventFactory()
        {
            // Init dictionaries for storing event reference types, history of execution, and execution count.
            _eventReference = new Dictionary<EventKey, Type>();

            // Collect all of the event types with the attribute decorated on them.
            var randomEvents = AttributeExtensions.GetTypesWith<DirectorEventAttribute>(true);
            foreach (var eventObject in randomEvents)
            {
                // Check if the class is abstract base class, we don't want to add that.
                if (eventObject.IsAbstract)
                    continue;

                // Check the attribute itself from the event we are working on, which gives us the event type enum.
                var eventAttribute = eventObject.GetAttributes<DirectorEventAttribute>(true).First();
                var eventType = eventAttribute.EventCategory;

                // Initialize the execution history dictionary with every event type.
                foreach (var modeType in Enum.GetValues(typeof(EventCategory)))
                {
                    // Only proceed if enum value matches attribute enum value for event type.
                    if (!modeType.Equals(eventType))
                        continue;

                    // Init key for the event execution counter.
                    var eventKey = new EventKey((EventCategory) modeType, eventObject.Name,
                        eventAttribute.EventExecutionType);

                    // Reference type for creating instances.
                    if (!EventReference.ContainsKey(eventKey))
                        EventReference.Add(eventKey, eventObject);
                }
            }
        }

        /// <summary>
        ///     References all of the events that have been triggered by the system in chronological order they occurred.
        /// </summary>
        private Dictionary<EventKey, Type> EventReference
        {
            get { return _eventReference; }
        }

        /// <summary>Creates a new event based on system type which we keep track of in dictionary of event references.</summary>
        /// <param name="eventType">The type of event which we should create an instance of.</param>
        /// <returns>Instance of event type given in parameter.</returns>
        internal EventProduct CreateInstance(Type eventType)
        {
            // Check if event type exists in reference dictionary.
            if (!EventReference.ContainsValue(eventType))
                throw new ArgumentException(
                    string.Format(
                        "Attempted to create instance of {0} without any known reference to it in event factory! ",
                        eventType.Name) +
                    "Perhaps you are missing the [DirectorEvent()] attribute.");

            // Grab the key value pair from event references that matches inputted type via equality reference.
            var directorEventKeyValuePair = EventReference.FirstOrDefault(x => x.Value == eventType);

            // Check if the class is abstract base class, we don't want to add that.
            if (directorEventKeyValuePair.Value.IsAbstract)
                return null;

            // Create the event product, but don't call any constructor.
            var eventInstance = ScriptableObject.CreateInstance(directorEventKeyValuePair.Value) as EventProduct;

            // If the event instance is null then complain.
            if (eventInstance == null)
                throw new ArgumentException(string.Format("Attempted to create instance of {0} event but failed!",
                    eventType));

            // Fire event that acts like our own constructor for the object but only calling it when we say here.
            eventInstance.OnEventCreate();

            // Increment the history for loading this type of event.
            return eventInstance;
        }

        /// <summary>Gathers all of the events by specified type and picks one of them at random to return.</summary>
        /// <param name="eventCategory">Enum value of the type of event such as medical, person, vehicle, etc.</param>
        /// <returns>Created event product based on enum value.</returns>
        public EventProduct CreateRandomByType(EventCategory eventCategory)
        {
            // Query all of the reference event types that match the given enumeration value.
            var groupedEventList = new List<Type>();
            foreach (var type in EventReference)
                if (type.Key.Category.Equals(eventCategory) &&
                    type.Key.ExecutionType == EventExecution.RandomOrManual)
                    groupedEventList.Add(type.Value);

            // Check to make sure there is at least one type of event of this type.
            if (groupedEventList.Count <= 0)
                return null;

            // Roll the dice against the event reference ceiling count to see which one we use.
            var diceRoll = EngineApp.Random.Next(groupedEventList.Count);

            // Create the event we decided to execute from these types of event types.
            var randomEvent = CreateInstance(groupedEventList[diceRoll]);

            // Clear the temporary list we made to get by category and return create event instance.
            groupedEventList.Clear();
            return randomEvent;
        }
    }
}