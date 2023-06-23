// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage( "Minor Code Smell", "S1125:Boolean literals should not be redundant", Justification = "I prefer the ==false", Scope = "module" )]
[assembly: SuppressMessage( "Minor Code Smell", "S1125:Remove the unnecessary Boolean literals", Justification = "I prefer the ==false", Scope = "module" )]
