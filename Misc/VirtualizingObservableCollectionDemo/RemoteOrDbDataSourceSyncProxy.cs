using System;
using System.Linq;
using System.Threading.Tasks;
using AlphaChiTech.Virtualization.Interfaces;
using AlphaChiTech.Virtualization.Pageing;
using VirtualizingObservableCollectionDemo.filtersort;

namespace VirtualizingObservableCollectionDemo
{
    #region
    #endregion

    /// <summary>
    /// Remote/disk async data proxy.
    /// Also add a delay on calls to simulate network/disk delay.
    /// </summary>
    public class RemoteOrDbDataSourceSyncProxy: IPagedSourceProvider<RemoteOrDbDataItem>
    {
        #region fields

        private readonly RemoteOrDbDataSourceEmulation _remoteDatas;

        #endregion

        public RemoteOrDbDataSourceSyncProxy(RemoteOrDbDataSourceEmulation remoteDatas)
        {
            this._remoteDatas = remoteDatas;
        }

        #region properties



        public FilterDescriptionList FilterDescriptionList => this._remoteDatas.FilterDescriptionList;
        #endregion

        #region IFilteredSortedSourceProviderAsync Members

        public SortDescriptionList SortDescriptionList => this._remoteDatas.SortDescriptionList;
        #endregion

        #region IPagedSourceProvider<RemoteOrDbDataItem> Members (synchronous not available members)

        int IPagedSourceProvider<RemoteOrDbDataItem>.IndexOf(RemoteOrDbDataItem item) { return this.IndexOfAsync(item).Result; }
        public bool Contains(RemoteOrDbDataItem item) { throw new NotImplementedException(); }

        public PagedSourceItemsPacket<RemoteOrDbDataItem> GetItemsAt(int pageoffset, int count, bool usePlaceholder)
        {
            return this.GetItemsAtAsync(pageoffset, count, usePlaceholder).Result;
        }

        public int Count => this._remoteDatas.Count;


        #endregion

        #region public members



        public Task<int> GetCountAsync()
        {
            return Task.Run(() =>
            {
                Task.Delay(1000).Wait(); // Just to slow it down !
                return this._remoteDatas.FilteredOrderedItems.Count;
            });
        }

        public Task<PagedSourceItemsPacket<RemoteOrDbDataItem>> GetItemsAtAsync(int pageoffset, int count, bool usePlaceholder)
        {
            return Task.Run(() =>
            {
                Task.Delay(1000).Wait(); // Just to slow it down !
                return new PagedSourceItemsPacket<RemoteOrDbDataItem>
                {
                    LoadedAt = DateTime.Now,
                    Items = (from items in this._remoteDatas.FilteredOrderedItems select items).Skip(pageoffset).Take(count)
                };
            });
        }

        public RemoteOrDbDataItem GetPlaceHolder(int index, int page, int offset)
        {
            return new RemoteOrDbDataItem { Name = "Waiting [" + page + "/" + offset + "]" };
        }

        /// <summary>
        /// This returns the index of a specific item. This method is optional � you can just return -1 if you 
        /// don�t need to use IndexOf. It�s not strictly required if don�t need to be able to seeking to a 
        /// specific item, but if you are selecting items implementing this method is recommended.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<int> IndexOfAsync(RemoteOrDbDataItem item)
        {
            return Task.Run(() => { return this._remoteDatas.FilteredOrderedItems.IndexOf(item); });
        }

        /// <summary>
        /// This is a callback that runs when a Reset is called on a provider. Implementing this is also optional. 
        /// If you don�t need to do anything in particular when resets occur, you can leave this method body empty.
        /// </summary>
        /// <param name="count"></param>
        public void OnReset(int count)
        {
            // Do nothing for now
        }

        #endregion

        #region Implementation of ISynchronized
        public object SyncRoot { get; } = new object();
        public bool IsSynchronized { get; } = false;
        #endregion
    }
}
