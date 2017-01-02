// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 06/18/2016@1:06 AM

namespace Assets.Engine.Utility.CircuitBreaker
{
    /// <summary>
    ///     Contains all of the methods required for a circuit breaker state, this should be implemented into a class and used
    ///     in a concrete implementation making up each individual type of circuit state.
    /// </summary>
    public interface ICircuitBreakerState
    {
        /// <summary>
        ///     Circuit is going to attempt to make request to grid network.
        /// </summary>
        void RequestIsAboutToBeMade(out bool shouldCancel);

        /// <summary>
        ///     Circuit has sent request to grid network, awaiting confirmation.
        /// </summary>
        void RequestHasBeenMade();

        /// <summary>
        ///     Circuit sent a request and it bounced back with failure, we need to handle this differently depending on what state
        ///     we are currently in.
        /// </summary>
        void RequestFailed();

        /// <summary>
        ///     Circuit sent a request and it bounced back with a success, depending on our current state this could change the
        ///     state to closed.
        /// </summary>
        void RequestSucceeded();

        /// <summary>
        ///     NeoAxis entity system ticked the game world, which allows us to work with a single frame of work.
        /// </summary>
        void CircuitTick();
    }
}