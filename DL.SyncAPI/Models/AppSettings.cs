using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DL.SyncAPI.Models
{
    public class AppConfig
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }


}