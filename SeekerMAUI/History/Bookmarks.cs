using SeekerMAUI.Game;

namespace SeekerMAUI.History
{
    class Bookmarks
    {
        private static List<string> BookmarkList(string bookmarksName)
        {
            if (!Preferences.Default.ContainsKey(bookmarksName))
                return new List<string>();
            else
                return Preferences.Default.Get(bookmarksName, string.Empty).Split(',').ToList();
        }

        public static Dictionary<string, string> List(out string bookmarksName)
        {
            bookmarksName = $"{Data.CurrentGamebook}-BOOKMARKS";
            Dictionary<string, string> bookmarks = new Dictionary<string, string>();

            foreach (string bookmark in BookmarkList(bookmarksName))
                bookmarks[bookmark.Split(':')[1]] = bookmark.Split(':')[0];

            return bookmarks;
        }

        public static void Save(string bookmark)
        {
            Dictionary<string, string> bookmarks = List(out string bookmarksName);
            Services.BookmarkName(bookmarks, bookmark, out string bookmarkOut, out string saveName);

            if (bookmarks.Count == 0)
                Preferences.Default.Set(bookmarksName, $"{saveName}:{bookmarkOut}");
            else
                Preferences.Default.Set(bookmarksName, Preferences.Default.Get(bookmarksName, string.Empty) + $",{saveName}:{bookmarkOut}");

            Continue.Save($"{Data.CurrentGamebook}-{saveName}");
        }

        public static void Remove(string bookmark)
        {
            Dictionary<string, string> bookmarks = List(out string bookmarksName);
            string bookmarkIndex = bookmark.Split('-')[1];
            string newBookmarkList = string.Empty;

            foreach (string index in bookmarks.Keys)
            {
                if (bookmarks[index] == bookmarkIndex)
                {
                    Preferences.Default.Remove(bookmark);
                }
                else
                {
                    if (!string.IsNullOrEmpty(newBookmarkList))
                        newBookmarkList += ",";

                    newBookmarkList += $"{bookmarks[index]}:{index}";
                }
            }

            if (string.IsNullOrEmpty(newBookmarkList))
                Preferences.Default.Remove(bookmarksName);
            else
                Preferences.Default.Set(bookmarksName, newBookmarkList);
        }

        public static void Clean()
        {
            foreach (string gamebook in Gamebook.List.GetBooks())
            {
                string bookmarksName = $"{gamebook}-BOOKMARKS";

                foreach (string bookmark in BookmarkList(bookmarksName))
                    Preferences.Default.Remove($"{gamebook}-{bookmark.Split(':')[0]}");

                if (Preferences.Default.ContainsKey(bookmarksName))
                    Preferences.Default.Remove(bookmarksName);
            }
        }
    }
}
