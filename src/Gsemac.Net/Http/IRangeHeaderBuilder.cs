namespace Gsemac.Net.Http {

    public interface IRangeHeaderBuilder {

        IRangeHeaderBuilder AddRange(int from, int to);
        IRangeHeaderBuilder AddRange(long from, long to);
        IRangeHeaderBuilder AddRange(int range);
        IRangeHeaderBuilder AddRange(long range);
        IRangeHeaderBuilder AddRange(string rangeSpecifier, long from, long to);
        IRangeHeaderBuilder AddRange(string rangeSpecifier, int range);
        IRangeHeaderBuilder AddRange(string rangeSpecifier, long range);
        IRangeHeaderBuilder AddRange(string rangeSpecifier, int from, int to);

    }

}