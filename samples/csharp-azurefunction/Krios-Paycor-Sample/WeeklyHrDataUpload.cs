using System;
using System.Text;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Krios.DataLoad
{
    public class WeeklyHrDataUpload
    {
        private readonly ILogger _logger;
        private HttpClient _client;

        public WeeklyHrDataUpload(ILoggerFactory loggerFactory, IHttpClientFactory httpClientFactory)
        {
            _logger = loggerFactory.CreateLogger<WeeklyHrDataUpload>();
            _client = httpClientFactory.CreateClient();
        }

        [Function("WeeklyHrDataUpload")]
        public async Task Run([TimerTrigger("0 0 0 ? * MON *")] MyInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            var accessTokenBody = new FormUrlEncodedContent(new []
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", "{last refresh token from storage}"),
                new KeyValuePair<string, string>("client_id", "{client id}"), // oauth client id
                new KeyValuePair<string, string>("client_secret", "{client secret}"), // oauth client secret
            });
            _client.BaseAddress = new Uri("https://apis-sandbox.paycor.com/sts/");
            var accessTokenRequest = await _client.PostAsync("v1/common/token?subscription-key={subscriptionKey}", accessTokenBody);
            var tokenJson = await accessTokenRequest.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<PaycorAccessToken>(tokenJson);
            var tenantId = "{paycor tenant id}";
            _client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "{subscriptionKey}");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.access_token}");
            
            var employees = await GetEmployees(tenantId);
            var timeOffReport = await GetTimeOffForEmployees(employees, tenantId);

            var kriosData = new List<KriosData>();
            CreateKriosData(kriosData, employees, timeOffReport);

            await SendKriosData(kriosData, "{azure tenant id}");
        }

        private async Task SendKriosData(List<KriosData> kriosData, string tenantId)
        {
            _client.BaseAddress = new Uri("https://kz-krios-apim-prod.azure-api.net/");
            var kriosRequestBody = new StringContent(JsonConvert.SerializeObject(kriosData), Encoding.UTF8, "application/json");
            var kriosRequest = await _client.PostAsync($"api/DataLoad/{tenantId}/json", kriosRequestBody);
        }

        private void CreateKriosData(List<KriosData> kriosData, List<EmployeeDetails> employeeDetails, List<TimeOffRecord> timeOffRecords)
        {
            foreach (var employee in employeeDetails)
            {
                var kriosEmployee = new KriosData()
                {
                    EmployeeId = employee.Email.EmailAddress,
                    Dob = "",
                    NextPayroll = CalculateNextPayrollDate(),
                    WorkAnniversary = employee.EmploymentDateData.HireDate?.ToString("yyyy-MM-dd"),
                    PtoBalance = CalculatePtoBalance(employee, timeOffRecords),
                    UserPrincipalName = employee.Email.EmailAddress,
                    CustomData1 = "",
                    CustomData2 = "",
                    CustomData3 = ""
                };
            }
        }

        // This is a sample PTO calculation.  You will need to calculate the PTO balance based on your company's policies
        private decimal CalculatePtoBalance(EmployeeDetails employee, List<TimeOffRecord> timeOffRecords)
        {
            foreach (var timeOffRecord in timeOffRecords.Where(r => r.EmployeeId == employee.Id))
            {
                foreach (var typeBalance in timeOffRecord.TypeBalances)
                {
                    if (typeBalance.TimeOffTypeCode == "PTO")
                    {
                        return typeBalance.CurrentBalance ?? 0;
                    }
                }
            }
            return 0;
        }

        // This is a sample weekly payroll calculation.  You will need to calculate the next Payroll date based on your company's policies
        private string CalculateNextPayrollDate()
        {
            var nextPayrollDate = DateTime.Now;
            while (nextPayrollDate.DayOfWeek != DayOfWeek.Friday)
            {
                nextPayrollDate = nextPayrollDate.AddDays(1);
            }
            return nextPayrollDate.ToString("yyyy-MM-dd");
        }

        private async Task<List<TimeOffRecord>> GetTimeOffForEmployees(List<EmployeeDetails> employeeRecords, string tenantId)
        {
            var timeOffRecords = new List<TimeOffRecord>();
            foreach (var employeeRecord in employeeRecords)
            {
                var timeOffRequest = await _client.GetAsync($"v1/employees/{employeeRecord.Id}/timeoffaccruals");
                var timeOffJson = await timeOffRequest.Content.ReadAsStringAsync();
                var timeOffResponse = JsonConvert.DeserializeObject<TimeOffResult>(timeOffJson);
                timeOffRecords.AddRange(timeOffResponse.Records);
            }
            return timeOffRecords;
        }

        private async Task<List<EmployeeDetails>> GetEmployees(string tenantId)
        {
            var employees = new List<EmployeeRecord>();
            var employeesRequest = await _client.GetAsync($"v1/tenants/{tenantId}/employees?statusFilter=Active");
            var employeesJson = await employeesRequest.Content.ReadAsStringAsync();
            var employeesResponse = JsonConvert.DeserializeObject<PaycorEmployeesResponse>(employeesJson);
            employees.AddRange(employeesResponse.Records);
            while (!string.IsNullOrEmpty(employeesResponse.AdditionalResultsUrl))
            {
                employeesRequest = await _client.GetAsync(employeesResponse.AdditionalResultsUrl);
                employeesJson = await employeesRequest.Content.ReadAsStringAsync();
                employeesResponse = JsonConvert.DeserializeObject<PaycorEmployeesResponse>(employeesJson);
                employees.AddRange(employeesResponse.Records);
            }

            var employeeData = new List<EmployeeDetails>();
            foreach (var employeeRecord in employees.Select(r => r.Employee.Url))
            {
                var employeeRequest = await _client.GetAsync(employeeRecord);
                var employeeJson = await employeeRequest.Content.ReadAsStringAsync();
                var employee = JsonConvert.DeserializeObject<EmployeeDetails>(employeeJson);
                employeeData.Add(employee);
            }
            return employeeData;
        }
    }


    public class PaycorAccessToken
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
        public string token_type { get; set; }
    }

    public class MyInfo
    {
        public MyScheduleStatus ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
