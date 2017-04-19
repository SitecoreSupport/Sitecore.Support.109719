using System.Threading;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Maintenance;

namespace Sitecore.Support.ContentSearch.SolrProvider
{
    public class SwitchOnRebuildSolrSearchIndex : Sitecore.ContentSearch.SolrProvider.SwitchOnRebuildSolrSearchIndex
    {
        public SwitchOnRebuildSolrSearchIndex(string name, string core, string rebuildcore, IIndexPropertyStore propertyStore) : base(name, core, rebuildcore, propertyStore)
        {
        }

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
    }
}