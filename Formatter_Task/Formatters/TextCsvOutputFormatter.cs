using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;
using Formatter_Task.Dtos;
using Formatter_Task.Entities;

namespace Formatter_Task.Formatters
{
    public class TextCsvOutputFormatter: TextOutputFormatter
    {
        public TextCsvOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }
        protected override bool CanWriteType(Type? type) => typeof(StudentDto).IsAssignableFrom(type)
            || typeof(IEnumerable<StudentDto>).IsAssignableFrom(type);

        public override async Task WriteResponseBodyAsync(
            OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var httpContext = context.HttpContext;
            var serviceProvider = httpContext.RequestServices;
            var logger = serviceProvider.GetRequiredService<ILogger<TextCsvOutputFormatter>>();
            var builder = new StringBuilder();

            FormatCsvHeader(builder);
            if (context.Object is IEnumerable<StudentDto> students)
            {
                foreach (var student in students)
                {
                    FormatCsv(builder, student);
                }
            }
            else
            {
                FormatCsv(builder, (StudentDto)context.Object!);
            }
            await httpContext.Response.WriteAsync(builder.ToString(), selectedEncoding);
        }

        private static void FormatCsvHeader(StringBuilder buffer)
        {
            buffer.AppendLine("Id - Fullname - SeriaNo - Age - Score");
        }
        private static void FormatCsv(
            StringBuilder buffer, StudentDto student)
        {
            buffer.AppendLine($"{student.Id}-{student.Fullname}-{student.SeriaNo}-{student.Age}-{student.Score}");
        }
    }

}
