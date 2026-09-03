using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace LENA.API.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
    public class CacheHeadersAttribute(int maxAgeSeconds) : Attribute, IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is not ObjectResult { Value: not null } objectResult)
            {
                return;
            }

            var jsonOptions = context.HttpContext.RequestServices
                .GetService<IOptions<JsonOptions>>()?.Value?.JsonSerializerOptions
                ?? JsonSerializerOptions.Default;

            var json = JsonSerializer.Serialize(objectResult.Value, jsonOptions);
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(json));
            var etag = $"\"{Convert.ToHexString(hash).ToLowerInvariant()}\"";

            context.HttpContext.Response.Headers["ETag"] = etag;
            context.HttpContext.Response.Headers["Cache-Control"] = $"public, max-age={maxAgeSeconds}";
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
        }
    }
}
