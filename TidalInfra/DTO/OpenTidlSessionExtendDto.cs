using OpenTidl.Methods;

namespace TidalInfra.DTO
{
    public class OpenTidlSessionExtendDto
    {
        public OpenTidlSession OpenTidlSession { get; set; }
        public string Error { get; set; }
        public int? ErrorCode { get; set; }
    }
}

