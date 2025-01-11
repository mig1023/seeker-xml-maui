using System;
using System.Collections.Generic;
using SeekerMAUI.Gamebook;
using System.Linq;
using System.Text.RegularExpressions;
using static SeekerMAUI.Game.Data;

namespace SeekerMAUI.Output
{
    class Interface
    {
        public enum TextFontSize
        {
            Micro,
            Small,
            Little,
            Normal,
            Big,
            Nope
        };

        public static Image GamebookImage(Description gamebookDescr) => new Image
        {
            StyleId = "cover" + gamebookDescr.Illustration, 
            Source = "Cover/" + gamebookDescr.Illustration,
            Aspect = Aspect.AspectFill,
        };

        public static Image IllustrationImage(string image) => new Image
        {
            Source = "Images/" + image,
            Aspect = Aspect.AspectFit,
        };

        public static Entry Field(object binding, EventHandler<TextChangedEventArgs> changed)
        {
            Entry field = new Entry
            {
                Placeholder = "Введите свой ответ",
                BindingContext = binding,
                FontFamily = Interface.TextFontFamily(),
            };

            field.TextChanged += changed;

            return field;
        }

        public static void Footer(ref StackLayout footer, EventHandler settingsHandler)
        {
            StackLayout footerLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                BackgroundColor = Colors.Gainsboro,
                Padding = new Thickness(5),
            };

            Label settings = new Label
            {
                Text = "Изменить настройки",
                Margin = new Thickness(30, 15, 15, 15),
            };

            TapGestureRecognizer settingsClick = new TapGestureRecognizer();
            settingsClick.Tapped += (s, e) => { settingsHandler(s, e); };
            footerLayout.GestureRecognizers.Add(settingsClick);

            footerLayout.Children.Add(settings);

            footer.Children.Add(footerLayout);
        }

        public static StackLayout SystemMenu() => new StackLayout()
        {
            Orientation = StackOrientation.Horizontal,
            Spacing = Constants.SYS_MENU_SPACING,
            HeightRequest = Constants.SYS_MENU_HIGHT,
            HorizontalOptions = LayoutOptions.FillAndExpand,
        };

        public static List<View> Represent(List<string> enemiesLines)
        {
            List<View> enemies = new List<View>();

            foreach (string enemyLine in enemiesLines)
            {
                if (enemyLine.Contains("SPLITTER|"))
                {
                    string[] param = enemyLine.Split('|');

                    Label splitter = new Label
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalTextAlignment = TextAlignment.Center,
                        Text = param[1],
                    };

                    enemies.Add(Splitter.Line(new Thickness(0, 10, 0, -2), null));
                    enemies.Add(splitter);
                    enemies.Add(Splitter.Line(new Thickness(0, -2, 0, 10), null));
                }
                else
                {
                    string[] enemyParam = enemyLine.Split('\n');

                    int index = 0;

                    foreach(string line in enemyParam)
                    {
                        Label enemy = new Label { HorizontalTextAlignment = TextAlignment.Center };

                        if (index > 0)
                        {
                            enemy.Text = line;
                            enemy.Margin = new Thickness(0, Constants.REPRESENT_PADDING, 0, 0);
                            enemy.FontFamily = TextFontFamily();
                            enemy.FontSize = FontSize(TextFontSize.Small);
                        }
                        else
                        {
                            enemy.Text = line.ToUpper();
                            enemy.FontFamily = TextFontFamily(bold: true);
                            enemy.FontSize = FontSize(TextFontSize.Normal);
                        }

                        enemies.Add(enemy);

                        index += 1;
                    }
                }
            }

