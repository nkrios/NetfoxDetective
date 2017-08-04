﻿/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2012-2013 Brno University of Technology - Faculty of Information Technology (http://www.fit.vutbr.cz)
 * Author(s):
 * Vladimir Vesely (mailto:ivesely@fit.vutbr.cz)
 * Martin Mares (mailto:xmares04@stud.fit.vutbr.cz)
 * Jan Plusal (mailto:xplusk03@stud.fit.vutbr.cz)
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
 * documentation files (the "Software"), to deal in the Software without restriction, including without limitation
 * the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
 * and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
 * TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
 * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
 * TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
using System;
using System.Collections.Concurrent;
using System.IO;

namespace Netfox.Framework.Models.PmLib
{
    /// <remarks>
    ///     Pool of binary readers for concurrent data retrieval
    /// </remarks>
    public class BinaryReadersPool:IDisposable
    {
        private readonly FileInfo _fileInfo;
        private readonly ConcurrentBag<BinaryReader> _readersAllOpened;
        private readonly ConcurrentBag<BinaryReader> _readersPool;

        public BinaryReadersPool(FileInfo fileInfo)
        {
            this._readersPool = new ConcurrentBag<BinaryReader>();
            this._fileInfo = fileInfo;
            this._readersAllOpened = new ConcurrentBag<BinaryReader>();
        }

        public BinaryReader GetReader()
        {
            BinaryReader reader;

            if(this._readersPool.TryTake(out reader)) { return reader; }
            reader = new BinaryReader(new FileStream(this._fileInfo.FullName, FileMode.Open, FileAccess.Read,FileShare.Read));
            this._readersAllOpened.Add(reader);
            return reader;
        }

        public void PutReader(BinaryReader item) => this._readersPool.Add(item);

        #region Implementation of IDisposable
        bool _disposed = false;
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (this._disposed)
                return;

            if (disposing)
            {
                foreach (var reader in this._readersAllOpened) { reader.Close(); }
                // Free any other managed objects here.
            }

            // Free any unmanaged objects here.
            this._disposed = true;
        }
        #endregion
    }
}