using System;
using SeekerMAUI.Gamebook;

namespace SeekerMAUI.Output
{
    class Buttons
    {
        public enum ButtonTypes
        {
            Main,
            Action,
            Option,
            ButtonFont,
            Border,
            Continue,
            System,
        };

        public static Button Action(string actionName, EventHandler onClick, bool enabled = true)
        {
            string color = Game.Data.Constants.GetColor(ButtonTypes.Action);

            Button actionButton = new Button
            {
                Text = actionName.ToUpper(),
                TextColor = Colors.White,
                IsEnabled = enabled,
                BackgroundColor = (enabled ? Color.FromHex(color) : Colors.Gray),
                FontFamily = Interface.TextFontFamily(standart: true),
                FontSize = Interface.Font(NamedSize.Default),
                LineBreakMode = LineBreakMode.WordWrap,
            };

            actionButton.Clicked += onClick;

            return SetBorderAndTextColor(actionButton);
        }

        public static void EmptyOptionTextFuse(Game.Option option)
        {
            if (String.IsNullOrEmpty(option.Text))
                option.Text = (option.Goto == 0 ? "Начать сначала" : "Далее");
        }

        public static Button Option(Game.Option option, EventHandler onClick, bool action)
        {
            bool optionColor = !String.IsNullOrEmpty(option.Availability) &&
                !option.Availability.Contains(">") && !option.Availability.Contains("<");

            int aftertextsCount = option.Aftertexts?.Count ?? 0;

            bool availability = true;

            if (!String.IsNullOrEmpty(option.Availability))
                availability = Game.Data.Actions.Availability(option.Availability);

            if (!Game.Data.Constants.GetBool("HideDisabledOption") || (aftertextsCount > 0))
                optionColor = !availability;
                
            string color = Game.Data.Constants.GetColor(optionColor ?
                Buttons.ButtonTypes.Option : Buttons.ButtonTypes.Main);

            if (!String.IsNullOrEmpty(option.Style))
                color = option.Style;

            bool isEnabled = !(!String.IsNullOrEmpty(option.Availability) && !availability);

            Button optionButton = new Button
            {
                Text = option.Text.ToUpper(),
                IsEnabled = isEnabled,
                FontFamily = Interface.TextFontFamily(standart: true),
                FontSize = Interface.Font(NamedSize.Default),
                IsVisible = String.IsNullOrEmpty(option.Input),
                LineBreakMode = LineBreakMode.WordWrap,
                BindingContext = option.Tag,
            };

            if (action)
                optionButton.Margin = new Thickness(0, 0, 0, 5);

            if (optionButton.IsEnabled)
                optionButton.BackgroundColor = Color.FromHex(color);

            optionButton.Clicked += onClick;

            return SetBorderAndTextColor(optionButton);
        }

        public static Button Additional(string text, EventHandler onClick, bool anywayLarge = false)
        {
            string color = Game.Data.Constants.GetColor(Buttons.ButtonTypes.Continue);

            Button additionButton = new Button
            {
                Text = text.ToUpper(),
                BackgroundColor = (String.IsNullOrEmpty(color) ? Colors .LightGray : Color.FromHex(color)),
                FontFamily = Interface.TextFontFamily(standart: true),
                LineBreakMode = LineBreakMode.WordWrap,
            };

            if (anywayLarge || Game.Settings.IsEnabled("LargeAddButtons"))
            {
                additionButton.FontSize = Interface.Font(NamedSize.Default);
            }
            else
            {
                additionButton.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
                additionButton.HeightRequest = Constants.SYS_MENU_HIGHT;
                additionButton.Padding = 0;
            }

            additionButton.Clicked += onClick;

            return SetBorderAndTextColor(additionButton);
        }

        public static Button System(string text, EventHandler onClick)
        {
            string defaultColor = Game.Data.Constants.GetColor(Buttons.ButtonTypes.Continue);
            string systemColor = Game.Data.Constants.GetColor(Buttons.ButtonTypes.System);
            string color = (String.IsNullOrEmpty(systemColor) ? defaultColor : systemColor);

            Button systemButton = new Button
            {
                Text = text,
                BackgroundColor = (String.IsNullOrEmpty(color) ? Colors.LightGray : Color.FromHex(color)),
                FontFamily = Interface.TextFontFamily(standart: true),
                FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
                Padding = 0,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                LineBreakMode = LineBreakMode.WordWrap,
            };

            systemButton.Clicked += onClick;

            return SetBorderAndTextColor(systemButton, system: true);
        }

