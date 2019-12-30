﻿using System;
using System.Collections.Generic;
using EventStore.Core.Data;
using EventStore.Core.Messages;
using EventStore.Core.Messaging;
using EventStore.Core.Services.RequestManager;
using EventStore.Core.Services.RequestManager.Managers;
using EventStore.Core.Tests.Fakes;
using EventStore.Core.TransactionLog.LogRecords;
using NUnit.Framework;

namespace EventStore.Core.Tests.Services.Replication.WriteStream {
	[TestFixture]
	public class when_write_stream_gets_timeout_before_local_commit : RequestManagerSpecification {
		protected override IRequestManager OnManager(FakePublisher publisher) {
			return new WriteStreamRequestManager(publisher,  CommitTimeout, false);
		}

		protected override IEnumerable<Message> WithInitialMessages() {
			yield return new ClientMessage.WriteEvents(InternalCorrId, ClientCorrId, Envelope, true, "test123",
				ExpectedVersion.Any, new[] {DummyEvent()}, null);			
		}

		protected override Message When() {
			return new StorageMessage.RequestManagerTimerTick(DateTime.UtcNow + CommitTimeout + CommitTimeout);
		}

		[Test]
		public void request_completed_as_failed_published() {
			Assert.AreEqual(1, Produced.Count);
			Assert.AreEqual(false, ((StorageMessage.RequestCompleted)Produced[0]).Success);
		}

		[Test]
		public void the_envelope_is_replied_to() {
			Assert.AreEqual(1, Envelope.Replies.Count);

		}
	}
}