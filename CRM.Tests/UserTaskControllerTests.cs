using Microsoft.VisualStudio.TestTools.UnitTesting;
using CRM.Controllers;

namespace CRM.Tests
{
    [TestClass]
    public class UserTaskControllerTests
    {
        [TestMethod]
        public void GetMeta()
        {
            UserTaskController userTaskController = new UserTaskController(new MockObjects.MockLoggerService(), new MockObjects.MockRepository(), new MockObjects.MockModelTransformer());
            var result = userTaskController.Get();

            Assert.IsNotNull(result);
        }
    }
}
