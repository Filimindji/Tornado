using System;
using System.Collections.Generic;
using System.Globalization;
using MediaInfoDotNet;
using MediaInfoDotNet.Models;

namespace Tornado.Plugin.MediaInfo
{
    public class MediaInfoPlugin : TornadoPluginBase
    {
        public override IEnumerable<Metadata> GetMetadata(string filePath)
        {
            MediaFile mediaFile = new MediaFile(filePath);
            if (!string.IsNullOrWhiteSpace(mediaFile.format))
                yield return new Metadata("mediainfo-format", mediaFile.format);

            if (!string.IsNullOrWhiteSpace(mediaFile.title))
                yield return new Metadata("mediainfo-title", mediaFile.title);

            if (!string.IsNullOrWhiteSpace(mediaFile.encodedBy))
                yield return new Metadata("mediainfo-encodedBy", mediaFile.encodedBy);

            if (!string.IsNullOrWhiteSpace(mediaFile.encodedLibrary))
                yield return new Metadata("mediainfo-encodedLibrary", mediaFile.encodedLibrary);

            if (mediaFile.encodedDate != DateTime.MinValue)
                yield return new Metadata("mediainfo-encodedDate", mediaFile.encodedDate.ToString(CultureInfo.InvariantCulture));

            if (mediaFile.bitRate != 0)
                yield return new Metadata("mediainfo-bitRate", mediaFile.bitRate.ToString(CultureInfo.InvariantCulture));

            if (mediaFile.duration != 0)
                yield return new Metadata("mediainfo-duration", mediaFile.duration.ToString(CultureInfo.InvariantCulture));

            if (mediaFile.size != 0)
                yield return new Metadata("mediainfo-size", mediaFile.size.ToString(CultureInfo.InvariantCulture));


            if (mediaFile.Video != null)
                foreach (var videoMetadata in GetVideoMetadata(mediaFile.Video))
                    yield return videoMetadata;

            if (mediaFile.Audio != null)
                foreach (var audioMetadata in GetAudioMetadata(mediaFile.Audio))
                    yield return audioMetadata;
        }

        private IEnumerable<Metadata> GetVideoMetadata(IDictionary<int, VideoStream> videos)
        {
            yield return new Metadata("mediainfo-videostream-count", videos.Count.ToString(CultureInfo.InvariantCulture));
            foreach (var videoStream in videos)
            {
                yield return new Metadata("mediainfo-videostream-duration", videoStream.Value.duration.ToString(CultureInfo.InvariantCulture), videoStream.Key);
                yield return new Metadata("mediainfo-videostream-width", videoStream.Value.width.ToString(CultureInfo.InvariantCulture), videoStream.Key);
                yield return new Metadata("mediainfo-videostream-height", videoStream.Value.height.ToString(CultureInfo.InvariantCulture), videoStream.Key);
                yield return new Metadata("mediainfo-videostream-framerate", videoStream.Value.frameRate.ToString(CultureInfo.InvariantCulture), videoStream.Key);

                if (videoStream.Value.codecCommonName != null)
                yield return new Metadata("mediainfo-videostream-codec", videoStream.Value.codecCommonName, videoStream.Key);

                if (videoStream.Value.format != null)
                    yield return new Metadata("mediainfo-videostream-format", videoStream.Value.format, videoStream.Key);
            }
        }

        private IEnumerable<Metadata> GetAudioMetadata(IDictionary<int, AudioStream> audios)
        {
            yield return new Metadata("mediainfo-audiostream-count", audios.Count.ToString(CultureInfo.InvariantCulture));
            foreach (var audioStream in audios)
            {
                yield return new Metadata("mediainfo-audiostream-duration", audioStream.Value.duration.ToString(CultureInfo.InvariantCulture), audioStream.Key);
                yield return new Metadata("mediainfo-audiostream-channels", GetChannels(audioStream.Value.channels), audioStream.Key);
                yield return new Metadata("mediainfo-audiostream-framerate", audioStream.Value.frameRate.ToString(CultureInfo.InvariantCulture), audioStream.Key);

                string language = GetLanguage(audioStream.Value.language);
                if (language != null)
                    yield return new Metadata("mediainfo-audiostream-language", language, audioStream.Key);

                if (audioStream.Value.format != null)
                    yield return new Metadata("mediainfo-audiostream-format", audioStream.Value.format, audioStream.Key);

                if (audioStream.Value.codecCommonName != null)
                    yield return new Metadata("mediainfo-audiostream-codec", audioStream.Value.codecCommonName, audioStream.Key);
            }
        }

        private string GetLanguage(string language)
        {
            return language != null ? new CultureInfo(language).ToString() : null;
        }

        private string GetChannels(int channels)
        {
            switch (channels)
            {
                case 1:
                    return "1.0";

                case 2:
                    return "2.0";

                case 3:
                    return "2.1";

                case 5:
                    return "5.0";

                case 6:
                    return "5.1";

                case 7:
                    return "6.1";
            }

            return channels.ToString(CultureInfo.InvariantCulture);
        }
    }
}
