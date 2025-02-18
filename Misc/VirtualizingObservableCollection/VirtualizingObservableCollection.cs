﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using AlphaChiTech.Virtualization.Interfaces;
using AlphaChiTech.Virtualization.Pageing;
using Castle.MicroKernel.ComponentActivator;

namespace AlphaChiTech.Virtualization
{
    public class VirtualizingObservableCollection<T> : IEnumerable, IEnumerable<T>, ICollection, ICollection<T>, IList, IList<T>,IObservableCollection<T>, INotifyCollectionChanged, IReplaySubjectImplementation<T>, INotifyPropertyChanged where T : class
    {
        //#region IEnumerable Implementation
        ///// <summary>
        /////     Returns an enumerator that iterates through a collection.
        ///// </summary>
        ///// <returns>
        /////     An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        ///// </returns>
        //IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        //#endregion IEnumerable Implementation

        //#region IEnumerable<T> Implementation
        ///// <summary>
        /////     Returns an enumerator that iterates through the collection.
        ///// </summary>
        ///// <returns>
        /////     A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        ///// </returns>
        //public IEnumerator<T> GetEnumerator()
        //{
        //    var sc = new Guid().ToString();

        //    this.EnsureCountIsGotNonaSync();

        //    var count = this.InternalGetCount();

        //    for(var iLoop = 0; iLoop < count; iLoop++) { yield return this.InternalGetValue(iLoop, sc); }
        //}
        //#endregion IEnumerable<T> Implementation

        IEnumerator IEnumerable.GetEnumerator() { return this.GetEnumerator(); }
        public IEnumerator<T> GetEnumerator() => new Enumerator<T>(this);

        public class Enumerator<TT>:IEnumerator<TT> where TT : class
        {
            public VirtualizingObservableCollection<TT> BaseCollection { get;  }
            private int iLoop = 0;
            /// <summary>
            /// Initializes a new instance of the <see cref="T:System.Object"/> class.
            /// </summary>
            public Enumerator(VirtualizingObservableCollection<TT> baseCollection) { this.BaseCollection = baseCollection; }

            #region Implementation of IDisposable
            public void Dispose() { }
            #endregion

            #region Implementation of IEnumerator
            public bool MoveNext()
            {
                var sc = new Guid().ToString();

                this.BaseCollection.EnsureCountIsGotNonaSync();

                //lock(this.BaseCollection.SyncRoot) {
                //    var count = this.BaseCollection.InternalGetCount();
                //    if(this.iLoop < count)
                //    {
                //        this.Current = this.BaseCollection.InternalGetValue(this.iLoop++, sc);
                //        return true;
                //    }
                //    return false;
                //}

                try
                {
                    var count = this.BaseCollection.InternalGetCount();
                    if(this.iLoop < count)
                    {
                        this.Current = this.BaseCollection.InternalGetValue(this.iLoop++, sc);
                        if(this.Current == null) Debugger.Break();
                        return true;
                    }
                    return false;
                }
                catch(Exception) {
                    return false;
                }
            }

            public void Reset() {
                this.iLoop = 0;
                this.Current = null;
            }

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            /// <returns>
            /// The element in the collection at the current position of the enumerator.
            /// </returns>
            public TT Current { get; private set; }

            object IEnumerator.Current => this.Current;
            #endregion
        }


