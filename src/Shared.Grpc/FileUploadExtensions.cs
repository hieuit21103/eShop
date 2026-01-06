using Google.Protobuf;
using Grpc.Core;
using Microsoft.AspNetCore.Http;

public static class FormFileGrpcExtensions
{
    private const int DefaultChunkSize = 64 * 1024;

    public static async Task StreamContentToGrpcAsync<TRequest>(
        this IFormFile file,
        IClientStreamWriter<TRequest> requestStream,
        Func<ByteString, TRequest> chunkMapper,
        CancellationToken cancellationToken = default)
    {
        if (file == null || file.Length == 0) return;

        var buffer = new byte[DefaultChunkSize];
        using var stream = file.OpenReadStream();
        int bytesRead;

        while ((bytesRead = await stream.ReadAsync(buffer, cancellationToken)) > 0)
        {
            var byteString = ByteString.CopyFrom(buffer, 0, bytesRead);

            var request = chunkMapper(byteString);

            await requestStream.WriteAsync(request, cancellationToken);
        }
    }
}