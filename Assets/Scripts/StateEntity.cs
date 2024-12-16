using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    [System.Serializable]
    public class StateEntity
    {
        public string UserToken;
        public string ImageId;
        public bool IsAvailable;
    }
    [System.Serializable]
    public class ImageUpdate
    {
        public int id;
        public bool updateFlag;
    }
    [System.Serializable]
    public class ImageUpdateStatus
    {
        public int status;
        public string content;
    }
}
