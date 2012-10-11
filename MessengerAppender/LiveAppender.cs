using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Appender;
using log4net.Core;

namespace MessengerAppender
{
  public class LiveAppender : AppenderSkeleton
  {
    public LiveAppender()
    {
    }

    protected override void Append(LoggingEvent loggingEvent)
    {
    }
  }
}
