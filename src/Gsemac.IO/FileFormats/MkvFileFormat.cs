using System.Collections.Generic;

namespace Gsemac.IO.FileFormats {

    // The extensions MKV and MK3D are used for video, while MKA is used for audio and MKS for subtitles.
    // WEBM is a specific profile of MKV with VP8 video and Vorbis audio.
    // https://en.wikipedia.org/wiki/Matroska

    public sealed class MkvFileFormat :
        FileFormatBase {

        public override IEnumerable<string> Extensions => new[] {
            ".mkv",
            ".mka",
            ".mks",
            ".mk3d",
        };
        public override IEnumerable<IFileSignature> Signatures => new[] {
            new FileSignature(0x1A, 0x45, 0xF, 0xA3),
        };
        public override IEnumerable<IMimeType> MimeTypes => new[] {
            new MimeType("video/x-matroska"),
            new MimeType("audio/x-matroska"),
        };

    }

}