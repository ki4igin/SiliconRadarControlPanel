using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Serilog;

public static class StatusSinkExtention
{
    public static LoggerConfiguration StatusSink(
        this LoggerSinkConfiguration loggerSinkConfiguration,
        StatusSink statusSink)
    {
        return loggerSinkConfiguration.Sink(statusSink);
    }
}

public class StatusSink : ILogEventSink
{    
    private readonly IFormatProvider? _formatProvider;
    public TextBlock? Status { get; set; }

    public StatusSink(IFormatProvider? formatProvider = null)
    {        
        _formatProvider = formatProvider;
    }

    public void Emit(LogEvent logEvent)
    {
        var message = logEvent.RenderMessage(_formatProvider);
        if (Status is not null)
        {
            Status.Text = message;
        }
    }
}
