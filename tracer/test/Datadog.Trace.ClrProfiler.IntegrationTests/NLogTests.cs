// <copyright file="NLogTests.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Datadog.Trace.ClrProfiler.AutoInstrumentation.Logging.NLog.DirectSubmission.Formatting;
using Datadog.Trace.ClrProfiler.IntegrationTests.Helpers;
using Datadog.Trace.Configuration;
using Datadog.Trace.ExtensionMethods;
using Datadog.Trace.Logging.DirectSubmission;
using Datadog.Trace.TestHelpers;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Abstractions;
using static Datadog.Trace.ClrProfiler.IntegrationTests.Helpers.CombinatorialParams;

#pragma warning disable 0618 // MDC and MDLC are obsolete

namespace Datadog.Trace.ClrProfiler.IntegrationTests
{
    public class NLogTests : LogsInjectionTestBase
    {
        private const string ContextNone = "None";
        private const string CustomContextKey = "CustomContextKey";
        private const string CustomContextValue = "CustomContextValue";

        private readonly LogFileTest _textFileWithInjection = new()
        {
            FileName = "log-textFile-withInject.log",
            RegexFormat = @"{0}: {1}",
            // txt format can't conditionally add properties
            UnTracedLogTypes = UnTracedLogTypes.EmptyProperties,
            PropertiesUseSerilogNaming = false
        };

        public NLogTests(ITestOutputHelper output)
            : base(output, "LogsInjection.NLog")
        {
            SetServiceVersion("1.0.0");
        }

        public enum ConfigurationType
        {
            /// <summary>
            /// No configuration provided at all.
            /// </summary>
            None,

            /// <summary>
            /// All targets in configuration will _not_ contain targets pre-configured with logs injection related elements.
            /// (e.g., "includeMdc = true" would be omitted from the JSON target)
            /// </summary>
            NoLogsInjection,

            /// <summary>
            /// All targets in configuration _will_ contain targets pre-configured with logs injection related elements.
            /// (e.g., "includeMdc = true" would be present in the JSON target)
            /// </summary>
            LogsInjection,

            /// <summary>
            /// Configuration file contains targets that are and aren't pre-configured with logs injection related elements.
            /// </summary>
            Both
        }

        public enum LoggingContext
        {
            /// <summary>
            /// No logging context.
            /// </summary>
            None,

            /// <summary>
            /// Use MDC as logging context.
            /// </summary>
            Mdc,

            /// <summary>
            /// Use MDLC as logging context.
            /// </summary>
            Mdlc,

            /// <summary>
            /// Use ScopeContext as logging context.
            /// </summary>
            ScopeContext
        }

        public static IEnumerable<object[]> GetLogsInjectionTestData()
        {
            foreach (var packageVersionArray in PackageVersions.NLog)
            {
                var packageVersion = (string)packageVersionArray[0];
                var version = GetNLogVersion(packageVersion);

                // Determine test intensity based on version - focus on v5 and v6 (customer-relevant)
                var isOldVersion = version < new Version("4.0.0"); // v1, v2
                var isMidVersion = version >= new Version("4.0.0") && version < new Version("5.0.0"); // v4
                var isCurrentVersion = version >= new Version("5.0.0"); // v5, v6

                // Filter direct log submission testing based on version
                var directSubmissionValues = isOldVersion ? new[] { false } : new[] { true, false };

                foreach (var enableDirectLogSubmission in directSubmissionValues)
                {
                    // Filter logging contexts based on version
                    List<string> contexts;
                    if (isOldVersion)
                    {
                        // v1, v2: Minimal - only test Mdc
                        contexts = new List<string> { "Mdc" };
                    }
                    else if (isMidVersion)
                    {
                        // v4: Reduced - only test Mdc (no Mdlc even though available)
                        contexts = new List<string> { "Mdc" };
                    }
                    else
                    {
                        // v5, v6: Full testing
                        contexts = new List<string> { "None", "Mdc", "Mdlc", "ScopeContext" };
                    }

                    foreach (var context in contexts)
                    {
                        // Filter configuration types
                        List<string> configTypes;
                        if (isOldVersion)
                        {
                            // v1, v2: Minimal - only test LogsInjection
                            configTypes = new List<string> { "LogsInjection" };
                        }
                        else if (isMidVersion)
                        {
                            // v4: Reduced - test LogsInjection and NoLogsInjection only
                            configTypes = new List<string> { "LogsInjection", "NoLogsInjection" };
                        }
                        else
                        {
                            // v5, v6: Full testing
                            configTypes = new List<string> { "LogsInjection", "Both", "NoLogsInjection" };
                        }

                        foreach (var configType in configTypes)
                        {
                            // Filter 128-bit testing based on version
                            var enable128BitValues = isOldVersion || isMidVersion ? new[] { false } : new[] { false, true };

                            foreach (var enable128Bit in enable128BitValues)
                            {
                                yield return new object[] { packageVersion, enableDirectLogSubmission, context, configType, enable128Bit };
                            }
                        }
                    }
                }
            }
        }

