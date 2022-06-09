using System.ComponentModel.DataAnnotations;

namespace ProcessSystem.UI.Blazor.Server.Middleware
{
    public class ProcessSystemConnectOptions
    {
        public const string RootPropertyName = "ProcessSystem";

        [Required]
        public string Endpoint { get; set; }
    }
}
