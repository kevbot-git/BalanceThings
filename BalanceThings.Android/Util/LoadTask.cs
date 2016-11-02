namespace BalanceThings.Util
{
    class LoadTask
    {
        internal delegate void Task();

        private string _name;
        private Task _task;

        internal LoadTask(string name, Task task)
        {
            _name = name;
            _task = task;
        }

        internal string Do()
        {
            _task();
            return Name;
        }

        internal string Name { get { return _name; } }
    }
}