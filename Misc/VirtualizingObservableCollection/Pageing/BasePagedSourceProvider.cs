﻿using System;
using System.Collections;
using AlphaChiTech.Virtualization.Interfaces;

namespace AlphaChiTech.Virtualization.Pageing
{
    public class BasePagedSourceProvider<T> : IPagedSourceProvider<T>
    {
        public BasePagedSourceProvider()
        {
        }

        public BasePagedSourceProvider(
            Func<int, int, PagedSourceItemsPacket<T>> funcGetItemsAt = null,
            Func<int> funcGetCount = null,
            Func<T, int> funcIndexOf = null,
            Func<T, bool> funcContains = null,
            Action<int> actionOnReset = null
            )
        {
            this.FuncGetItemsAt = funcGetItemsAt;
            this.FuncGetCount = funcGetCount;
            this.FuncIndexOf = funcIndexOf;
            this.FuncContains = funcContains;
            this.ActionOnReset = actionOnReset;
        }

        public Func<int, int, PagedSourceItemsPacket<T>> FuncGetItemsAt { get; set; } = null;

        public Func<int> FuncGetCount { get; set; } = null;

        public Func<T, int> FuncIndexOf { get; set; } = null;
        public Func<T, bool> FuncContains { get; set; }

        public Action<int> ActionOnReset { get; set; } = null;

        public virtual PagedSourceItemsPacket<T> GetItemsAt(int pageoffset, int count, bool usePlaceholder)
        {
            return this.FuncGetItemsAt?.Invoke(pageoffset, count);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.ICollection"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection"/>. The <see cref="T:System.Array"/> must have zero-based indexing. </param><param name="index">The zero-based index in <paramref name="array"/> at which copying begins. </param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null. </exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is less than zero. </exception><exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.-or- The number of elements in the source <see cref="T:System.Collections.ICollection"/> is greater than the available space from <paramref name="index"/> to the end of the destination <paramref name="array"/>.-or-The type of the source <see cref="T:System.Collections.ICollection"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.</exception>
        public void CopyTo(Array array, int index) { throw new NotImplementedException(); }

        public virtual int Count
        {
            get
            {
                var ret = 0;

                if (this.FuncGetCount != null) ret = this.FuncGetCount.Invoke();

                return ret;
            }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.
        /// </summary>
        /// <returns>
        /// An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.
        /// </returns>
        public object SyncRoot => this;

        /// <summary>
        /// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe).
        /// </summary>
        /// <returns>
        /// true if access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe); otherwise, false.
        /// </returns>
        public bool IsSynchronized { get; } = false;

        public virtual int IndexOf(T item)
        {
            return this.FuncIndexOf?.Invoke(item) ?? -1 ;
        }

        public bool Contains(T item)
        {
            return this.FuncContains?.Invoke(item) ?? false;
        }

        public virtual void OnReset(int count)
        {
            this.ActionOnReset?.Invoke(count);
        }

        #region Implementation of IEnumerable
        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator GetEnumerator() { throw new NotImplementedException(); }
        #endregion
    }
}
