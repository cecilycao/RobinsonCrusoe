using System.Collections;
using System.Collections.Generic;
using UniRx;
using System;

[Serializable]
public class WatcherManager
{
    public List<string> runningWatcherNameList;
    public List<string> pausedWatcherNameList;

    private Dictionary<string, IDisposable> runningWatcher = new Dictionary<string, IDisposable>();
    private Dictionary<string, IDisposable> pausedWatcher = new Dictionary<string, IDisposable>();
    public void RegisterWatcher(IDisposable watcher)
    {
       
    }

}
