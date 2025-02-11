using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AADizErp.Services.RequestServices
{
    public class ConveyanceService
    {
        HttpClient _client;
        JsonSerializerOptions _serializerOptions;

        private readonly NotificationService _notify;
        public ConveyanceService(NotificationService notify)
        {
            _client = new HttpClient() { BaseAddress = new Uri(App.BaseAddress) };
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            _notify = notify;
        }
    }
}
