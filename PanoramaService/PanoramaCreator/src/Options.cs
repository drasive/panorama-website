using CommandLine;
using CommandLine.Text;

namespace DimitriVranken.PanoramaCreator {
    class Options {
        [ParserState]
        public IParserState LastParserState { get; set; }


        [Option('a', "ip-address", Required = true, HelpText = "The ip address of the camera.")]
        public string IpAddress { get; set; }

        [Option('o', "output", Required = true, HelpText = "The file to output the panoramic image to.")]
        public string Output { get; set; }

        [Option('f', "force", DefaultValue = false, HelpText = "Force an overwrite if the output file already exists. Use with care.")]
        public bool Force { get; set; }

        [Option('c', "image-count", DefaultValue = 5, HelpText = "The number of images to generate the panoramic image from.")]
        public int ImageCount { get; set; }

        [Option('v', "verbose", DefaultValue = false, HelpText = "Print verbose details during execution.")]
        public bool Verbose { get; set; }


        [HelpOption]
        public string GetUsage() {
            // Build an automatic help and error message
            return HelpText.AutoBuild(this, (current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
