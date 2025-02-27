using System;

namespace SeekerMAUI.Gamebook.MurderAtColefaxManor
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public override List<string> Status()
        {
            var progress = new List<string>();

            for (int i = 0; i < 8; i++)
            {
                var isTriggered = Game.Option.IsTriggered($"Ячейка {i + 1}");
                progress.Add(isTriggered ? Constants.Active[i] : Constants.Passive[i]);
            }

            return new List<string> { $"Прогресс:   {String.Join("   ", progress)}" };
        }

        public override bool Availability(string option)
        {
            if (String.IsNullOrEmpty(option))
            {
                return true;
            }
            else
            {
                foreach (string oneOption in option.Split(','))
                {
                    if (oneOption.Contains("!"))
                    {
                        if (Game.Option.IsTriggered(oneOption.Replace("!", String.Empty).Trim()))
                            return false;
                    }
                    else if (!Game.Option.IsTriggered(oneOption.Trim()))
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
