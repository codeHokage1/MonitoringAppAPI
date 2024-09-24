//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace MonitoringAppAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class MonitoringAppAPIController : ControllerBase
//    {
//        private readonly ILogger _logger;

//        public MonitoringAppAPIController(ILogger<MonitoringAppAPIController> logger)
//        {
//            _logger = logger;
//        }
//        [HttpGet]
//        public IActionResult Get()
//        {
//            return Ok("Hello from MonitoringAppAPI!");
//        }

//        [HttpPost]
//        public IActionResult receiveData(object data)
//        {
//            {
//                _logger.LogInformation("LOGGER!! - Data received: {data}", data);
//                return StatusCode(StatusCodes.Status201Created);
//            }
//        }

//        [HttpPost("traces")]
//        public IActionResult ReceiveTraces([FromBody] object traceData)
//        {
//            _logger.LogInformation("Received trace data: {TraceData}", traceData);
//            return Ok();
//        }

//        [HttpPost("metrics")]
//        public IActionResult ReceiveMetrics([FromBody] object metricData)
//        {
//            _logger.LogInformation("Received metric data: {MetricData}", metricData);
//            return Ok();
//        }

//        [HttpPost("logs")]
//        public IActionResult ReceiveLogs([FromBody] object logData)
//        {
//            _logger.LogInformation("Received log data: {LogData}", logData);
//            return Ok();
//        }


//    }
//}

//// ============== Last correct endpoint ==============
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace MonitoringAppAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class MonitoringAppAPIController : ControllerBase
//    {
//        private readonly ILogger<MonitoringAppAPIController> _logger;

//        public MonitoringAppAPIController(ILogger<MonitoringAppAPIController> logger)
//        {
//            _logger = logger;
//        }

//        [HttpGet]
//        public IActionResult Get()
//        {
//            return Ok("Hello from MonitoringAppAPI!");
//        }

//        [HttpPost]
//        public IActionResult ReceiveData([FromBody] object data)
//        {
//            if (data == null)
//            {
//                _logger.LogWarning("Received empty data.");
//                return BadRequest("Data cannot be null.");
//            }

//            _logger.LogInformation("Data received: {Data}", data);
//            return StatusCode(StatusCodes.Status201Created);
//        }

//        [HttpPost("traces")]
//        public IActionResult ReceiveTraces([FromBody] object traceData)
//        {
//            _logger.LogInformation("TRACE ENDPOINT HIT!");

//            if (traceData == null)
//            {
//                _logger.LogWarning("Received empty trace data.");
//                return BadRequest("Trace data cannot be null.");
//            }

//            _logger.LogInformation("Received trace data: {TraceData}", traceData);
//            return Ok();
//        }

//        [HttpPost("metrics")]
//        public IActionResult ReceiveMetrics([FromBody] object metricData)
//        {
//            _logger.LogInformation("METRICS ENDPOINT HIT!");

//            if (metricData == null)
//            {
//                _logger.LogWarning("Received empty metric data.");
//                return BadRequest("Metric data cannot be null.");
//            }

//            _logger.LogInformation("Received metric data: {MetricData}", metricData);
//            return Ok();
//        }

//        [HttpPost("logs")]
//        public IActionResult ReceiveLogs([FromBody] object logData)
//        {
//            _logger.LogInformation("LOGS ENDPOINT HIT!");
//            if (logData == null)
//            {
//                _logger.LogWarning("Received empty log data.");
//                return BadRequest("Log data cannot be null.");
//            }

//            _logger.LogInformation("Received log data: {LogData}", logData);
//            return Ok();
//        }
//    }
//}



//// ============== Lastest Latest correct endpoint ==============

//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System.IO;
//using System.Threading.Tasks;
//using OpenTelemetry.Proto.Collector.Metrics.V1;
//using Google.Protobuf;
//using Microsoft.AspNetCore.Mvc;

//namespace MonitoringAppAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class MonitoringAppAPIController : ControllerBase
//    {
//        private readonly ILogger<MonitoringAppAPIController> _logger;

//        public MonitoringAppAPIController(ILogger<MonitoringAppAPIController> logger)
//        {
//            _logger = logger;
//        }

