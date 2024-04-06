using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;

namespace LaptopStore.Core.Utilities
{


    public class AsyncLocalLogger
    {
        private static JsonSerializerSettings settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        private static AsyncLocal<List<LogEntry>> asyncLocalLog = new AsyncLocal<List<LogEntry>>();

        public static void StartRequestLog()
        {
            asyncLocalLog.Value = new List<LogEntry>();
        }

        public static void GuardInitLog() 
        {
            if (asyncLocalLog.Value == null)
            {
                asyncLocalLog.Value = new List<LogEntry>();
            }
        }

        // Log với các argument bổ sung thông qua Dictionary
        public static void Log(string message, Dictionary<string, object> additionalData = null)
        {
            GuardInitLog();

            var logEntry = new LogEntry
            {
                Timestamp = DateTime.Now,
                Message = message,
                Data = additionalData
            };

            asyncLocalLog.Value.Add(logEntry);
        }

        // Log với các argument bổ sung thông qua Object
        public static void Log(string message, object additionalData = null)
        {
            GuardInitLog();

            var logEntry = new LogEntry
            {
                Timestamp = DateTime.Now,
                Message = message,
                Data = additionalData
            };

            asyncLocalLog.Value.Add(logEntry);
        }

        public static void Log(string message)
        {
            GuardInitLog();

            var logEntry = new LogEntry
            {
                Timestamp = DateTime.Now,
                Message = message,
                Data = null
            };

            asyncLocalLog.Value.Add(logEntry);
        }

        public static List<LogEntry> EndRequestLog()
        {
            var logEntries = asyncLocalLog.Value;
            asyncLocalLog.Value = null;
            return logEntries;
        }

        public static List<LogEntry> GetFormatLogEntry()
        {
            GuardInitLog();
            var logEntries = EndRequestLog();
            for (int i = 0; i < logEntries.Count; i++)
            {
                logEntries[i].Message = $"{i + 1}. {logEntries[i].Message}, Data = {JsonConvert.SerializeObject(logEntries[i].Data, settings)}";
            }
            return logEntries;
        }

        // Phương thức hỗ trợ để serialize danh sách log entries ra JSON
        public static string EndRequestLogAndSerialize()
        {
            List<LogEntry> entries = GetFormatLogEntry();
            return JsonConvert.SerializeObject(entries, settings);
        }

        public class LogEntry
        {
            public DateTime Timestamp { get; set; }
            public string Message { get; set; }

            [JsonIgnore]
            public object Data { get; set; }
        }
    }
}