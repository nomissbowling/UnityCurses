﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using Assets.Engine;
using Assets.OregonTrail.Entity.Item;

namespace Assets.OregonTrail.Window.Travel.Hunt
{
    /// <summary>
    ///     Defines a given animal the player can shoot and kill for it's meat. Depending on weather and current conditions the
    ///     type of animal created may vary.
    /// </summary>
    public sealed class PreyItem : ITick
    {
        private readonly SimItem _animal;
        private readonly int _lifetimeMax;
        private readonly int _targetTimeMax;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Assets.OregonTrail.Window.Travel.Hunt.PreyItem" /> class.
        /// </summary>
        public PreyItem()
        {
            // Randomly generate a shooting time up to total hunting time.
            Lifetime = 0;
            _lifetimeMax = EngineApp.Random.Next(HuntManager.HUNTINGTIME);

            // Randomly generate maximum amount of time this animal will be a valid target.
            TargetTime = 0;
            _targetTimeMax = EngineApp.Random.Next(HuntManager.MINTARGETINGTIME,
                HuntManager.MAXTARGETINGTIME);

            // Randomly select a animal for this prey to be from default animals.
            var preyIndex = EngineApp.Random.Next(HuntManager.DefaultAnimals.Count);

            // Select a random animal that we can be from default animals.
            _animal = new SimItem(HuntManager.DefaultAnimals[preyIndex], 1);
        }

        /// <summary>
        ///     Creates a prey item from another one as reference.
        /// </summary>
        /// <param name="preyItem">Prey item that should be copied.</param>
        public PreyItem(PreyItem preyItem)
        {
            // Prey specific settings for how long they live on the field.
            Lifetime = preyItem.Lifetime;
            _lifetimeMax = preyItem.LifetimeMax;

            // Prey specific for total length of time they may be targeted.
            TargetTime = preyItem.TargetTime;
            _targetTimeMax = preyItem.TargetTimeMax;

            // Copy of the animal from the prey item.
            _animal = new SimItem(preyItem.Animal, 1);
        }

        /// <summary>
        ///     Determines what type of animal this prey will be if the player kills it.
        /// </summary>
        public SimItem Animal
        {
            get { return _animal; }
        }

        /// <summary>
        ///     Determines the total number of seconds this animal has been a valid target for the player to shoot.
        /// </summary>
        public int TargetTime { get; private set; }

        /// <summary>
        ///     Determines the total amount of time that the given prey is allowed the be a target of the player, if this threshold
        ///     is reached the prey will sense the player and run away.
        /// </summary>
        public int TargetTimeMax
        {
            get { return _targetTimeMax; }
        }

        /// <summary>
        ///     Keeps track of the total number of seconds this prey has been visible. Once it reaches it's maximum shoot time it
        ///     will be removed from the list of possible animals the player can hunt.
        /// </summary>
        public int Lifetime { get; private set; }

        /// <summary>
        ///     Maximum amount of time this animal will be in the list of things that can be killed and are available on the field.
        /// </summary>
        public int LifetimeMax
        {
            get { return _lifetimeMax; }
        }

        /// <summary>
        ///     Gets or sets the value indicating if the prey target has become aware of the players intentions to shoot them and
        ///     have decided to run away.
        /// </summary>
        public bool ShouldRunAway { get; private set; }

        /// <summary>
        ///     Called when the simulation is ticked by underlying operating system, game engine, or potato. Each of these system
        ///     ticks is called at unpredictable rates, however if not a system tick that means the simulation has processed enough
        ///     of them to fire off event for fixed interval that is set in the core simulation by constant in milliseconds.
        /// </summary>
        /// <remarks>Default is one second or 1000ms.</remarks>
        /// <param name="systemTick">
        ///     TRUE if ticked unpredictably by underlying operating system, game engine, or potato. FALSE if
        ///     pulsed by game simulation at fixed interval.
        /// </param>
        /// <param name="skipDay">
        ///     Determines if the simulation has force ticked without advancing time or down the trail. Used by
        ///     special events that want to simulate passage of time without actually any actual time moving by.
        /// </param>
        public void OnTick(bool systemTick, bool skipDay)
        {
            // No work is done on system ticks.
            if (systemTick)
                return;

            // No work is done if force ticked.
            if (skipDay)
                return;

            // Increment the total shot time until maximum.
            if (Lifetime < LifetimeMax)
                Lifetime++;
            else if (Lifetime > LifetimeMax)
                Lifetime = LifetimeMax;
            else
                Lifetime = LifetimeMax;
        }

        /// <summary>
        ///     Ticks the prey as an active target, it will only count up to randomly determined amount before the prey senses the
        ///     player and runs away.
        /// </summary>
        public void TickTarget()
        {
            // Skip if the target has chosen to flee and run away from the hunter.
            if (ShouldRunAway)
                return;

            // Check if the target time has gone above ceiling.
            if (TargetTime >= TargetTimeMax)
            {
                ShouldRunAway = true;
                TargetTime = TargetTimeMax;
                return;
            }

            // Tick the target time up towards the maximum.
            TargetTime++;
        }
    }
}