        #region Ctors Etc
        /// <summary>
        ///     Initializes a new instance of the <see cref="VirtualizingObservableCollection{T}" /> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        public VirtualizingObservableCollection(IItemSourceProvider<T> provider) : this()
        {
            this.Provider = provider;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="VirtualizingObservableCollection{T}" /> class.
        /// </summary>
        /// <param name="asyncProvider">The asynchronous provider.</param>
        public VirtualizingObservableCollection(IItemSourceProviderAsync<T> asyncProvider) : this()
        {
            this.ProviderAsync = asyncProvider;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="VirtualizingObservableCollection{T}" /> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="reclaimer">The optional reclaimer.</param>
        /// <param name="expiryComparer">The optional expiry comparer.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="maxPages">The maximum pages.</param>
        /// <param name="maxDeltas">The maximum deltas.</param>
        /// <param name="maxDistance">The maximum distance.</param>
        public VirtualizingObservableCollection(
            IPagedSourceProvider<T> provider,
            IPageReclaimer<T> reclaimer = null,
            IPageExpiryComparer expiryComparer = null,
            int pageSize = 100,
            int maxPages = 100,
            int maxDeltas = -1,
            int maxDistance = -1) : this()
        {
            this.Provider = new PaginationManager<T>(provider, reclaimer, expiryComparer, pageSize, maxPages, maxDeltas, maxDistance);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="VirtualizingObservableCollection{T}" /> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="reclaimer">The optional reclaimer.</param>
        /// <param name="expiryComparer">The optional expiry comparer.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="maxPages">The maximum pages.</param>
        /// <param name="maxDeltas">The maximum deltas.</param>
        /// <param name="maxDistance">The maximum distance.</param>
        public VirtualizingObservableCollection(
            IPagedSourceObservableProvider<T> provider,
            IPageReclaimer<T> reclaimer = null,
            IPageExpiryComparer expiryComparer = null,
            int pageSize = 100,
            int maxPages = 100,
            int maxDeltas = -1,
            int maxDistance = -1):this()
        {
            this.Provider = new PaginationManager<T>(provider, reclaimer, expiryComparer, pageSize, maxPages, maxDeltas, maxDistance);
            (this.Provider as PaginationManager<T>).CollectionChanged += this.VirtualizingObservableCollection_CollectionChanged;
            this.IsSourceObservable = true;

        }

        protected VirtualizingObservableCollection()
        {
            //To enable reset in case that noone set UiThreadExcecuteAction
            if (VirtualizationManager.Instance.UiThreadExcecuteAction == null)
                VirtualizationManager.Instance.UiThreadExcecuteAction = a => Dispatcher.CurrentDispatcher.Invoke(a);
        }
        #endregion Ctors Etc

        private bool IsSourceObservable { get; } = false;

        #region ICollection Implementation
        public void CopyTo(Array array, int index) { throw new NotImplementedException(); }

        /// <summary>
        ///     Gets the number of elements contained in the <see cref="T:System.Collections.ICollection" />.
        /// </summary>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.ICollection" />.</returns>
        public int Count => this.InternalGetCount();

        /// <summary>
        ///     Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized
        ///     (thread safe).
        /// </summary>
        /// <returns>
        ///     true if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe);
        ///     otherwise, false.
        /// </returns>
        public bool IsSynchronized => false;

        /// <summary>
        ///     Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.
        /// </summary>
        /// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</returns>
        public object SyncRoot { get; } = new object();
        #endregion ICollection Implementation

        #region ICollection<T> Implementation
        /// <summary>
        ///     Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        public void Add(T item)
        {
            this.InternalAdd(item, null);
        }

        /// <summary>
        ///     Resets the collection - aka forces a get all data, including the count
        ///     <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public void Clear()
        {
            this.InternalClear();
        }

        /// <summary>
        ///     Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        ///     true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />;
        ///     otherwise, false.
        /// </returns>
        public bool Contains(T item)
        {
            return this.Provider.Contains(item);
            //return this.IndexOf(item) != -1? true : false;
        }

        public void CopyTo(T[] array, int arrayIndex) {
            foreach(var item in this) { array[arrayIndex++] = item; }
        }

        /// <summary>
        ///     Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, false.</returns>
        public bool IsReadOnly => false;

        /// <summary>
        ///     Removes the first occurrence of a specific object from the
        ///     <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        ///     true if <paramref name="item" /> was successfully removed from the
        ///     <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if
        ///     <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        public bool Remove(T item)
        {
            return this.InternalRemove(item);
        }
        #endregion ICollection<T> Implementation

        #region Extended CRUD operators that take into account the DateTime of the change
        /// <summary>
        ///     Removes the specified item - extended to only remove the item if the page was not pulled before the updatedat
        ///     DateTime.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="updatedAt">The updated at.</param>
        /// <returns></returns>
        public bool Remove(T item, object updatedAt)
        {
            return this.InternalRemove(item, updatedAt);
        }

        /// <summary>
        ///     Removes at the given index - extended to only remove the item if the page was not pulled before the updatedat
        ///     DateTime.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="updatedAt">The updated at.</param>
        /// <returns></returns>
        public bool RemoveAt(int index, object updatedAt)
        {
            return this.InternalRemoveAt(index, updatedAt);
        }

        /// <summary>
        ///     Adds (appends) the specified item - extended to only add the item if the page was not pulled before the updatedat
        ///     DateTime.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="updatedAt">The updated at.</param>
        /// <returns></returns>
        public int Add(T item, object updatedAt)
        {
            return this.InternalAdd(item, updatedAt);
        }

        /// <summary>
        ///     Inserts the specified index - extended to only insert the item if the page was not pulled before the updatedat
        ///     DateTime.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        /// <param name="updatedAt">The updated at.</param>
        public void Insert(int index, T item, object updatedAt)
        {
            this.InternalInsertAt(index, item, updatedAt);
        }

        /// <summary>
        ///     Adds the range.
        /// </summary>
        /// <param name="newValues">The new values.</param>
        /// <param name="timestamp">The updatedat object.</param>
        /// <returns>Index of the last appended object</returns>
        public int AddRange(IEnumerable<T> newValues, object timestamp = null)
        {
            var edit = this.GetProviderAsEditable();

            var index = -1;
            var items = new List<T>();

            foreach(var item in newValues)
            {
                items.Add(item);
                index = edit.OnAppend(item, timestamp);

                if(!this.IsSourceObservable)
                {
                    var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index);
                    this.RaiseCollectionChangedEvent(args);
                }
            }


            this.OnCountTouched();


            return index;
        }
        #endregion Extended CRUD operators that take into account the DateTime of the change

        #region IList Implementation
        /// <summary>
        ///     Adds an item to the <see cref="T:System.Collections.IList" />.
        /// </summary>
        /// <param name="value">The object to add to the <see cref="T:System.Collections.IList" />.</param>
        /// <returns>
        ///     The position into which the new element was inserted, or -1 to indicate that the item was not inserted into the
        ///     collection.
        /// </returns>
        public int Add(object value)
        {
            return this.InternalAdd((T)value, null);
        }

        bool IObservableCollection<T>.Remove(object item) { throw new NotImplementedException(); }

        bool IObservableCollection.Remove(object item) { throw new NotImplementedException(); }

        /// <summary>
        ///     Determines whether the <see cref="T:System.Collections.IList" /> contains a specific value.
        /// </summary>
        /// <param name="value">The object to locate in the <see cref="T:System.Collections.IList" />.</param>
        /// <returns>
        ///     true if the <see cref="T:System.Object" /> is found in the <see cref="T:System.Collections.IList" />; otherwise,
        ///     false.
        /// </returns>
        public bool Contains(object value)
        {
            if(value == null) return false; //some UI asked for null... wtf
            return this.Contains((T)value);
        }

        /// <summary>
        ///     Determines the index of a specific item in the <see cref="T:System.Collections.IList" />.
        /// </summary>
        /// <param name="value">The object to locate in the <see cref="T:System.Collections.IList" />.</param>
        /// <returns>
        ///     The index of <paramref name="value" /> if found in the list; otherwise, -1.
        /// </returns>
        public int IndexOf(object value)
        {
            return this.IndexOf((T)value);
        }

        /// <summary>
        ///     Inserts an item to the <see cref="T:System.Collections.IList" /> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="value" /> should be inserted.</param>
        /// <param name="value">The object to insert into the <see cref="T:System.Collections.IList" />.</param>
        public void Insert(int index, object value)
        {
            this.Insert(index, (T)value);
        }

        /// <summary>
        ///     Gets a value indicating whether the <see cref="T:System.Collections.IList" /> has a fixed size.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Collections.IList" /> has a fixed size; otherwise, false.</returns>
        public bool IsFixedSize => false;

        void IObservableCollection.Add(object item) { this.Add(item); }

        /// <summary>
        ///     Removes the first occurrence of a specific object from the <see cref="T:System.Collections.IList" />.
        /// </summary>
        /// <param name="value">The object to remove from the <see cref="T:System.Collections.IList" />.</param>
        public void Remove(object value)
        {
            this.Remove((T)value);
        }

        /// <summary>
        ///     Removes the <see cref="T:System.Collections.IList" /> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        public void RemoveAt(int index)
        {
            this.InternalRemoveAt(index);
        }

        /// <summary>
        ///     Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        object IList.this[int index]
        {
            get { return this.InternalGetValue(index, this.DefaultSelectionContext); }
            set { this.InternalSetValue(index, (T) value); }
        }
        #endregion IList Implementation

        #region IList<T> Implementation
        /// <summary>
        ///     Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1" />.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1" />.</param>
        /// <returns>
        ///     The index of <paramref name="item" /> if found in the list; otherwise, -1.
        /// </returns>
        public int IndexOf(T item)
        {
            return this.InternalIndexOf(item);
        }

        /// <summary>
        ///     Inserts an item to the <see cref="T:System.Collections.Generic.IList`1" /> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1" />.</param>
        public void Insert(int index, T item)
        {
            this.InternalInsertAt(index, item);
        }

        /// <summary>
        ///     Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public T this[int index]
        {
            get { return this.InternalGetValue(index, this.DefaultSelectionContext); }
            set { this.InternalSetValue(index, value); }
        }
        #endregion IList<T> Implementation

        #region Public Properties
        /// <summary>
        ///     Gets or sets the provider if its asynchronous.
        /// </summary>
        /// <value>
        ///     The provider asynchronous.
        /// </value>
        public IItemSourceProviderAsync<T> ProviderAsync
        {
            get { return this._providerAsync; }
            set
            {
                this.ClearCountChangedHooks();
                this._providerAsync = value;
                if(this._providerAsync is INotifyCountChanged) { (this._providerAsync as INotifyCountChanged).CountChanged += this.VirtualizingObservableCollection_CountChanged; }
            }
        }

        private void VirtualizingObservableCollection_CountChanged(object sender, CountChangedEventArgs args)
        {
            if(args.NeedsReset)
            {
                // Send a reset..
                this.RaiseCollectionChangedEvent(_ccResetArgs);
            }
            this.OnCountTouched();
        }

        /// <summary>
        ///     Gets or sets the provider if its not asynchronous.
        /// </summary>
        /// <value>
        ///     The provider.
        /// </value>
        public IItemSourceProvider<T> Provider
        {
            get { return this._provider; }
            set
            {
                this.ClearCountChangedHooks();
                this._provider = value;

                if(this._provider is INotifyCountChanged) { (this._provider as INotifyCountChanged).CountChanged += this.VirtualizingObservableCollection_CountChanged; }
                //if(this._provider is INotifyCollectionChanged) {
                //    (this._provider as INotifyCollectionChanged).CollectionChanged += this.VirtualizingObservableCollection_CollectionChanged;
                //}
            }
        }

        private void VirtualizingObservableCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Debugger.Break();
            this.RaiseCollectionChangedEvent(e);
            this.OnCountTouched();
        }

        private void ClearCountChangedHooks()
        {
            if(this._provider is INotifyCountChanged) { (this._provider as INotifyCountChanged).CountChanged -= this.VirtualizingObservableCollection_CountChanged; }

            if(this._providerAsync is INotifyCountChanged) { (this._providerAsync as INotifyCountChanged).CountChanged -= this.VirtualizingObservableCollection_CountChanged; }

            //if(this._provider is INotifyCollectionChanged) {
            //    (this._provider as INotifyCollectionChanged).CollectionChanged -= this.VirtualizingObservableCollection_CollectionChanged;
            //}
            //if(this._providerAsync is INotifyCollectionChanged) {
            //    (this._providerAsync as INotifyCollectionChanged).CollectionChanged -= this.VirtualizingObservableCollection_CollectionChanged;
            //}
        }
        #endregion Public Properties

        #region INotifyCollectionChanged Implementation
        public bool SupressEventErrors { get; set; } = false;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
        {
            add { this.CollectionChanged += value; }
            remove { this.CollectionChanged -= value; }
        }

        /// <summary>
        ///     Raises the collection changed event.
        /// </summary>
        /// <param name="args">The <see cref="NotifyCollectionChangedEventArgs" /> instance containing the event data.</param>
        internal void RaiseCollectionChangedEvent(NotifyCollectionChangedEventArgs args)
        {
            if(this._bulkCount > 0) { return; }

            var eventHandler = this.CollectionChanged;
            if(eventHandler == null) { return; }

            // Walk thru invocation list.
            var delegates = eventHandler.GetInvocationList();

            foreach(var @delegate in delegates)
            {
                var handler = (NotifyCollectionChangedEventHandler) @delegate;

                // If the subscriber is a DispatcherObject and different thread.
                var dispatcherObject = handler.Target as DispatcherObject;
                try
                {
                    if(dispatcherObject != null && !dispatcherObject.CheckAccess())
                    {
                        // Invoke handler in the target dispatcher's thread... 
                        // asynchronously for better responsiveness.
                        dispatcherObject.Dispatcher.BeginInvoke(DispatcherPriority.DataBind, handler, this, args);
                    }
                    else
                    {
                        // Execute handler as is.
                        handler(this, args);
                    }
                }
                catch(ComponentActivatorException ex)
                {
                    Debug.WriteLine(ex.Message);
                    Debugger.Break();
                }
                catch(Exception ex) //WTF? exception catch during remove operations with collection, try add and remove investigation
                {
                    Debug.WriteLine(ex.Message);
                    Debugger.Break();
                    //SystemMessage.SendSystemMessage(SystemMessage.Type.Warning, "AsyncVirtualizingObservableCollection", ex.Message);
                    //        if (!this.SupressEventErrors)
                    //        {
                    //            throw ex;
                    //        }
                }
            }
        }
        #endregion INotifyCollectionChanged Implementation

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { this.PropertyChanged += value; }
            remove { this.PropertyChanged -= value; }
        }

