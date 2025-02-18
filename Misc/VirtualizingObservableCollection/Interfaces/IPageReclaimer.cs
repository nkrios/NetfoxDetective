﻿using System.Collections.Generic;

namespace AlphaChiTech.Virtualization.Interfaces
{
    public interface IPageReclaimer<T>
    {
        /// <summary>
        /// Reclaims the pages.
        /// </summary>
        /// <param name="pages">The pages.</param>
        /// <param name="pagesNeeded">The pages needed.</param>
        /// <returns></returns>
        IEnumerable<ISourcePage<T>> ReclaimPages(IEnumerable<ISourcePage<T>> pages, int pagesNeeded, string sectionContext);

        /// <summary>
        /// Called when [page touched].
        /// </summary>
        /// <param name="page">The page.</param>
        void OnPageTouched(ISourcePage<T> page);

        /// <summary>
        /// Called when [page released].
        /// </summary>
        /// <param name="page">The page.</param>
        void OnPageReleased(ISourcePage<T> page);

        /// <summary>
        /// Makes the page.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        ISourcePage<T> MakePage(int page, int size);
    }
}
