namespace Gsemac.Text.Codecs {

    public interface IBinaryEncoder {

        byte[] Encode(byte[] bytesToEncode, int startIndex, int length);

    }

}