using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using log4net;

namespace MessengerAppender.Console
{
  class Program
  {
    public static ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    static void Main(string[] args)
    {
      string line;
      while (string.IsNullOrEmpty(line = System.Console.ReadLine()) == false)
      {
        Log.Debug(line);
      }
    }
  }
}
