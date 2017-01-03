﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Assets.Engine.Utility;
using Assets.OregonTrail.Entity.Item;
using Assets.OregonTrail.Entity.Person;
using Assets.OregonTrail.Event;

namespace Assets.OregonTrail.Entity.Vehicle
{
    /// <summary>
    ///     Vessel that holds all the players, their inventory, money, and keeps track of total miles traveled in the form of
    ///     an odometer.
    /// </summary>
    public sealed class Vehicle : IEntity
    {
        private readonly string _name;

        /// <summary>
        ///     References the vehicle itself, it is important to remember the vehicle is not an entity and not an item.
        /// </summary>
        private Dictionary<Entities, SimItem> _inventory;

        /// <summary>
        ///     References all the critical infrastructure that makes up the
        /// </summary>
        private List<SimItem> _parts;

        /// <summary>
        ///     References all of the people inside of the vehicle.
        /// </summary>
        private List<Person.Person> _passengers;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:TrailEntities.Entities.Vehicle" /> class.
        /// </summary>
        public Vehicle()
        {
            ResetVehicle(0);
            _name = "Vehicle";
            Pace = TravelPace.Steady;
            Mileage = 1;
            Status = VehicleStatus.Stopped;
        }

        /// <summary>
        ///     Determines if all the passengers in the vehicle are dead by adding up number of truths to total passenger count.
        /// </summary>
        public bool PassengersDead
        {
            get
            {
                // Everybody cannot be dead if nobody is there!
                if (Passengers.Count <= 0)
                    return false;

                // Loop through all passengers and add their death flag to array.
                var allDead = new bool[Passengers.Count];
                for (var i = 0; i < Passengers.Count; i++)
                {
                    var passenger = Passengers[i];
                    allDead[i] = passenger.HealthStatus == HealthStatus.Dead;
                }

                // Determine if everybody is dead by checking if truths are greater than passenger count.
                return allDead.TrueCount() >= Passengers.Count;
            }
        }

        /// <summary>
        ///     References the vehicle itself, it is important to remember the vehicle is not an entity and not an item.
        /// </summary>
        public IDictionary<Entities, SimItem> Inventory
        {
            get { return _inventory; }
        }

        /// <summary>
        ///     References all of the people inside of the vehicle.
        /// </summary>
        public ReadOnlyCollection<Person.Person> Passengers
        {
            get { return _passengers.AsReadOnly(); }
        }

        /// <summary>
        ///     Current ration level, determines the amount food that will be consumed each day of the simulation.
        /// </summary>
        public RationLevel Ration { get; private set; }

        /// <summary>
        ///     Current travel pace, determines how fast the vehicle will attempt to move down the trail.
        /// </summary>
        public TravelPace Pace { get; private set; }

        /// <summary>
        ///     Total number of miles the vehicle has traveled since the start of the simulation.
        /// </summary>
        public int Odometer { get; private set; }

        /// <summary>
        ///     In general, you will travel 200 miles plus some additional distance which depends upon the quality of your team of
        ///     oxen. This mileage figure is an ideal, assuming nothing goes wrong. If you run into problems, mileage is subtracted
        ///     from this ideal figure; the revised total is printed at the start of the next trip segment.
        /// </summary>
        public int Mileage { get; private set; }

        /// <summary>
        ///     Defines what the trail module is currently processing if anything in regards to movement of vehicle and player
        ///     entities down the trail.
        /// </summary>
        public VehicleStatus Status { get; set; }

        /// <summary>
        ///     Returns the total value of all the cash the vehicle and all party members currently have.
        ///     Setting this value will change the quantity of dollar bills in player inventory.
        /// </summary>
        public float Balance
        {
            get { return _inventory[Entities.Cash].TotalValue; }
            private set
            {
                // Skip if the quantity already matches the value we are going to set it to.
                if (value.Equals(_inventory[Entities.Cash].Quantity))
                    return;

                // Check if the value being set is zero, if so just reset it.
                if (value <= 0)
                    _inventory[Entities.Cash].Reset();
                else
                    _inventory[Entities.Cash] = new SimItem(_inventory[Entities.Cash],
                        (int) value);
            }
        }

