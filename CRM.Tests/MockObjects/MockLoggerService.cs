using System;
using System.Collections.Generic;
using System.Text;
using CRM.Interfaces;

namespace CRM.Tests.MockObjects
{
    public class MockLoggerService : ICustomLogger
    {
        public void LogError(Exception ex)
        {
            
        }

        public void LogError(string errorMessage)
        {
            
        }

        public void LogInfo(string infoMessage)
        {
           
        }

        public void LogWarn(string warningMessage)
        {
           
        }
    }
}
