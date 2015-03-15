using System;
using System.IO;
using System.Net;
using CommandLine;
using CommandLine.Text;

namespace DimitriVranken.PanoramaCreator
{
    class Options
    {
        [Option('c', "ip-address", Required = true,
            HelpText = "The ip address of the camera.")]
        public string IpAddress { get; set; }
        public IPAddress IpAddressParsed { get; set; }

        // TODO: (Networking) Update updated default value
        [Option('i', "image-count", DefaultValue = 8,
            HelpText = "The number of images to generate the panoramic image from.")]
        public int ImageCount { get; set; }

        [Option('o', "output", Required = true,
            HelpText = "The folder to output the panoramic image to.")]
        public string Output { get; set; }
        public DirectoryInfo OutputParsed { get; set; }

        [Option('a', "archive", DefaultValue = false,
            HelpText = "Save the panoramic image into an archive subfolder too.")]
        public bool Archive { get; set; }

        [Option('t', "thumbnail", DefaultValue = false,
            HelpText = "Save a thumbnail with a reduced resolution too.")]
        public bool Thumbnail { get; set; }


        [Option("proxy-address", HelpText = "The address of the proxy server to use.")]
        public string ProxyAddress { get; set; }
        public Uri ProxyAddressParsed { get; set; }

        [Option("proxy-username", HelpText = "The username for the proxy server.")]
        public string ProxyUsername { get; set; }

        [Option("proxy-password", HelpText = "The password for the proxy server.")]
        public string ProxyPassword { get; set; }


        [Option('v', "verbose", DefaultValue = false,
            HelpText = "Print verbose details during execution.")]
        public bool Verbose { get; set; }

        [Option('f', "force", DefaultValue = false,
            HelpText = "Accept any possible confirmations automatically. Use with care.")]
        public bool Force { get; set; }

        // TODO: Skip method calls at a higher level?
        [Option("no-network", DefaultValue = false,
            HelpText = "Do not use any network resources like the camera.")]
        public bool NoNetwork { get; set; }

        // TODO: Implement no-merge
        [Option("no-merge", DefaultValue = false,
            HelpText = "Do not merge the created snapshots into a panoramic image.")]
        public bool NoMerge { get; set; }


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
