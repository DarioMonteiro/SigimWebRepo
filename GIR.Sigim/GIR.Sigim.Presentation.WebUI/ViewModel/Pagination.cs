using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GIR.Sigim.Presentation.WebUI.ViewModel
{
    public class Pagination
    {
        private int pageIndex;
        private int pageSize;
        private int totalPages;
        private int listSize;
        private int totalRecords;
        private List<Page> pageList;

        public int FirstRecord
        {
            get
            {
                if (pageIndex > 0)
                    return this.pageSize * pageIndex + 1;

                return 1;
            }
        }

        public int LastRecord
        {
            get
            {
                var value = this.pageSize;
                if (pageIndex > 0)
                    value = this.pageSize * pageIndex + this.pageSize;

                return value < this.totalRecords ? value : this.totalRecords;
            }
        }

        public int TotalRecords
        {
            get { return this.totalRecords; }
        }

        public List<Page> PageList
        {
            get { return pageList; }
        }

        public Pagination(int pageIndex, int pageSize, int totalRecords, int listSize)
        {
            this.pageIndex = pageIndex;
            this.pageSize = pageSize;
            this.totalRecords = totalRecords;
            this.totalPages = CalculateTotalPages(totalRecords, pageSize);
            this.listSize = AdjustListSize(totalPages, listSize);
            PopulatePageList(pageIndex);
        }

        private int CalculateTotalPages(int totalRecords, int pageSize)
        {
            return ((totalRecords + pageSize) - 1) / pageSize;
        }

        private int AdjustListSize(int totalPages, int listSize)
        {
            return totalPages >= listSize ? listSize : totalPages;
        }

        private void PopulatePageList(int pageIndex)
        {
            pageList = new List<Page>();
            int startPage = CalculateStartPage(pageIndex);
            int endPage = CalculateEndPage(pageIndex, ref startPage);

            if (pageIndex > 0)
            {
                pageList.Add(CreatePage("<<", 0, pageIndex));
                pageList.Add(CreatePage("<", pageIndex - 1, pageIndex));
            }

            for (int i = startPage; i <= endPage; i++)
            {
                pageList.Add(CreatePage(i.ToString(), i - 1, pageIndex));
            }

            if (pageIndex < (totalPages - 1))
            {
                pageList.Add(CreatePage(">", pageIndex + 1, pageIndex));
                pageList.Add(CreatePage(">>", totalPages - 1, pageIndex));
            }
        }

        private int CalculateStartPage(int pageIndex)
        {
            var startPage = (pageIndex + 1) - (this.listSize / 2);
            return startPage > 0 ? startPage : 1;
        }

        private int CalculateEndPage(int pageIndex, ref int startPage)
        {
            var endPage = startPage + this.listSize - 1;
            if (endPage > this.totalPages)
            {
                endPage = this.totalPages;
                startPage = this.totalPages - this.listSize + 1;
            }

            return endPage;
        }

        private Page CreatePage(string description, int pageIndex, int currentPageIndex)
        {
            var page = new Page();
            page.Decription = description;
            page.Index = pageIndex;
            page.Active = pageIndex == currentPageIndex;
            return page;
        }
    }
}