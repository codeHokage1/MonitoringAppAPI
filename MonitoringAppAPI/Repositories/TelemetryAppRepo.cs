
using OpenTelemetry.Metrics;
using Proto;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using MonitoringAppAPI.Data;
using MonitoringAppAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace MonitoringAppAPI.Repositories
{
    public class TelemetryAppRepo
    {
        private readonly AppDBContext _context;

        public TelemetryAppRepo(AppDBContext context)
        {
            _context = context;
        }

       

       

        
    }
}



