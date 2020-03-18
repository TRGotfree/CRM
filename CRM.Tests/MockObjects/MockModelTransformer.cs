using System;
using System.Collections.Generic;
using System.Text;
using CRM.Interfaces;

namespace CRM.Tests.MockObjects
{
    public class MockModelTransformer : ITransformer
    {
        public Models.UserTask UserTaskDTOModelToModel(DTOModels.UserTask userTask)
        {
            return new Models.UserTask();
        }

        public IEnumerable<DTOModels.UserTask> UserTasksToDTOModels(IEnumerable<Models.UserTask> userTasks)
        {
            return new List<DTOModels.UserTask>(0);
        }

        public DTOModels.UserTask UserTaskToDTOModel(Models.UserTask userTaskModel)
        {
            return new DTOModels.UserTask();
        }
    }
}
