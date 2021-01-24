namespace Gsemac.Core {

    public interface IBinaryDecoder {

        byte[] Decode(byte[] encodedBytes, int startIndex, int length);

    }

}