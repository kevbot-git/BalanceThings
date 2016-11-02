using System.Collections.Generic;

namespace BalanceThings.Util
{
    internal class ContentManager
    {
        private Queue<LoadTask> _content;

        private int _totalTasks;

        internal ContentManager()
        {
            _content = new Queue<LoadTask>();
            _totalTasks = 0;
        }

        internal string LoadNext()
        {
            if (_content.Count == 0)
                return null;

            LoadTask t = _content.Dequeue();

            t.Do();

            return t.Name;
        }

        internal string PeekNext()
        {
            if (_content.Count == 0)
                return null;

            return _content.Peek().Name;
        }

        internal bool AddTask(LoadTask loadTask)
        {
            if (loadTask == null)
                return false;

            foreach (LoadTask t in _content)
            {
                if (t.Name.Equals(loadTask.Name))
                    return false;
            }

            _content.Enqueue(loadTask);
            _totalTasks = _content.Count;

            return true;
        }

        internal void LoadAll()
        {
            while(!IsFinishedLoading)
            {
                string finishedTaskName = LoadNext();
            }
        }

        internal bool IsFinishedLoading { get { return _content.Count == 0; } }

        internal float LoadProgress
        {
            get
            {
                if (_content.Count == 0)
                {
                    if (_totalTasks == 0)
                        return 0f;

                    return 1f;
                }
                return 1f - ((float)_content.Count / _totalTasks);
            }
        }
    }
}