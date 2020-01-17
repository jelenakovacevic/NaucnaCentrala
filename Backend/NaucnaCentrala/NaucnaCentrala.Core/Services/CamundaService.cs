using NaucnaCentrala.Core.Models;
using NaucnaCentrala.Domain.Models;
using NaucnaCentrala.Interfaces.Services;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;

namespace NaucnaCentrala.Core.Services
{
    public class CamundaService : ICamundaService
    {
        private HttpClient _httpClient = new HttpClient();
        private string _url;

        public CamundaService()
        {
            _url = "http://localhost:8080/engine-rest";
        }

        public bool CompleteExternalTask(string taskId, string workerId, string content)
        {
            content = $"{{ \"workerId\": \"{workerId}\", \"variables\" : {content}}}";

            var response = _httpClient.PostAsync($"{_url}/external-task/{taskId}/complete", new StringContent(content, Encoding.UTF8, "application/json")).Result;

            return response.IsSuccessStatusCode;
        }

        public bool CompleteTask(string taskId, string content)
        {
            content = "{ \"variables\" : " + content + "}";
            var response = _httpClient.PostAsync($"{_url}/task/{taskId}/complete", new StringContent(content, Encoding.UTF8, "application/json")).Result;

            return response.IsSuccessStatusCode;
        }

        public bool DeleteProcessInstance(string processInstanceId)
        {
            throw new System.NotImplementedException();
        }

        public FetchAndLockResponse FetchAndLockExternalTask(string workerId, string topicName, string[] variables)
        {
            var content = new
            {
                workerId,
                maxTasks = 1,
                topics = new[]
                {
                    new
                    {
                        topicName,
                        lockDuration = 10000,
                        variables
                    }
                }
            };

            string contentAsString = JsonConvert.SerializeObject(content);

            var response = _httpClient.PostAsync($"{_url}/external-task/fetchAndLock",
                new StringContent(contentAsString, Encoding.UTF8, "application/json")).Result;

            string responseContentString = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode && !responseContentString.Equals("[]"))
            {
                var responseContentObject = JsonConvert.DeserializeObject<FetchAndLockResponseEntry[]>(responseContentString);

                return new FetchAndLockResponse(true, responseContentObject[0]);
            }

            return new FetchAndLockResponse(false);
        }

        public string GetFormVariables(string taskId)
        {
            throw new System.NotImplementedException();
        }

        public string GetAssignedTaskId(string taskDefinitionKey, string userId)
        {
            var response = _httpClient.GetAsync($"{_url}/task?assignee={userId}&taskDefinitionKey={taskDefinitionKey}").Result;
            string responseAsString = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode && !responseAsString.Equals("[]"))
            {
                dynamic responseContent = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
                return responseContent[0]["id"];
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return "Not Found";
            }

            return null;
        }

        public string GetUnassignedTaskId(string taskDefinitionKey)
        {
            var response = _httpClient.GetAsync($"{_url}/task?taskDefinitionKey={taskDefinitionKey}").Result;
            string responseAsString = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode && !responseAsString.Equals("[]"))
            {
                dynamic responseContent = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
                return responseContent[0]["id"];
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return "Not Found";
            }

            return null;
        }

        public T GetVariableValue<T>(string variableName, long userId)
        {
            throw new System.NotImplementedException();
        }

        public bool SetAssignee(string taskId, string userId)
        {
            throw new System.NotImplementedException();
        }

        public void StartProcess(string processId, string user)
        {
            string content = JsonConvert.SerializeObject(new
            {
                variables = new
                {
                    starter = new CamundaVariable<string>(user)
                }
            });

            var response = _httpClient
                    .PostAsync($"{_url}/process-definition/key/{processId}/start", 
                    new StringContent(content, Encoding.UTF8, "application/json"))
                .Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }
        }

        public bool SubmitTaskForm(string taskId, string content)
        {
            content = "{ \"variables\" : " + content + "}";
            var response = _httpClient.PostAsync($"{_url}/task/{taskId}/submit-form", new StringContent(content, Encoding.UTF8, "application/json")).Result;

            return response.IsSuccessStatusCode;
        }
    }
}
