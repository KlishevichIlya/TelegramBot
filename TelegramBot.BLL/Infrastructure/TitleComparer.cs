using System.Collections.Generic;
using TelegramBot.BLL.DTO;

namespace TelegramBot.BLL.Infrastructure
{
    public class TitleComparer : IEqualityComparer<NewsDTO>
    {
        public int GetHashCode(NewsDTO news) =>
            news == null ? 0 : news.Title.GetHashCode();

        public bool Equals(NewsDTO n1, NewsDTO n2)
        {
            if (object.ReferenceEquals(n1, n2))
            {
                return true;
            }
            if (object.ReferenceEquals(n1, null) ||
                object.ReferenceEquals(n2, null))
            {
                return false;
            }
            return n1.Title == n2.Title;
        }
    }
}
