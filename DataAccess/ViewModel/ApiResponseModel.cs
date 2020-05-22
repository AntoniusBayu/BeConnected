using System.Collections.Generic;

namespace DataAccess
{
    public class ApiResponseModel
    {
        public string Version { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }
        public List<Validation> Validations { get; set; }
    }

    public class Validation
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
