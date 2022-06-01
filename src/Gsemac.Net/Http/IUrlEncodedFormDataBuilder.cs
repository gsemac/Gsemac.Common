namespace Gsemac.Net.Http {

    public interface IUrlEncodedFormDataBuilder {

        UrlEncodedFormDataBuilder WithField(string key, string value);

        byte[] Build();

    }

}