export interface UserTask {
    id: number;
    userTaskType: string;
    userTaskTypeId: number;
    description: string;
    userTaskState: string;
    userTaskStateId: number;
    priority: string;
    priorityId: number;
    executorUser: string;
    executorUserId: number;
    taskManagerUser: string;
    taskManagerUserId: number;
    executeTaskUntilDate: string;
    payloadId: number;
    isPayloadExists: boolean;
    totalCountOfTasks: number;
}