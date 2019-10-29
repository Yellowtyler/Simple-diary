using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Xml.Serialization;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
namespace Prctc
{

    // [Serializable]
    [DataContract]
    public class User
        {
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string password { get; set; }
        [DataMember]
       public List<string> sheet;
        [DataMember]
        public bool activated;
        public User(string s, string s1)
            {
                username = s;
                password = s1;
            sheet = new List<string>();
            activated = false;
            }
        public User(string s, string s1,List<string> sh)
        {
            username = s;
            password = s1;
            sheet = sh;
            activated = false;
        }
        public User() { sheet = new List<string>(); }
        }
    
}
