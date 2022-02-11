using System.Collections.Generic;
using System.Linq;
using Miracle_Business_Solutions_Framework.Managers;
using Styx.TreeSharp;

namespace Miracle_Business_Solutions_Framework.Extensions
{
    /// <summary>
    /// Sequential casting extension of the basic TreeRoot behavior
    /// </summary>
    internal class SequenceCast
    {
        private readonly List<Composite> _children;
        private int _current;
        private readonly int _endSequence;
        private bool _sequenceRunning;

        public SequenceCast(List<Composite> l)
        {
            _children = l;
            _current = 0;
            _endSequence = l.Count();
            _sequenceRunning = false;
        }

        public Composite Execute(Root.Selection<bool> reqs = null)
        {
            return new Decorator(ret => _sequenceRunning || (reqs == null || reqs(ret)), new PrioritySelector(WaitForCast, ExecuteCurrentNode()));
        }

        private static Composite WaitForCast
        {
            get { return new Decorator(ret => CastManager.IsCastingorChanneling() || CastManager.IsGlobalCooldown(), new Action(ret => RunStatus.Success)); }
        }

        private Composite ExecuteCurrentNode()
        {
            return new Action(context =>
            {
                //Check for end of sequence
                if (_current >= _endSequence)
                {
                    _current = 0;
                    _sequenceRunning = false;
                    return RunStatus.Failure;
                }

                //Sequence isnt over, try to run next node
                var node = _children.ElementAt(_current);
                node.Start(context);
                while (node.Tick(context) == RunStatus.Running)
                {
                    //Run Node
                }
                node.Stop(context);

                //Node Failed, so sequence over!
                if (node.LastStatus == RunStatus.Failure)
                {
                    _current = 0;
                    _sequenceRunning = false;
                    return RunStatus.Failure;
                }

                //Node Succeed, Increment!!
                _current++;
                _sequenceRunning = true;
                return RunStatus.Success;
            });
        }
    }
}