            return enemies;
        }

        public static string TextFontFamily(bool bold = false, bool italic = false, bool standart = false)
        {
            string font = String.Empty;
            int fontSetting = Game.Settings.GetValue("FontType");
            string addLine = String.Empty;
            
            if (italic)
            {
                addLine = "Italic";
            }
            else if (bold)
            {
                addLine = "Bold";
            }

            if (fontSetting > 0)
            {
                font = $"{Constants.FONT_TYPE_VALUES[fontSetting]}{addLine}";
            }
            else if (standart || (Game.Data.Constants == null) || String.IsNullOrEmpty(Game.Data.Constants.GetFont()))
            {
                font = $"YanoneFont{addLine}";
            }
            else
            {
                font = $"{Game.Data.Constants.GetFont()}{addLine}";
            }

            return font;
        }

        public static double FontSize(TextFontSize size)
        {
            if (Constants.FontSizeItalic.ContainsKey(size))
            {
                return Constants.FontSize[size];
            }
            else
            {
                return Font(NamedSize.Medium);
            }
        }

        public static double Font(NamedSize namedSize)
        {
            if (namedSize == NamedSize.Default)
            {
                bool robotoFont = (Game.Settings.GetValue("FontType") == 1);
                NamedSize size = (robotoFont ? NamedSize.Medium : NamedSize.Large);
                return Device.GetNamedSize(size, typeof(Label));
            }
            else
            {
                return Device.GetNamedSize(namedSize, typeof(Label));
            }
        }

        private static string RedStyle(string line) =>
            Game.Settings.IsEnabled("RedStyle") ? line.Replace("\\n\\n", "\\n\\t\\t\\t\\t") : line;

        public static View TextBySelect(Text text)
        {
            if (text.Selected)
            {
                return SelectedText(text);
            }
            else if (text.Box)
            {
                return BoxedText(text);
            }
            else
            {
                return Text(text);
            }
        }

        public static Label Text(Text text)
        {
            bool selected = text.Selected || text.Box;

            Label label = Text(RedStyle(text.Content.Trim()), italic: text.Italic,
                upper: text.Upper, size: text.Size, selected: selected);

            label.FontFamily = TextFontFamily(bold: text.Bold, italic: text.Italic);

            if (text.Alignment == "Center")
            {
                label.HorizontalTextAlignment = TextAlignment.Center;

                if (text.Bold)
                    label.Margin = new Thickness(Constants.TEXT_LABEL_MARGIN, Constants.TITLE_TXT_LABEL_MARGIN);
            }
            else if (text.Alignment == "Right")
            {
                label.HorizontalTextAlignment = TextAlignment.End;
            }

            return label;
        }

        private static View BoxedText(Text text)
        {
            string backgroundColor = Game.Data.Constants.GetColor(ColorTypes.Background);

            if (!String.IsNullOrEmpty(text.Background))
            {
                backgroundColor = text.Background;
            }
            else if (String.IsNullOrEmpty(backgroundColor))
            {
                backgroundColor = Constants.DEFAULT_COLORS[ColorTypes.BookColor];
            }

            StackLayout content = new StackLayout
            {
                BackgroundColor = Color.FromHex(backgroundColor),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(Constants.BOX_PADDING),
                Children = { Text(text) },
            };

            string borderColor = Game.Data.Constants.GetColor(ColorTypes.Font);

            if (String.IsNullOrEmpty(borderColor))
                borderColor = Constants.DEFAULT_COLORS[ColorTypes.Font];

            StackLayout border = new StackLayout
            {
                BackgroundColor = Color.FromHex(borderColor),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(Constants.BOX_BORDER),
                Children = { content },
            };

            return border;
        }

        private static View SelectedText(Text text)
        {
            Grid grid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition()
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(2) },
                    new ColumnDefinition { Width = new GridLength(2) },
                    new ColumnDefinition { }
                },
                Margin = new Thickness(5, 0, 0, 0),
            };

            BoxView verticalLine = new BoxView
            {
                Color = Color.FromHex(Game.Data.Constants.GetColor(ColorTypes.StatusBar)),
            };

            grid.Add(verticalLine, 0, 0);
            grid.Add(Text(text), 2, 0);

            return grid;
        }

        public static Label Text(string text, bool defaultParams = false,
            bool italic = false, bool bold = false, bool upper = false,
            TextFontSize size = TextFontSize.Nope, bool selected = false)
        {
            Label label = new Label
            {
                FontSize = FontSize(defaultParams ? TextFontSize.Normal : TextFontSize.Little),
                Text = Regex.Unescape(RedStyle(text)),
                FontFamily = TextFontFamily(bold: bold),
                LineHeight = Constants.LINE_HEIGHT,
            };

            if (bold)
            {
                label.FontAttributes = FontAttributes.Bold;
            }

            if (upper)
            {
                label.Margin = new Thickness(0, -5, 0, 10);
            }
            else if (!selected)
            {
                label.Margin = Constants.TEXT_LABEL_MARGIN;
            }

            if (selected)
            {
                label.VerticalTextAlignment = TextAlignment.Center;
            }

            if (defaultParams)
            {
                return label;
            }

            int fontSize = Game.Settings.GetValue("FontSize");

            if (fontSize > 0)
            {
                label.FontSize = Constants.FONT_SIZE_VALUES[fontSize];
            }
            else if (size != TextFontSize.Nope)
            {
                label.FontSize = FontSize(size);
            }
            else if (Game.Data.Constants != null)
            {
                string constantFontLine = Game.Data.Constants.GetString("FontSize");
                TextFontSize labelFontSize = TextFontSize.Normal;

                if (Enum.TryParse(constantFontLine, out TextFontSize constantFontSize))
                    labelFontSize = constantFontSize;

                label.FontSize = FontSize(labelFontSize);
            }

            bool coloredText = ColorFormConstants(ColorTypes.Font, out string color);
            label.TextColor = coloredText ? Color.FromHex(color) : Color.FromHex(Constants.COLOR_TEXT);

            return label;
        }

        public static StackLayout ActionPlace()
        {

            StackLayout stackLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Spacing = Constants.ACTIONPLACE_SPACING,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = Constants.ACTIONPLACE_PADDING,
                BackgroundColor = Colors.LightGray,
                Margin = new Thickness(0, 5, 0, 5),
            };

            if (ColorFormConstants(ColorTypes.ActionBox, out string color))
                stackLayout.BackgroundColor = Color.FromHex(color);

            return stackLayout;
        }

        public static bool ColorFormConstants(ColorTypes colorType, out string color)
        {
            color = String.Empty;

            if (Game.Data.Constants == null)
                return false;

            color = Game.Data.Constants.GetColor(colorType);

            return !String.IsNullOrEmpty(color);
        }

        public static StackLayout MultipleButtonsPlace() => new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            Spacing = Constants.ACTIONPLACE_SPACING,
            HorizontalOptions = LayoutOptions.FillAndExpand,
        };

        private static Color GetGoodColors(ColorTypes color, Color defaultColor)
        {
            string hexColor = Game.Data.Constants.GetColor(color);
            return String.IsNullOrEmpty(hexColor) ? defaultColor : Color.FromHex(hexColor);
        }

        public static List<View> Actions(List<string> actionsLines)
        {
            List<View> actionLabels = new List<View>();

            foreach (string actionLine in actionsLines)
            {
                Label actions = new Label();

                string text = actionLine;
                bool bold = false;

                Dictionary<string, Color?> textTypes = new Dictionary<string, Color?>
                {
                    ["RED|"] = Colors.Red,
                    ["BLUE|"] = Colors.Blue,
                    ["YELLOW|"] = Colors.Yellow,
                    ["GREEN|"] = Colors.Green,
                    ["GRAY|"] = Colors.Gray,
                    ["BAD|"] = GetGoodColors(ColorTypes.BadColor, Colors.Red),
                    ["GOOD|"] = GetGoodColors(ColorTypes.GoodColor, Colors.Green),
                    ["BIG|"] = null,
                    ["BOLD|"] = null,
                    ["HEAD|"] = null,
                    ["LINE|"] = null,
                };

                foreach (string color in textTypes.Keys.Where(x => text.Contains(x)))
                    actions.TextColor = textTypes[color] ?? actions.TextColor;

                actions.FontSize = FontSize(text.Contains("BIG|") ? TextFontSize.Normal : TextFontSize.Little);

                if (text.Contains("BOLD|"))
                    bold = true;

                if (text.Contains("HEAD|"))
                {
                    actions.HorizontalTextAlignment = TextAlignment.Center;
                    actions.FontAttributes = FontAttributes.Bold;
                }
                else
                {
                    actions.HorizontalTextAlignment = TextAlignment.Start;
                }

                if (text.Contains("LINE|"))
                    actionLabels.Add(Splitter.Line(null, null));

                foreach (string key in textTypes.Keys)
                    text = text.Replace(key, String.Empty);

                actions.Text = text;
                actions.FontFamily = TextFontFamily(bold: bold);

                actionLabels.Add(actions);
            }

            return actionLabels;
        }

        public static Entry BookmarkField()
        {
            Entry field = new Entry
            {
                Placeholder = Constants.BOOKMARK_SAVE_HOLDER,
                FontFamily = TextFontFamily(),
                FontSize = FontSize(TextFontSize.Big),
                BackgroundColor = Colors.Gainsboro,
            };

            if (!String.IsNullOrEmpty(Game.Data.Constants.GetColor(ColorTypes.Font)))
            {
                field.TextColor = Color.FromHex(Game.Data.Constants.GetColor(ColorTypes.Font));
            }

            return field;
        }
    }
}
