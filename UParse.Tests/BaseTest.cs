using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;

namespace UParse.Tests
{
    public class BaseTest
    {
        protected void Log(string message)
        {
            Logger.LogMessage("{0}", message);
        }
    }
}