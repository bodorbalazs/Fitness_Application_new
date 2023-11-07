using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.BLL.Settings
{
    public class SendGridSettings
    {
        public string FromEmail { get; set; }
        public string EmailName { get; set; }
        public string ApiKey { get; set; }

    }
}
