using Org.BouncyCastle.Asn1.Mozilla;

namespace ISLWebService.Dto
{
    public class InfoMsg
    {
        public string Messaggio;
        public DateTime Data;

        public InfoMsg(string messaggio, DateTime data)
        {
            Messaggio = messaggio;
            Data = data;
        }
    }
}
