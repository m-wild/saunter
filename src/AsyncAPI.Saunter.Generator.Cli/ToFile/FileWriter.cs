﻿using Microsoft.Extensions.Logging;

namespace AsyncAPI.Saunter.Generator.Cli.ToFile;

internal class FileWriter(IStreamProvider streamProvider, ILogger<FileWriter> logger)
{
    public void Write(string outputPath, string fileTemplate, string documentName, string format, Action<Stream> streamWriter)
    {
        var fullFileName = AddFileExtension(outputPath, fileTemplate, documentName, format);
        this.WriteFile(fullFileName, streamWriter);
    }

    private void WriteFile(string outputPath, Action<Stream> writeAction)
    {
        using var stream = streamProvider.GetStreamFor(outputPath);
        writeAction(stream);

        if (outputPath != null)
        {
            logger.LogInformation($"AsyncAPI {Path.GetExtension(outputPath).TrimStart('.')} successfully written to {outputPath}");
        }
    }

    private static string AddFileExtension(string outputPath, string fileTemplate, string documentName, string extension)
    {
        if (outputPath == null)
        {
            return outputPath;
        }

        return Path.GetFullPath(Path.Combine(outputPath, fileTemplate.Replace("{document}", documentName ?? "")
                                                                     .Replace("{extension}", extension).TrimStart('_')));
    }
}
