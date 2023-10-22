namespace Gsemac.Net.Dns {

    public interface IDnsResolver {

        IDnsMessage Resolve(IDnsMessage message);

    }

}