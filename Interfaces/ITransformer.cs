using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Interfaces
{
    public interface ITransformer
    {

        DTOModels.UserTask UserTaskToDTOModel(Models.UserTask userTaskModel);

        IEnumerable<DTOModels.UserTask> UserTasksToDTOModels(IEnumerable<Models.UserTask> userTasks);

        Models.UserTask UserTaskDTOModelToModel(DTOModels.UserTask userTask);
    }
}
