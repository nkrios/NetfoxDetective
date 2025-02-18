﻿using System.Threading.Tasks;

namespace AlphaChiTech.Virtualization.Interfaces
{
    public interface IItemSourceProviderAsync<T> : IBaseSourceProvider<T>
    {
        Task<T> GetAt(int index, object voc, bool usePlaceholder);

        T GetPlaceHolder(int index);

        Task<int> Count { get; }

        Task<int> IndexOf(T item);
    }
}
