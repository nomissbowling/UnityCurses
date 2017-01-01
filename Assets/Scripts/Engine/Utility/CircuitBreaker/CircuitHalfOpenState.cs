// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 06/18/2016@1:06 AM

namespace Assets.Scripts.Engine.Utility.CircuitBreaker
{
    /// <summary>
    ///     Half-open circuits are the product of a circuit in open state ticking until it reaches timeout.
    ///     This state will try to make the request again, but if it fails we will go right back to open state and tick to
    ///     timeout max again.
    /// </summary>
    public class CircuitHalfOpenState : CircuitBreakerState
    {
        /// <summary>
        ///     Creates a new circuit breaker state, allowing any previous state data to be carried along into the next state.
        /// </summary>
        /// <param name="circuitBreaker">Circuit breaker object which we would like to transition with us into the next state type.</param>
        public CircuitHalfOpenState(CircuitBreaker circuitBreaker) : base(circuitBreaker)
        {
        }

        /// <summary>
        ///     NeoAxis entity system ticked the game world, which allows us to work with a single frame of work.
        /// </summary>
        public override void CircuitTick()
        {
            base.CircuitTick();

            // Attempt to make a request in the half-open state, if it fails again we are going back to tick counting.
            _circuitBreaker.SendRequest();
        }

        /// <summary>
        ///     Circuit sent a request and it bounced back with failure, we need to handle this differently depending on what state
        ///     we are currently in.
        /// </summary>
        public override void RequestFailed()
        {
            // Increment failures.
            base.RequestFailed();

            // Circuit in half-open state tried request and it failed, we go back to open state and start ticking to timeout max again.
            _circuitBreaker.MoveToOpenState();
        }

        /// <summary>
        ///     Circuit sent a request and it bounced back with a success, depending on our current state this could change the
        ///     state to closed.
        /// </summary>
        public override void RequestSucceeded()
        {
            // Increment successes.
            base.RequestSucceeded();

            // Circuit in half-open state tried request and it succeeded, we switch to closed state and begin making requests at normal rate again.
            _circuitBreaker.MoveToClosedState();
        }
    }
}