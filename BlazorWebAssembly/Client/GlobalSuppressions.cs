// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage( "IDisposableAnalyzers.Correctness", "IDISP014:Use a single instance of HttpClient", Justification = "It's being injected as a service", Scope = "member", Target = "~M:BlazorWebAssembly.Client.Program.Main(System.String[])~System.Threading.Tasks.Task" )]
