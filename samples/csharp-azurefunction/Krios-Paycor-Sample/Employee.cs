namespace Krios.DataLoad
{
    internal class EmployeeRecord
    {
        public Guid Id { get; set; }
        public int EmployeeNumber { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Employee Employee { get; set; }
    }

    internal class Employee
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
    }

    internal class EmployeeDetails
    {
        public Guid Id { get; set; }
        public int EmployeeNumber { get; set; }
        public int AlternateEmployeeNumber { get; set; }
        public int BadgeNumber { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Email Email { get; set;}
        public EmploymentDateData EmploymentDateData { get; set; }
    }

    internal class EmploymentDateData
    {
        public DateTime? HireDate { get; set; }
        public DateTime? AdjustedHireDate { get; set; }
        public DateTime? ReHireDate { get; set; }
        public DateTime? SeniorityDate { get; set; }
    }

    internal class TimeOffResult
    {
        public string HasMoreResults { get; set; }
        public string ContinuationToken { get; set; }
        public string AdditionalResultsUrl { get; set; }
        public IEnumerable<TimeOffRecord> Records { get; set; }
    }

    internal class TimeOffRecord
    {
        public string EmployeeName { get; set; }
        public int EmployeeNumber { get; set; }
        public Guid EmployeeId { get; set; }
        public IEnumerable<TypeBalance> TypeBalances { get; set; }
    }

    internal class TypeBalance
    {
        public string TimeOffPlanName { get; set; }
        public Guid TimeOffTypeId { get; set; }
        public string TimeOffTypeCode { get; set; }
        public string TimeOffTypeName { get; set; }
        public DateTime? ActivityStartDate { get; set; }
        public DateTime? ActivityEndDate { get; set; }
        public decimal? CurrentBalance { get; set; }
        public decimal? AccruedBalance { get; set; }
        public decimal? UsedBalance { get; set; }
        public decimal? ScheduledBalance { get; set; }
    }

    internal class Email
    {
        public string Type { get; set;}
        public string EmailAddress { get; set; }
    }
}