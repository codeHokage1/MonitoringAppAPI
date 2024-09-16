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

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using OpenTelemetry.Proto.Collector.Metrics.V1;
using Google.Protobuf;
using Microsoft.AspNetCore.Mvc;

namespace MonitoringAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonitoringAppAPIController : ControllerBase
    {
        private readonly ILogger<MonitoringAppAPIController> _logger;

        public MonitoringAppAPIController(ILogger<MonitoringAppAPIController> logger)
        {
            _logger = logger;
        }

        [HttpPost("traces")]
        [Consumes("application/x-protobuf")]
        public async Task<IActionResult> ReceiveTraces()
        {
            _logger.LogInformation("TRACE ENDPOINT HIT!");
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();
            _logger.LogInformation("Received trace data: {TraceData}", body);
            return Ok();
        }

        [HttpPost("metrics")]
        [Consumes("application/x-protobuf")]
        public async Task<IActionResult> ReceiveMetrics()
        {
            _logger.LogInformation("METRICS ENDPOINT HIT!");
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();
            _logger.LogInformation("Received metric data: {MetricData}", body);
            return Ok();
        }

        //[HttpPost("metrics")]
        //[Consumes("application/x-protobuf")]
        //public async Task<IActionResult> ReceiveMetrics()
        //{
        //    _logger.LogInformation("METRICS ENDPOINT HIT!");

        //    using var ms = new MemoryStream();
        //    await Request.Body.CopyToAsync(ms);
        //    ms.Position = 0;

        //    try
        //    {
        //        var exportMetricsServiceRequest = ExportMetricsServiceRequest.Parser.ParseFrom(ms);

        //        foreach (var resourceMetrics in exportMetricsServiceRequest.ResourceMetrics)
        //        {
        //            foreach (var scopeMetrics in resourceMetrics.ScopeMetrics)
        //            {
        //                foreach (var metric in scopeMetrics.Metrics)
        //                {
        //                    _logger.LogInformation("Received metric: {Name}, {Description}",
        //                        metric.Name, metric.Description);

        //                    // Log different types of metric data
        //                    if (metric.Gauge != null)
        //                    {
        //                        foreach (var dataPoint in metric.Gauge.DataPoints)
        //                        {
        //                            _logger.LogInformation("Gauge value: {Value}", dataPoint.AsDouble);
        //                        }
        //                    }
        //                    // Add similar blocks for other metric types (Sum, Histogram, etc.)
        //                }
        //            }
        //        }
        //    }
        //    catch (InvalidProtocolBufferException ex)
        //    {
        //        _logger.LogError(ex, "Failed to parse metrics data");
        //        return BadRequest("Invalid metrics data");
        //    }

        //    return Ok();
        //}


        [HttpPost("logs")]
        [Consumes("application/x-protobuf")]
        public async Task<IActionResult> ReceiveLogs()
        {
            _logger.LogInformation("LOGS ENDPOINT HIT!");
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();
            _logger.LogInformation("Received log data: {LogData}", body);
            return Ok();
        }
    }
}