        public static IEnumerable<object[]> GetLogsNotInjectedTestData()
        {
            foreach (var packageVersionArray in PackageVersions.NLog)
            {
                var packageVersion = (string)packageVersionArray[0];
                var version = GetNLogVersion(packageVersion);

                // Determine test intensity based on version
                var isOldVersion = version < new Version("4.0.0"); // v1, v2
                var isMidVersion = version >= new Version("4.0.0") && version < new Version("5.0.0"); // v4
                var isCurrentVersion = version >= new Version("5.0.0"); // v5, v6

                // Filter direct log submission testing based on version
                var directSubmissionValues = isOldVersion ? new[] { false } : new[] { true, false };

                foreach (var enableDirectLogSubmission in directSubmissionValues)
                {
                    // Filter logging contexts based on version
                    List<string> contexts;
                    if (isOldVersion)
                    {
                        // v1, v2: Minimal - only test None
                        contexts = new List<string> { "None" };
                    }
                    else if (isMidVersion)
                    {
                        // v4: Reduced - test None and Mdc only
                        contexts = new List<string> { "None", "Mdc" };
                    }
                    else
                    {
                        // v5, v6: Full testing
                        contexts = new List<string> { "None", "Mdc", "Mdlc", "ScopeContext" };
                    }

                    foreach (var context in contexts)
                    {
                        // Filter 128-bit testing based on version
                        var enable128BitValues = isOldVersion || isMidVersion ? new[] { false } : new[] { false, true };

                        foreach (var enable128Bit in enable128BitValues)
                        {
                            yield return new object[] { packageVersion, enableDirectLogSubmission, context, enable128Bit };
                        }
                    }
                }
            }
        }

        public static IEnumerable<object[]> GetDirectLogSubmissionTestData()
        {
            foreach (var packageVersionArray in PackageVersions.NLog)
            {
                var packageVersion = (string)packageVersionArray[0];
                var version = GetNLogVersion(packageVersion);

                // Determine test intensity based on version
                var isOldVersion = version < new Version("4.0.0"); // v1, v2
                var isMidVersion = version >= new Version("4.0.0") && version < new Version("5.0.0"); // v4
                var isCurrentVersion = version >= new Version("5.0.0"); // v5, v6

                // Filter logging contexts based on version
                List<string> contexts;
                if (isOldVersion)
                {
                    // v1, v2: Minimal - only test Mdc
                    contexts = new List<string> { "Mdc" };
                }
                else if (isMidVersion)
                {
                    // v4: Reduced - only test Mdc
                    contexts = new List<string> { "Mdc" };
                }
                else
                {
                    // v5, v6: Full testing
                    contexts = new List<string> { "None", "Mdc", "Mdlc", "ScopeContext" };
                }

                foreach (var context in contexts)
                {
                    // Filter configuration types
                    List<string> configTypes;
                    if (isOldVersion)
                    {
                        // v1, v2: Minimal - only test LogsInjection
                        configTypes = new List<string> { "LogsInjection" };
                    }
                    else if (isMidVersion)
                    {
                        // v4: Reduced - test LogsInjection and NoLogsInjection only
                        configTypes = new List<string> { "LogsInjection", "NoLogsInjection" };
                    }
                    else
                    {
                        // v5, v6: Full testing
                        configTypes = new List<string> { "None", "LogsInjection", "Both", "NoLogsInjection" };
                    }

                    foreach (var configType in configTypes)
                    {
                        // Filter 128-bit testing based on version
                        var enable128BitValues = isOldVersion || isMidVersion ? new[] { false } : new[] { false, true };

                        foreach (var enable128Bit in enable128BitValues)
                        {
                            yield return new object[] { packageVersion, context, configType, enable128Bit };
                        }
                    }
                }
            }
        }