        private static readonly PropertyChangedEventArgs _pcCountArgs = new PropertyChangedEventArgs("Count");
        private static readonly NotifyCollectionChangedEventArgs _ccResetArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);

        private void OnCountTouched() { this.RaisePropertyChanged(_pcCountArgs); }

        protected void RaisePropertyChanged(PropertyChangedEventArgs args)
        {
            if(this._bulkCount > 0) { return; }

            var evnt = this.PropertyChanged;
            evnt?.Invoke(this, args);
        }
        #endregion INotifyPropertyChanged implementation

        #region Bulk Operation implementation
        /// <summary>
        ///     Releases the bulk mode.
        /// </summary>
        internal void ReleaseBulkMode()
        {
            if(this._bulkCount > 0) { this._bulkCount--; }

            if(this._bulkCount == 0)
            {
                this.RaiseCollectionChangedEvent(_ccResetArgs);
                this.RaisePropertyChanged(_pcCountArgs);
            }
        }

        /// <summary>
        ///     Enters the bulk mode.
        /// </summary>
        /// <returns></returns>
        public BulkMode EnterBulkMode()
        {
            this._bulkCount++;

            return new BulkMode(this);
        }

        /// <summary>
        ///     The Bulk mode IDisposable proxy
        /// </summary>
        public class BulkMode : IDisposable
        {
            private bool _isDisposed;
            private readonly VirtualizingObservableCollection<T> _voc;

