using System;

namespace Everyone
{
    public class FakeMessageLogger : Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging.IMessageLogger
    {
        public void SendMessage(Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging.TestMessageLevel testMessageLevel, string message)
        {
            throw new NotImplementedException();
        }
    }
}
