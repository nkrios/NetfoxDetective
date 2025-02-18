﻿using System;
using System.Threading.Tasks;
using AlphaChiTech.Virtualization.Interfaces;

namespace AlphaChiTech.Virtualization.Pageing
{
    public class PagedSourceProviderMakeAsync<T> : BasePagedSourceProvider<T>, IPagedSourceProviderAsync<T>, IProviderPreReset
    {
        public PagedSourceProviderMakeAsync()
        {
        }

        public PagedSourceProviderMakeAsync(
            Func<int, int, PagedSourceItemsPacket<T>> funcGetItemsAt = null,
            Func<int> funcGetCount = null,
            Func<T, Task<int>> funcIndexOfAsync = null,
            Func<T, bool> funcContains = null,
            Func<T, Task<bool>> funcContainsAsync = null,
            Action<int> actionOnReset = null,
            Func<int, int, int, T> funcGetPlaceHolder = null,
            Action actionOnBeforeReset = null
            )
            : base(funcGetItemsAt, funcGetCount, null, funcContains, actionOnReset)
            //: base(funcGetItemsAt, funcGetCount, funcIndexOf, actionOnReset)
        {
            this.FuncGetPlaceHolder = funcGetPlaceHolder;
            this.ActionOnBeforeReset = actionOnBeforeReset;
          this.FuncIndexOfAsync = funcIndexOfAsync;
            this.FuncContainsAsync = funcContainsAsync;
        }

        public virtual void OnBeforeReset()
        {
            if (this.ActionOnBeforeReset != null)
            {
                this.ActionOnBeforeReset.Invoke();
            }
        }

        public Action ActionOnBeforeReset { get; set; } = null;

        public Func<int, int, int, T> FuncGetPlaceHolder { get; set; } = null;

        public Func<T, Task<int>> FuncIndexOfAsync { get; set; } = null;
        public Func<T, Task<bool>> FuncContainsAsync { get; set; }

        public Task<PagedSourceItemsPacket<T>> GetItemsAtAsync(int pageoffset, int count, bool usePlaceholder)
        {
            var tcs = new TaskCompletionSource<PagedSourceItemsPacket<T>>();

            try
            {
                tcs.SetResult(this.GetItemsAt(pageoffset, count, usePlaceholder));
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }

            return tcs.Task;
        }

        public virtual T GetPlaceHolder(int index, int page, int offset)
        {
            var ret = default(T);

            if (this.FuncGetPlaceHolder != null)
                ret = this.FuncGetPlaceHolder.Invoke(index, page, offset);

            return ret;
        }

        public Task<int> GetCountAsync()
        {
            var tcs = new TaskCompletionSource<int>();

            try
            {
                tcs.SetResult(this.Count);
            } catch(Exception e)
            {
                tcs.SetException(e);
            }

            return tcs.Task;
        }

      public Task<int> IndexOfAsync( T item )
      {
            return this.FuncIndexOfAsync?.Invoke(item) ?? default(Task<int>);
      }

        public Task<bool> ContainsAsync(T item)
        {
            return this.FuncContainsAsync?.Invoke(item) ?? default(Task<bool>);
        }
    }
}
