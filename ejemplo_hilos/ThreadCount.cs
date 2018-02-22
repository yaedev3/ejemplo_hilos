using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ejemplo_hilos
{
    public delegate void ShowData(List<String> match, List<int> count);
    class ThreadCount
    {
        private Thread t;
        private string file;
        private List<String> words, match;
        private List<int> count;
        private int index;
        private ShowData sd;
        private EventWaitHandle waitHandle;
        private ProgressBar pb;

        public ThreadCount(string file, bool which)
        {
            match = new List<string>();
            count = new List<int>();
            index = 0;
            waitHandle = new ManualResetEvent(initialState: true);
            if (which)
            {
                t = new Thread(CountLetters);
                this.file = file.Replace(" ", "").Replace("\n", "").Replace("\r", "");
            }
            else
            {
                t = new Thread(CountWords);
                words = new List<string>(file.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
            }
            t.Start();
        }

        private void CountLetters()
        {
            while (index < file.Length)
            {
                if (!match.Contains(file[index].ToString()))
                {
                    match.Add(file[index].ToString());
                    count.Add(1);
                }
                else
                {
                    count[match.IndexOf(file[index].ToString())]++;
                }
                index++;
                sd(match, count);
                pb(true);
                waitHandle.WaitOne();
                Thread.Sleep(13);
            }
        }

        private void CountWords()
        {
            while (index < words.Count)
            {
                if (!match.Contains(words[index].Replace(",","").Replace(".","")))
                {
                    match.Add(words[index].Replace(",", "").Replace(".", ""));
                    count.Add(1);
                }
                else
                {
                    count[match.IndexOf(words[index].Replace(",", "").Replace(".", ""))]++;
                }
                index++;
                sd(match, count);
                pb(false);
                waitHandle.WaitOne();
                Thread.Sleep(13);
            }
        }

        public ShowData AccessData
        {
            get
            {
                return sd;
            }
            set
            {
                sd = value;
            }
        }

        public int GetSize(bool which)
        {
            int size;
            if (which)
                size = file.Length;
            else size = words.Count;
            return size;
        }
        
        public void Resume()
        {
            waitHandle.Set();
        }

        public void Pause()
        {
            waitHandle.Reset();
        }

        public void Stop()
        {
            t.Abort();
        }

        public ProgressBar AccessBar
        {
            get
            {
                return pb;
            }
            set
            {
                pb = value;
            }
        }
    }
}
