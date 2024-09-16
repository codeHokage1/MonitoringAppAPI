using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MonitoringAppAPI.Formatters
{
    public class ProtobufInputFormatter : InputFormatter
    {
        public ProtobufInputFormatter()
        {
            SupportedMediaTypes.Add("application/x-protobuf");
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var memoryStream = new MemoryStream();
            await context.HttpContext.Request.Body.CopyToAsync(memoryStream);
            return InputFormatterResult.Success(memoryStream.ToArray());
        }

        protected override bool CanReadType(Type type)
        {
            return true;
        }
    }
}