        public static Button Gamebook(Description gamebook)
        {
            Button gamebookButton = new Button
            {
                Text = gamebook.Title.ToUpper(),
                BackgroundColor = Color.FromHex(gamebook.BookColor),
                FontFamily = Interface.TextFontFamily(),
                FontSize = Interface.Font(NamedSize.Default),
                LineBreakMode = LineBreakMode.WordWrap,
            };

            //gamebookButton.Clicked += onClick;

            if (!String.IsNullOrEmpty(gamebook.BorderColor))
            {
                gamebookButton.BorderColor = Color.FromHex(gamebook.BorderColor);
                gamebookButton.BorderWidth = Constants.BORDER_WIDTH;
            }

            if (!String.IsNullOrEmpty(gamebook.FontColor))
                gamebookButton.TextColor = Color.FromHex(gamebook.FontColor);
            else
                gamebookButton.TextColor = Colors.White;

            return gamebookButton;
        }

        public static Button ClosePage(EventHandler onClick, bool bookmark = false)
        {
            Color buttonColor = Colors.Gainsboro;

            if (bookmark)
            {
                string color = Game.Data.Constants.GetColor(Buttons.ButtonTypes.Main);
                buttonColor = Color.FromHex(color);
            }

            Button button = new Button
            {
                Text = Constants.BACK_LINK.ToUpper(),
                BackgroundColor = buttonColor,
                FontFamily = Interface.TextFontFamily(),
                FontSize = Interface.Font(NamedSize.Default),
                Margin = new Thickness(0, 30),
                LineBreakMode = LineBreakMode.WordWrap,
            };

            button.Clicked += onClick;

            return button;
        }

        public static Button Bookmark(EventHandler onClick, string text,
            bool bookmark = false, bool topMargin = false, bool bottomMargin = false)
        {
            string color = Game.Data.Constants.GetColor(bookmark ?
                Buttons.ButtonTypes.Option : Buttons.ButtonTypes.Main);

            Button button = new Button
            {
                Text = text,
                BackgroundColor = Color.FromHex(color),
                FontFamily = Interface.TextFontFamily(),
                FontSize = Interface.Font(NamedSize.Default),
                LineBreakMode = LineBreakMode.WordWrap,
            };

            button.Margin = new Thickness(0, topMargin ? 30 : 0, 0, bottomMargin ? 30 : 0);
            button.Clicked += onClick;

            return SetBorderAndTextColor(button);
        }

        public static Button GameOver(string text, EventHandler onClick)
        {
            string colorLine = Game.Data.Constants.GetColor(Buttons.ButtonTypes.Continue);

            Color color = Colors.Gray;

            if (!String.IsNullOrEmpty(colorLine))
                color = Color.FromHex(colorLine);

            Button gameoverButton = new Button
            {
                Text = text.ToUpper(),
                TextColor = Colors.White,
                BackgroundColor = color,
                FontFamily = Interface.TextFontFamily(),
                FontSize = Interface.Font(NamedSize.Default),
                LineBreakMode = LineBreakMode.WordWrap,
            };

            gameoverButton.Clicked += onClick;

            return SetBorderAndTextColor(gameoverButton);
        }

        public static Button SetBorderAndTextColor(Button button, bool system = false)
        {
            if (!system)
            {
                if (!String.IsNullOrEmpty(Game.Data.Constants.GetColor(Buttons.ButtonTypes.Border)))
                {
                    button.BorderColor = Color.FromHex(Game.Data.Constants.GetColor(Buttons.ButtonTypes.Border));
                    button.BorderWidth = Constants.BORDER_WIDTH;
                }
                else
                {
                    button.BorderWidth = 0;
                }
            }
          
            if (system)
            {
                string systemFont = Game.Data.Constants.GetColor(Game.Data.ColorTypes.SystemFont);
                button.TextColor = (String.IsNullOrEmpty(systemFont) ? Colors.Black : Color.FromHex(systemFont));
            }
            else
            {
                string font = Game.Data.Constants.GetColor(Buttons.ButtonTypes.ButtonFont);
                button.TextColor = (String.IsNullOrEmpty(font) ? Colors.White : Color.FromHex(font));
            }

            return button;
        }

        public static void Loading(Button button)
        {
            button.BackgroundColor = Colors.WhiteSmoke;
            button.TextColor = Colors.DarkGray;
            button.BorderColor = Colors.DarkGray;
            button.BorderWidth = Constants.BORDER_WIDTH;
            button.Text = Constants.LOADING;
        }
    }
}
