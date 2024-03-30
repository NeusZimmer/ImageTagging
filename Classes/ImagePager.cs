using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageTagging.Classes
{

    class ImagePager
    {
        private class PagerImageInfo
        {
            public int image_sessionID;
            public int page=0;
            public bool active = true;
            public PagerImageInfo(int _sessionID,int _page)
            {
            image_sessionID=_sessionID;
            page = _page;
            active = true;
            }   
        }
        public int Page { get { return current_page; } }
        private int _pager_limit = 24;
        private int current_page = 0;
        private int last_page = 0;
        private List<PagerImageInfo> PagerInfo= new List<PagerImageInfo>();
        public int pager_limit 
            {
                get 
                { return _pager_limit; }
                set
                { _pager_limit = value; } 
            }

        public ImagePager(List<ImageData> Images)
        {
            int resto = Images.Count % _pager_limit;
            last_page = ((Images.Count - resto) / _pager_limit);

            for (int counter = 0; counter < Images.Count; counter++)
            {
                ImageData imagen = Images[counter];
                add_image_to_list(imagen, counter);
            }
        }
        /// <summary>
        /// Makes a new pager and forgets about all the hidden images
        /// </summary>
        public void repagination()
        {
            List<PagerImageInfo> temp_pager = new List<PagerImageInfo>();
            foreach (PagerImageInfo PagerData in PagerInfo)
                if (PagerData.active)
                    temp_pager.Add(PagerData);

            int resto = temp_pager.Count % _pager_limit;
            this.last_page = (temp_pager.Count() - resto) / _pager_limit;

            PagerInfo.Clear();
            for (int counter = 0; counter < temp_pager.Count; counter++)
            {
                PagerImageInfo imagen_info = temp_pager[counter];
                add_image_to_list(imagen_info, counter);
            }
            this.current_page = 0;

        }

        public void hide_image(int image_session_id)
        {
            PagerImageInfo image_info= this.PagerInfo.Find(x => x.image_sessionID == image_session_id);
            image_info.active = false;
        }

        private void add_image_to_list(PagerImageInfo imagen_info, int counter)
        {
            int resto = counter % _pager_limit;
            int page = (counter - resto) / _pager_limit;
            int sessionid = imagen_info.image_sessionID;
            PagerImageInfo temp_info = new PagerImageInfo(sessionid, page);
            PagerInfo.Add(temp_info);
        }

        private void add_image_to_list(ImageData imagen, int counter)
        {
            int resto = counter % _pager_limit;
            int page = (counter - resto) / _pager_limit;
            int sessionid = imagen.ImageSessionID;
            PagerImageInfo temp_info = new PagerImageInfo(sessionid,page);
            PagerInfo.Add(temp_info);
        }

        public int next_page()
        {
            if (this.current_page < this.last_page) this.current_page++;
            return current_page;
        }
        public int previous_page()
        {
            if (this.current_page > 0) this.current_page--;
            return current_page;
        }

        public List<int> get_all_inactive_ids()
        {
            List<int> all_inactive_ids = new List<int>();
            for (int counter = 0; counter <= this.last_page; counter++)
            {
                List<int> temp_ids_list = get_inactive_ids_for_page(counter);
                all_inactive_ids =all_inactive_ids.Concat(temp_ids_list).ToList();
            }
            return all_inactive_ids;
        }
        public List<int> get_all_active_ids()
        {
            List<int> all_active_ids = new List<int>();
            for (int counter = 0; counter <= this.last_page; counter++)
            {
                List<int> temp_ids_list = get_active_ids_for_page(counter);
                all_active_ids = all_active_ids.Concat(temp_ids_list).ToList();
            }
            return all_active_ids;
        }
        public List<int> get_inactive_ids_for_page()
        {
            return this.get_ids_for_page(this.current_page, true, false);
        }
        public List<int> get_inactive_ids_for_page(int Page_number)
        {
            return this.get_ids_for_page(Page_number, true, false);
        }
        public List<int> get_active_ids_for_page()
        {
            return this.get_ids_for_page(this.current_page, true, true);
        }
        public List<int> get_active_ids_for_page(int Page_number)
        {
            return this.get_ids_for_page(Page_number,true, true);
        }
        public List<int> get_all_ids_for_page()
        {
            return this.get_ids_for_page(this.current_page, false, true);
        }
        public List<int> get_all_ids_for_page(int Page_number)
        {
            return this.get_ids_for_page(Page_number,false,true);
        }
        private List<int> get_ids_for_page(int Page_number, bool active_filter, bool active)
        {
            List<int> list_ids = new List<int>();
            List<PagerImageInfo> list_images;
            if (active_filter) {
                list_images = this.PagerInfo.Where(result =>
                (result.page == Page_number) && (result.active ==active)
                ).ToList();
            }
            else {
                list_images = this.PagerInfo.Where(result =>
                (result.page == Page_number)
                ).ToList();
            }

            foreach (PagerImageInfo elemento in list_images)
                list_ids.Add(elemento.image_sessionID);
            return list_ids;
        }
    }
}
