using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;
using Formatter_Task.Dtos;
using Formatter_Task.Entities;

namespace Formatter_Task.Formatters
{
    public class TextCsvInputFormatter: TextInputFormatter
    {
        public TextCsvInputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanReadType(Type type) => type == typeof(StudentAddDto);

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(
            InputFormatterContext context, Encoding effectiveEncoding)
        {
            var httpContext = context.HttpContext;
            var serviceProvider = httpContext.RequestServices;

            var logger = serviceProvider.GetRequiredService<ILogger<StudentAddDto>>();
            using var reader = new StreamReader(httpContext.Request.Body, effectiveEncoding);
            string? addLine = null;

            try
            {
                await ReadLineAsync("Fullname - SeriaNo - Age - Score", reader, context, logger);

                addLine = await ReadLineAsync("", reader, context, logger);

                var enDash = addLine.Split("-");

                var student = new StudentAddDto()
                {
                    Fullname = enDash[0].Trim(),
                    SeriaNo = enDash[1].Trim(),
                    Age = int.Parse(enDash[2].Trim()),
                    Score = double.Parse(enDash[3].Trim())
                };

                return await InputFormatterResult.SuccessAsync(student);
            }
            catch
            {
                return await InputFormatterResult.FailureAsync();
            }
        }

        private static async Task<string> ReadLineAsync(
            string expectedText, StreamReader reader, InputFormatterContext context,
            ILogger logger)
        {
            var line = await reader.ReadLineAsync();

            if (line is null || !line.StartsWith(expectedText))
            {
                var errorMessage = $"Looked for '{expectedText}' and got '{line}'";
                context.ModelState.TryAddModelError(context.ModelName, errorMessage);
                logger.LogError(errorMessage);

                throw new Exception(errorMessage);
            }

            return line;
        }
    }
}
