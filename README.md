# log4net Loki Appender

This appender will allow log4net to be configured to send log messages to Loki directly.

![Dot Net Framework 4](https://github.com/anaselhajjaji/log4net.Appender.Loki/workflows/Dot%20Net%20Framework%204/badge.svg?branch=master)

## Installation

The log4net.Appender.LokiAppender NuGet [package can be found here](https://www.nuget.org/packages/log4net.Appender.Loki/). Alternatively you can install it via one of the following commands below:

NuGet command:
```bash
Install-Package log4net.Appender.Loki
```

## Log4net configuration

Sample Log4net config:

```xml
<log4net>
  <appender name="loki" type="log4net.Appender.LokiAppender, log4net.Appender.Loki">
    <BufferSize  value="3" /> <!-- To configure the buffer size, default: 512 -->
    <ServiceUrl value="http://localhost:3100" /> <!-- Loki URL -->
    <BasicAuthUserName value="username" /> <!-- To be added if basic authent enabled  -->
    <BasicAuthPassword value="password" /> <!-- To be added if basic authent enabled  -->
    <TrustSelfCignedCerts value="false" /> <!-- To trust self signed certificates. Default: false -->
  </appender>
</log4net>
```
