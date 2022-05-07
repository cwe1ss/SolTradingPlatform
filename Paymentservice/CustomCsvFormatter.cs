using Microsoft.AspNetCore.Mvc.Formatters;
using System.Collections;
using System.Text;

namespace Paymentservice
{
    public class CustomCsvFormatter : TextOutputFormatter
    {
        public CustomCsvFormatter()
        {
            SupportedMediaTypes.Add(Microsoft.Net.Http.Headers.MediaTypeHeaderValue.Parse("text/csv"));
            SupportedMediaTypes.Add(Microsoft.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/xml"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            ArgumentNullException.ThrowIfNull(context.Object);
    
            StringBuilder csv = new StringBuilder();
            Type type = context.Object.GetType();

            csv.AppendLine(
                string.Join<string>(
                    ",", type.GetProperties().Select(x => x.Name)
                )
            );

            return context.HttpContext.Response.WriteAsync(csv.ToString(), selectedEncoding);
        }

        protected override bool CanWriteType(Type? type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type);
        }
    }
}
