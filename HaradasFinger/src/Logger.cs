using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Targets;
using NLog.Config;

namespace Tekken7 {
    class LoggerConfig {

        public LoggerConfig(string logName, bool consoleLoggingOn = false) {
            _logName = logName;
            CreateConfig(consoleLoggingOn);
        }

        public void CreateConfig(bool consoleLoggingOn) {
            _config = new LoggingConfiguration();

            var fileTarget = new FileTarget();
            _config.AddTarget("file", fileTarget);
            fileTarget.FileName = "${basedir}/" + _logName + ".log";
            fileTarget.Layout = @"[${date:format=HH\:mm\:ss}::${logger}::${threadid}] ${message}";
            var rule = new LoggingRule("*", LogLevel.Trace, fileTarget);
            _config.LoggingRules.Add(rule);

            if (consoleLoggingOn) {
                var consoleTarget = new ConsoleTarget();
                _config.AddTarget("console", consoleTarget);
                consoleTarget.Layout = @"[${date:format=HH\:mm\:ss}::${threadid}] ${message}";
                var rule2 = new LoggingRule("*", LogLevel.Trace, consoleTarget);
                _config.LoggingRules.Add(rule2);
            }
            LogManager.Configuration = _config;

        }

        public LoggingConfiguration Config {
            get {
                return _config;
            }
        }

        LoggingConfiguration _config;
        string _logName;
    }
}
