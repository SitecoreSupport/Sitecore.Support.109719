using System.Threading;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Maintenance;

namespace Sitecore.Support.ContentSearch.SolrProvider
{
    public class SolrSearchIndex : Sitecore.ContentSearch.SolrProvider.SolrSearchIndex
    {
        protected override void PerformRefresh(IIndexable indexableStartingPoint, IndexingOptions indexingOptions, CancellationToken cancellationToken)
        {
            if (base.ShouldStartIndexing(indexingOptions))
            {
                using (IProviderUpdateContext context = this.CreateUpdateContext())
                {
                    foreach (IProviderCrawler crawler in base.Crawlers)
                    {
                        crawler.RefreshFromRoot(context, indexableStartingPoint, indexingOptions, cancellationToken);
                    }
                    context.Commit();
                }
                //this.OptimizeIndex();
            }
        }

        public SolrSearchIndex(string name, string core, IIndexPropertyStore propertyStore, string @group) : base(name, core, propertyStore, @group)
        {
        }

        public SolrSearchIndex(string name, string core, IIndexPropertyStore propertyStore) : base(name, core, propertyStore)
        {
        }
    }
}