        /// <summary>
        ///     Defines all the default parts for a vehicle so it can be considered healthy and in a proper working condition. If
        ///     any of these parts becomes broken or permanently damaged the vehicle will be unable to continue until the player
        ///     repairs or replaces with a spare part.
        /// </summary>
        private static IEnumerable<SimItem> DefaultParts
        {
            get
            {
                // Init inventory of parts for the vehicle.
                var defaultParts = new List<SimItem>
                {
                    Parts.Wheel,
                    Parts.Axle,
                    Parts.Tongue
                };

                // Set proper quantities for each entity item type.
                foreach (var part in defaultParts)
                    switch (part.Category)
                    {
                        case Entities.Wheel:
                            // You need four (4) wheels.
                            part.ReduceQuantity(part.MaxQuantity);
                            part.AddQuantity(4);
                            break;
                        case Entities.Axle:
                            // You need one (1) axle.
                            part.ReduceQuantity(part.MaxQuantity);
                            part.AddQuantity(1);
                            break;
                        case Entities.Tongue:
                            // You need one (1) tongue.
                            part.ReduceQuantity(part.MaxQuantity);
                            part.AddQuantity(1);
                            break;
                        case Entities.Animal:
                        case Entities.Food:
                        case Entities.Clothes:
                        case Entities.Ammo:
                        case Entities.Vehicle:
                        case Entities.Person:
                        case Entities.Cash:
                        case Entities.Location:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                // Now we have default inventory of a store with all quantities zeroed out.
                return defaultParts;
            }
        }

        /// <summary>
        ///     Default items every vehicle and store will have, their prices increase with distance from starting point.
        /// </summary>
        internal static IDictionary<Entities, SimItem> DefaultInventory
        {
            get
            {
                // Init inventory of items with default starting amounts.
                var defaultInventory = new Dictionary<Entities, SimItem>
                {
                    {Entities.Animal, Parts.Oxen},
                    {Entities.Clothes, Resources.Clothing},
                    {Entities.Ammo, Resources.Bullets},
                    {Entities.Wheel, Parts.Wheel},
                    {Entities.Axle, Parts.Axle},
                    {Entities.Tongue, Parts.Tongue},
                    {Entities.Food, Resources.Food},
                    {Entities.Cash, Resources.Cash}
                };

                // Zero out all of the quantities by removing their max quantity.
                foreach (var simItem in defaultInventory)
                    simItem.Value.ReduceQuantity(simItem.Value.MaxQuantity);

                // Now we have default inventory of a store with all quantities zeroed out.
                return defaultInventory;
            }
        }

        /// <summary>
        ///     In general, you will travel 200 miles plus some additional distance which depends upon the quality of your team of
        ///     oxen. This mileage figure is an ideal, assuming nothing goes wrong. If you run into problems, mileage is subtracted
        ///     from this ideal figure; the revised total is printed at the start of the next trip segment.
        /// </summary>
        /// <returns>The expected mileage over the next two week segment.</returns>
        private int RandomMileage
        {
            get
            {
                // Total amount of monies the player has spent on animals to pull their vehicle.
                var cost_animals = Inventory[Entities.Animal].TotalValue;

                // Variables that will hold the distance we should travel in the next day.
                var total_miles = Mileage + (cost_animals - 110) / 2.5 +
                                  10 * OregonTrailApp.Instance.Random.NextDouble();

                return (int) Math.Abs(total_miles);
            }
        }

        /// <summary>
        ///     Locates all of the parts in the vehicle that are no longer operational and require repair or replacement by the
        ///     player.
        /// </summary>
        /// <returns>Broken item part if any found, NULL if no broken parts located in vehicle operation.</returns>
        public SimItem BrokenPart { get; internal set; }

        /// <summary>
        ///     Locates the leader in the passenger manifest and returns the person object that represents them.
        /// </summary>
        public Person.Person PassengerLeader
        {
            get
            {
                // Leaders profession, used to determine points multiplier at end.
                Person.Person leaderPerson = null;

                // Check if passenger manifest exists.
                if (Passengers == null)
                    return null;

                // Check if there are any passengers to work with.
                if (!Passengers.Any())
                    return null;

                foreach (var person in Passengers)
                    if (person.Leader)
                        leaderPerson = person;

                return leaderPerson;
            }
        }

        /// <summary>
        ///     Grabs the averaged health of all the passengers in the vehicle, only adds towards total if they are alive. Will be
        ///     recalculated each time this is called.
        /// </summary>
        public HealthStatus PassengerHealthStatus
        {
            get
            {
                // Check if passenger manifest exists.
                if (Passengers == null)
                    return HealthStatus.Dead;

                // Check if there are any passengers to work with, return good health if none.
                if (!Passengers.Any())
                    return HealthStatus.Dead;

                // Builds up a list of enumeration health values for living passengers.
                var livingPassengersHealth = new List<HealthStatus>();
                foreach (var person in Passengers)
                    if (person.HealthStatus != HealthStatus.Dead)
                        livingPassengersHealth.Add(person.HealthStatus);

                // Casts all the enumeration health values to integers and averages them.
                var averageHealthValue = 0;
                if (livingPassengersHealth.Count > 0)
                    averageHealthValue = (int) livingPassengersHealth.Cast<int>().Average();

                // Look for the closest health level to the average health level from all living passengers.
                var closest = Enum.GetValues(typeof(HealthStatus)).Cast<int>().ClosestTo(averageHealthValue);
                return (HealthStatus) closest;
            }
        }

        /// <summary>
        ///     Calculates the total number of passengers that are still alive in the vehicle and consuming resources every turn.
        /// </summary>
        public int PassengerLivingCount
        {
            get
            {
                // Check if passenger manifest exists.
                if (Passengers == null)
                    return 0;

                // Check if there are any passengers to work with.
                if (!Passengers.Any())
                    return 0;

                // Builds up a list of enumeration health values for living passengers.
                var alivePersonsHealth = new List<HealthStatus>();
                foreach (var person in Passengers)
                    if (person.HealthStatus != HealthStatus.Dead)
                        alivePersonsHealth.Add(person.HealthStatus);

                return alivePersonsHealth.Count;
            }
        }

        /// <summary>
        ///     Name of the entity as it should be known in the simulation.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>The compare.</summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>The <see cref="int" />.</returns>
        public int Compare(IEntity x, IEntity y)
        {
            var result = string.Compare(x.Name, y.Name, StringComparison.Ordinal);
            if (result != 0) return result;

            return result;
        }

        /// <summary>The compare to.</summary>
        /// <param name="other">The other.</param>
        /// <returns>The <see cref="int" />.</returns>
        public int CompareTo(IEntity other)
        {
            var result = string.Compare(other.Name, Name, StringComparison.Ordinal);
            if (result != 0) return result;

            return result;
        }

        /// <summary>The equals.</summary>
        /// <param name="other">The other.</param>
        /// <returns>The <see cref="bool" />.</returns>
        public bool Equals(IEntity other)
        {
            // Reference equality check
            if (this == other)
                return true;

            if (other == null)
                return false;

            if (other.GetType() != GetType())
                return false;

            if (Name.Equals(other.Name))
                return true;

            return false;
        }

        /// <summary>The equals.</summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>The <see cref="bool" />.</returns>
        public bool Equals(IEntity x, IEntity y)
        {
            return x.Equals(y);
        }

        /// <summary>The get hash code.</summary>
        /// <param name="obj">The obj.</param>
        /// <returns>The <see cref="int" />.</returns>
        public int GetHashCode(IEntity obj)
        {
            var hash = 23;
            hash = hash * 31 + Name.GetHashCode();
            return hash;
        }

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
            // Only can tick vehicle on interval.
            if (systemTick)
                return;

            // Loop through all the people in the vehicle and tick them moving or ticking time or not.
            foreach (var person in _passengers)
                person.OnTick(false, skipDay);

            // Only advance the vehicle if we are actually traveling and not skipping a day of simulation.
            if (Status != VehicleStatus.Moving || skipDay)
                return;

            // Figure out how far we need to go to reach the next point.
            Mileage = RandomMileage;

            // Sometimes things just go slow on the trail, cut mileage in half if above zero randomly.
            if (OregonTrailApp.Instance.Random.NextBool() && Mileage > 0)
                Mileage = Mileage / 2;

            // Check for random events that might trigger regardless of calculations made.
            OregonTrailApp.Instance.EventDirector.TriggerEventByType(this, EventCategory.Vehicle);

            // Check to make sure mileage is never below or at zero.
            if (Mileage <= 0)
                Mileage = 10;

            // Use our altered mileage to affect how far the vehicle has traveled in todays tick..
            Odometer += Mileage;
        }