        [SkippableTheory]
        [MemberData(nameof(GetLogsInjectionTestData))]
        [Trait("Category", "EndToEnd")]
        [Trait("RunOnWindows", "True")]
        [Trait("SupportsInstrumentationVerification", "True")]
        public async Task InjectsLogsWhenEnabled(
            string packageVersion,
            bool enableDirectLogSubmission,
            string loggingContext,
            string configurationType,
            bool enable128BitTraceIds)
        {
            var configType = (ConfigurationType)Enum.Parse(typeof(ConfigurationType), configurationType);

            SetEnvironmentVariable("DD_TRACE_128_BIT_TRACEID_LOGGING_ENABLED", enable128BitTraceIds ? "true" : "false");
            SetInstrumentationVerification();
            using var logsIntake = new MockLogsIntake();
            if (enableDirectLogSubmission)
            {
                EnableDirectLogSubmission(logsIntake.Port, nameof(IntegrationId.NLog), nameof(InjectsLogsWhenEnabled));
            }

            var expectedCorrelatedTraceCount = 1;
            var expectedCorrelatedSpanCount = 1;

            using (var agent = EnvironmentHelper.GetMockAgent())
            using (var processResult = await RunSampleAndWaitForExit(agent, packageVersion: packageVersion, arguments: string.Join(" ", new object[] { loggingContext, configType })))
            {
                var spans = await agent.WaitForSpansAsync(1, 2500);
                Assert.True(spans.Count >= 1, $"Expecting at least 1 span, only received {spans.Count}");

                var testFiles = GetTestFiles(packageVersion, true, configType);
                ValidateLogCorrelation(spans, testFiles, expectedCorrelatedTraceCount, expectedCorrelatedSpanCount, packageVersion, use128Bits: enable128BitTraceIds);
                VerifyInstrumentation(processResult.Process);
                VerifyContextProperties(testFiles, packageVersion, loggingContext);
            }
        }

        [SkippableTheory]
        [MemberData(nameof(GetLogsNotInjectedTestData))]
        [Trait("Category", "EndToEnd")]
        [Trait("RunOnWindows", "True")]
        [Trait("SupportsInstrumentationVerification", "True")]
        public async Task DoesNotInjectLogsWhenDisabled(
            string packageVersion,
            bool enableDirectLogSubmission,
            string loggingContext,
            bool enable128BitTraceIds)
        {
            var configType = ConfigurationType.LogsInjection;

            SetEnvironmentVariable("DD_LOGS_INJECTION", "false");
            SetEnvironmentVariable("DD_TRACE_128_BIT_TRACEID_LOGGING_ENABLED", enable128BitTraceIds ? "true" : "false");
            SetInstrumentationVerification();
            using var logsIntake = new MockLogsIntake();
            if (enableDirectLogSubmission)
            {
                EnableDirectLogSubmission(logsIntake.Port, nameof(IntegrationId.NLog), nameof(DoesNotInjectLogsWhenDisabled));
            }

            var expectedCorrelatedTraceCount = 0;
            var expectedCorrelatedSpanCount = 0;

            using (var agent = EnvironmentHelper.GetMockAgent())
            using (var processResult = await RunSampleAndWaitForExit(agent, packageVersion: packageVersion, arguments: string.Join(" ", new object[] { loggingContext, configType })))
            {
                var spans = await agent.WaitForSpansAsync(1, 2500);
                Assert.True(spans.Count >= 1, $"Expecting at least 1 span, only received {spans.Count}");

                var testFiles = GetTestFiles(packageVersion, logsInjectionEnabled: false, configType);
                ValidateLogCorrelation(spans, testFiles, expectedCorrelatedTraceCount, expectedCorrelatedSpanCount, packageVersion, disableLogCorrelation: true, use128Bits: enable128BitTraceIds);

                VerifyInstrumentation(processResult.Process);
                VerifyContextProperties(testFiles, packageVersion, loggingContext);
            }
        }

