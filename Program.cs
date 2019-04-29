// Decompiled with JetBrains decompiler
// Type: coinminner.Program
// Assembly: rnocoinminer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D88D9D4B-A690-412D-B858-EE24C0C62E1A
// Assembly location: C:\Program Files (x86)\RNO\RnoMiner(Beta)\rnocoinminer.exe

using System;
using System.Threading;
using System.Windows.Forms;

namespace coinminner
{
  internal static class Program
  {
    [STAThread]
    private static void Main()
    {
      bool createdNew;
      Mutex mutex = new Mutex(true, "MutexName", out createdNew);
      if (createdNew)
      {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run((Form) new CoinMinner());
        mutex.ReleaseMutex();
      }
      else
        Application.Exit();
    }
  }
}