//        [HttpPost("traces")]
//        [Consumes("application/x-protobuf")]
//        public async Task<IActionResult> ReceiveTraces()
//        {
//            _logger.LogInformation("TRACE ENDPOINT HIT!");
//            using var reader = new StreamReader(Request.Body);
//            var body = await reader.ReadToEndAsync();
//            _logger.LogInformation("Received trace data: {TraceData}", body);
//            return Ok();
//        }

//        [HttpPost("metrics")]
//        [Consumes("application/x-protobuf")]
//        public async Task<IActionResult> ReceiveMetrics()
//        {
//            _logger.LogInformation("METRICS ENDPOINT HIT!");
//            using var reader = new StreamReader(Request.Body);
//            var body = await reader.ReadToEndAsync();
//            _logger.LogInformation("Received metric data: {MetricData}", body);
//            return Ok();
//        }



//        [HttpPost("logs")]
//        [Consumes("application/x-protobuf")]
//        public async Task<IActionResult> ReceiveLogs()
//        {
//            _logger.LogInformation("LOGS ENDPOINT HIT!");
//            using var reader = new StreamReader(Request.Body);
//            var body = await reader.ReadToEndAsync();
//            _logger.LogInformation("Received log data: {LogData}", body);
//            return Ok();
//        }
//    }
//}




/// +====== latest one that returns chunks of the data and not all ========
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using MonitoringAppAPI.Services;
using MonitoringAppAPI.Data;
using Microsoft.EntityFrameworkCore;
using MonitoringAppAPI.Models;
using MonitoringAppAPI.Models.DTOs;
using System.Text.Json;

