using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Serilog;

public class LoggerServiceBase
{
    public List<ILogger> Loggers { get; set; }

    public LoggerServiceBase()
    {
        Loggers = [];
    }

    public LoggerServiceBase(List<ILogger> loggers)
    {
        Loggers = loggers;
    }

    public void AddLogger(ILogger logger)
    {
        Loggers.Add(logger);
    }

    public void Verbose(string message)
    {
        foreach (var logger in Loggers)
        {
            logger.Verbose(message);
        }
    }

    public void Fatal(string message)
    {
        foreach (var logger in Loggers)
        {
            logger.Fatal(message);
        }
    }

    public void Info(string message)
    {
        foreach (var logger in Loggers)
        {
            logger.Information(message);
        }
    }

    public void Warn(string message)
    {
        foreach (var logger in Loggers)
        {
            logger.Warning(message);
        }
    }

    public void Debug(string message)
    {
        foreach (var logger in Loggers)
        {
            logger.Debug(message);
        }
    }

    public void Error(string message)
    {
        foreach (var logger in Loggers)
        {
            logger.Error(message);
        }
    }
}
