﻿// <auto-generated/>
#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Datadog.Trace.ClrProfiler
{
    internal static partial class AspectDefinitions
    {
        public static string[] Aspects = new string[] {
"[AspectClass(\"mscorlib\",[None],Propagation,[])] Datadog.Trace.Iast.Aspects.HashAlgorithmAspect",
"  [AspectMethodInsertBefore(\"System.Security.Cryptography.HashAlgorithm::ComputeHash(System.Byte[])\",\"\",[1],[False],[None],Propagation,[])] ComputeHash(System.Security.Cryptography.HashAlgorithm)",
"  [AspectMethodInsertBefore(\"System.Security.Cryptography.HashAlgorithm::ComputeHash(System.Byte[],System.Int32,System.Int32)\",\"\",[3],[False],[None],Propagation,[])] ComputeHash(System.Security.Cryptography.HashAlgorithm)",
"  [AspectMethodInsertBefore(\"System.Security.Cryptography.HashAlgorithm::ComputeHash(System.IO.Stream)\",\"\",[1],[False],[None],Propagation,[])] ComputeHash(System.Security.Cryptography.HashAlgorithm)",
"  [AspectMethodInsertBefore(\"System.Security.Cryptography.HashAlgorithm::ComputeHashAsync(System.IO.Stream,System.Threading.CancellationToken)\",\"\",[2],[False],[None],Propagation,[])] ComputeHash(System.Security.Cryptography.HashAlgorithm)",
"[AspectClass(\"mscorlib\",[None],Propagation,[])] Datadog.Trace.Iast.Aspects.SymmetricAlgorithmAspect",
"  [AspectCtorReplace(\"System.Security.Cryptography.DESCryptoServiceProvider::.ctor()\",\"\",[0],[False],[None],Propagation,[])] InitDES()",
"  [AspectCtorReplace(\"System.Security.Cryptography.RC2CryptoServiceProvider::.ctor()\",\"\",[0],[False],[None],Propagation,[])] InitRC2()",
"  [AspectCtorReplace(\"System.Security.Cryptography.TripleDESCryptoServiceProvider::.ctor()\",\"\",[0],[False],[None],Propagation,[])] InitTripleDES()",
"  [AspectCtorReplace(\"System.Security.Cryptography.RijndaelManaged::.ctor()\",\"\",[0],[False],[None],Propagation,[])] InitRijndaelManaged()",
"  [AspectCtorReplace(\"System.Security.Cryptography.AesCryptoServiceProvider::.ctor()\",\"\",[0],[False],[None],Propagation,[])] InitAesCryptoServiceProvider()",
"  [AspectMethodInsertAfter(\"System.Security.Cryptography.DES::Create()\",\"\",[0],[False],[None],Propagation,[])] Create(System.Security.Cryptography.SymmetricAlgorithm)",
"  [AspectMethodInsertAfter(\"System.Security.Cryptography.DES::Create(System.String)\",\"\",[0],[False],[None],Propagation,[])] Create(System.Security.Cryptography.SymmetricAlgorithm)",
"  [AspectMethodInsertAfter(\"System.Security.Cryptography.RC2::Create()\",\"\",[0],[False],[None],Propagation,[])] Create(System.Security.Cryptography.SymmetricAlgorithm)",
"  [AspectMethodInsertAfter(\"System.Security.Cryptography.RC2::Create(System.String)\",\"\",[0],[False],[None],Propagation,[])] Create(System.Security.Cryptography.SymmetricAlgorithm)",
"  [AspectMethodInsertAfter(\"System.Security.Cryptography.TripleDES::Create()\",\"\",[0],[False],[None],Propagation,[])] Create(System.Security.Cryptography.SymmetricAlgorithm)",
"  [AspectMethodInsertAfter(\"System.Security.Cryptography.TripleDES::Create(System.String)\",\"\",[0],[False],[None],Propagation,[])] Create(System.Security.Cryptography.SymmetricAlgorithm)",
"  [AspectMethodInsertAfter(\"System.Security.Cryptography.Rijndael::Create()\",\"\",[0],[False],[None],Propagation,[])] Create(System.Security.Cryptography.SymmetricAlgorithm)",
"  [AspectMethodInsertAfter(\"System.Security.Cryptography.Rijndael::Create(System.String)\",\"\",[0],[False],[None],Propagation,[])] Create(System.Security.Cryptography.SymmetricAlgorithm)",
"  [AspectMethodInsertAfter(\"System.Security.Cryptography.Aes::Create()\",\"\",[0],[False],[None],Propagation,[])] Create(System.Security.Cryptography.SymmetricAlgorithm)",
"  [AspectMethodInsertAfter(\"System.Security.Cryptography.Aes::Create(System.String)\",\"\",[0],[False],[None],Propagation,[])] Create(System.Security.Cryptography.SymmetricAlgorithm)",
"[AspectClass(\"mscorlib,netstandard,System.Private.CoreLib,System.Runtime\",[StringOptimization],Propagation,[])] Datadog.Trace.Iast.Aspects.System.StringAspects",
"  [AspectMethodReplace(\"System.String::Trim()\",\"\",[0],[False],[StringLiteral_0],Propagation,[])] Trim(System.String)",
"  [AspectMethodReplace(\"System.String::Trim(System.Char[])\",\"\",[0],[False],[StringLiteral_0],Propagation,[])] Trim(System.String,System.Char[])",
"  [AspectMethodReplace(\"System.String::TrimStart(System.Char[])\",\"\",[0],[False],[StringLiteral_0],Propagation,[])] TrimStart(System.String,System.Char[])",
"  [AspectMethodReplace(\"System.String::TrimEnd(System.Char[])\",\"\",[0],[False],[StringLiteral_0],Propagation,[])] TrimEnd(System.String,System.Char[])",
"  [AspectMethodReplace(\"System.String::Concat(System.String,System.String)\",\"\",[0],[False],[StringLiterals_Any],Propagation,[])] Concat(System.String,System.String)",
"  [AspectMethodReplace(\"System.String::Concat(System.String,System.String)\",\"\",[0],[False],[StringLiteral_0],Propagation,[])] Concat_0(System.String,System.String)",
"  [AspectMethodReplace(\"System.String::Concat(System.String,System.String)\",\"\",[0],[False],[StringLiteral_1],Propagation,[])] Concat_1(System.String,System.String)",
"  [AspectMethodReplace(\"System.String::Concat(System.Object,System.Object)\",\"\",[0],[False],[None],Propagation,[])] Concat(System.Object,System.Object)",
"  [AspectMethodReplace(\"System.String::Concat(System.String,System.String,System.String)\",\"\",[0],[False],[StringLiterals],Propagation,[])] Concat(System.String,System.String,System.String)",
"  [AspectMethodReplace(\"System.String::Concat(System.Object,System.Object,System.Object)\",\"\",[0],[False],[None],Propagation,[])] Concat(System.Object,System.Object,System.Object)",
"  [AspectMethodReplace(\"System.String::Concat(System.String,System.String,System.String,System.String)\",\"\",[0],[False],[StringLiterals],Propagation,[])] Concat(System.String,System.String,System.String,System.String)",
"  [AspectMethodReplace(\"System.String::Concat(System.Object,System.Object,System.Object,System.Object)\",\"\",[0],[False],[None],Propagation,[])] Concat(System.Object,System.Object,System.Object,System.Object)",
"  [AspectMethodReplace(\"System.String::Concat(System.String,System.String,System.String,System.String,System.String)\",\"\",[0],[False],[StringLiterals],Propagation,[])] Concat(System.String,System.String,System.String,System.String,System.String)",
"  [AspectMethodReplace(\"System.String::Concat(System.Object,System.Object,System.Object,System.Object,System.Object)\",\"\",[0],[False],[None],Propagation,[])] Concat(System.Object,System.Object,System.Object,System.Object,System.Object)",
"  [AspectMethodReplace(\"System.String::Concat(System.String[])\",\"\",[0],[False],[None],Propagation,[])] Concat(System.String[])",
"  [AspectMethodReplace(\"System.String::Concat(System.Object[])\",\"\",[0],[False],[None],Propagation,[])] Concat(System.Object[])",
"  [AspectMethodReplace(\"System.String::Concat(System.Collections.Generic.IEnumerable`1<System.String>)\",\"\",[0],[False],[None],Propagation,[])] Concat(System.Collections.IEnumerable)",
"  [AspectMethodReplace(\"System.String::Concat(System.Collections.Generic.IEnumerable`1<!!0>)\",\"\",[0],[False],[None],Propagation,[])] Concat2(System.Collections.IEnumerable)",
"  [AspectMethodReplace(\"System.String::Substring(System.Int32)\",\"\",[0],[False],[StringLiteral_0],Propagation,[])] Substring(System.String,System.Int32)",
"  [AspectMethodReplace(\"System.String::Substring(System.Int32,System.Int32)\",\"\",[0],[False],[StringLiteral_0],Propagation,[])] Substring(System.String,System.Int32,System.Int32)",
"  [AspectMethodReplace(\"System.String::ToCharArray()\",\"\",[0],[False],[StringLiteral_0],Propagation,[])] ToCharArray(System.String)",
"  [AspectMethodReplace(\"System.String::ToCharArray(System.Int32,System.Int32)\",\"\",[0],[False],[StringLiteral_0],Propagation,[])] ToCharArray(System.String,System.Int32,System.Int32)",
"  [AspectMethodReplace(\"System.String::Join(System.String,System.String[],System.Int32,System.Int32)\",\"\",[0],[False],[None],Propagation,[])] Join(System.String,System.String[],System.Int32,System.Int32)",
"  [AspectMethodReplace(\"System.String::Join(System.String,System.Object[])\",\"\",[0],[False],[None],Propagation,[])] Join(System.String,System.Object[])",
"  [AspectMethodReplace(\"System.String::Join(System.String,System.String[])\",\"\",[0],[False],[None],Propagation,[])] Join(System.String,System.String[])",
"  [AspectMethodReplace(\"System.String::Join(System.String,System.Collections.Generic.IEnumerable`1<System.String>)\",\"\",[0],[False],[None],Propagation,[])] Join(System.String,System.Collections.IEnumerable)",
"  [AspectMethodReplace(\"System.String::Join(System.String,System.Collections.Generic.IEnumerable`1<!!0>)\",\"\",[0],[False],[None],Propagation,[])] Join2(System.String,System.Collections.IEnumerable)",
"  [AspectMethodReplace(\"System.String::ToUpper()\",\"\",[0],[False],[StringLiteral_0],Propagation,[])] ToUpper(System.String)",
"  [AspectMethodReplace(\"System.String::ToUpper(System.Globalization.CultureInfo)\",\"\",[0],[False],[StringLiteral_0],Propagation,[])] ToUpper(System.String,System.Globalization.CultureInfo)",
"  [AspectMethodReplace(\"System.String::ToUpperInvariant()\",\"\",[0],[False],[StringLiteral_0],Propagation,[])] ToUpperInvariant(System.String)",
"  [AspectMethodReplace(\"System.String::ToLower()\",\"\",[0],[False],[StringLiteral_0],Propagation,[])] ToLower(System.String)",
"  [AspectMethodReplace(\"System.String::ToLower(System.Globalization.CultureInfo)\",\"\",[0],[False],[StringLiteral_0],Propagation,[])] ToLower(System.String,System.Globalization.CultureInfo)",
"  [AspectMethodReplace(\"System.String::ToLowerInvariant()\",\"\",[0],[False],[StringLiteral_0],Propagation,[])] ToLowerInvariant(System.String)",
"  [AspectMethodReplace(\"System.String::Remove(System.Int32)\",\"\",[0],[False],[StringLiteral_0],Propagation,[])] Remove(System.String,System.Int32)",
"  [AspectMethodReplace(\"System.String::Remove(System.Int32,System.Int32)\",\"\",[0],[False],[StringLiteral_0],Propagation,[])] Remove(System.String,System.Int32,System.Int32)",
"  [AspectMethodReplace(\"System.String::Insert(System.Int32,System.String)\",\"\",[0],[False],[StringOptimization],Propagation,[])] Insert(System.String,System.Int32,System.String)",
"  [AspectMethodReplace(\"System.String::PadLeft(System.Int32)\",\"\",[0],[False],[StringLiteral_0],Propagation,[])] PadLeft(System.String,System.Int32)",
"  [AspectMethodReplace(\"System.String::PadLeft(System.Int32,System.Char)\",\"\",[0],[False],[StringLiteral_0],Propagation,[])] PadLeft(System.String,System.Int32,System.Char)",
"  [AspectMethodReplace(\"System.String::PadRight(System.Int32)\",\"\",[0],[False],[StringLiteral_0],Propagation,[])] PadRight(System.String,System.Int32)",
"  [AspectMethodReplace(\"System.String::PadRight(System.Int32,System.Char)\",\"\",[0],[False],[StringLiteral_0],Propagation,[])] PadRight(System.String,System.Int32,System.Char)",
        };
    }
}
