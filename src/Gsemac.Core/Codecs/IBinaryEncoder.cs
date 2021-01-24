namespace Gsemac.Core.Codecs {

    public interface IBinaryEncoder {

        byte[] Encode(byte[] bytesToEncode, int startIndex, int length);

    }

}