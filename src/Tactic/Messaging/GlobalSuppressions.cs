// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the 
// Error List, point to "Suppress Message(s)", and click 
// "In Project Suppression File".
// You do not need to add suppressions to this file manually.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1014:MarkAssembliesWithClsCompliant")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "LightStudio.Tactic.Messaging.SyncDictionary`2.#GetCopy()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "LightStudio.Tactic.Messaging.SyncList`1.#GetCopy()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "LightStudio.Tactic.Messaging.MessageWaiter.#Wait()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member", Target = "LightStudio.Tactic.Messaging.SyncDictionary`2.#.ctor()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member", Target = "LightStudio.Tactic.Messaging.SyncList`1.#.ctor()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "LightStudio.Tactic.Messaging.Dispatcher+Work.#Wait()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member", Target = "LightStudio.Tactic.Messaging.Dispatcher.#BeginInvoke(System.Delegate)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member", Target = "LightStudio.Tactic.Messaging.Dispatcher.#BeginInvoke(System.Delegate,System.Object[])")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member", Target = "LightStudio.Tactic.Messaging.Dispatcher.#Invoke(System.Delegate)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member", Target = "LightStudio.Tactic.Messaging.Dispatcher.#Invoke(System.Delegate,System.Object[])")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member", Target = "LightStudio.Tactic.Messaging.Messager.#RegisterEventWaiter(System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member", Target = "LightStudio.Tactic.Messaging.Messager.#RegisterGeneralWaiter()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "LightStudio.Tactic.Messaging.SyncDictionary`2.#AddRange(System.Collections.Generic.IEnumerable`1<System.Collections.Generic.KeyValuePair`2<!0,!1>>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Stop", Scope = "member", Target = "LightStudio.Tactic.Messaging.MessageServer`1.#Stop()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "LightStudio.Tactic.Messaging.Dispatcher.#Process()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Stop", Scope = "member", Target = "LightStudio.Tactic.Messaging.IMessageServer`1.#Stop()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "exception", Scope = "member", Target = "LightStudio.Tactic.Messaging.MessageServer`1.#OnSessionIOException(!0,System.IO.IOException)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member", Target = "LightStudio.Tactic.Messaging.MessageCenter`1.#.ctor()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "LightStudio.Tactic.Messaging.MessageServer`1.#OnSessionIOException(!0,System.IO.IOException)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "LightStudio.Tactic.Messaging.DictionaryExtensions.#AddRange`2(System.Collections.Generic.IDictionary`2<!!0,!!1>,System.Collections.Generic.IEnumerable`1<System.Collections.Generic.KeyValuePair`2<!!0,!!1>>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "LightStudio.Tactic.Messaging.Dispatcher.#SetAddWorkEvent()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "LightStudio.Tactic.Messaging.Dispatcher+Work.#SetComplete()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "LightStudio.Tactic.Messaging.MessageWaiter.#SetWaiter()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Stop", Scope = "member", Target = "LightStudio.Tactic.Messaging.IAcceptor.#Stop()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Stop", Scope = "member", Target = "LightStudio.Tactic.Messaging.IMessageServer.#Stop()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "LightStudio.Tactic.Messaging.MessageResolverFacade.#ToMessageOrNull(System.Object)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "LightStudio.Tactic.Messaging.MessageResolverFacade.#GetMessageObjectOrNull(LightStudio.Tactic.Messaging.IMessage)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "LightStudio.Tactic.Messaging.Messager.#OnSafeUnhandledException(LightStudio.Tactic.Messaging.MessageException)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "LightStudio.Tactic.Messaging.Messager.#OnSafeReceive(LightStudio.Tactic.Messaging.IMessage)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "LightStudio.Tactic.Messaging.Messager.#OnSafeException(System.String,System.Exception)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "LightStudio.Tactic.Messaging.Dispatcher+Work.#DoWork()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "LightStudio.Tactic.Messaging.Connector.#OnSafeDisconnected()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "LightStudio.Tactic.Messaging.Connector.#OnSafeConnectFailed(System.String,System.Exception)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "LightStudio.Tactic.Messaging.Connector.#OnSafeConnectFailed(LightStudio.Tactic.Messaging.MessageException)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "LightStudio.Tactic.Messaging.Connector.#OnSafeConencted()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "LightStudio.Tactic.Messaging.Acceptor.#OnSafeUnhandledException(LightStudio.Tactic.Messaging.MessageException)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "LightStudio.Tactic.Messaging.Acceptor.#OnSafeException(System.String,System.Exception)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "LightStudio.Tactic.Messaging.Acceptor.#OnSafeAccepted(LightStudio.Tactic.Messaging.IMessager)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member", Target = "LightStudio.Tactic.Messaging.DataContractResolver.#GetMessageObject(LightStudio.Tactic.Messaging.IMessage)")]
