// @author : Alex

using UnityEngine;
using System.Collections;

namespace Extinction
{
    namespace Utils
    {

        /// <summary>
        /// Provide a Timer Utility with multiple callbacks of delegate type"TimerCallback".
        /// You could use it with lambda functions: () => { /* some code */} or with anonymous functions: delegate() { /* some code*/}
        /// From a maximum time execution the step callback would be executed
        /// Callbacks: start, step, end, stop.
        /// </summary>
        public class Timer : MonoBehaviour
        {
            public delegate void TimerCallback();

            /// <summary>
            /// _currentTime of Timer in seconds
            /// </summary>
            private float _currentTime;

            /// <summary>
            /// Return the current Time of the Timer
            /// </summary>
            public float currentTime { get { return _currentTime; } }

            /// <summary>
            /// _maxTime of Timer to be processed in seconds
            /// </summary>
            private float _maxTime;

            /// <summary>
            /// Return the max time of the Timer
            /// </summary>
            public float maxTime { get { return _maxTime; } }

            private bool _isStopped;

            private TimerCallback _startCallback;
            private TimerCallback _stepCallback;
            private TimerCallback _endCallback;
            private TimerCallback _stopCallback;

            /// <summary>
            /// Modify the properties of the timer
            /// </summary>
            /// <param name="maxTime">The time after the timer will stop</param>
            /// <param name="start">Callback which is called when the timer starts </param>
            /// <param name="step">Callback which is called at each loop step of the timer</param>
            /// <param name="end">Callback which is called when the timer ends</param>
            /// <param name="stop">Callback which is called when the timer is stopped before end</param>
            public void init( float maxTime, TimerCallback start, TimerCallback step, TimerCallback end, TimerCallback stop )
            {
                if ( maxTime < 0 )
                    throw new System.Exception( "Timer: maxTime must be greater than 0" );

                _isStopped = false;
                _maxTime = maxTime;
                _currentTime = 0;
                _startCallback = start;
                _stepCallback = step;
                _endCallback = end;
                _stopCallback = stop;
            }

            /// <summary>
            /// Warning != from MonoBehaviour Start (utility script)
            /// Start the timer coroutine from user trigger.
            /// Calls the start callback if provided.
            /// </summary>
            public void start()
            {
                _isStopped = false;
                StopCoroutine( routine() );

                _currentTime = 0;

                if ( _startCallback != null )
                    _startCallback();

                StartCoroutine(routine());
                //Debug.Log( "Utils.Timer was started at: " + Time.time );
            }


            /// <summary>
            /// Stop the timer from user trigger.
            /// Calls the stop callback if provided.
            /// </summary>
            public void stop()
            {
                StopCoroutine( routine() );

                _isStopped = true;

                if ( _stopCallback != null )
                    _stopCallback();

                //Debug.Log( "Utils.Timer was stoped at: " + Time.time );
            }

            public IEnumerator routine() {
                while (true) {
                    if ( _isStopped ) yield break;

                    _currentTime += Time.deltaTime;

                    if ( _currentTime > _maxTime )
                        break;

                    if ( _stepCallback != null )
                        _stepCallback();

                    yield return null;
                }
                
                if (_endCallback != null)
                    _endCallback();

                //Debug.Log( "Utils.Timer has ended at: " + Time.time );
            }
        }
    }
}
