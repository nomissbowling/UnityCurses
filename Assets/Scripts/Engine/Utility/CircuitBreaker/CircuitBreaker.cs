// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 06/18/2016@1:06 AM

using System;
using UnityEngine;

namespace Assets.Scripts.Engine.Utility.CircuitBreaker
{
    /// <summary>
    ///     Implementation of circuit breaker design pattern interface using ticks as form of time measurement so it flows with
    ///     NeoAxis engine.
    /// </summary>
    public class CircuitBreaker : ICircuitBreaker
    {
        /// <summary>
        ///     Action that will be refereed to for making requests when circuit is in closed or half-open state.
        /// </summary>
        protected readonly Action _circuitAction;

        private readonly int _circuitThreshold;
        private readonly int _circuitTimeout;

        /// <summary>
        ///     Defines the current operational state the circuit breaker is in.
        /// </summary>
        private CircuitBreakerState _state;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Assets.Scripts.Engine.Utility.CircuitBreaker.CircuitBreaker" />
        ///     class.
        /// </summary>
        /// <param name="threshold">Total number of failures that will trip the circuit into an open state.</param>
        /// <param name="timeout">Number of ticks that will be counted in open circuits until attempting requests again.</param>
        /// <param name="request">
        ///     Method with no parameters that will check request logic and create request and add it to grid
        ///     network if needed.
        /// </param>
        public CircuitBreaker(int threshold, int timeout, Action request)
        {
            // Default number of failures the circuit has experienced so far.
            CircuitFailures = 0;

            // Check that threshold is greater than one.
            if (threshold < 1)
            {
                Debug.LogError("Circuit threshold should be greater than zero.");
                return;
            }

            // Check that tick timeout is greater than one.
            if (timeout < 1)
            {
                Debug.LogError("Circuit tick timeout should be greater than zero.");
                return;
            }

            // Copy the request action from entity we will call if in closed or half-open state.
            _circuitAction = request;

            // Copy the threshold for failure and timeout for counting ticks when in open state.
            _circuitThreshold = threshold;
            _circuitTimeout = timeout;

            // Start the circuit in normal closed state, and let requests flow each tick.
            MoveToClosedState();
        }

        /// <summary>
        ///     Total number of failures this circuit has experienced, this will only happen so many times until threshold is
        ///     reached.
        /// </summary>
        public int CircuitFailures { get; private set; }

        /// <summary>
        ///     Number of failures that will trip the circuit into an open state where it will stop making requests every tick and
        ///     wait before trying again.
        /// </summary>
        public int CircuitThreshold
        {
            get { return _circuitThreshold; }
        }

        /// <summary>
        ///     Total number of ticks the circuit will wait when in open state before trying the half-open state and another
        ///     request to know if it should stay open or close itself.
        /// </summary>
        public int CircuitTimeout
        {
            get { return _circuitTimeout; }
        }

        /// <summary>
        ///     Determines if the circuit is closed and letting requests flow every tick.
        /// </summary>
        public bool IsCircuitClosed()
        {
            return _state is CircuitClosedState;
        }

        /// <summary>
        ///     Determines if circuit is open and blocking all requests and waiting for tick timeout before trying again.
        /// </summary>
        public bool IsCircuitOpen()
        {
            return _state is CircuitOpenState;
        }

        /// <summary>
        ///     Determines if the circuit is in half-open state where it will try to make a request again after waiting for total
        ///     tick timeout.
        /// </summary>
        public bool IsHalfOpen()
        {
            return _state is CircuitHalfOpenState;
        }

        /// <summary>
        ///     Determines if the circuit request failure threshold has been reached and should trip circuit into the open state.
        /// </summary>
        public bool CircuitThresholdReached()
        {
            return CircuitFailures >= CircuitThreshold;
        }

        /// <summary>
        ///     Forcefully moves the circuit into an closed state which allows requests to flow every tick if required by logic.
        /// </summary>
        public void CircuitClose()
        {
            MoveToClosedState();
        }

        /// <summary>
        ///     Forcefully opens the circuit and blocks requests from being made until tick timeout has been reached.
        /// </summary>
        public void CircuitOpen()
        {
            MoveToOpenState();
        }

        /// <summary>
        ///     Ticks the internal logic of the circuit so it works in accordance with the NeoAxis entity system and how it handles
        ///     time dilation and deltas.
        /// </summary>
        public void TickCircuit()
        {
            // Each state has the ability to do something with the tick event that is passed to us from parent entity.
            _state.CircuitTick();
        }

        /// <summary>
        ///     Circuit sent a request and it bounced back with failure, we need to handle this differently depending on what state
        ///     we are currently in.
        /// </summary>
        public void RequestFailed()
        {
            // Pass the failed request trigger along to the circuit state.
            _state.RequestFailed();
        }

        /// <summary>
        ///     Circuit sent a request and it bounced back with a success, depending on our current state this could change the
        ///     state to closed.
        /// </summary>
        public void RequestSucceeded()
        {
            // Pass the completed request trigger to the circuit state.
            _state.RequestSucceeded();
        }

        /// <summary>
        ///     Passes request creation logic to circuit for throttling, will only call request logic again if there is reliable
        ///     chance it will get handled.
        /// </summary>
        internal void SendRequest()
        {
            // Each state can work with the request before we make it depending on what the state is.
            bool shouldCancel;
            _state.RequestIsAboutToBeMade(out shouldCancel);

            // Cancel the circuit from making requests if instructed to do so.
            if (shouldCancel)
                return;

            // Execute the logic related to the circuit making requests which is setup by entity that created us.
            _circuitAction.Invoke();

            // Fired after invoking the request so states can do something with the event if they want.
            _state.RequestHasBeenMade();
        }

        /// <summary>
        ///     Forces the circuit into closed state that allows a request every tick so long as it gets a reply from the grid
        ///     network.
        /// </summary>
        internal void MoveToClosedState()
        {
            _state = new CircuitClosedState(this);
        }

        /// <summary>
        ///     Forces the circuit into open state that blocks all requests and counts ticks until the tick timeout is reached.
        /// </summary>
        internal void MoveToOpenState()
        {
            _state = new CircuitOpenState(this);
        }

        /// <summary>
        ///     Forces the circuit into a half-open state that attempts to make a request and will trip to open if it fails and
        ///     count to tick timeout.
        /// </summary>
        internal void MoveToHalfOpenState()
        {
            _state = new CircuitHalfOpenState(this);
        }

        /// <summary>
        ///     Increases the number of failures this circuit breaker has attempted to send a request to grid network and got back
        ///     a failure reply.
        /// </summary>
        internal void IncreaseFailureCount()
        {
            CircuitFailures++;
        }

        /// <summary>
        ///     Resets the failure count back to zero. Normally called when flipping to closed state, or a request succeeds in
        ///     general.
        /// </summary>
        internal void ResetFailureCount()
        {
            CircuitFailures = 0;
        }
    }
}