namespace MonitoringAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonitoringAppAPIController : ControllerBase
    {
        private readonly ILogger<MonitoringAppAPIController> _logger;
        private readonly IRedisService _redisService;
        private readonly AppDBContext _appDBContext;

        public MonitoringAppAPIController(ILogger<MonitoringAppAPIController> logger, RedisService redisService, AppDBContext appDBContext)
        {
            _logger = logger;
            _redisService = redisService;
            _appDBContext = appDBContext;
        }

        // set up route for getting all apps
        [HttpGet("apps")]
        public async Task<IActionResult> GetAllApps()
        {
            var apps = await _appDBContext.MonitoredApps.ToListAsync();
            return Ok(apps);
        }

        // get telemetry data for an app
        [HttpGet("apps/{id}")]
        public async Task<IActionResult> GetAppData(Guid id)
        {
            var app = await _appDBContext.MonitoredApps.FindAsync(id);
            if (app == null)
            {
                _logger.LogInformation("No app found with ID {AppID}", id);
                return NotFound();
            }

            var key = id.ToString().ToUpper();
            _logger.LogInformation("Getting telemetry data for app with ID {AppID}", key);
            var telemetryData = await _redisService.GetTelemetryDataAsync2(key);
            if (telemetryData == null)
            {
                _logger.LogInformation("No telemetry data found for app with ID {AppID}", id);
                return NotFound();
            }

            return Ok(telemetryData);
        }

        // route for adding app
        [HttpPost("apps")]
        [Consumes("application/json")]
        public async Task<IActionResult> AddApp(AddAppDTO app)
        {
            var appToAdd = new MonitoredApp
            {
                AppName = app.AppName
            };
            var newApp = await _appDBContext.MonitoredApps.AddAsync(appToAdd);
            await _appDBContext.SaveChangesAsync();
            // convert to AppDTO
            var appToSend = new AppDTO()
            {
                ID = newApp.Entity.ID,
                AppName = newApp.Entity.AppName,
            };
            
            return Ok(appToSend);
        }

        [HttpPost("traces")]
        [Consumes("application/x-protobuf")]
        public async Task<IActionResult> ReceiveTraces()
        {
            _logger.LogInformation("TRACE ENDPOINT HIT!");
            var data = await ReadRequestBody();
            _logger.LogInformation("Received trace data length: {TraceDataLength}", data.Length);
            return Ok();
        }

        [HttpPost("metrics")]
        [Consumes("application/x-protobuf")]
        public async Task<IActionResult> ReceiveMetrics()
        {
            _logger.LogInformation("METRICS ENDPOINT HIT!");
            var data = await ReadRequestBody();
            ProcessMetrics(data);
            return Ok();
        }

        [HttpPost("logs")]
        [Consumes("application/x-protobuf")]
        public async Task<IActionResult> ReceiveLogs()
        {
            _logger.LogInformation("LOGS ENDPOINT HIT!");
            var data = await ReadRequestBody();
            ProcessLogs(data);
            return Ok();
        }

        private async Task<byte[]> ReadRequestBody()
        {
            using var ms = new MemoryStream();
            await Request.Body.CopyToAsync(ms);
            return ms.ToArray();
        }

        /// ===== Returns body in Binary format =============
        //private void ProcessMetrics(byte[] data)
        //{
        //    try
        //    {
        //        // Here you would typically decode the protobuf message
        //        // Since we can't use ExportMetricsServiceRequest, we'll just log some basic info
        //        _logger.LogInformation("Received metrics data of length: {Length}", data.Length);

        //        // You could implement custom logic here to parse the protobuf data
        //        // and extract the metrics you're interested in

        //        // For demonstration, let's just log the first few bytes
        //        var sampleData = BitConverter.ToString(data.Take(20).ToArray());
        //        _logger.LogInformation("Sample of metric data: {SampleData}", sampleData);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error processing metrics data");
        //    }
        //}

        //private void ProcessLogs(byte[] data)
        //{
        //    try
        //    {
        //        // Similar to ProcessMetrics, but for logs
        //        _logger.LogInformation("Received logs data of length: {Length}", data.Length);

        //        // You could implement custom logic here to parse the protobuf data
        //        // and extract the logs you're interested in

        //        // For demonstration, let's just log the first few bytes
        //        var sampleData = BitConverter.ToString(data.Take(20).ToArray());
        //        _logger.LogInformation("Sample of log data: {SampleData}", sampleData);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error processing logs data");
        //    }
        //}


        /// ===== Return body in String format ==============
        private void ProcessMetrics(byte[] data)
        {
            try
            {
                // Log the length of the data
                _logger.LogInformation("Received metrics data of length: {Length}", data.Length);

                // Convert the first part of the data to a string (assuming it contains ASCII characters)
                var sampleDataAsString = Encoding.UTF8.GetString(data.Take(100).ToArray());  // Take the first 100 bytes
                _logger.LogInformation("Sample of metric data (as string): {SampleData}", sampleDataAsString);

                // create an object to store in redis
                var MetricData = new RedisData
                {
                    Type = "metrics",
                    Data = sampleDataAsString
                };

                // convert object to string
                var MetricDataString = JsonSerializer.Serialize(MetricData);
                StoreDataInRedis2(MetricDataString);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing metrics data");
            }
        }

        private void ProcessLogs(byte[] data)
        {
            try
            {
                // Log the length of the data
                _logger.LogInformation("Received logs data of length: {Length}", data.Length);

                // Convert the first part of the data to a string (assuming it contains ASCII characters)
                var sampleDataAsString = Encoding.UTF8.GetString(data.Take(100).ToArray());  // Take the first 100 bytes
                _logger.LogInformation("Sample of log data (as string): {SampleData}", sampleDataAsString);

                // create an object to store in redis
                var LogData = new RedisData
                {
                    Type = "logs",
                    Data = sampleDataAsString
                };

                // convert object to string
                var LogDataString = JsonSerializer.Serialize(LogData);
                StoreDataInRedis2(LogDataString);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing logs data");
            }
        }

        private async void StoreDataInRedis(string logData)
        {
            var key = "18BB9789-D0C7-42A4-AFE0-08DCDC812D04";
            _logger.LogInformation("Storing log data in Redis for app with ID {AppID}", key);
            _logger.LogInformation("Data to store: {LogData}", logData);

            await _redisService.SaveTelemetryDataAsync(key, logData, TimeSpan.FromHours(1));  // Store with TTL of 1 hour
        }

        private async void StoreDataInRedis2(string logData)
        {
            var baseId = "18BB9789-D0C7-42A4-AFE0-08DCDC812D04";
            var randomNumber = Guid.NewGuid().ToString(); // or you can use a random number generator
            var key = $"{baseId}:{randomNumber}"; // Create a unique key
            _logger.LogInformation("Storing log data in Redis with key {Key}", key);
            _logger.LogInformation("Data to store: {LogData}", logData);

            await _redisService.SaveTelemetryDataAsync(key, logData, TimeSpan.FromHours(1)); // Store with TTL of 1 hour
        }



    }

    public class RedisData
    {
        public string Type { get; set; }
        public string Data { get; set; }
        public DateTime TimeStored { get; set; } = DateTime.Now;
    }
}
