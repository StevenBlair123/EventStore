﻿using EventStore.ClusterNode;
using EventStore.Core.TransactionLog.Chunks;

namespace EventStore.Client {
	public class TestVNodeBuilder : ClusterVNodeBuilder {

		public TFChunkDb GetDb() {
			return _db;
		}

	}
}
