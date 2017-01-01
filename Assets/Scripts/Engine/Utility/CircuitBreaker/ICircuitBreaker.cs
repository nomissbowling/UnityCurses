// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 06/18/2016@1:06 AM

namespace Assets.Scripts.Engine.Utility.CircuitBreaker
{
    /// <summary>
    ///     Contains the functions and properties required to rig up a circuit breaker design pattern.
    ///     This interface is intended to be used in conjunction with the grid node and grid network system.
    ///     It's design goal is to limit the number of requests coming into the system.
    /// </summary>
    public interface ICircuitBreaker
    {
        /// <summary>
        ///     Total number of failures this circuit has experienced, this will only happen so many times until threshold is
        ///     reached.
        /// </summary>
        int CircuitFailures { get; }

        /// <summary>
        ///     Number of failures that will trip the circuit into an open state where it will stop making requests every tick and
        ///     wait before trying again.
        /// </summary>
        int CircuitThreshold { get; }

        /// <summary>
        ///     Total number of ticks the circuit will wait when in open state before trying the half-open state and another
        ///     request to know if it should stay open or close itself.
        /// </summary>
        int CircuitTimeout { get; }

        /// <summary>
        ///     Determines if the circuit is closed and letting requests flow every tick.
        /// </summary>
        bool IsCircuitClosed();

        /// <summary>
        ///     Determines if circuit is open and blocking all requests and waiting for tick timeout before trying again.
        /// </summary>
        bool IsCircuitOpen();

        /// <summary>
        ///     Determines if the circuit is in half-open state where it will try to make a request again after waiting for total
        ///     tick timeout.
        /// </summary>
        bool IsHalfOpen();

        /// <summary>
        ///     Determines if the circuit request failure threshold has been reached and should trip circuit into the open state.
        /// </summary>
        bool CircuitThresholdReached();

        /// <summary>
        ///     Forcefully moves the circuit into an closed state which allows requests to flow every tick if required by logic.
        /// </summary>
        void CircuitClose();

        /// <summary>
        ///     Forcefully opens the circuit and blocks requests from being made until tick timeout has been reached.
        /// </summary>
        void CircuitOpen();

        /// <summary>
        ///     Ticks the internal logic of the circuit so it works in accordance with the NeoAxis entity system and how it handles
        ///     time dilation and deltas.
        /// </summary>
        void TickCircuit();

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
    }
}