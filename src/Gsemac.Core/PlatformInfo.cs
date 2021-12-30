namespace Gsemac.Core {

    public class PlatformInfo :
          IPlatformInfo {

        // Public members

        public PlatformId Id { get; } = PlatformId.Unknown;

        // Internal members

        public PlatformInfo(PlatformId id) {

            Id = id;

        }

    }

}