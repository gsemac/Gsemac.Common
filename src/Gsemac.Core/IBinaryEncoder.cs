namespace Gsemac.Core {

    public interface IBinaryEncoder {

        byte[] Encode(byte[] bytesToEncode, int startIndex, int length);

    }

}