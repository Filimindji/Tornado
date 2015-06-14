using System;
using System.Collections.Generic;
using System.Globalization;
using MediaInfoDotNet;

namespace Tornado.Plugin.MediaInfo
{
    public class MediaInfoPlugin : TornadoPluginBase
    {
        public override Metadata[] GetMetadata(string filePath)
        {
            MediaFile mediaFile = new MediaFile(filePath);

            List<Metadata> result = new List<Metadata>();

            if (!string.IsNullOrWhiteSpace(mediaFile.format))
                result.Add(new Metadata("mediainfo-format", mediaFile.format));

            if (!string.IsNullOrWhiteSpace(mediaFile.title))
                result.Add(new Metadata("mediainfo-title", mediaFile.title));

            if (!string.IsNullOrWhiteSpace(mediaFile.encodedBy))
                result.Add(new Metadata("mediainfo-encodedBy", mediaFile.encodedBy));

            if (!string.IsNullOrWhiteSpace(mediaFile.encodedLibrary))
                result.Add(new Metadata("mediainfo-encodedLibrary", mediaFile.encodedLibrary));

            if (mediaFile.encodedDate != DateTime.MinValue)
                result.Add(new Metadata("mediainfo-encodedDate", mediaFile.encodedDate.ToString(CultureInfo.InvariantCulture)));

            if (mediaFile.bitRate != 0)
                result.Add(new Metadata("mediainfo-bitRate", mediaFile.bitRate.ToString(CultureInfo.InvariantCulture)));

            if (mediaFile.duration != 0)
                result.Add(new Metadata("mediainfo-duration", mediaFile.duration.ToString(CultureInfo.InvariantCulture)));

            if (mediaFile.size != 0)
                result.Add(new Metadata("mediainfo-size", mediaFile.size.ToString(CultureInfo.InvariantCulture)));

            return result.ToArray();
        }

    }
}
