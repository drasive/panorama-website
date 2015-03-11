using System;
using System.Net;
using CommandLine;
using CommandLine.Text;

namespace DimitriVranken.PanoramaCreator
{
    class Options
    {
        [Option('a', "ip-address", Required = true, HelpText = "The ip address of the camera.")]
        public string IpAddress { get; set; }
        public IPAddress IpAddressParsed { get; set; }

        [Option('o', "output", Required = true, HelpText = "The file to output the panoramic image to.")]
        public string Output { get; set; }

        [Option('f', "force", DefaultValue = false, HelpText = "Force an overwrite if the output file already exists. Use with care.")]
        public bool Force { get; set; }

        [Option('c', "image-count", DefaultValue = 8, HelpText = "The number of images to generate the panoramic image from.")]
        public int ImageCount { get; set; }


        [Option("proxy-address", HelpText = "The address of the proxy server to use.")]
        public string ProxyAddress { get; set; }
        public Uri ProxyAddressParsed { get; set; }

        [Option("proxy-username", HelpText = "The username for the proxy server.")]
        public string ProxyUsername { get; set; }

        [Option("proxy-password", HelpText = "The password for the proxy server.")]
        public string ProxyPassword { get; set; }


        [Option('v', "verbose", DefaultValue = false, HelpText = "Print verbose details during execution.")]
        public bool Verbose { get; set; }

        // TODO: Implement no-network
        [Option("no-network", DefaultValue = false, HelpText = "Do not use any network resources like the camera.")]
        public bool NoNetwork { get; set; }


        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            // Build an automatic help and error message
            return HelpText.AutoBuild(this, (current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
