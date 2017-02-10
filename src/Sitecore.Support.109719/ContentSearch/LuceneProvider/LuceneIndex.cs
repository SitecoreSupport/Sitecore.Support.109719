﻿namespace Sitecore.Support.ContentSearch.LuceneProvider
{
    using System.Linq;
    using System.Threading;
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.Maintenance;

    public class LuceneIndex : Sitecore.ContentSearch.LuceneProvider.LuceneIndex
    { 
        protected override void PerformRefresh(IIndexable indexableStartingPoint, IndexingOptions indexingOptions, CancellationToken cancellationToken)
        {
            this.VerifyNotDisposed();

            if (!this.ShouldStartIndexing(indexingOptions))
                return;

            lock (this.indexUpdateLock)
            {
                if (!this.Crawlers.Any(c => c.HasItemsToIndex()))
                    return;

                using (var context = this.CreateUpdateContext())
                {
                    foreach (var crawler in this.Crawlers)
                    {
                        crawler.RefreshFromRoot(context, indexableStartingPoint, indexingOptions, cancellationToken);
                    }

                    context.Commit();
                }
            }
        }

        public LuceneIndex(string name, string folder, IIndexPropertyStore propertyStore, string @group) : base(name, folder, propertyStore, @group)
        {
        }

        public LuceneIndex(string name, string folder, IIndexPropertyStore propertyStore) : base(name, folder, propertyStore)
        {
        }

        public LuceneIndex(string name) : base(name)
        {
        }
    }
}
