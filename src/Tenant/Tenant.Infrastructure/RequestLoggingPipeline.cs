using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Tenant.Infrastructure;

public class RequestLoggingPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<RequestLoggingPipeline<TRequest, TResponse>> _logger;

    public RequestLoggingPipeline(ILogger<RequestLoggingPipeline<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var serializedRequest = JsonSerializer.Serialize(request, new JsonSerializerOptions() { WriteIndented = true });
        var requestType = typeof(TRequest).GetTypeInfo().Name;
        _logger.LogInformation($"Request Type : {requestType}  Request Model : {serializedRequest} \n");

        var watch = new Stopwatch();
        watch.Start();
        var response = await next();
        watch.Stop();

        var serializedResponse =
            JsonSerializer.Serialize(response, new JsonSerializerOptions() { WriteIndented = true });
        var responseType = typeof(TResponse).GetTypeInfo().Name;
        _logger.LogInformation($"Response Type : {responseType} - Response Model : {serializedResponse} \n");
        _logger.LogInformation(
            $"Process Time : {watch.Elapsed.TotalSeconds.ToString(CultureInfo.InvariantCulture)} sn");

        return response;
    }
}