        /// <summary>
        ///     Removes a spare part from the vehicles inventory of the same type of the broken part in order to repair it.
        /// </summary>
        /// <returns>TRUE if the spare part existed and vehicle was repaired, FALSE if no spare exists and vehicle still broken.</returns>
        public bool TryUseSparePart()
        {
            // Skip if the vehicle does not contain part to fix vehicle.
            if (!ContainsItem(BrokenPart))
                return false;

            // Remove one (1) of the spare parts of the broken part category.
            Inventory[BrokenPart.Category].ReduceQuantity(1);

            // Repair the vehicle.
            RepairAllParts();

            // Returns true so the calling method knows the vehicle is now repaired.
            return true;
        }

        /// <summary>
        ///     Goes through all of the vehicle parts and will randomly decide to break one of them. If any of the vehicle parts
        ///     are already broken this will not run.
        /// </summary>
        /// <returns>Read-only collection containing the parts that were broken.</returns>
        public void BreakRandomPart()
        {
            // Skip if there is already a broken part.
            if (BrokenPart != null)
                return;

            // Randomly select one of the parts to break in the vehicle.
            var randomPartIndex = OregonTrailApp.Instance.Random.Next(_parts.Count);

            // Sets the broken part for other processes to deal with.
            BrokenPart = _parts[randomPartIndex];
        }

