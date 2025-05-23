﻿using static SeekerMAUI.Output.Interface;
using static SeekerMAUI.Output.Buttons;
using static SeekerMAUI.Game.Data;

namespace SeekerMAUI.Output
{
    class Constants
    {
        public static bool DEBUG_FLAG = true;

        public static string START_TEXT = "В путь!";
        public static string GAMEOVER_TEXT = "Начать сначала";
        public static string BACK_LINK = "НАЗАД";
        public static string BOOKMARK_SAVE = "СДЕЛАТЬ ЗАКЛАДКУ";
        public static string BOOKMARK_SAVE_HEAD = "Сделать новую закладку:";
        public static string BOOKMARK_SAVE_HOLDER = "Название закладки";
        public static string BOOKMARK_LOAD_HEAD = "Вернуться к закладке:";
        public static string BOOKMARK_REMOVE = "X";
        public static string DISCLAIMER_LINK = " ➝ подробнее";
        public static string DISCLAIMER_LINK_OPENED = " ⤵";
        public static string COLOR_WHITE = "#ffffff";
        public static string COLOR_BLACK = "#000000";
        public static string COLOR_TEXT= "#757575";

        public static Color BACKGROUND = Color.FromHex("#f7f7f7");
        public static Color LINK_COLOR_DEFAULT = Colors.Black;
        public static Color PICKER_BACKGROUND = Color.FromHex("#ededed");

        public static double BIG_FONT = 25;
        public static double STATUSBAR_FONT = 12;
        public static double LINE_HEIGHT = 1.20;
        public static double ACTIONPLACE_SPACING = 5;
        public static double ACTIONPLACE_PADDING = 20;
        public static double TEXT_LABEL_MARGIN = 5;
        public static double TITLE_TXT_LABEL_MARGIN = 15;
        public static double BORDER_WIDTH = 1;
        public static double REPRESENT_PADDING = -10;
        public static double DISCLAIMER_BORDER = 8;
        public static double SYS_MENU_SPACING = 4;
        public static double SYS_MENU_HIGHT = 25;
        public static double BOX_BORDER = 1;
        public static double BOX_PADDING = 10;
        public static double DEBUG_GRIDROW_HEIGHT = 14;

        public static float HORIZONTAL_HEIGHT = 30;

        public static float VERTICAL_YPOS_TEXT = -5;
        public static float VERTICAL_YPOS_LINE = -9;
        public static float VERTICAL_LINE_LEN = 3;
        public static float VERTICAL_LINE_BONUS = 3;
        public static float VERTICAL_FONT = 12;

        public enum SortBy 
        {
            Title = 1,
            Author,
            Size,
            Paragraphs,
            Texts,
            Year,
            Setting,
            Time,
            Random,
        };

        public static Dictionary<TextFontSize, double> FontSize = new Dictionary<TextFontSize, double>
        {
            [TextFontSize.Micro] = Font(NamedSize.Micro),
            [TextFontSize.Small] = Font(NamedSize.Small),
            [TextFontSize.Little] = Font(NamedSize.Medium),
            [TextFontSize.Normal] = Font(NamedSize.Large),
            [TextFontSize.Nope] = Font(NamedSize.Large),
            [TextFontSize.Big] = BIG_FONT,
        };

        public static Dictionary<TextFontSize, double> FontSizeItalic = new Dictionary<TextFontSize, double>
        {
            [TextFontSize.Micro] = Font(NamedSize.Micro),
            [TextFontSize.Small] = Font(NamedSize.Micro),
            [TextFontSize.Little] = Font(NamedSize.Small),
            [TextFontSize.Normal] = Font(NamedSize.Small),
            [TextFontSize.Nope] = Font(NamedSize.Small),
            [TextFontSize.Big] = Font(NamedSize.Large),
        };

        public static Dictionary<int, string> FONT_TYPE_VALUES = new Dictionary<int, string>
        {
            [0] = "YanoneFont",
            [1] = "RobotoFont",
            [2] = "Daray",
            [3] = "St.Sign",
        };

        public static Dictionary<int, double> FONT_SIZE_VALUES = new Dictionary<int, double>
        {
            [1] = Interface.Font(NamedSize.Small),
            [2] = Interface.Font(NamedSize.Medium),
            [3] = Interface.Font(NamedSize.Large),
            [4] = Constants.BIG_FONT,
        };

        public static Dictionary<string, int> PLAYTHROUGH_ORDER = new Dictionary<string, int>
        {
            ["На пять минут"] = 0,
            ["На полчаса"] = 1,
            ["На часок-полтора"] = 2,
            ["На пару часов"] = 3,
            ["На весь вечер"] = 4,
            ["На пару дней"] = 5,
        };

        public static Dictionary<ButtonTypes, string> DEFAULT_BUTTONS = new Dictionary<ButtonTypes, string>
        {
            [ButtonTypes.Main] = "#dcdcdc",
            [ButtonTypes.Action] = "#9d9d9d",
            [ButtonTypes.Option] = "#f1f1f1",
            [ButtonTypes.ButtonFont] = "#000000",
            [ButtonTypes.Continue] = "#f1f1f1",
            [ButtonTypes.System] = "#f1f1f1",
        };

        public static Dictionary<ColorTypes, string> DEFAULT_COLORS = new Dictionary<ColorTypes, string>
        {
            [ColorTypes.ActionBox] = "#d7d7d7",
            [ColorTypes.StatusBar] = "#5e5e5e",
            [ColorTypes.StatusFont] = "#ffffff",
            [ColorTypes.Font] = "#000000",
            [ColorTypes.BookColor] = "#ffffff",
            [ColorTypes.BookFontColor] = "#000000",
            [ColorTypes.BookBorderColor] = "#000000",
        };
    }
}
