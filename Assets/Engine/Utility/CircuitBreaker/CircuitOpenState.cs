// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 06/18/2016@1:06 AM

namespace Assets.Engine.Utility.CircuitBreaker
{
    /// <summary>
    ///     Open circuits will block requests each tick, but count the tick and work towards a timeout that will try to
    ///     eventually make another request.
    /// </summary>
    public class CircuitOpenState : CircuitBreakerState
    {
        /// <summary>
        ///     Ceiling for tick timer, we will count ticks in open state and block all requests from occurring until we reach this
        ///     limit.
        /// </summary>
        private readonly int tickTimeoutMax;

        /// <summary>
        ///     Current number of ticks that have happened in the open state.
        /// </summary>
        private int tickTimer;

        /// <summary>
        ///     Creates a new circuit breaker state, allowing any previous state data to be carried along into the next state.
        /// </summary>
        /// <param name="circuitBreaker">Circuit breaker object which we would like to transition with us into the next state type.</param>
        public CircuitOpenState(CircuitBreaker circuitBreaker) : base(circuitBreaker)
        {
            // Set the timeout maximum to the circuit timeout.
            tickTimeoutMax = circuitBreaker.CircuitTimeout;

            // Set the starting timer at zero, begin counting towards timeout maximum.
            tickTimer = 0;
        }

        /// <summary>
        ///     NeoAxis entity system ticked the game world, which allows us to work with a single frame of work.
        /// </summary>
        public override void CircuitTick()
        {
            base.CircuitTick();

            // Count the tick timer upwards if it is below the maximum timeout value we got from circuit.
            if (tickTimer < tickTimeoutMax)
                tickTimer++;
            else
                _circuitBreaker.MoveToHalfOpenState();
        }

        /// <summary>
        ///     Circuit is going to attempt to make request to grid network.
        /// </summary>
        public override void RequestIsAboutToBeMade(out bool shouldCancel)
        {
            // Stop the request from happening by overriding it's ability to call the method below it.
            shouldCancel = true;
        }
    }
}