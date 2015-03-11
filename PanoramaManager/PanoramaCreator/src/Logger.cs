using NLog;

namespace DimitriVranken.PanoramaCreator
{
    static class Logger
    {
        private static readonly NLog.Logger _default = LogManager.GetCurrentClassLogger();
        private static readonly NLog.Logger _userInterface = LogManager.GetLogger("UserInterface");


        public static NLog.Logger Default
        {
            get { return _default; }
        }

        public static NLog.Logger UserInterface
        {
            get { return _userInterface; }
        }
    }
}
