// <copyright file="XunitFileGenerator.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace GeneratePackageVersions
{
    public class XUnitFileGenerator : FileGenerator
    {
        private const string HeaderConst =
@"//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the GeneratePackageVersions tool. To safely
//     modify this file, edit PackageVersionsGeneratorDefinitions.json and
//     re-run the GeneratePackageVersions project in Visual Studio. See the
//     launchSettings.json for the project if you would like to run the tool
//     with the correct arguments outside of Visual Studio.

//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated. 
// </auto-generated>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Datadog.Trace.ClrProfiler.IntegrationTests
{{
    [SuppressMessage(""StyleCop.CSharp.LayoutRules"", ""SA1516:Elements must be separated by blank line"", Justification = ""This is an auto-generated file."")]
    public class {0}
    {{";

        private const string FooterConst =
@"    }
}";

        private const string EntryFormat = @"
                new object[] {{ ""{0}"" }},";

        private const string BodyFormat =
@"      public static IEnumerable<object[]> {0} =>

            new List<object[]>
            {{
#if DEFAULT_SAMPLES
                new object[] {{ string.Empty }},
#else{1}
#endif
            }};
";

        private const string EndIfDirectiveConst =
@"
#endif";

        private readonly string _className;

        public XUnitFileGenerator(string filename, string className)
            : base(filename)
        {
            _className = className;
        }

        protected override string Header
        {
            get
            {
                return string.Format(HeaderConst, _className);
            }
        }

        protected override string Footer
        {
            get
            {
                return FooterConst;
            }
        }

        public override void Write(PackageVersionEntry packageVersionEntry, IEnumerable<(TargetFramework framework, IEnumerable<Version> versions)> versions, bool requiresDockerDependency)
        {
            Debug.Assert(Started, "Cannot call Write() before calling Start()");
            Debug.Assert(!Finished, "Cannot call Write() after calling Finish()");

            var bodyStringBuilder = new StringBuilder();

            foreach (var (framework, packageVersions) in versions)
            {
                var fx = framework.ToString().ToUpper().Replace('.', '_');

                bodyStringBuilder.Append(@$"
#if {fx}");
                foreach (var version in packageVersions)
                {
                    bodyStringBuilder.AppendFormat(EntryFormat, version);
                }

                bodyStringBuilder.Append(EndIfDirectiveConst);
            }

            if (bodyStringBuilder.Length == 0)
            {
                // If we don't have _any_ versions, add the "default sample" so that we have _something_
                // Otherwise XUnit will error if it's called
                bodyStringBuilder.Append(@"
                new object[] { string.Empty },");
            }

            FileStringBuilder.AppendLine(string.Format(BodyFormat, packageVersionEntry.IntegrationName, bodyStringBuilder.ToString()));
        }
    }
}
