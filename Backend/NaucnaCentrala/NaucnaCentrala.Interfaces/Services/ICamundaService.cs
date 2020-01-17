using NaucnaCentrala.Domain.Models;

namespace NaucnaCentrala.Interfaces.Services
{
    public interface ICamundaService
    {
        void StartProcess(string processId, string user);
        bool DeleteProcessInstance(string processInstanceId);
        string GetAssignedTaskId(string taskDefinitionKey, string userId);
        string GetUnassignedTaskId(string taskDefinitionKey);
        string GetFormVariables(string taskId);
        bool SetAssignee(string taskId, string userId);
        bool SubmitTaskForm(string taskId, string content);
        bool CompleteTask(string taskId, string content);
        FetchAndLockResponse FetchAndLockExternalTask(string workerId, string topicName, string[] variables);
        bool CompleteExternalTask(string taskId, string workerId, string content);
        T GetVariableValue<T>(string variableName, long userId);
    }
}
