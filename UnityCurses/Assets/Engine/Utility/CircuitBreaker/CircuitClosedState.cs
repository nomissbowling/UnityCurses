// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 06/18/2016@1:06 AM

namespace Assets.Engine.Utility.CircuitBreaker
{
    /// <summary>
    ///     Closed circuit will allow requests to flow every tick if machine logic requires it.
    /// </summary>
    public class CircuitClosedState : CircuitBreakerState
    {
        /// <summary>
        ///     Closed circuits allow requests to flow through each tick, if required by the logic in the action driving them.
        /// </summary>
        /// <param name="circuitBreaker"></param>
        public CircuitClosedState(CircuitBreaker circuitBreaker) : base(circuitBreaker)
        {
            // Closed state will reset failure count so request will have to fail multiple times to open circuit again.
            circuitBreaker.ResetFailureCount();
        }

        /// <summary>
        ///     NeoAxis entity system ticked the game world, which allows us to work with a single frame of work.
        /// </summary>
        public override void CircuitTick()
        {
            base.CircuitTick();

            // Invokes the action which was assigned to the circuit breaker which should be a request being sent.
            _circuitBreaker.SendRequest();
        }

        /// <summary>
        ///     Every time a circuit in closed state fails a request we will decrease the reliability of the grid for that type of
        ///     request.
        /// </summary>
        public override void RequestFailed()
        {
            // Increase failure count towards threshold of total failures before circuit is tripped to open state.
            base.RequestFailed();

            // Check if maximum amount of failures have been reached.
            if (_circuitBreaker.CircuitThresholdReached())
                _circuitBreaker.MoveToOpenState();
        }
    }
}