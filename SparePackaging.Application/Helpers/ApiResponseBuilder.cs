using System.Net;
namespace SparePackaging.Application.Helpers;
public static class ApiResponseBuilder
{
    public static ApiResponse GenerateOK(
        object? data,
        int statusCode,
        string message,
        string? description)
    {
        return new ApiResponse(
            data,
            (int)HttpStatusCode.OK,
            "OK",
            message,
            description);
    }

    public static ApiResponse GenerateInternalServerError(
        object? data,
        int statusCode,
        string message,
        string? description)
    {
        return new ApiResponse(
            data,
            (int)HttpStatusCode.InternalServerError,
            "InternalServerError",
            message,
            description);
    }

    public static ApiResponse GenerateBadRequest(
        string message,
        string? description)
    {
        return new ApiResponse(
            null,
            (int)HttpStatusCode.BadRequest,
            "Bad Request",
            message,
            description);
    }

    public static ApiResponse GenerateUnAuthorized(
        string message,
        string? description)
    {
        return new ApiResponse(
            null,
            (int)HttpStatusCode.Unauthorized,
            "Un authorized",
            message,
            description);
    }

    public static ApiResponse GenerateForbidden(
        string message,
        string? description)
    {
        return new ApiResponse(
            null,
            (int)HttpStatusCode.Forbidden,
            "Forbidden",
            message,
            description);
    }

    public static ApiResponse GenerateNotfound(
        string message,
        string? description)
    {
        return new ApiResponse(
            null,
            (int)HttpStatusCode.NotFound,
            "NotFound",
            message,
            description);
    }
}
