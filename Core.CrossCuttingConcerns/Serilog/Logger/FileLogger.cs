﻿using Core.CrossCuttingConcerns.Serilog.ConfigurationModels;
using Core.CrossCuttingConcerns.Serilog.Messages;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Serilog.Logger;

public class FileLogger : LoggerServiceBase
{
    public FileLogger(IConfiguration configuration)
    {
        FileLogConfiguration logConfiguration = 
            configuration.GetSection("SeriLogConfigurations:FileLogConfiguration").Get<FileLogConfiguration>()
            ?? throw new Exception(SerilogMessages.NullOptionsMessage);

        string logFilePath = string.Format(format: "{0}{1}", arg0: Directory.GetCurrentDirectory() + logConfiguration.FolderPath, arg1: ".txt");

        var fileLogger = new LoggerConfiguration().WriteTo.File(
            logFilePath,
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: null,
            fileSizeLimitBytes: 5000000,
            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}"
            ).CreateLogger();

        AddLogger(fileLogger);
    }
}