        /// <summary>
        ///     Reduces the total mileage the vehicle has rolled to move within the next two week block section. Will not allow
        ///     mileage to be reduced below zero.
        /// </summary>
        /// <param name="amount">Amount of mileage that will be reduced.</param>
        internal void ReduceMileage(int amount)
        {
            // Mileage cannot be reduced when parked.
            if (Status != VehicleStatus.Moving)
                return;

            // Check if current mileage is below zero.
            if (Mileage <= 0)
                return;

            // Calculate new mileage.
            var updatedMileage = Mileage - amount;

            // Check if updated mileage is below zero.
            if (updatedMileage <= 0)
                updatedMileage = 0;

            // Check that mileage doesn't already exist as this value somehow.
            if (!updatedMileage.Equals(Mileage))
                Mileage = updatedMileage;
        }

        /// <summary>Sets the current speed of the game simulation.</summary>
        /// <param name="castedSpeed">The casted Speed.</param>
        public void ChangePace(TravelPace castedSpeed)
        {
            // Change game simulation speed.
            Pace = castedSpeed;
        }

        /// <summary>Adds a new person object to the list of vehicle passengers.</summary>
        /// <param name="person">Person that wishes to become a vehicle passenger.</param>
        public void AddPerson(Person.Person person)
        {
            _passengers.Add(person);
        }

        /// <summary>Adds the item to the inventory of the vehicle and subtracts it's cost multiplied by quantity from balance.</summary>
        /// <param name="transaction">The transaction.</param>
        public void Purchase(SimItem transaction)
        {
            // Check of the player can afford this item.
            if (Balance < transaction.TotalValue)
                return;

            // Init new item based on old one, with new quantity value from store, trader, random event, etc.
            Balance -= transaction.TotalValue;

            // Make sure we add the quantity and not just replace it.
            _inventory[transaction.Category].AddQuantity(transaction.Quantity);
        }

