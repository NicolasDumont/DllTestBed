﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting;

namespace Remoting
{
    ///Interface that can be run over the remote AppDomain boundary.
    public interface IRemote
    {
        object Invoke(string methodName, object[] methodParameters);
    }

    /// <summary>
    /// Enables access to objects across application domain boundaries.
    /// This type differs from <see cref="MarshalByRefObject"/> by ensuring that the
    /// service lifetime is managed deterministically by the consumer.
    /// </summary>
    //http://nbevans.wordpress.com/2011/04/17/memory-leaks-with-an-infinite-lifetime-instance-of-marshalbyrefobject/
    public abstract class CrossAppDomainObject : MarshalByRefObject, IDisposable
    {

        private bool _disposed;

        /// <summary>
        /// Gets an enumeration of nested <see cref="MarshalByRefObject"/> objects.
        /// </summary>
        protected virtual IEnumerable<MarshalByRefObject> NestedMarshalByRefObjects
        {
            get { yield break; }
        }

        ~CrossAppDomainObject()
        {
            Dispose(false);
        }

        /// <summary>
        /// Disconnects the remoting channel(s) of this object and all nested objects.
        /// </summary>
        private void Disconnect()
        {
            RemotingServices.Disconnect(this);

            foreach (var tmp in NestedMarshalByRefObjects)
                RemotingServices.Disconnect(tmp);
        }

        public sealed override object InitializeLifetimeService()
        {
            //
            // Returning null designates an infinite non-expiring lease.
            // We must therefore ensure that RemotingServices.Disconnect() is called when
            // it's no longer needed otherwise there will be a memory leak.
            //
            return null;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            Disconnect();
            _disposed = true;
        }
    }
}
