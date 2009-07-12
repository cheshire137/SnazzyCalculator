using System.Collections.Generic;

namespace Gui
{
    public static class GuiData
    {
        public const int WIN_WIDTH = 500;
        public const int WIN_HEIGHT = 480;
        public const string TITLE = "Snazzy Calculator";
        public const uint NUM_ROWS = 6;
        public const uint NUM_COLS = 4;

        public static Dictionary<string, uint[]> ButtonPlacements =
            new Dictionary<string, uint[]>
                {
                    {"/", new uint[] {0, 1, 2, 2}},
                    {"*", new uint[] {2, 1, 3, 2}},
                    {"-", new uint[] {3, 1, 4, 2}},
                    {"7", new uint[] {0, 2, 1, 3}},
                    {"8", new uint[] {1, 2, 2, 3}},
                    {"9", new uint[] {2, 2, 3, 3}},
                    {"+", new uint[] {3, 2, 4, 4}},
                    {"4", new uint[] {0, 3, 1, 4}},
                    {"5", new uint[] {1, 3, 2, 4}},
                    {"6", new uint[] {2, 3, 3, 4}},
                    {"1", new uint[] {0, 4, 1, 5}},
                    {"2", new uint[] {1, 4, 2, 5}},
                    {"3", new uint[] {2, 4, 3, 5}},
                    {"=", new uint[] {3, 4, 4, 6}},
                    {"0", new uint[] {0, 5, 2, 6}},
                    {".", new uint[] {2, 5, 3, 6}}
                };
    }
}