            public BulkMode(VirtualizingObservableCollection<T> voc) { this._voc = voc; }

            public void Dispose() { this.OnDispose(); }

            private void OnDispose()
            {
                if(!this._isDisposed)
                {
                    this._isDisposed = true;
                    if(this._voc != null) { this._voc.ReleaseBulkMode(); }
                }
            }

            ~BulkMode() { this.OnDispose(); }
        }
        #endregion Bulk Operation implementation

        #region Private Properties
        protected String DefaultSelectionContext = new Guid().ToString();
        private IItemSourceProvider<T> _provider;
        private IItemSourceProviderAsync<T> _providerAsync;
        private int _bulkCount;
        #endregion Private Properties

        #region Internal implementation
        /// <summary>
        ///     Gets the provider as editable.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>
        protected IEditableProvider<T> GetProviderAsEditable()
        {
            IEditableProvider<T> ret = null;

            if(this.Provider != null) {
                ret = this.Provider as IEditableProvider<T>;
            }
            else
            {
                ret = this.ProviderAsync as IEditableProvider<T>;
            }

            if(ret == null) { throw new NotSupportedException(); }

            return ret;
        }

        /// <summary>
        ///     Replaces oldValue with newValue at index if updatedat is newer or null.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="timestamp">The timestamp.</param>
        internal void ReplaceAt(int index, T oldValue, T newValue, object timestamp)
        {
            var edit = this.GetProviderAsEditable();

            if(edit != null)
            {
                edit.OnReplace(index, oldValue, newValue, timestamp);
                if(!this.IsSourceObservable)
                {
                    var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newValue, oldValue, index);
                    this.RaiseCollectionChangedEvent(args);
                }
            }
        }

        private void InternalClear()
        {
            if(this.Provider != null)
            {
                if(this.Provider is IProviderPreReset) { (this.Provider as IProviderPreReset).OnBeforeReset(); }
                this.Provider.OnReset(-1);
            }
            else
            {
                if(this.ProviderAsync is IProviderPreReset) { (this.ProviderAsync as IProviderPreReset).OnBeforeReset(); }
                this.ProviderAsync.OnReset(-1);
            }
        }

        private CancellationTokenSource _resetToken;

        public void Reset()
        {
            this.ResetAsync().Wait();
        }
        public async Task ResetAsync()
        {
            CancellationTokenSource cts = null;

            lock(this)
            {
                if(this._resetToken != null)
                {
                    this._resetToken.Cancel();
                    this._resetToken = null;
                }

                cts = this._resetToken = new CancellationTokenSource();
            }

            if(this.Provider != null)
            {
                if(this.Provider is IProviderPreReset)
                {
                    (this.Provider as IProviderPreReset).OnBeforeReset();
                    if(cts.IsCancellationRequested) { return; }
                }

                this.Provider.OnReset(-2);

               await Task.Run(async () =>
                {
                    if(this.Provider is IAsyncResetProvider)
                    {
                        var count = await (this.Provider as IAsyncResetProvider).GetCountAsync();
                        if(!cts.IsCancellationRequested) { VirtualizationManager.Instance.RunOnUI(() => this.Provider.OnReset(count)); }
                    }
                    else
                    {
                        var count = this.Provider.GetCount(false);
                        if(!cts.IsCancellationRequested) { VirtualizationManager.Instance.RunOnUI(() => this.Provider.OnReset(count)); }
                    }
                });
            }
            else
            {
                if(this.ProviderAsync is IProviderPreReset) { (this.ProviderAsync as IProviderPreReset).OnBeforeReset(); }
                this.ProviderAsync.OnReset(await this.ProviderAsync.Count);
            }

            lock(this) { if(this._resetToken == cts) { this._resetToken = null; } }
        }

        private T InternalGetValue(int index, string selectionContext)
        {
            var allowPlaceholder = true;
            if(selectionContext != this.DefaultSelectionContext) { allowPlaceholder = false; }

            if(this.Provider != null) { return this.Provider.GetAt(index, this, allowPlaceholder); }
            return Task.Run(() => this.ProviderAsync.GetAt(index, this, allowPlaceholder)).Result;
        }

        private T InternalSetValue(int index, T newValue)
        {
            var oldValue = this.InternalGetValue(index, this.DefaultSelectionContext);
            var edit = this.GetProviderAsEditable();
            edit.OnReplace(index, oldValue, newValue, null);

            var newItems = new List<T>();
            newItems.Add(newValue);
            var oldItems = new List<T>();
            oldItems.Add(oldValue);

            if(!this.IsSourceObservable)
            {
                var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItems, oldItems, index);
                this.RaiseCollectionChangedEvent(args);
            }

            return oldValue;
        }

        private int InternalAdd(T newValue, object timestamp)
        {
            var edit = this.GetProviderAsEditable();
            var index = edit.OnAppend(newValue, timestamp);
            this.OnCountTouched();
            if (!this.IsSourceObservable)
            {
                var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newValue, index);
                this.RaiseCollectionChangedEvent(args);
            }
            return index;
        }

        private int InternalGetCount()
        {
            var ret = 0;

            if(this.Provider != null) {
                ret = this.Provider.GetCount(false);
            }
            else
            {
                ret = Task.Run(() => this.ProviderAsync.Count).Result;
            }


            return ret;
        }

        private void InternalInsertAt(int index, T item, object timestamp = null)
        {
            var edit = this.GetProviderAsEditable();
            edit.OnInsert(index, item, timestamp);

            this.OnCountTouched();
            if (!this.IsSourceObservable)
            {
                var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index);
                this.RaiseCollectionChangedEvent(args);
            }

            
        }

        private bool InternalRemoveAt(int index, object timestamp = null)
        {
            var edit = this.Provider as IEditableProviderIndexBased<T>;
            if (edit == null) return false;
            var oldValue = edit.OnRemove(index, timestamp);

            this.OnCountTouched();
            if (!this.IsSourceObservable)
            {
                var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldValue, index);
                this.RaiseCollectionChangedEvent(args);
            }
            

            return true;
        }

        private bool InternalRemove(T item, object timestamp = null)
        {
            if (item == null) { return false; }
            var edit = this.Provider as IEditableProviderItemBased<T>;
            if(edit == null)
                return false;
            var index = edit.OnRemove(item, timestamp);

            this.OnCountTouched();
            if (!this.IsSourceObservable)
            {
                var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index);
                this.RaiseCollectionChangedEvent(args);
            }
            

            return true;
        }

        private int InternalIndexOf(T item)
        {
            return this.Provider?.IndexOf(item) ?? Task.Run(() => this.ProviderAsync.IndexOf(item)).Result;
        }

        private void EnsureCountIsGotNonaSync()
        {
            this.Provider?.GetCount(false);
        }
        #endregion Internal implementation

        //#region Implementation of IObservable<out T>
        ///// <summary>
        ///// Notifies the provider that an observer is to receive notifications.
        ///// </summary>
        ///// <returns>
        ///// A reference to an interface that allows observers to stop receiving notifications before the provider has finished sending them.
        ///// </returns>
        ///// <param name="observer">The object that is to receive notifications.</param>
        //public IDisposable Subscribe(IObserver<T> observer) { throw new NotImplementedException(); }
        //#endregion

        #region Implementation of IObservable<out T>
        private readonly object _gate = new object();

        private Exception _error;
        private bool _isDisposed;
        private bool _isStopped;
        private ImmutableList<IObserver<T>> _observers = new ImmutableList<IObserver<T>>();


        public bool HasObservers
        {
            get
            {
                var immutableList = this._observers;
                return immutableList?.Data.Length > 0;
            }
        }

        #region Implementation of IDisposable
        public void Dispose() { this.Dispose(true); }
        #endregion

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null) { throw new ArgumentNullException(nameof(observer)); }
            var subscription = new Subscription((IReplaySubjectImplementation<T>)this, observer);
            lock (this._gate)
            {
                this.CheckDisposed();
                this._observers = this._observers.Add(observer);
                this.ReplayBuffer(observer);
                if (this._error != null) { observer.OnError(this._error); }
                else if (this._isStopped) { observer.OnCompleted(); }
            }
            return subscription;
        }

        public void OnCompleted()
        {
            lock (this._gate)
            {
                this.CheckDisposed();
                if (this._isStopped) { return; }
                this._isStopped = true;
                foreach (var item_0 in this._observers.Data) { item_0.OnCompleted(); }
                this._observers = new ImmutableList<IObserver<T>>();
            }
        }

        public void OnError(Exception error)
        {
            if (error == null) { throw new ArgumentNullException("error"); }
            lock (this._gate)
            {
                this.CheckDisposed();
                if (this._isStopped) { return; }
                this._isStopped = true;
                this._error = error;
                foreach (var item_0 in this._observers.Data) { item_0.OnError(error); }
                this._observers = new ImmutableList<IObserver<T>>();
            }
        }

        public void OnNext(T value)
        {
            lock (this._gate)
            {
                this.CheckDisposed();
                if (this._isStopped) { return; }
                foreach (var item_0 in this._observers.Data) { item_0.OnNext(value); }
            }
        }

        public void Dispose(bool disposing)
        {
            lock (this._gate)
            {
                this._isDisposed = true;
                this._observers = null;
            }
        }

        public void Unsubscribe(IObserver<T> observer)
        {
            lock (this._gate)
            {
                if (this._isDisposed) { return; }
                this._observers = this._observers.Remove(observer);
            }
        }


        protected void ReplayBuffer(IObserver<T> observer) { foreach (var obj in this) { observer.OnNext(obj); } }


        private void CheckDisposed() { if (this._isDisposed) { throw new ObjectDisposedException(string.Empty); } }


        private class Subscription : IDisposable
        {
            private IObserver<T> _observer;
            private IReplaySubjectImplementation<T> _subject;

            public Subscription(IReplaySubjectImplementation<T> subject, IObserver<T> observer)
            {
                this._subject = subject;
                this._observer = observer;
            }

            public void Dispose()
            {
                var observer = Interlocked.Exchange(ref this._observer, null);
                if (observer == null) { return; }
                this._subject.Unsubscribe(observer);
                this._subject = null;
            }
        }

        private class ImmutableList<TT>
        {
            public ImmutableList() { this.Data = new TT[0]; }

            private ImmutableList(TT[] data) { this.Data = data; }

            public TT[] Data { get; }

            public ImmutableList<TT> Add(TT value)
            {
                var newData = new TT[this.Data.Length + 1];
                Array.Copy(this.Data, newData, this.Data.Length);
                newData[this.Data.Length] = value;
                return new ImmutableList<TT>(newData);
            }

            private int IndexOf(TT value)
            {
                for (var i = 0; i < this.Data.Length; ++i) { if (this.Data[i].Equals(value)) { return i; } }
                return -1;
            }

            public ImmutableList<TT> Remove(TT value)
            {
                var i = this.IndexOf(value);
                if (i < 0) { return this; }
                var newData = new TT[this.Data.Length - 1];
                Array.Copy(this.Data, 0, newData, 0, i);
                Array.Copy(this.Data, i + 1, newData, i, this.Data.Length - i - 1);
                return new ImmutableList<TT>(newData);
            }
        }

        #endregion
    }
}