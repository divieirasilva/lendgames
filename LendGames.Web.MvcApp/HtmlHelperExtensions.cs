using LendGames.Utils.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LendGames.Web.MvcApp
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, PagingData pagingData, Func<int, string> pageUrl)
        {
            return PageLinks(html, pagingData, pageUrl, click => string.Empty);
        }

        public static MvcHtmlString PageLinks(this HtmlHelper html, PagingData pagingData, Func<int, string> pageUrl, Func<int, string> onClick)
        {
            StringBuilder result = new StringBuilder();

            string prevLink = (pagingData.CurrentPage == 1) ? BuildHtmlItem(pageUrl(pagingData.CurrentPage - 1), "«", onClick(pagingData.CurrentPage - 1), false, true) : BuildHtmlItem(pageUrl(pagingData.CurrentPage - 1), "«", onClick(pagingData.CurrentPage - 1));
            result.Append(prevLink);

            var start = (pagingData.CurrentPage <= pagingData.PagesPerGroup + 1) ? 1 : (pagingData.CurrentPage - pagingData.PagesPerGroup);
            var end = (pagingData.CurrentPage > (pagingData.TotalPages - pagingData.PagesPerGroup)) ? pagingData.TotalPages : pagingData.CurrentPage + pagingData.PagesPerGroup;

            for (int i = start; i <= end; i++)
            {
                string pageHtml = string.Empty;
                pageHtml = (i == pagingData.CurrentPage) ? BuildHtmlItem(pageUrl(i), i.ToString(), onClick(i), true) : BuildHtmlItem(pageUrl(i), i.ToString(), onClick(i));

                result.Append(pageHtml);
            }

            string nextLink = (pagingData.CurrentPage == pagingData.TotalPages)
                ? BuildHtmlItem(pageUrl(pagingData.CurrentPage + 1), "»", onClick(pagingData.CurrentPage + 1), false, true)
                : BuildHtmlItem(pageUrl(pagingData.CurrentPage + 1), "»", onClick(pagingData.CurrentPage + 1));
            result.Append(nextLink);

            return MvcHtmlString.Create(result.ToString());
        }

        private static string BuildHtmlItem(string url, string display, string onClick, bool active = false, bool disabled = false)
        {
            TagBuilder liTag = new TagBuilder("li");

            if (disabled)
            {
                liTag.MergeAttribute("class", "disabled");
                var spanTag = new TagBuilder("span");
                spanTag.InnerHtml = display;
                liTag.InnerHtml = spanTag.ToString();
            }
            else
            {
                if (active)
                {
                    liTag.MergeAttribute("class", "active");
                }

                TagBuilder tag = new TagBuilder("a");
                
                if (!string.IsNullOrEmpty(onClick))
                {
                    tag.MergeAttribute("onclick", onClick);
                    tag.MergeAttribute("href", "#");
                }
                else                
                    tag.MergeAttribute("href", url);
                
                tag.InnerHtml = display;
                liTag.InnerHtml = tag.ToString();
            }
            return liTag.ToString();
        }
    }
}