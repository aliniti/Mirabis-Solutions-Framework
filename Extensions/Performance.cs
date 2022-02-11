using System;
using System.Diagnostics;
using System.Windows.Forms;
using JetBrains.Annotations;
using Miracle_Business_Solutions_Framework.Base;
using Miracle_Business_Solutions_Framework.Managers;
using Miracle_Business_Solutions_Framework.UserInterfaces.Settings;
using Styx;
using Styx.Common.Helpers;
using Styx.TreeSharp;
using Action = Styx.TreeSharp.Action;

namespace Miracle_Business_Solutions_Framework.Extensions
{
    /// <summary>
    /// Performance influencing class
    /// </summary>
    [UsedImplicitly]
    class Performance
    {
        #region Tidy : Throttle
        /// <summary>
        /// ChinaJade's Throttle Class
        /// </summary>
        internal class Throttle : DecoratorContinue
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="throttleTime"></param>
            /// <param name="composite"></param>
            public Throttle(TimeSpan throttleTime, Composite composite)
                : base(composite)
            {
                _throttle.WaitTime = throttleTime;
                // _throttle was created with "0" time--this makes it "good to go" 
                // on first visit to CompositeThrottle node
            }

            /// <summary>
            /// To support seconds
            /// </summary>
            /// <param name="seconds"></param>
            /// <param name="composite"></param>
            public Throttle(int seconds, Composite composite)
                : base(composite)
            {
                _throttle.WaitTime = TimeSpan.FromSeconds(seconds);
                // _throttle was created with "0" time--this makes it "good to go" 
                // on first visit to CompositeThrottle node
            }

            /// <summary>
            /// 250ms default
            /// </summary>
            /// <param name="composite"></param>
            public Throttle(Composite composite)
                : base(composite)
            {
                _throttle.WaitTime = TimeSpan.FromMilliseconds(250);
                // _throttle was created with "0" time--this makes it "good to go" 
                // on first visit to CompositeThrottle node
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="context"></param>
            /// <returns></returns>
            protected override bool CanRun(object context)
            {
                if (!_throttle.IsFinished)
                { return false; }

                _throttle.Reset();
                return true;
            }

            private readonly WaitTimer _throttle = new WaitTimer(TimeSpan.FromSeconds(0));
        }

        #endregion

        #region Tidy : FrameLockSelector

        /// <summary>
        /// This behavior wraps the child behaviors in a 'frame lock' which can provide
        /// a big performance improvement if the child behaviors makes multiple HB API
        /// calls that internally run off a frame in WoW in one CC pulse.
        /// </summary>
        internal class FrameLock : PrioritySelector
        {
            /// <summary>
            /// Wraps the child behaviors in a 'frame lock'
            /// </summary>
            /// <param name="children"></param>
            public FrameLock(params Composite[] children)
                : base(children)
            {
                /* Things to know:
                 *  Doing too much work inside one frame lock...
                 *  ...is frequently the reason users complain about 'lag' or WoWclient 'stuttering'
                 *  
                 * Many HB API calls cannot be used under the auspices of a frame lock.
                 * There is no list of which HB API calls are permitted and which are not--its a "try it and see" exercise. If you make a bad 
                 * choice, it will most likely lock up Honorbuddy and the WoWclient, and you will be restarting both.
                 */
            }

            /// <summary>
            ///  Wraps the child behaviors in a 'frame lock', including a contextchange handler
            /// </summary>
            /// <param name="contextChange"></param>
            /// <param name="children"></param>
            public FrameLock(ContextChangeHandler contextChange, params Composite[] children)
                : base(contextChange, children)
            {
                /* Things to know:
              *  Doing too much work inside one frame lock...
              *  ...is frequently the reason users complain about 'lag' or WoWClient 'stuttering'
              *  
              * Many HB API calls cannot be used under the auspices of a frame lock.
              * There is no list of which HB API calls are permitted and which are not--its a "try it and see" exercise. If you make a bad 
              * choice, it will most likely lock up Honorbuddy and the WoWClient, and you will be restarting both.
              */
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="context"></param>
            /// <returns></returns>
            public override RunStatus Tick(object context)
            {
                using (StyxWoW.Memory.AcquireFrame())
                {
                    return base.Tick(context);
                }
            }

        }

