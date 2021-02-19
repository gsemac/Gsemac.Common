namespace Gsemac.Text.Codecs {

    public interface IBinaryDecoder {

        byte[] Decode(byte[] encodedBytes, int startIndex, int length);

    }

}