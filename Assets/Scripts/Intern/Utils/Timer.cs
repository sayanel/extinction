// @author : Alex

using UnityEngine;
using System.Collections;

namespace Extinction {
    namespace Utils {

        /// <summary>
        /// Provide a Timer Utility with multiple callbacks of delegate type"TimerCallback".
        /// You could use it with lambda functions: () => { /* some code */} or with anonymous functions: delegate() { /* some code*/}
        /// From a maximum time execution the step callback would be executed
        /// Callbacks: start, step, end, stop.
        /// </summary>
        public class Timer : MonoBehaviour {
            public delegate void TimerCallback();

            /// <summary>
            /// _currentTime of Timer in seconds
            /// </summary>
            private float _currentTime;

            /// <summary>
            /// _maxTime of Timer to be processed in seconds
            /// </summary>

            private float _maxTime;

            private TimerCallback _startCallback;
            private TimerCallback _stepCallback;
            private TimerCallback _endCallback;
            private TimerCallback _stopCallback;

            public Timer(float maxTime, TimerCallback start, TimerCallback step, TimerCallback end, TimerCallback stop) {
                if (maxTime < 0)
                    throw new System.Exception("Timer: maxTime must be greater than 0");

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
            public void Start() {
                StopCoroutine(Routine());
                _currentTime = 0;

                if (_startCallback != null)
                    _startCallback();

                StartCoroutine(Routine());
                Debug.Log("Utils.Timer was started at: " + Time.time);
            }


            /// <summary>
            /// Stop the timer from user trigger.
            /// Calls the stop callback if provided.
            /// </summary>
            public void Stop() {
                StopCoroutine(Routine());

                if (_stopCallback != null)
                    _stopCallback();
                Debug.Log("Utils.Timer was stoped at: " + Time.time);
            }

            public IEnumerator Routine() {
                while (true) {
                    _currentTime += Time.deltaTime;

                    if (_currentTime > _maxTime)
                        break;

                    if (_stepCallback != null)
                        _stepCallback();

                    yield return null;
                }
                if (_endCallback != null)
                    _endCallback();

                Debug.Log("Utils.Timer has ended at: " + Time.time);
            }
        }
    }
}