        [SkippableTheory]
        [MemberData(nameof(GetDirectLogSubmissionTestData))]
        [Trait("Category", "EndToEnd")]
        [Trait("RunOnWindows", "True")]
        [Trait("SupportsInstrumentationVerification", "True")]
        public async Task DirectlyShipsLogs(
            string packageVersion,
            string loggingContext,
            string configurationType,
            bool enable128BitTraceIds)
        {
            var configType = (ConfigurationType)Enum.Parse(typeof(ConfigurationType), configurationType);
            var hostName = "integration_nlog_tests";
            using var logsIntake = new MockLogsIntake();

            SetInstrumentationVerification();
            SetEnvironmentVariable("DD_TRACE_128_BIT_TRACEID_LOGGING_ENABLED", enable128BitTraceIds ? "true" : "false");
            SetEnvironmentVariable("DD_LOGS_INJECTION", "true");
            SetEnvironmentVariable("INCLUDE_CROSS_DOMAIN_CALL", "false");
            EnableDirectLogSubmission(logsIntake.Port, nameof(IntegrationId.NLog), hostName);

            using var telemetry = this.ConfigureTelemetry();
            using var agent = EnvironmentHelper.GetMockAgent();
            using var processResult = await RunSampleAndWaitForExit(agent, packageVersion: packageVersion, arguments: string.Join(" ", new object[] { loggingContext, configType }));

            ExitCodeException.ThrowIfNonZero(processResult.ExitCode, processResult.StandardError);

            var logs = logsIntake.Logs;

            using var scope = new AssertionScope();
            logs.Should().NotBeNull();
            logs.Should().HaveCountGreaterOrEqualTo(3);
            logs.Should()
                .OnlyContain(x => x.Service == "LogsInjection.NLog")
                .And.OnlyContain(x => x.Env == "integration_tests")
                .And.OnlyContain(x => x.Version == "1.0.0")
                .And.OnlyContain(x => x.Host == hostName)
                .And.OnlyContain(x => x.Source == "csharp")
                .And.OnlyContain(x => x.Exception == null)
                .And.OnlyContain(x => x.LogLevel == DirectSubmissionLogLevel.Information)
                .And.OnlyContain(x => x.TryGetProperty(NLogLogFormatter.LoggerNameKey).Exists);

            logs
               .Where(x => !x.Message.Contains(ExcludeMessagePrefix))
               .Should()
               .HaveCount(1)
               .And.OnlyContain(x => !string.IsNullOrEmpty(x.TraceId))
               .And.OnlyContain(x => !string.IsNullOrEmpty(x.SpanId));
            VerifyInstrumentation(processResult.Process);

            if (loggingContext != ContextNone)
            {
                Func<MockLogsIntake.Log, string, string, bool> hasProperty = (log, key, value) =>
                {
                    var prop = log.TryGetProperty(key);
                    return prop.Exists && prop.Value == value;
                };
                logs.Should().Contain(x => hasProperty(x, CustomContextKey, CustomContextValue));
            }

            await telemetry.AssertIntegrationEnabledAsync(IntegrationId.NLog);
        }