        /// <summary>Resets the vehicle status to the defaults.</summary>
        /// <param name="startingMonies">Amount of money the vehicle should have to work with.</param>
        public void ResetVehicle(int startingMonies)
        {
            // Parts for the vehicle to keep it in working order and moving.
            _parts = new List<SimItem>(DefaultParts);

            // Inventory items for the passengers to use like food, clothes, spare parts.
            _inventory = new Dictionary<Entities, SimItem>(DefaultInventory);

            // Passengers the vehicle will be moving along the trail.
            _passengers = new List<Person.Person>();

            // Money the passengers use collectively to purchase items.
            Balance = startingMonies;

            // Determines amount of food consumed per day.
            Ration = RationLevel.Filling;

            // Number of miles the vehicle has traveled.
            Odometer = 0;

            // Vehicle is not moving and currently stopped.
            Status = VehicleStatus.Stopped;
        }

        /// <summary>
        ///     Changes the current ration level to new value if it is not already set to that. Also fires even about this for
        ///     subscribers to get event notification about the change.
        /// </summary>
        /// <param name="ration">The rate at which people are permitted to eat in the vehicle party.</param>
        public void ChangeRations(RationLevel ration)
        {
            Ration = ration;
        }

        /// <summary>
        ///     Selects a random item from the default inventory layout a vehicle would have. It will also generate a random
        ///     quantity it desires for the item within the bounds of the minimum and maximum quantities.
        /// </summary>
        /// <returns>Returns a randomly created item with random quantity. Returns NULL if anything bad happens.</returns>
        public static SimItem CreateRandomItem()
        {
            // Loop through the inventory and decide which items to give free copies of.
            foreach (var itemPair in DefaultInventory)
            {
                // Determine if we will be making more of this item, if it's the last one then we have to.
                if (OregonTrailApp.Instance.Random.NextBool())
                    continue;

                // Skip certain items that cannot be traded.
                switch (itemPair.Value.Category)
                {
                    case Entities.Food:
                    case Entities.Clothes:
                    case Entities.Ammo:
                    case Entities.Wheel:
                    case Entities.Axle:
                    case Entities.Tongue:
                    case Entities.Vehicle:
                    case Entities.Animal:
                    case Entities.Person:
                    {
                        // Init a random number within the range we need to create an item.
                        var amountToMake = itemPair.Value.MaxQuantity / 4;

                        // Check if created amount goes above ceiling.
                        if (amountToMake > itemPair.Value.MaxQuantity)
                            amountToMake = itemPair.Value.MaxQuantity;

                        // Check if created amount goes below floor.
                        if (amountToMake <= 0)
                            amountToMake = 1;

                        // Add some random amount of the item from one to total amount.
                        var createdAmount = OregonTrailApp.Instance.Random.Next(1, amountToMake);

                        // Init a new item with generated quantity.
                        var createdItem = new SimItem(itemPair.Value, createdAmount);
                        return createdItem;
                    }
                    case Entities.Cash:
                    case Entities.Location:
                        continue;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            // Default response is to return a NULL item if something terrible happens.
            return null;
        }

        /// <summary>
        ///     Creates some random items that will be given to the player, this is normally used when the player encounters a
        ///     abandoned vehicle.
        /// </summary>
        /// <returns>
        ///     The dictionary of items created and their quantities.
        /// </returns>
        public IDictionary<Entities, int> CreateRandomItems()
        {
            // Items that will be created by this method.
            IDictionary<Entities, int> createdItems = new Dictionary<Entities, int>();

            // Make a copy of the inventory to iterate over.
            var copiedInventory = new Dictionary<Entities, SimItem>(Inventory);

            // Loop through the inventory and decide which items to give free copies of.
            foreach (var itemPair in copiedInventory)
            {
                // Skip item if quantity is at maximum.
                if (itemPair.Value.Quantity >= itemPair.Value.MaxQuantity)
                    continue;

                // Determine if we will be making more of this item.
                if (OregonTrailApp.Instance.Random.NextBool())
                    continue;

                // Add some random amount of the item from one to total amount.
                var createdAmount = OregonTrailApp.Instance.Random.Next(1, itemPair.Value.MaxQuantity / 4);

                // Add the amount ahead of time so we can figure out of it is above maximum.
                var simulatedAmountAdd = itemPair.Value.Quantity + createdAmount;

                // Adjust the simulated added amount to item max quantity if it ended up being above it.
                if (simulatedAmountAdd >= itemPair.Value.MaxQuantity)
                    simulatedAmountAdd = itemPair.Value.MaxQuantity;

                // Add the amount we created to total of actual item in inventory.
                Inventory[itemPair.Key] = new SimItem(itemPair.Value, simulatedAmountAdd);

                // Tabulate the amount we created in dictionary to be returned to caller.
                createdItems.Add(itemPair.Key, createdAmount);
            }

            // Clear out the copied list we made for iterating.
            copiedInventory.Clear();

            // Return the created item summary.
            return createdItems;
        }

        /// <summary>
        ///     Destroys some of the inventory items in no particular order and or reason. That is left up the caller to decide.
        /// </summary>
        /// <returns>
        ///     The dictionary of items destroyed and their quantities.
        /// </returns>
        public IDictionary<Entities, int> DestroyRandomItems()
        {
            // Dictionary that will keep track of enumeration item type and destroyed amount for record keeping purposes.
            IDictionary<Entities, int> destroyedItems = new Dictionary<Entities, int>();

            // Make a copy of the inventory to iterate over.
            var copiedInventory = new Dictionary<Entities, SimItem>(Inventory);

            // Loop through the inventory and decide to randomly destroy some inventory items.
            foreach (var itemPair in copiedInventory)
            {
                // Skip item if quantity is less than one.
                if (itemPair.Value.Quantity < 1)
                    continue;

                // Determine if we will be destroying this item.
                if (OregonTrailApp.Instance.Random.NextBool())
                    continue;

                // Shutdown some random amount of the item from one to total amount.
                var destroyAmount = OregonTrailApp.Instance.Random.Next(1, itemPair.Value.Quantity);

                // Remove the amount we destroyed from the actual inventory.
                Inventory[itemPair.Key].ReduceQuantity(destroyAmount);

                // Tabulate the amount we destroyed in dictionary to be returned to caller.
                destroyedItems.Add(itemPair.Key, destroyAmount);
            }

            // Clear out the copied list we made for iterating.
            copiedInventory.Clear();

            // Return the destroyed item summary.
            return destroyedItems;
        }

        /// <summary>
        ///     Determines if the vehicles inventory contains the inputted item in the specified quantity.
        /// </summary>
        /// <param name="wantedItem">Item wanted configured with desired quantity.</param>
        /// <returns>TRUE if the vehicle has this item, FALSE if it does not have it or does but not enough quantity for trade.</returns>
        public bool ContainsItem(SimItem wantedItem)
        {
            // Loop through vehicle inventory.
            foreach (var simItem in Inventory)
                if (simItem.Value.Name == wantedItem.Name && simItem.Value.Category == wantedItem.Category &&
                    simItem.Value.Quantity >= wantedItem.MinQuantity)
                    return true;

            return false;
        }

        /// <summary>
        ///     Repairs any broken parts the vehicle may have and returns them to fully operational condition.
        /// </summary>
        private void RepairAllParts()
        {
            // Loop through every part in the vehicle and repair it.
            foreach (var part in _parts)
                part.Repair();

            // Set the vehicle status to be stopped.
            Status = VehicleStatus.Stopped;
        }

        /// <summary>
        ///     Runs logic to check and see if the vehicle is currently stuck and unable to move. This could be because there are
        ///     not enough oxen to pull it, or it could mean some of the parts on the wagon are broken and need to be replaced.
        /// </summary>
        public void CheckStatus()
        {
            // Checks if the player has animals to pull their vehicle.
            if (Inventory[Entities.Animal].Quantity <= 0)
            {
                Status = VehicleStatus.Disabled;
                return;
            }

            // Don't change the state from disabled to moving.
            if (Status == VehicleStatus.Disabled)
                return;

            // Default response it to allow the vehicle to move.
            Status = VehicleStatus.Moving;
        }
    }
}