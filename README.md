The application can be deployed or deleted using below commands
  - sc create "Power Positions Service" binpath="c:\PowerPositions\PowerPositions.exe"
  - sc delete "Power Positions Service"

Following decisions were made to keep the solution simple
 - Serilog with file sink is used for debug logging. Logging is enabled by default at Verbose level
 - A simple retry logic is used instead of a framework like Polly
 - A simple periodic timer is used instead of a framework like Quartz.net
 - Report file generator overwrites any existing file with the same name
 
