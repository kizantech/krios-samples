namespace Krios.DataLoad
{
    internal class PaycorEmployeesResponse
    {
        public IEnumerable<EmployeeRecord> Records { get; internal set; }
        public string HasMoreResults { get; internal set; }
        public string ContinuationToken { get; internal set; }
        public string AdditionalResultsUrl { get; internal set; }
    }
}