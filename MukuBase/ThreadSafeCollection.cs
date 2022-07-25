using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VBase
{
    public class ThreadSafeCollection<T> : ICollection<T>
    {
        private List<T> _list;
        public ThreadSafeCollection()
        {
            _list = new List<T>();
            Task.Run(() =>
            {
                while (true)
                {
                    if (ops.Count == 0)
                    {
                        opadded.WaitOne();
                    }
                    nextopid = ops[0];
                    nextop.Set();
                    nextop.Reset();
                    opdone.WaitOne();
                    opdone.Reset();
                }
            });
        }

        public int Count => _list.Count;

        public bool IsReadOnly => false;

        private List<int> ops = new List<int>();
        private static Random RNG = new Random();
        private int Regop()
        {
            int a = RNG.Next();
            ops.Add(a);
            opadded.Set();
            return a;
        }
        private ManualResetEvent nextop = new ManualResetEvent(false);
        private ManualResetEvent opdone = new ManualResetEvent(false);
        private ManualResetEvent opadded = new ManualResetEvent(false);
        private int nextopid = int.MinValue;
        public void Add(T item)
        {
            int i = Regop();
            while (true)
            {
                nextop.WaitOne();
                if (nextopid == i)
                {
                    break;
                }
            }
            _list.Add(item);
            opdone.Set();
        }

        public void Clear()
        {
            int i = Regop();
            while (true)
            {
                nextop.WaitOne();
                if (nextopid == i)
                {
                    break;
                }
            }
            _list.Clear();
            opdone.Set();
        }

        public bool Contains(T item)
        {
            int i = Regop();
            while (true)
            {
                nextop.WaitOne();
                if (nextopid == i)
                {
                    break;
                }
            }
            bool b = _list.Contains(item);
            opdone.Set();
            return b;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            int i = Regop();
            while (true)
            {
                nextop.WaitOne();
                if (nextopid == i)
                {
                    break;
                }
            }
            _list.CopyTo(array, arrayIndex);
            opdone.Set();
        }

        public IEnumerator<T> GetEnumerator()
        {
            int i = Regop();
            while (true)
            {
                nextop.WaitOne();
                if (nextopid == i)
                {
                    break;
                }
            }
            var a = _list.GetEnumerator();
            opdone.Set();
            return a;
        }

        public bool Remove(T item)
        {
            int i = Regop();
            while (true)
            {
                nextop.WaitOne();
                if (nextopid == i)
                {
                    break;
                }
            }
            bool b = _list.Remove(item);
            opdone.Set();
            return b;
        }
        public void RemoveAt(int index)
        {
            int i = Regop();
            while (true)
            {
                nextop.WaitOne();
                if (nextopid == i)
                {
                    break;
                }
            }
            _list.RemoveAt(index);
            opdone.Set();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            int i = Regop();
            while (true)
            {
                nextop.WaitOne();
                if (nextopid == i)
                {
                    break;
                }
            }
            var a = _list.GetEnumerator();
            opdone.Set();
            return a;
        }

        public T this[int i]
        {
            get
            {
                int id = Regop();
                while (true)
                {
                    nextop.WaitOne();
                    if (nextopid == id)
                    {
                        break;
                    }
                }
                var a = _list[i];
                opdone.Set();
                return a;
            }
            set
            {
                int id = Regop();
                while (true)
                {
                    nextop.WaitOne();
                    if (nextopid == id)
                    {
                        break;
                    }
                }
                _list[i] = value;
                opdone.Set();
            }
        }

    }
}