        #endregion Tidy : FramelockSelector

        #region Tidy : CompositePerformance
        private static readonly Stopwatch CompositePerformanceTimer = new Stopwatch();
        /// <summary>  Usage: Spell.CompositePerformance(Composite, "SomeComposite") within a composite. </summary>
        internal static Composite CompositePerformance(Composite child, string name = "SomeComposite")
        {
            return new Sequence(
                new Action(delegate
                {
                    CompositePerformanceTimer.Reset();
                    CompositePerformanceTimer.Start();
                    return RunStatus.Success;
                }),
                child,
                new Action(delegate
                {
                    CompositePerformanceTimer.Stop();
                    Logger.DebugLog("[CompositePerformance] {0} took {1} ms", name, CompositePerformanceTimer.ElapsedMilliseconds);
                    return RunStatus.Success;
                }));
        }

        #endregion

        #region Tidy : TreePerformance
        private static readonly Stopwatch TreePerformanceTimer = new Stopwatch();
        /// <summary>
        /// Credits to Weischbier
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal static Composite Tree(string obj)
        {
            return new Action(ret =>
            {
                if (TreePerformanceTimer.ElapsedMilliseconds > 0)
                {
                    var elap = (int)TreePerformanceTimer.ElapsedMilliseconds;
                    if (HotkeyManager.IsKeyDown(Keys.F9))
                        Logger.PerfLog(@"[TreePerformance] Elapsed Time to traverse {0}: {1} ms ({2} ms client lag)", obj, elap, Lag.TotalMilliseconds);

                    TreePerformanceTimer.Stop();
                    TreePerformanceTimer.Reset();
                }

                TreePerformanceTimer.Start();

                return RunStatus.Failure;
            });
        }
        #endregion

        #region Tidy : BlockPerformance
        [DebuggerStepThrough]
        public class Block : IDisposable
        {
            private readonly string _blockName;
            private readonly Stopwatch _stopwatch;
            private bool _isDisposed;
            private readonly LogCategory _category;

            #region Tidy : Usage Function
            public Block(string blockName, LogCategory category)
            {
                if (MySettings.Instance.PERFORMANSESETTINGS.HasFlag(category))
                {
                    _blockName = blockName;
                    _category = category;
                    _stopwatch = new Stopwatch();
                    _stopwatch.Start();
                }
            }
            #endregion

            #region Tidy : IDisposable Members

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            /// <filterpriority>2</filterpriority>
            public void Dispose()
            {
                if (!_isDisposed)
                {
                    _isDisposed = true; //TODO: Change log category to settings 
                    if (_stopwatch != null && _category == LogCategory.Auras&& HotkeyManager.IsKeyDown(Keys.F10))
                    {
                        _stopwatch.Stop();
                        if (_stopwatch.Elapsed.Ticks > 0)
                        {
                            Logger.PerfLog("[Performance] Execution of the block {0} took {1:00.00}ms.", _blockName, _stopwatch.Elapsed.TotalMilliseconds);
                        }
                    }

                    GC.SuppressFinalize(this);
                }
            }

            #endregion IDisposable Members

            #region Tidy : MySettings Abbreviation


            /// <summary>
            /// Abbreviation for our Settings
            /// </summary>
            private static MySettings ST
            {

                get { return MySettings.Instance; }
            }

            #endregion External : MySettings Abbreviation


            /// <summary>
            /// Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.
            /// </summary>
            ~Block()
            {
                Dispose();
            }
        }
        #endregion

        #region Tidy : Lag
        /// <summary>
        /// Returns our latency in milliseconds
        /// </summary>
        private static TimeSpan Lag
        {
            get { return TimeSpan.FromMilliseconds((StyxWoW.WoWClient.Latency)); }
        }
        #endregion
    }
}