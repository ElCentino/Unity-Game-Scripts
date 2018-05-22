using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Literals {

    public sealed class Buttons 
    {
        public const string BUTTON_SURVIVAL = "Survival";
        public const string BUTTON_NORMAL = "Normal";
        public const string BUTTON_HEAT = "Heat";
    }

    public sealed class GAME_MODES
    {
        public const string NORNAL_GAME_MODE = "NORMAL_GAME_MODE";
        public const string SURVIVAL_GAME_MODE = "SURVIVAL_GAME_MODE";
        public const string HEAT_GAME_MODE = "HEAT_GAME_MODE";
    }

    public sealed class ButtonState
    {
        public const string SURVIVAL_ENTER = "SurvivalEnter";
        public const string SURVIVAL_EXIT = "SurvivalExit";
        public const string NORMAL_ENTER = "NormalEnter";
        public const string NORMAL_EXIT = "NormalExit";
    }

    public sealed class Saves
    {
        public const string BEST_TIME_HEAT = "BEST_TIME_HEAT";
        public const string BEST_TIME_NORMAL = "BEST_TIME_NORMAL";
        public const string BEST_SCORE_NORMAL = "BEST_SCORE_NORMAL";
        public const string BEST_SCORE_HEAT = "BEST_SCORE_HEAT";
        public const string LONGEST_KILLSTREAK = "LONGEST_KILLSTREAK";
        public const string LONGEST_KILLSTREAK_NORMAL = "LONGEST_KILLSTREAK_NORMAL";
        public const string HIGHEST_ZOMBIES_KILLED = "HIGHEST_ZOMBIES_KILLED";
        public const string HIGHEST_ZOMBIES_KILLED_NORMAL = "HIGHEST_ZOMBIES_KILLED_NORMAL";
        public const string TOP_LEVEL = "TOP_LEVEL";
        public const string TOP_HEAT = "TOP_HEAT";
        public const string First_SAVE = "0";
        public const string MAIN_VOLUME = "MAIN_VOLUME";
    }
}
