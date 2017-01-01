// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 06/18/2016@1:06 AM

namespace Assets.Scripts.ProjectCommon.Utility.CircuitBreaker
{
    /// <summary>
    ///     Implementation of the circuit breaker design pattern, the state we use will be determined by number of failures
    ///     leading up to a threshold.
    ///     Closed means the circuit is operating normally, Open means the circuit is broken and will not serve requests until
    ///     closed again.
    /// </summary>
    public abstract class CircuitBreakerState : ICircuitBreakerState
    {
        /// <summary>
        ///     Holds the current state which the circuit breaker exists in.
        /// </summary>
        protected readonly CircuitBreaker _circuitBreaker;

        /// <summary>
        ///     Creates a new circuit breaker state, allowing any previous state data to be carried along into the next state.
        /// </summary>
        /// <param name="circuitBreaker">Circuit breaker object which we would like to transition with us into the next state type.</param>
        protected CircuitBreakerState(CircuitBreaker circuitBreaker)
        {
            _circuitBreaker = circuitBreaker;
        }

        /// <summary>
        ///     Circuit is going to attempt to make request to grid network.
        /// </summary>
        public virtual void RequestIsAboutToBeMade(out bool shouldCancel)
        {
            // Default action is to not stop requests from being made unless overridden.
            shouldCancel = false;
        }

        /// <summary>
        ///     Circuit has sent request to grid network, awaiting confirmation.
        /// </summary>
        public virtual void RequestHasBeenMade()
        {
        }

        /// <summary>
        ///     Circuit sent a request and it bounced back with failure, we need to handle this differently depending on what state
        ///     we are currently in.
        /// </summary>
        public virtual void RequestFailed()
        {
            _circuitBreaker.IncreaseFailureCount();
        }

        /// <summary>
        ///     Circuit sent a request and it bounced back with a success, depending on our current state this could change the
        ///     state to closed.
        /// </summary>
        public virtual void RequestSucceeded()
        {
            _circuitBreaker.ResetFailureCount();
        }

        /// <summary>
        ///     NeoAxis entity system ticked the game world, which allows us to work with a single frame of work.
        /// </summary>
        public virtual void CircuitTick()
        {
        }
    }
}