        /// <summary>
        /// Gets the NLog version from the package version string.
        /// Returns the default version for empty package version based on runtime.
        /// </summary>
        private static Version GetNLogVersion(string packageVersion)
        {
            if (string.IsNullOrEmpty(packageVersion))
            {
                // LogsInjection.NLog uses different default versions depending on framework
                return EnvironmentHelper.IsCoreClr()
                    ? new Version("5.0.0")
                    : new Version("2.1.0");
            }

            return new Version(packageVersion);
        }

        private void VerifyContextProperties(LogFileTest[] testFiles, string packageVersion, string context)
        {
            if (context == ContextNone) { return; }

            // Skip for versions that don't support json
            foreach (var testFile in testFiles)
            {
                if (testFile.FileName.Contains("json"))
                {
                    var test = testFile; // jsonFile
                    var logFilePath = Path.Combine(EnvironmentHelper.GetSampleApplicationOutputDirectory(packageVersion), test.FileName);
                    var logs = GetLogFileContents(logFilePath);
                    foreach (var log in logs)
                    {
                        log.Should().MatchRegex(string.Format(test.RegexFormat, CustomContextKey, $@"""{CustomContextValue}"""));
                    }
                }
            }
        }

        private LogFileTest[] GetTestFiles(string packageVersion, bool logsInjectionEnabled = true, ConfigurationType configType = ConfigurationType.Both)
        {
            if (packageVersion is null or "")
            {
#if NETFRAMEWORK
                packageVersion = "2.1.0";
#else
                packageVersion = "4.5.0";
#endif
            }

            var version = new Version(packageVersion);

            if (version < new Version("4.0.0"))
            {
                // pre 4.0 can't write to json file
                if (configType == ConfigurationType.Both || configType == ConfigurationType.LogsInjection)
                {
                    return new[] { _textFileWithInjection };
                }
                else if (configType == ConfigurationType.NoLogsInjection)
                {
                    throw new Exception("NLog versions below 4.0.0 don't have JSON, so no automated logs injection");
                }
            }

            var unTracedLogType = logsInjectionEnabled switch
            {
                // When logs injection is enabled, untraced logs get env, service etc
                true => UnTracedLogTypes.EnvServiceTracingPropertiesOnly,
                // When logs injection is disabled, no enrichment
                false => UnTracedLogTypes.None
            };

            if (logsInjectionEnabled && configType == ConfigurationType.Both)
            {
                return new[] { _textFileWithInjection, GetJsonTestFile(unTracedLogType), GetJsonTestFileNoInjection(unTracedLogType) };
            }
            else if (logsInjectionEnabled && configType == ConfigurationType.LogsInjection)
            {
                return new[] { _textFileWithInjection, GetJsonTestFile(unTracedLogType) };
            }
            else if (logsInjectionEnabled && configType == ConfigurationType.NoLogsInjection)
            {
                return new[] {  GetJsonTestFileNoInjection(unTracedLogType) };
            }
            else
            {
                return new[] { _textFileWithInjection, GetJsonTestFile(unTracedLogType) };
            }
        }

        private LogFileTest GetJsonTestFile(UnTracedLogTypes unTracedLogType) => new()
        {
            FileName = "log-jsonFile-withInject.log",
            RegexFormat = @"""{0}"":\s*{1}",
            UnTracedLogTypes = unTracedLogType,
            PropertiesUseSerilogNaming = false
        };

        private LogFileTest GetJsonTestFileNoInjection(UnTracedLogTypes unTracedLogType) => new()
        {
            FileName = "log-jsonFile-noInject.log",
            RegexFormat = @"""{0}"":\s*{1}",
            UnTracedLogTypes = unTracedLogType,
            PropertiesUseSerilogNaming = false
        };
    }
}
